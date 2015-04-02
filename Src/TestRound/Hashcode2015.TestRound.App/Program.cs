
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Hashcode2015.TestRound.App.Model;
using Kunai.Extentions;

namespace Hashcode2015.TestRound.App
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var stream = File.OpenText(@"..\..\..\Input\test_round.in"))
			{
				//C: \Users\Vincent\Documents\GITHUB\Hashcode2015\Src\TestRound\Input\test_round.in
				var input = new Input();

				var param = stream.ExtractValues<int>().ToArray();
				input.Rows = param[0];
				input.Columns = param[1];
				input.MinHam = param[2];
				input.MaxSize = param[3];

				input.Pizza = stream.CreateMatrix<PizzaCell, char>(new Size(input.Columns, input.Rows), (y, x, val, self) => new PizzaCell(val));

				// part on one line
				//var parts = DummyExtendLineAlgo(R, C, H, S, matrix);
				var parts = SmartAlgo(input);


				Console.WriteLine(parts.Count);
				foreach (var p in parts)
				{
					Console.WriteLine("{0} {1} {2} {3}", p.Start.Y, p.Start.X, p.End.Y, p.End.X);
				}

				using (var sw = File.CreateText("output.txt"))
				{
					sw.WriteLine(parts.Count);
					foreach (var p in parts)
					{
						sw.WriteLine("{0} {1} {2} {3}", p.Start.Y, p.Start.X, p.End.Y, p.End.X);
					}
				}

				var totalUnsed = 0;
				var hamUnsed = 0;
				var otherUnsed = 0;
				var score = 0;

				input.Pizza.ForEach((cell, point) => { if (!cell.IsInPart) totalUnsed++; });
				input.Pizza.ForEach((cell, point) => { if (!cell.IsInPart && cell.IsHam) hamUnsed++; });
				input.Pizza.ForEach((cell, point) => { if (!cell.IsInPart && !cell.IsHam) otherUnsed++; });
				input.Pizza.ForEach((cell, point) => { if (cell.IsInPart) score++; });

				var notValid = parts.Where(p => !p.IsValid(input));
			}



		}

		private static List<PizzaPart> SmartAlgo(Input input)
		{
			List<PizzaPart> allPosibilities = new List<PizzaPart>();
			List<PizzaPart> final = new List<PizzaPart>();
			input.Pizza.ForEach((cell, point) =>
			{
				cell.PossibleParts = GetAllPossiblePart(point.Y, point.X, input);
				allPosibilities.AddRange(cell.PossibleParts);
			});

			var allcell = allPosibilities.SelectMany(p => p.Cells).Distinct();

			while (allPosibilities.Any())
			{

				var sorted = allPosibilities.OrderByDescending(p => p.CellCount);
				var first = sorted.First();
				final.Add(first);

				//allPosibilities = allPosibilities
				//	.Where(p => (p.Start.Y >= first.Start.Y && p.Start.Y <= first.End.Y)   // CREATE a between extention !!!!
				//				&& (p.End.Y >= first.Start.Y && p.Start.Y <= first.End.Y)


				//	.ToList();

			}



			return null;
		}

		private static List<PizzaPart> GetAllPossiblePart(int y, int x, Input input)
		{
			List<PizzaPart> allParts = new List<PizzaPart>();

			for (int y2 = 0; y2 < input.MaxSize; y2++)	// de 0 a 12
			{
				for (int x2 = (input.MaxSize / (y2+1))-1; x2 > 0; x2--) 
				{
					var part = CreatePart(y, x, y2, x2, input);
					//if (part.IsValid(input))
				//	{
						allParts.Add(part);
					//}
				}
			}
			return allParts;
		}

		private static PizzaPart CreatePart(int y, int x, int y2, int x2, Input input)
		{
			PizzaPart part = new PizzaPart {Start = new Point(x, y), End = new Point(x2, y2)};

			for (int i = y; i <= y2 && i < input.Rows; i++)
			{
				for (int j = x; j <= x2 && j < input.Columns; j++) // de 0 a 
				{
					part.Cells.Add(input.Pizza[i, j]);
				}
			}
			return part;
		}

		private static
			List<PizzaPart> DummyAlgo(Input input)
		{
			var parts = new List<PizzaPart>();

			for (int y = 0; y < input.Rows; y++)
			{
				PizzaPart part = null;
				for (int x = 0; x < input.Columns; x++)
				{
					if (part != null)
					{
						part.Cells.Add(input.Pizza[y, x]);
						input.Pizza[y, x].IsInPart = true;

						// end
						if (part.HamCount == input.MinHam && part.CellCount <= input.MaxSize)
						{
							part.End = new Point(x, y);
							parts.Add(part);
							part = null;
						}
						else if (part.CellCount >= input.MaxSize)
						{
							foreach (var pizzaCell in part.Cells) // todo : move this in class
							{
								pizzaCell.IsInPart = false;
							}
							part = null;
						}
					}

					// Start part
					else if (input.Pizza[y, x].IsHam)
					{
						part = new PizzaPart { Start = new Point(x, y) };
						part.Cells.Add(input.Pizza[y, x]);
						input.Pizza[y, x].IsInPart = true;
					}
				}
			}
			return parts;
		}

		private static List<PizzaPart> DummyExtendLineAlgo(Input input)
		{
			var parts = DummyAlgo(input);

			// optimizer
			foreach (var pizzaPart in parts)
			{
				while (pizzaPart.IsValid(input) && pizzaPart.CellCount < 12)
				{
					// try left
					if (pizzaPart.Start.X > 0)
					{
						var left = input.Pizza[pizzaPart.Start.Y, pizzaPart.Start.X - 1];
						if (!left.IsInPart && !left.IsHam)
						{
							pizzaPart.Cells.Add(input.Pizza[pizzaPart.Start.Y, pizzaPart.Start.X - 1]);
							input.Pizza[pizzaPart.Start.Y, pizzaPart.Start.X - 1].IsInPart = true;
							pizzaPart.Start = new Point(pizzaPart.Start.X - 1, pizzaPart.Start.Y);
							continue;
						}
					}
					// try right
					if (pizzaPart.End.X < input.Columns - 1)
					{
						var right = input.Pizza[pizzaPart.End.Y, pizzaPart.End.X + 1];
						if (!right.IsInPart && !right.IsHam)
						{
							pizzaPart.Cells.Add(input.Pizza[pizzaPart.End.Y, pizzaPart.End.X + 1]);
							input.Pizza[pizzaPart.End.Y, pizzaPart.End.X + 1].IsInPart = true;
							pizzaPart.End = new Point(pizzaPart.End.X + 1, pizzaPart.End.Y);
							continue;
						}

					}
					break;
				}
			}

			return parts;
		}
	}
}
