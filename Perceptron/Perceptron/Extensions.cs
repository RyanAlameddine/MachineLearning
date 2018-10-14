using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron
{
    static class Extensions
    {
        public static double NextDouble(this Random gen, double min, double max)
        {
            return gen.NextDouble() * (max - min) + min;
        }
    }
}
