using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hashcode2015.Genetic.Genetic;
using Hashcode2015.Genetic.Model;

namespace Hashcode2015.Genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            const string FILE_NAME = @"Samples/dc.in";
            //const string FILE_NAME = @"Samples/sample.in";
            
          //  var input = new StringReader("");
          //  var input = Console.In;
            var input = File.OpenText(FILE_NAME);
         
            // parse first line to get Matrix Size
            var inputInt = input.ReadLine().Split(' ').Select(int.Parse).ToArray();

            var rowsCount = inputInt[0];
            var slotsCount = inputInt[1];
            var slotUnavailableCount = inputInt[2];
            var poolCount = inputInt[3];
            var serversCount = inputInt[4];

            var deadSlots = Enumerable.Range(0, slotUnavailableCount)
                    .Select(_ => input.ReadLine().Split(' ').Select(int.Parse).ToArray())
                    .Select(x => new Point(x[1], x[0]))
                    .ToArray();

            var servers = Enumerable.Range(0, serversCount)
                    .Select(i => new { index = i, values = input.ReadLine().Split(' ').Select(int.Parse).ToArray() })
                    .Select(x => new Server(x.index, x.values[0], x.values[1]))
                    .ToArray();
            

            var datacenter = new DataCenter(rowsCount, slotsCount, deadSlots, poolCount, servers);
            datacenter.SetServerInSlot();

            Debug.WriteLine("Available slot : " +datacenter.Rows.Sum(row => row.FindAvailableSlots().Count()));
            Debug.WriteLine("unused server : " + datacenter.AllServers.Count(server => !server.IsUsed));
            Debug.WriteLine("Datacenter capacity  : " + datacenter.AllServers.Where(server => server.IsUsed).Sum(server => server.Capacity));


            // PKI
            int allCap = datacenter.AllServers.Sum(s => s.Capacity);
            int allSize = datacenter.AllServers.Sum(s => s.Size);

            Console.WriteLine("Global count: {0}", datacenter.AllServers.Count());
            Console.WriteLine("Global capacity: {0}", allCap);
            Console.WriteLine("Global size: {0}", allSize);
            Console.WriteLine("Global ratio: {0}", allCap / (double)allSize);


            var taken = datacenter.AllServers.Where(s => s.IsUsed).ToArray();
            int takenCap = taken.Sum(s => s.Capacity);
            int takenSize = taken.Sum(s => s.Size);

            Console.WriteLine("Taken count: {0}", taken.Length);
            Console.WriteLine("Taken capacity: {0}", takenCap);
            Console.WriteLine("Taken size: {0}", takenSize);
            Console.WriteLine("Taken ratio: {0}", takenCap / (double)takenSize);


            int populationSize = 50;
            int maxGenerations = 16384;
            float crossoverRatio = 1f;
            float elitismRatio = .05f;
            float mutationRatio = 1f;

            
            Population population = new Population(populationSize, crossoverRatio, elitismRatio, mutationRatio,datacenter);

            Chromosome topChromosome = population.Chromosomes.First();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            int count = 1;

            while ((count++ <= maxGenerations) && (topChromosome.Fitness != 0))
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine(topChromosome.Fitness + " - " +topChromosome.Gene.Pools.Average(pool => pool.TotalCapacity));
                Console.ResetColor();

                population.Chromosomes.ForEach(chromosome => Console.WriteLine(topChromosome.Fitness + " - " + topChromosome.Gene.Pools.Average(pool => pool.TotalCapacity)));

                population.Evolve();
                topChromosome = population.Chromosomes.First();
            }
            Console.WriteLine(topChromosome.Fitness + " - " + topChromosome.Gene.Pools.Average(pool => pool.TotalCapacity));

            sw.Stop();

            Console.WriteLine("{0} generations in {1} ms", count, sw.ElapsedMilliseconds);
            Console.ReadLine();

            //foreach (var s in datacenter.AllServers.OrderBy(s => s.Index))
            //{
            //    if (s.IsUsed)
            //        Console.WriteLine("{0} {1} {2}", s.Row, s.Slot, s.Pool.Index);
            //    else
            //    {
            //        Console.WriteLine("x");
            //    }
            //}


            Console.ReadLine();
        }
    }
    
}
