using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json;

namespace Hashcode2015.Genetic.Model
{
    public class DataCenter
    {
        public IEnumerable<Server> AllServers { get; }

        public IEnumerable<Server> GetUsedServers() => AllServers.Where(server => server.IsUsed);

        public List<Pool> Pools { get; }
        public List<Row> Rows { get; }

        public DataCenter(int rowCount, int slotCount, IEnumerable<Point> deadSlots, int poolCount, IEnumerable<Server> allServers)
        {

            //create pools
            Pools = new List<Pool>(Enumerable.Range(0, poolCount).Select(i => new Pool(i)));

            // create rows
            Rows = Enumerable.Range(0, rowCount)
                    .Select(i => new Row(i, slotCount, deadSlots.Where(d => d.Y == i).Select(d => d.X).ToList()))
                    .ToList();

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

        public void SetServerInSlot()
        {
            //sort by capacity/size
            var serversByScoreAndSize = AllServers.OrderByDescending(s => s.Ratio).ThenByDescending(s => s.Size);

            foreach (var server in serversByScoreAndSize)
            {
                var rowsOrderedForPoolCapacity = Rows.OrderBy(r => r.TotalCapacity);

                var perfectFitRow = rowsOrderedForPoolCapacity.FirstOrDefault(row => row.FindAvailableSlots().Any(slot => slot.Size == server.Size));
                if (perfectFitRow != null)
                {
                    if (perfectFitRow.TryAddServer(server))
                        continue;

                }

                foreach (var row in rowsOrderedForPoolCapacity)
                {
                    if (row.TryAddServer(server))
                        break;
                }
            }

            //     var wantedCapacity = 420;

            //     foreach (var pool in Pools)
            //     {
            //      while (pool.GetGarantedCapacity(Rows.Count) < wantedCapacity)
            //      {
            //       Server selectedServer = null;

            //	var availableServer = AllServers.Where(s => s.Row >= 0 && s.Pool == null).ToList();

            //       if (pool.DeltaServer == null)
            //       {
            //        var server = availableServer.OrderByDescending(s => s.Capacity).First();
            //        pool.Servers.Add(server);
            //        server.IsUsed = true;
            //        server.Pool = pool;
            //        continue;
            //       }
            //       else
            //       {
            //        // qui n est pas sur la ligne du delta
            //        // dont la sum des capacity du pool sur la meme ligne + lui meme est inferieur ou egal a celle du delta
            //        // sort descending by capacity
            //        var pool1 = pool;
            //        var interrestingServers = availableServer.Where(s => s.Row != pool.DeltaServer.Row)
            //	        .Where(s => CheckRowCapacity(s,pool1))
            //			.OrderByDescending(s => s.Capacity);

            //        var perfect =
            //	        interrestingServers.Where(
            //		        s => pool1.TotalCapacity + s.Capacity <= wantedCapacity + pool1.DeltaServer.Capacity).ToList();


            //		if (perfect.Any())
            //		{
            //			//si y en a on prend un de ceux qui on la plus grosse capacity
            //			selectedServer = perfect.First();
            //		}

            //		else if (interrestingServers.Any())
            //        {
            //			//si y en a on prend un de ceux qui on la plus grosse capacity
            //			selectedServer = interrestingServers.First();
            //		}

            //       }


            //	//      var g = pool.GetGarantedCapacity(Rows.Count);
            //	//var delta = wantedCapacity - g;

            //	//// NON IL FAUT AUGMENTER LA CAPACITY GARANTIE !!!
            //	// selectedServer = availableServer.FirstOrDefault(s => s.Capacity == delta);

            //	//if (selectedServer == null)
            //	//{
            //	//	selectedServer = availableServer.OrderByDescending(s => s.Capacity).First();
            //	//}
            //	if (selectedServer != null)
            //	{
            //		pool.Servers.Add(selectedServer);

            //        selectedServer.IsUsed = true;
            //        selectedServer.Pool = pool;
            //       }

            //}
            //     }


            //        while (Rows.Any(r => r.Servers.Any(s => !s.IsUsed)))
            //        {
            //            // get lowest capacity pool
            //           var pool = Pools.OrderBy(p => p.TotalCapacity).First();
            //           var server = Rows.SelectMany(r => r.Servers.Where(s => !s.IsUsed)).OrderByDescending(s=>s.Score).First();

            //            pool.Servers.Add(server);
            //            server.IsUsed = true;
            //            server.Pool = pool;
            //        }
        }

        private bool CheckRowCapacity(Server s, Pool pool1)
        {
            var t = pool1.GetCapacityOnRow(s.Row);
            var t2 = t + s.Capacity;
            return t2 <= pool1.DeltaServer.Capacity;
        }

        public void PutServerAt(Server server, int row, int slot)
        {
            server.Row = row;
            server.Slot = slot;
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

        public int[] GetPoolEncoding()
        {
            foreach (var row in Rows)
            {
                row.Servers = row.Servers.Aggregate(new List<Server>(),
                    (list, server) => { list.Add(AllServers.First(x => x.Index == server.Index)); return list; });
            }

            return Rows.SelectMany(row => row.Servers).Select(server => server.Pool.Index).ToArray();

        }

        public DataCenter FromPoolEncoding(int[] encoded)
        {
            int i = 0;
            foreach (var row in Rows)
            {
                foreach (var server in row.Servers)
                {
                    var index = encoded[i];
                    server.Pool = Pools[index];
                }
            }

            return this;
        }

        public DataCenter Clone()
        {

            var data = JsonConvert.SerializeObject(this, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            return JsonConvert.DeserializeObject<DataCenter>(data);
        }

    }
}