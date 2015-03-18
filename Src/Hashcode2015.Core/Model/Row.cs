using System.Collections.Generic;
using System.Linq;
using Kunai.Enumerable;

namespace HashCode2015.Model
{
    public class Row
    {
        public const int EMPTY_SLOT = -9;
        public const int DEAD_SLOT = -1;

        private readonly int _slotCount;
        public int[] RowGrid { get; private set; }
        private readonly List<Server> _servers;


        public int Index { get; set; }

        public Row(int index, int slotCount, IEnumerable<int> deadCell)
        {
            Index = index;
            _slotCount = slotCount;

            RowGrid = new int[slotCount];
            Enumerable.Range(0,slotCount).ForEach(i => RowGrid[i] = EMPTY_SLOT);
            
            _servers = new List<Server>();
            
            var deadCell1 = deadCell.ToList();
            deadCell1.ForEach(i => RowGrid[i] = DEAD_SLOT);

        }

        private IEnumerable<Server> GetServersForPool(Pool pool)
        {
            return _servers.Where(s => s.Pool == pool);
        }

        public int GetCapacityForPool(Pool pool)
        {
            var capacity = GetServersForPool(pool).Sum(s => s.Capacity);
            return capacity;
        }


        // slot x, size
        public IEnumerable<RowSlot> AvailableSlot
        {
            get
            {
                var result = new List<RowSlot>();

                var slot = new RowSlot();

                for (var i = 0; i < _slotCount; i++)
                {
                    if (RowGrid[i] == EMPTY_SLOT)
                    {
                        if (slot.IsNew())
                        {
                            slot.Position = i;
                            slot.Size = 0;
                        }
                        slot.Size++;
                    }
                    else
                    {
                        if (!slot.IsNew()) // end of slot
                        {
                            result.Add(slot);
                            slot = new RowSlot();
                        }
                    }
                }
                return result;
            }
        }


        public bool TryAddServer(Server server)
        {
            var slot = AvailableSlot.FirstOrDefault(a => a.Size >= server.Size);
            if (slot != null)
            {
                server.Slot = slot.Position;
                server.Row = Index;
                server.IsUsed = true;
                _servers.Add(server);

                for (int i = server.Slot; i < server.Slot + server.Size; i++)
                {
                    RowGrid[i] = server.Index;
                }
            }
            else
            {
                server.IsUsed = false;
            }

            return server.IsUsed;
        }

        public void PutServerAt(Server server, int slot)
        {
            for (int i = slot; i < slot + server.Size; i++)
            {
                RowGrid[i] = server.Index;
            }
        }
    }
}


