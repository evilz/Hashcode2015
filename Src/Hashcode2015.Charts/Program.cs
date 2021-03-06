﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Hashcode2015.Core;
using Hashcode2015.Core.Model;
using HashCode2015.Model;
using RazorEngine;
using RazorEngine.Templating;

namespace Hashcode2015.Charts
{
    class Program
    {
        static void Main(string[] args)
        {
            // read args


            // ### read input file

            const string INPUT_FILE_NAME = @"C:\Users\Vincent\Documents\GITHUB\HashCode2015\Src\Samples\dc.in";
            const string OUTPUT_FILE_NAME = @"C:\Users\Vincent\Documents\GITHUB\HashCode2015\Src\Samples\output2.txt";
            // const string OUTPUT_FILE_NAME = @"C:\Users\Vincent\Documents\GITHUB\HashCode2015\src\Samples\341.txt";
            //const string FILE_NAME = @"Samples/sample.in";


            var rowsCount = 0;
            var slotsCount = 0;
            var poolCount = 0;

            List<Server> servers;
            List<Point> deadSlots;

            using (var sr = File.OpenRead(INPUT_FILE_NAME))
            {
                InputReader.Parse(sr, ref rowsCount, ref slotsCount, ref poolCount, out deadSlots, out servers);
            }

            var datacenter = new DataCenter(rowsCount, slotsCount, deadSlots, poolCount, servers);

            // ### read output file
            OutputReader.Parse(OUTPUT_FILE_NAME, datacenter);


            string template = File.ReadAllText("Index.cshtml");
            var result = Engine.Razor.RunCompile(template, "index", typeof(DataCenter), datacenter);

            using (var writer = File.CreateText("index.htm"))
            {
                writer.Write(result);
                writer.Close();
            }

            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", Environment.CurrentDirectory + "/index.htm");

        }
    }
}
