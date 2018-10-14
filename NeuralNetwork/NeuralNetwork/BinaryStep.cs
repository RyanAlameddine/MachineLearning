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
            throw new NotImplementedException();
        }

        public double GetMax()
        {
            return 1;
        }

        public double GetMin()
        {
            return 0;
        }
    }
}
