using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    interface IActivation
    {
        double Activate(double input);

        double Derivative(double input);

        double GetMin();
        double GetMax();
    }
}
