using System.Collections.Generic;
using System.IO;

namespace Hashcode2015.TestRound.App.Model
{
	public class Output
	{
		public Output()
		{
			Parts = new List<PizzaPart>();
		}

		public void AddPart(PizzaPart part)
		{
			Parts.Add(part);
			Score += part.CellCount;
		}

		public List<PizzaPart> Parts
		{
			get;
		}

		public void Save()
		{

			using (var sw = File.CreateText(Score + ".txt"))
			{
				sw.WriteLine(Parts.Count);
				foreach (var p in Parts)
				{
					sw.WriteLine("{0} {1} {2} {3}", p.Start.Y, p.Start.X, p.End.Y, p.End.X);
				}
			}
		}

		public int Score { get; private set; } = 0;

		public override string ToString()
		{
			return "Score : " + Score + " PartCount " + Parts.Count;
		}
	}
}
