using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleGen.Genetics
{
    /// <summary>
    /// Represents a genetic component
    /// </summary>
    interface IDNA<T>
    {
        void Mutate(Random random, int mutationRate);

        void Crossover(Random random, T other);

        void Randomize(Random random);

        T Copy();
    }
}
