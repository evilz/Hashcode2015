using System;
using System.Collections.Generic;
using System.Linq;
using Hashcode2015.Core.Model;

namespace HashCode2015.Model
{
    public class DataCenter
    {
        public IEnumerable<Server> AllServers { get; private set; }

        private readonly List<Pool> _pools = new List<Pool>();
        public List<Pool> Pools
        {
            get { return _pools; }
        }

        public List<Row> Rows { get; private set; }

        public DataCenter(int rowCount, int slotCount, List<Point> deadSlots, int poolCount, List<Server> allServers)
        {
            //create pools
            _pools.AddRange(Enumerable.Range(0, poolCount).Select(i => new Pool(i)));

            // create rows
            Rows = new List<Row>();
            Rows.AddRange(Enumerable.Range(0, rowCount).Select(i =>
                new Row(i, slotCount, deadSlots.Where(d => d.Y == i).Select(d => d.X))));

            // set All Server
            AllServers = allServers;
        }

        private static void OrganiseServerInGroup(IOrderedEnumerable<Server> serversByScoreAndSize, List<Pool> pools)
        {
            foreach (var server in serversByScoreAndSize)
            {
                var pool = pools.OrderBy(g => g.TotalCapacity).First();
                pool.Servers.Add(server);
                server.Pool = pool;
            }
        }


        public void ArrangeServers()
        {
            ArrangeInPoolFirst();
        }

        // SCORE OF 368 !
        private void ArrangeInPoolFirst()
        {
//sort by capacity/size
            var serversByScoreAndSize = AllServers.OrderByDescending(s => s.Score).ThenByDescending(s => s.Size);

            // put server in group to equilibrate Pool capacity
            OrganiseServerInGroup(serversByScoreAndSize, _pools);

            foreach (var server in serversByScoreAndSize)
            {
                var serverPool = server.Pool;
                var rowsOrderedForPoolCapacity = Rows.OrderBy(r => r.GetCapacityForPool(serverPool));

                foreach (var row in rowsOrderedForPoolCapacity)
                {
                    if (row.TryAddServer(server))
                        break;
                }
            }
        }

        private void ArrangeInRowFirst()
        {
            //sort by capacity/size
            var serversByScoreAndSize = AllServers.OrderByDescending(s => s.Score).ThenBy(s => s.Size);

            foreach (var server in serversByScoreAndSize)
            {
                var rowsOrderedForPoolCapacity = Rows.OrderBy(r => r.TotalCapacity);

                foreach (var row in rowsOrderedForPoolCapacity)
                {
                    if (row.TryAddServer(server))
                        break;
                }
            }

            var serverInRows = Rows.SelectMany(r => r.Servers);

            while (Rows.Any(r => r.Servers.Any(s => !s.IsUsed)))
            {
                // get lowest capacity pool
               var pool = Pools.OrderBy(p => p.TotalCapacity).First();
               var server = Rows.SelectMany(r => r.Servers.Where(s => !s.IsUsed)).OrderByDescending(s=>s.Score).First();

                pool.Servers.Add(server);
                server.IsUsed = true;
                server.Pool = pool;
            }
        }
		
        public void PutServerAt(Server server, int row, int slot)
        {
            server.Row = row;
            server.Slot = slot;
            server.IsUsed = true;
            var currentRow = Rows[row];
            currentRow.PutServerAt(server, slot);

        }

        public int GarantedCapacity
        {
            get
            {
                return Pools.Min(p => p.GetGarantedCapacity(Rows.Count));
            }
        }
    }
}