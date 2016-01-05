namespace Hashcode2015.Genetic.Model
{
	public struct Point
	{
		public Point(int x, int y) : this()
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }
	}
}
