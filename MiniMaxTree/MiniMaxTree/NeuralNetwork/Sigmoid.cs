using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class Sigmoid : IActivation
    {
        public override double Min => -6;

        public override double Max => 6;

        public override ActivationTypes ActivationType => ActivationTypes.Sigmoid;

        public override double Activate(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }

        public override double Derivative(double input)
        {
            return input * (1 - input);
        }
    }
}
