using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class BinaryStep : IActivation
    {
        public override double Activate(double input)
        {
             return input < 0 ? 0 : 1;
        }

        public override double Derivative(double input)
        {
            return 0;
        }

        public override ActivationTypes ActivationType => ActivationTypes.BinaryStep;

        public override double Max => 0.5;
        public override double Min => -0.5;
    }
}
