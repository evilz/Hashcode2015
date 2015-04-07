using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Hashcode2015.FinalRound.App.Model
{
	public class Input
	{
		public Input()
		{
			Targets = new List<Point>();
			AltitudesWinds = new Dictionary<int, Wind[,]>();
		}

		public static Input Parse(string inputFile)
		{
			var input = new Input();
			// read input TODO move this in class
			using (var stream = File.OpenText(@"Input/final_round.in"))
			{
				var param = stream.ExtractValues<int>().ToArray();
				input.RowsCount = param[0];
				input.ColumnsCount = param[1];
				input.AltitudesCount = param[2];

				param = stream.ExtractValues<int>().ToArray();
				input.TargetsCount = param[0];
				input.Radius = param[1];
				input.BallonsCount = param[2];
				input.Turns = param[3];

				param = stream.ExtractValues<int>().ToArray();
				input.Start = new Point(param[1], param[0]);

				// targets
				for (int i = 0; i < input.TargetsCount; i++)
				{
					param = stream.ExtractValues<int>().ToArray();
					input.Targets.Add(new Point(param[1], param[0]));
				}

				// altitude
				for (int a = 0; a < input.AltitudesCount; a++)
				{
					Wind[,] winds = new Wind[input.RowsCount, input.ColumnsCount];
					for (int y = 0; y < input.RowsCount; y++)
					{
						param = stream.ExtractValues<int>().ToArray();

						for (int x = 0; x < input.ColumnsCount; x = x + 2)
						{
							input.Targets.Add(new Point(param[1], param[0]));
							winds[y, x] = new Wind(param[x + 1], param[x]);
						}
					}
					input.AltitudesWinds.Add(a, winds);
				}
			}

			return input;
		}

		///(1 ≤ R ≤ 1000) ​: le nombre de lignes de la grille,
		public int RowsCount { get; set; }

		//(1 ≤ C ≤ 1000) : le nombre de colonnes de la grille,
		public int ColumnsCount { get; set; }

		//(1 ≤ A ≤ 1000) : le nombre d’altitudes différentes de la simulation ;
		public int AltitudesCount { get; set; }

		public int TargetsCount { get; set; } //(1 ≤ L ≤ 1000) : le nombre de cases cibles,

		public int Radius { get; set; } // ​(0 ≤ V ≤ 100) : le rayon de la couverture fournie par les ballons,

		public int BallonsCount { get; set; } //○ B​(1 ≤ B ≤ 1000) : le nombre de ballons disponibles,

		public int Turns { get; set; } // ○ T​(1 ≤ T ≤ 1000) : le nombre de tour de la simulation ;


		public Point Start { get; set; }

		public List<Point> Targets { get; set; } 

		public Dictionary<int,Wind[,]> AltitudesWinds { get; set; }
	}
}
