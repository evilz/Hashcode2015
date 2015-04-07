using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hashcode2015.FinalRound.App.Model
{
	public class Ballon
	{
		public Ballon(Point position, int altitude = -1)
		{
			Position = position;
			AllMoves = new List<Moves>();
			Altitude = altitude;
		}

		public List<Moves> AllMoves { get; private set; } 

		public Point Position { get; set; }

		public int Altitude { get; set; }

		public void Move(Moves move)
		{
			AllMoves.Add(move);
			switch (move)
			{
				case Moves.UP:
					Altitude++;
					
					break;
				case Moves.STAY:
					break;
				case Moves.DOWN:
					Altitude--;
					break;
				default:
					throw new ArgumentOutOfRangeException("move");
			}
		}

		//public Moves FindNextMove(Dictionary<int, Wind[,]> winds)
		//{
			
		//}
		
	
    }
}
