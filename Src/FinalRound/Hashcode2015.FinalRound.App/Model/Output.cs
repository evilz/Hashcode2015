using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode2015.FinalRound.App.Model
{
	public class Output
	{
		public Output()
		{
			TurnsMoves = new List<Moves[]>();
		}

		public List<Moves[]> TurnsMoves { get; private set; } 
	}
}
