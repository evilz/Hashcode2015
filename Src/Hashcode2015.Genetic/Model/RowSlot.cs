using System.Diagnostics;

namespace Hashcode2015.Genetic.Model
{
    [DebuggerDisplay("Slot at {Position} of {Size}")]
    public class RowSlot
    {
        private int _position = -1;
        private int _size = -1;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public bool IsNew()
        {
            return Position == -1;
        }
    }
}