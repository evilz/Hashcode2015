using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Hashcode2015.TestRound.App
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

	}
}
