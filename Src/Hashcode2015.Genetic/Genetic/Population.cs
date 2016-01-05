using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hashcode2015.Genetic.Model;

namespace Hashcode2015.Genetic.Genetic
{
    class Population
    {
        private readonly DataCenter _dataCenter;
        private const int TOURNAMENT_SIZE = 3;

        private readonly float _eliteism;

        private readonly float _mutation;

        private readonly float _crossover;

        public List<Chromosome> Chromosomes { get; set; }

        private static readonly Random _rand = new Random(Environment.TickCount);

        public Population(int size, float crossoverRatio, float eliteismRatio, float mutationRatio,DataCenter dataCenter)
        {
            _dataCenter = dataCenter;
            _crossover = crossoverRatio;
            _eliteism = eliteismRatio;
            _mutation = mutationRatio;

            InitializePopulation(size,dataCenter);
        }

        private void InitializePopulation(int size,DataCenter dataCenter)
        {
            Console.WriteLine("InitializePopulation");
            Chromosomes = new List<Chromosome>(size);
            for (int count = 0; count < size; count++)
            {
                Chromosomes.Add(Chromosome.GenerateRandom(dataCenter));
            }

            Chromosomes.Sort();
            Chromosomes.Reverse();
        }

        public void Evolve()
        {
            Console.WriteLine("Evolve");
            List<Chromosome> evolvedSet = new List<Chromosome>(Chromosomes);

            int unchangedIndex = (int)Math.Round(this.Chromosomes.Count * _eliteism);

            Parallel.For(unchangedIndex, Chromosomes.Count - 1, changedIndex =>
            {
                if (_rand.NextDouble() <= _crossover)
                {
                    List<Chromosome> parents = this.SelectParents();
                    List<Chromosome> children = parents.First().Mate(parents.Last());

                    evolvedSet[changedIndex] = children.First();

                    if (_rand.NextDouble() <= this._mutation)
                    {
                        evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
                    }

                    if (changedIndex < evolvedSet.Count - 1)
                    {
                        changedIndex++;

                        evolvedSet[changedIndex] = children.Last();
                        if (_rand.NextDouble() <= this._mutation)
                        {
                            evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
                        }
                    }
                }
                else
                {
                    if (_rand.NextDouble() <= _mutation)
                    {
                        evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
                    }
                }
                changedIndex++;
            });

            //for (int changedIndex = unchangedIndex; changedIndex < Chromosomes.Count - 1; changedIndex++)
            //{
            //    if (_rand.NextDouble() <= _crossover)
            //    {
            //        List<Chromosome> parents = this.SelectParents();
            //        List<Chromosome> children = parents.First().Mate(parents.Last());

            //        evolvedSet[changedIndex] = children.First();

            //        if (_rand.NextDouble() <= this._mutation)
            //        {
            //            evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
            //        }

            //        if (changedIndex < evolvedSet.Count - 1)
            //        {
            //            changedIndex++;

            //            evolvedSet[changedIndex] = children.Last();
            //            if (_rand.NextDouble() <= this._mutation)
            //            {
            //                evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (_rand.NextDouble() <= _mutation)
            //        {
            //            evolvedSet[changedIndex] = evolvedSet[changedIndex].Mutate();
            //        }
            //    }

            //    changedIndex++;
            //}

            evolvedSet.Sort();
            evolvedSet.Reverse();
            Chromosomes = evolvedSet;
        }

        private List<Chromosome> SelectParents()
        {
           // Console.WriteLine("Select Parents");
            List<Chromosome> parents = new List<Chromosome>(2);
            int unchangedIndex = (int)Math.Round(this.Chromosomes.Count * _eliteism);
            for (int parentIndex = 0; parentIndex < 2; parentIndex++)
            {
                //parents.Add(this.Chromosomes[_rand.Next(this.Chromosomes.Count - 1)]);
                parents.Add(this.Chromosomes[_rand.Next(unchangedIndex)]);

                for (int tournyIndex = 0; tournyIndex < TOURNAMENT_SIZE; tournyIndex++)
                {
                    //int randomIndex = _rand.Next(this.Chromosomes.Count - 1);
                    int randomIndex = _rand.Next(unchangedIndex);
                    if (this.Chromosomes[randomIndex].Fitness < parents[parentIndex].Fitness)
                    {
                        parents[parentIndex] = this.Chromosomes[randomIndex];
                    }
                }
            }

            return parents;
        }

      
    }
}
