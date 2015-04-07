using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Hashcode2015.FinalRound.App
{
	public static class Helpers
    {
		public static Stream AsStream(this string input)
		{
			// convert string to stream
			byte[] byteArray = Encoding.UTF8.GetBytes(input);
			//byte[] byteArray = Encoding.ASCII.GetBytes(contents);
			MemoryStream stream = new MemoryStream(byteArray);
			return stream;
		}
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            //TODO : GUARD HERE argument null checking omitted
            foreach (T item in sequence) action(item);
        }

        public static IEnumerable<T> ExtractValues<T>(this StreamReader reader, char separator = ' ')
        {
            var line = reader.ReadLine();
            return line.ExtractValues<T>(separator);
        }

		public static T Convert<T>(this string input)
		{
			// NOT in PCL :(
			var converter = TypeDescriptor.GetConverter(typeof(T));
		    //Cast ConvertFromString(string text) : object to (T)
		    return (T)converter.ConvertFromString(input);
		}

        public static IEnumerable<T> ExtractValues<T>(this string input, char separator = ' ')
        {
            var splited = input.Split(separator);
            var result = splited.Select(Convert<T>);
            return result;
        }


		public static TMatrix[,] CreateMatrix<TMatrix, TVal>(this StreamReader reader, Size matrixSize, Func<int, int, TVal, TMatrix[,], TMatrix> matcher)
		{
			int y = 0, x = 0;
			var matrix = new TMatrix[matrixSize.Height, matrixSize.Width];

			var line = reader.ReadLine();

			while (!string.IsNullOrEmpty(line))
			{
				x = 0;
				foreach (var col in line)
				{
					var val = col.ToString().Convert<TVal>();
					matrix[y, x] = matcher(y, x, val, matrix);
					x++;
				}
				y++;
				line = reader.ReadLine();
			}

			return matrix;
		}

		//public static T CloneJson<T>(this T source)
		//{
		//	// Don't serialize a null object, simply return the default for that object
		//	if (Object.ReferenceEquals(source, null))
		//	{
		//		return default(T);
		//	}

		//	return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
		//}

		private static Random random = new Random();

		public static T SelectRandom<T>(this IEnumerable<T> sequence)
		{
			if (sequence == null) { throw new ArgumentNullException(); }
			if (!sequence.Any()) { throw new ArgumentException("The sequence is empty.");}

			//optimization for ICollection<T>
			if (sequence is ICollection<T>)
			{
				ICollection<T> col = (ICollection<T>)sequence;
				return col.ElementAt(random.Next(col.Count));
			}

			int count = 1;
			T selected = default(T);

			foreach (T element in sequence)
			{
				if (random.Next(count++) == 0)
				{
					//Select the current element with 1/count probability
					selected = element;
				}
			}

			return selected;
		}

		public static void ForEach<TMatrix>(this TMatrix[,] matrix, Action<TMatrix, Point> action)
		{
			//if (matrix == null)
			//    throw Guard.ArgumentNull("source");
			//if (selector == null)
			//    throw Error.ArgumentNull("selector");
			for (int y = 0; y < matrix.GetLength(0); ++y)
				for (int x = 0; x < matrix.GetLength(1); ++x)
				{
					action(matrix[y, x], new Point(x, y));
				}
		}

		public static void ToBitmap<T>(this T[,] matrix, string outputPath, Func<T, Color> colorExporter, bool overwriteOutputFile = true)
		{
			if (!overwriteOutputFile && File.Exists(outputPath))
				return;

			Bitmap bitmap = new Bitmap(matrix.GetLength(1), matrix.GetLength(0));

			matrix.ForEach((cell, point) =>
			{
				var color = colorExporter(cell);

				bitmap.SetPixel(point.X, point.Y, color);
			});

			// Get thumbnail.
			Image thumbnail = bitmap.GetThumbnailImage(matrix.GetLength(1),
			matrix.GetLength(0), null, IntPtr.Zero);

			// Save thumbnail.
			thumbnail.Save(outputPath);

		}



		public static bool Between(this int x, int min, int max, bool exclusive = false)
		{
			if (exclusive)
				return x > min && x < max;
			return x >= min && x <= max;
		}
    }
}
