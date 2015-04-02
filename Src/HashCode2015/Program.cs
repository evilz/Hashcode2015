using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hashcode2015.Core;
using Hashcode2015.Core.Model;
using HashCode2015.Model;

namespace HashCode2015
{
    class Program
    {
        static void Main(string[] args)
        {
            const string FILE_NAME = @"Samples/dc.in";
            //const string FILE_NAME = @"Samples/sample.in";


            var rowsCount = 0;
            var slotsCount = 0;
            var poolCount = 0;

            List<Server> servers;
            List<Point> deadSlots;

            using (var sr = File.OpenRead(FILE_NAME))
            {
                InputReader.Parse(sr, ref rowsCount, ref slotsCount, ref poolCount, out deadSlots, out servers);
            }



            var datacenter = new DataCenter(rowsCount, slotsCount, deadSlots, poolCount,servers);
            datacenter.ArrangeServers();

	        foreach (var s in datacenter.AllServers.OrderBy(s => s.Index))
	        {
				if(s.IsUsed)
					Console.WriteLine("{0} {1} {2}", s.Row, s.Slot,s.Pool.Index);
				else
				{
					Console.WriteLine("x");
				}
	        }
	        

            Console.ReadLine();
        }

        
    }
}
