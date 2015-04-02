using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Hashcode2015.TestRound.App.Model
{
	public class PizzaPart
	{
		public PizzaPart()
		{
			Cells = new List<PizzaCell>();
		}

		public Point Start { get; set; }
		public Point End { get; set; }

		public List<PizzaCell> Cells { get; } 

		public int HamCount {get { return Cells.Count(cell => cell.IsHam); } }
		public int CellCount => Cells.Count();

		public bool IsValid(Input input)
		{
			return HamCount == input.MinHam && CellCount <= input.MaxSize;
		}

		public override string ToString()
		{
			return string.Format("Start {0} End {1} Cells  {2} Ham {3}", Start, End, CellCount, HamCount);
		}
	}
}
