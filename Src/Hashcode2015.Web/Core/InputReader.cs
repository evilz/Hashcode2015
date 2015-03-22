using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hashcode2015.Core.Model;
using HashCode2015.Model;

namespace Hashcode2015.Core
{
    public static class InputReader
    {
	  

	    public static void Parse(Stream inputStream, ref int rowsCount, ref int slotsCount, ref int poolCount,
            out List<Point> deadSlot,
            out List<Server> servers)
        {
            var reader = new StreamReader(inputStream);

                // parse first line to get Matrix Size
                var inputInt = reader.ExtractValues<int>().ToArray();

                rowsCount = inputInt[0];
                slotsCount = inputInt[1];
                var slotUnavailableCount = inputInt[2];
                poolCount = inputInt[3];
                var serversCount = inputInt[4];

                deadSlot =
                    Enumerable.Range(0, slotUnavailableCount)
                        .Select(_ => reader.ExtractValues<int>().ToArray())
                        .Select(x => new Point(x[1], x[0]))
                        .ToList();

                servers =
                    Enumerable.Range(0, serversCount)
                        .Select(i => new { index = i, values = reader.ExtractValues<int>().ToArray() })
                        .Select(x => new Server(x.index, x.values[0], x.values[1]))
                        .ToList();
            
        }
    }
}
