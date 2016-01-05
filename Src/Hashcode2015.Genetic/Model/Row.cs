using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Hashcode2015.Genetic.Model
{
    [DebuggerDisplay("Row: {Index} ({TotalCapacity})")]
    public class Row
    {
        public const int EMPTY_SLOT = -9;
        public const int DEAD_SLOT = -1;

        private readonly int _slotCount;
        private readonly int[] _rowGrid;

        public List<Server> Servers { get; set; }
        public int Index { get; set; }

        public Row(int index, int slotCount, IEnumerable<int> deadCell)
        {
            Index = index;
            _slotCount = slotCount;

            _rowGrid = new int[slotCount];
            for (var i = 0; i < slotCount; i++)
            {
                _rowGrid[i] = EMPTY_SLOT;
            }
        
            Servers = new List<Server>();

            deadCell?.ToList().ForEach(i => _rowGrid[i] = DEAD_SLOT);

        }

        private IEnumerable<Server> GetServersForPool(Pool pool)
        {
            return Servers.Where(s => s.Pool == pool);
        }

        public int GetCapacityForPool(Pool pool)
        {
            var capacity = GetServersForPool(pool).Sum(s => s.Capacity);
            return capacity;
        }

        public int TotalCapacity => Servers.Sum(s => s.Capacity);

        // slot x, size

        public IEnumerable<RowSlot> FindAvailableSlots()
        {
            var result = new List<RowSlot>();

            var slot = new RowSlot();

            for (var i = 0; i < _slotCount; i++)
            {
                if (_rowGrid[i] == EMPTY_SLOT)
                {
                    if (slot.IsNew())
                    {
                        result.Add(slot);
                        slot.Position = i;
                        slot.Size = 0;
                    }
                    slot.Size++;
                }
                else
                {
                    if (!slot.IsNew()) // end of slot
                    {
                        slot = new RowSlot();
                    }
                }
            }
            return result;
        }
        
        // perfect fit else the largest
		public bool TryAddServer(Server server)
		{
			var slot = FindAvailableSlots().OrderBy(a=>a.Size).FirstOrDefault(a => a.Size >= server.Size);

            if (slot != null)
		    {
                server.Slot = slot.Position;
		        server.Row = Index;
		        Servers.Add(server);

		        for (var i = server.Slot; i < server.Slot + server.Size; i++)
		        {
		            _rowGrid[i] = server.Index;
		        }
		    }

		    return server.IsUsed;
		}

		public void PutServerAt(Server server, int slot)
        {
	        if (!Servers.Contains(server))
	        {
		        Servers.Add(server);
	        }
            for (int i = slot; i < slot + server.Size; i++)
            {
                _rowGrid[i] = server.Index;
            }
        }
    }
}


