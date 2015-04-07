namespace Hashcode2015.FinalRound.App.Model
{
	public struct Wind
	{
		public Wind(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		public override string ToString()
		{
			return X + " " + Y;
		}
	}
}
