using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public int CellCount {get { return Cells.Count(); } }

		public bool IsValid(Input input)
		{
			return HamCount == input.MinHam && CellCount <= input.MaxSize;
		}
		
	}
}
