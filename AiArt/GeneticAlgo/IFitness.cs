using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    /// <summary>
    /// Represents a fitness calculation function
    /// </summary>
    public interface IFitness<T>
    {
        float GetFitness(T param);
    }
}
