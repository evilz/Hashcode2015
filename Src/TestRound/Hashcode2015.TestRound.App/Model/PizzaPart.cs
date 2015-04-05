using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Hashcode2015.TestRound.App.Model
{
	public class PizzaPart
	{
		private Point _start;
		private Point _end;
		private int _cellCount;

		public PizzaPart(Point start, Point end)
		{
			_start = start;
			_end = end;
			RefreshCellCount();
			Cells = new List<PizzaCell>();
		}

		public Point Start
		{
			get { return _start; }
			set { _start = value;
				RefreshCellCount();
			}
		}

		public Point End
		{
			get { return _end; }
			set { _end = value; RefreshCellCount(); }
		}

		public List<PizzaCell> Cells { get; } 

		public int HamCount {get { return Cells.Count(cell => cell.IsHam); } }
		public void RefreshCellCount() => _cellCount =(End.X - Start.X + 1) * (End.Y - Start.Y + 1);

		public int CellCount => _cellCount;

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
