using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class RELU : IActivation
    {
        public double Min => -0.5;

        public double Max => 0.5;

        public double Activate(double input)
        {
            return input < 0 ? 0.01 * input : input;
        }

        public double Derivative(double input)
        {
            return input < 0 ? 0.01 : 1;
        }
    }
}
