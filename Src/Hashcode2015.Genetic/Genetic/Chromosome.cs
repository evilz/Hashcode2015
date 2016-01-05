using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Hashcode2015.Genetic.Model;

namespace Hashcode2015.Genetic.Genetic
{
    [DebuggerDisplay("Fitness={Fitness}")]
    public class Chromosome : IComparable<Chromosome>
    {
        private static char[] TARGET_GENE = null;
        private static Random rand = new Random(Environment.TickCount);


        public DataCenter Gene { get; private set; }

        public int Fitness { get; set; }

        public Chromosome(DataCenter gene)
        {
            Gene = gene;
            Fitness = CalculateFitness(gene);
            Console.WriteLine(Fitness);
        }

        public int CompareTo(Chromosome other)
        {
            return Fitness.CompareTo(other.Fitness);
        }
        
        public static int CalculateFitness(DataCenter gene)
        {
            return gene.GarantedCapacity;
        }

        public static Chromosome GenerateRandom(DataCenter dataCenter)
        {
            var clone = dataCenter.Clone();


          foreach (var server in clone.GetUsedServers())
            {
                var poolIndex = rand.Next(0, clone.Pools.Count);
                var pool = clone.Pools[poolIndex];
                server.Pool = pool;
                pool.Servers.Add(server);
            }
            
            return new Chromosome(clone);
        }

        public Chromosome Mutate()
        {
            //Console.WriteLine("Mutate");
            var mutatedGene = this.Gene.GetPoolEncoding();

            var mutateChange = Gene.Pools
                .OrderBy(pool => pool.TotalCapacity)
                .Select(pool => pool.Index)
                .First();

            
            int randomIndex = rand.Next(0, mutatedGene.Length);
            //int mutateChange = rand.Next(0, Gene.Pools.Count);

            mutatedGene[randomIndex] = mutateChange;

            return new Chromosome(Gene.FromPoolEncoding(mutatedGene));
        }

        public List<Chromosome> Mate(Chromosome mate)
        {
            var firstgene = Gene.GetPoolEncoding();
            var secondGene = mate.Gene.GetPoolEncoding();

            int pivotIndex = rand.Next(0, firstgene.Length - 1);

            var firstSplit =  firstgene.Take(pivotIndex).Concat(secondGene.Skip(pivotIndex)).ToArray();
            var secondSplit = secondGene.Take(pivotIndex).Concat(firstgene.Skip(pivotIndex)).ToArray();

            return new List<Chromosome>(2)
            {
                new Chromosome(Gene.Clone().FromPoolEncoding(firstSplit)),
                new Chromosome(mate.Gene.Clone().FromPoolEncoding(secondSplit))
            };

        }

    }
}