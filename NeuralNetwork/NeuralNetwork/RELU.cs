using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class RELU : IActivation
    {
        public override double Min => -0.5;

        public override double Max => 0.5;

        public override ActivationTypes ActivationType => ActivationTypes.RELU;

        public override double Activate(double input)
        {
            return input < 0 ? 0.01 * input : input;
        }

        public override double Derivative(double input)
        {
            return input < 0 ? 0.01 : 1;
        }
    }
}
