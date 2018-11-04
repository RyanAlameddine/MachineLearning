using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class Sigmoid : IActivation
    {
        public double Min => -6;

        public double Max => 6;

        public double Activate(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }

        public double Derivative(double input) //derivative with already calculated output
        {
            return input * (1 - input);
        }
    }
}
