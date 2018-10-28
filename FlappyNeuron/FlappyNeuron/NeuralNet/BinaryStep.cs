using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class BinaryStep : IActivation
    {
        public double Activate(double input)
        {
             return input < 0 ? 0 : 1;
        }

        public double Derivative(double input)
        {
            return 0;
        }

        public double Max => 0.5;
        public double Min => -0.5;
    }
}
