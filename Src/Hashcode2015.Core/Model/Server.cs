namespace HashCode2015.Model
{
    public class Server
    {
        public Server(int index, int size, int capacity)
        {
            Index = index;
            Size = size;
            Capacity = capacity;
            IsUsed = false;

	        Row = -1;
	        Slot = -1;
        }

        public bool IsUsed { get; set; }

        public int Index { get; private set; }
        public int Size { get; private set; }
        public int Capacity { get; private set; }

        public int Row { get; set; }
        public int Slot { get; set; }

        public Pool Pool { get; set; }

        public float Score
        {
            get { return Capacity/(float)Size; }
        }

        public override string ToString()
        {
            return IsUsed ? string.Format("{0} {1} {2}", Row, Slot, Pool.Index) : "x";
        }
    }
}
