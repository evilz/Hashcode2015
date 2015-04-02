using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode2015.TestRound.App.Model
{
	public class PizzaCell
	{
		private readonly char _type;

		public PizzaCell(char type)
		{
			_type = type;
		}

		public bool IsHam => _type == 'H';

		public bool IsInPart { get; set; }

		public List<PizzaPart> PossibleParts {get; set; } 
	}
}
