using System.Collections.Generic;
using System.Linq;

namespace Hashcode2015.Genetic.Model
{
    public class Pool
    {
        public int Index { get; private set; }
        public List<Server> Servers { get; private set; }

        public Pool(int index)
        {
            Index = index;
            Servers = new List<Server>();
        }

        public int TotalCapacity
        {
            get { return Servers.Sum(x => x.Capacity); }
        }

        public int GetGarantedCapacity(int rowCount)
        {
            Dictionary<int, int> capacityWhenDown = new Dictionary<int, int>();

            for (int r = 0; r < rowCount; r++)
            {
                var cap = TotalCapacity - GetCapacityOnRow(r);
                capacityWhenDown[r] = cap;
            }

            return capacityWhenDown.Min(x => x.Value);
        }

	    public Server DeltaServer
	    {
		    get { return Servers.OrderByDescending(s => s.Capacity).FirstOrDefault(); }
	    }

	    public int GetCapacityOnRow(int r)
	    {
		    return Servers.Where(s => s.Row == r).Sum(x => x.Capacity);
	    }
    }



}
