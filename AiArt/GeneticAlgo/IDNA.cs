using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    /// <summary>
    /// Represents a genetic component
    /// </summary>
    public interface IDNA<T>
    {
        void Mutate(Random random, float mutationRate);

        void CrossoverFrom(Random random, T winner);

        void Randomize(Random random);

        T Copy();
    }
}
