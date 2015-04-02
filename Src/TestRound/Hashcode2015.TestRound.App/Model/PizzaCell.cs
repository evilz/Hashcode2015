using System.Collections.Generic;

namespace Hashcode2015.TestRound.App.Model
{
	public class PizzaCell
	{
		private readonly char _type;

		public PizzaCell(char type)
		{
			_type = type;
			//PossibleParts = new List<PizzaPart>();
		}

		public bool IsHam => _type == 'H';

		public bool Analyzed { get; set; }

		public bool IsInPart { get; set; }

		//public List<PizzaPart> PossibleParts {get; set; } 
		
		public List<int> Used { get;private set; } = new List<int>(); 
	}
}
