using System.Diagnostics;

namespace Hashcode2015.Genetic.Model
{
    [DebuggerDisplay("Server: {Index} ({Ratio}) of {Capacity} for {Size}")]
    public class Server
    {
        public Server(int index, int size, int capacity)
        {
            Index = index;
            Size = size;
            Capacity = capacity;
            Row = -1;
	        Slot = -1;
        }

        public bool IsUsed => Row != -1;
        public int Index { get; }
        public int Size { get; }
        public int Capacity { get; }
        public int Row { get; set; }
        public int Slot { get; set; }
        public Pool Pool { get; set; }

        public float Ratio => Capacity/(float)Size;

        public override string ToString()
        {
            return IsUsed ? $"{Row} {Slot} {Pool.Index}" : "x";
        }
    }
}
