using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hashcode2015.FinalRound.App.Model;

namespace Hashcode2015.FinalRound.App
{
	class Program
	{
		static void Main(string[] args)
		{
			// PARSE
			var input = Input.Parse("Input/final_round.in");
			
			// CREATE BALLON and start
			Ballon[] ballons = new Ballon[input.BallonsCount];
			for (int i = 0; i < input.BallonsCount; i++)
			{
				ballons[i] = new Ballon(input.Start);
				ballons[i].Move(Moves.UP);
            }

			for (int i = 0; i < input.Turns; i++)
			{
				foreach (var ballon in ballons)
				{
					ballon.Move(Moves.STAY);
				}
			}


			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < input.Turns; i++)
			{
				for (int b = 0; b < input.BallonsCount; b++)
				{
					var ballon = ballons[b];
					var move = ((int) ballon.AllMoves[i]).ToString();
                    sb.Append(move);
					if(b < input.BallonsCount - 1)
						{ sb.Append(" ");}
				}
				if (i < input.Turns - 1)
					sb.AppendLine();
			}

			using (var sw = File.CreateText("output.out"))
			{
				char[] buffer = sb.ToString().ToCharArray();
				sw.Write(buffer);
			}



				ExportToBitmap(input);
		}

		private static void ExportToBitmap(Input input)
		{
			// bitmap
			Bitmap bitmap = new Bitmap(input.ColumnsCount, input.RowsCount);

			for (int y = 0; y < input.RowsCount; y++)
			{
				for (int x = 0; x < input.ColumnsCount; x++)
				{
					if (input.Targets.Contains(new Point(x, y)))
					{
						bitmap.SetPixel(x, y, Color.MediumSeaGreen);
					}
					else
					{
						bitmap.SetPixel(x, y, Color.White);
					}
				}
			}

			// draw radius
			var covered = GetCoverage(40, 200, input.Radius, input.ColumnsCount);
			foreach (var point in covered)
			{
				bitmap.SetPixel(point.X, point.Y, Color.Crimson);
			}

			bitmap.Save("Targets.bmp");
		}

		public static List<Point> GetCoverage(int r, int c, int radius, int columnsCount)
		{


			List<Point> inRange = new List<Point>();
			for (int u = r - radius; u <= r + radius; u++)
			{
				for (int v = c - radius; v <= c + radius; v++)
				{
					var vbis = v;
					if (v < 0)
					{
						vbis = columnsCount + v;
					}
					if (v >= columnsCount)
					{
						vbis = columnsCount - v;
					}

					var a = Math.Pow(r - u, 2);
					var b = columndist(c, vbis, columnsCount);
					var bb = Math.Pow(columndist(c, vbis, columnsCount), 2);
					var cc = Math.Pow(radius, 2);
					if (a + bb <= cc)
					//if (Math.Pow(r - u,2) + Math.Pow(columndist(c, vbis, columnsCount),2) <= Math.Pow(radius,2))
					{
						inRange.Add(new Point(vbis, u));
					}
				}
			}
			return inRange.OrderBy(point => point.X).ThenBy(point => point.Y).ToList();


		}


		private static int columndist(int c1, int c2, int ColumnsCount)
		{
			var m = Math.Min(Math.Abs(c1 - c2), ColumnsCount - Math.Abs(c1 - c2));
			return m;
		}

	}
}
