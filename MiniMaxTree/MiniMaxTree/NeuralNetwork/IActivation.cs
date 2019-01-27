using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    abstract class IActivation
    {
        public abstract double Activate(double input);

        public abstract double Derivative(double input);

        public abstract double Min { get; }
        public abstract double Max { get; }

        public abstract ActivationTypes ActivationType { get; }


        public static IActivation CreateActivation(ActivationTypes type)
        {
            switch (type)
            {
                case ActivationTypes.Sigmoid:
                    return new Sigmoid();
                    
                default:
                    throw new InvalidOperationException("Unknown type");
                    
            }
        }
    }

    enum ActivationTypes
    {
        Sigmoid,
        RELU,
        BinaryStep
    }
}
