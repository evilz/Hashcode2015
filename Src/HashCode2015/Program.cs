using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Hashcode2015.Core;
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
            
           InputReader.Parse(FILE_NAME, ref rowsCount, ref slotsCount, ref poolCount,out deadSlots, out servers);




            var datacenter = new DataCenter(rowsCount, slotsCount, deadSlots, poolCount,servers);
            datacenter.ArrangeServers();

           datacenter.DisplayGrid();
          //  datacenter.DisplayEmptySlot();
            
            Debug.Print(servers.Count(server => server.IsUsed).ToString());

            Console.ReadLine();
        }

        
    }
}
