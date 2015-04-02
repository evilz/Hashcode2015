using System.Collections.Generic;
using System.Linq;
using Hashcode2015.Core;

namespace HashCode2015.Model
{
    public class Row
    {
        public const int EMPTY_SLOT = -9;
        public const int DEAD_SLOT = -1;

        private readonly int _slotCount;
        public int[] RowGrid { get; private set; }
        public List<Server> Servers { get; private set; }


        public int Index { get; set; }

        public Row(int index, int slotCount, IEnumerable<int> deadCell)
        {
            Index = index;
            _slotCount = slotCount;

            RowGrid = new int[slotCount];
            Enumerable.Range(0,slotCount).ForEach(i => RowGrid[i] = EMPTY_SLOT);
            
            Servers = new List<Server>();
            
            var deadCell1 = deadCell.ToList();
            deadCell1.ForEach(i => RowGrid[i] = DEAD_SLOT);

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

        public int TotalCapacity
        {
            get { return Servers.Sum(s => s.Capacity); }
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
        }


        public bool TryAddServer(Server server)
        {
            var slot = AvailableSlot.FirstOrDefault(a => a.Size >= server.Size);
            if (slot != null)
            {
                server.Slot = slot.Position;
                server.Row = Index;
                server.IsUsed = true;
                Servers.Add(server);

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

		// perfect fit else the largest
		public bool TryAddServer2(Server server)
		{
			// perfect fit
			var slot = AvailableSlot.FirstOrDefault(a => a.Size == server.Size);


			slot = AvailableSlot.OrderByDescending(a=>a.Size).FirstOrDefault(a => a.Size >= server.Size);
			if (slot != null)
			{
				server.Slot = slot.Position;
				server.Row = Index;
				server.IsUsed = true;
				Servers.Add(server);

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
	        if (!Servers.Contains(server))
	        {
		        Servers.Add(server);
	        }
            for (int i = slot; i < slot + server.Size; i++)
            {
                RowGrid[i] = server.Index;
            }
        }
    }
}


