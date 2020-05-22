using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgo
{
    public class GeneticPopulation<T> where T : IDNA<T>
    {
        public T[] Population;
        public int Generation { get; private set; }

        /// <param name="count">The population count</param>
        /// <param name="generator">Function which returns a new <see cref="T"/></param>
        public GeneticPopulation(int count, Func<T> generator)
        {
            Population = new T[count];

            for (int i = 0; i < count; i++)
            {
                Population[i] = generator();
            }
        }

        public void Randomize(Random random)
        {
            foreach(T pop in Population)
            {
                pop.Randomize(random);
            }
        }

        /// <summary>
        /// Runs the entire genetic process for one generation. 
        /// Fitness is calculated, then Crossover occurs, then mutation, then survivor selection.
        /// </summary>
        public void RunGeneration(Random random, Func<T, double> fitnessFunc, float mutationRate, float topPercent = .9f, float bottomPercent = .1f)
        {
            SortByFitness(fitnessFunc);
            Crossover(random, topPercent, bottomPercent);
            Mutate(random, mutationRate);
            SurvivorSelect(random, bottomPercent);

            Generation++;
        }

        public void SortByFitness(Func<T, double> fitnessFunc)
        {
            LinkedList<(T, double)> fitnessPairs = new LinkedList<(T, double)>();
            for(int i = 0; i < Population.Length; i++)
            {
                fitnessPairs.AddLast((Population[i], fitnessFunc(Population[i])));
            }
            Population = Population.OrderBy(fitnessFunc).ToArray();

        }

        /// <summary>
        /// Crosses over pop between the top and bottom percent with pop in the top percent
        /// </summary>
        public void Crossover(Random random, float topPercent = .9f, float bottomPercent = .1f)
        {
            int start = (int)(bottomPercent * Population.Length);
            int end   = (int)(topPercent    * Population.Length);
            for (int i = start; i < end; i++)
            {
                int winnerIndex = random.Next(end, Population.Length);
                Population[i].CrossoverFrom(random, Population[winnerIndex]);
            }
        }

        public void Mutate(Random random, float mutationRate)
        {
            foreach(T pop in Population)
            {
                pop.Mutate(random, mutationRate);
            }
        }

        /// <param name="bottomPercent">Value from 0-1. Anything under that value in fitness order is randomized</param>
        public void SurvivorSelect(Random random, float bottomPercent = .1f)
        {
            int end = (int)(bottomPercent * Population.Length);
            for(int i = 0; i < end; i++)
            {
                Population[i].Randomize(random);
            }
        }
    }
}
