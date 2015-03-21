using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;


namespace Hashcode2015.Core
{
	public static class Helpers
    {
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
			if (converter != null)
			{
			    //Cast ConvertFromString(string text) : object to (T)
			    return (T)converter.ConvertFromString(input);
			}
			return default(T);
		}

        public static IEnumerable<T> ExtractValues<T>(this string input, char separator = ' ')
        {
            var splited = input.Split(separator);
            var result = splited.Select(Convert<T>);
            return result;
        }
		
    }
}
