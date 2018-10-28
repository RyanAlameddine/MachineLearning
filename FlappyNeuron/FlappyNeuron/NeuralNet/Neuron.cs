using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class Neuron
    {
        public double Bias;
        public double[] Weights;
        public double Output;
        
        IActivation activation;

        public Neuron(int inputCount, IActivation activation)
        {
            this.activation = activation;
            Weights = new double[inputCount];
        }

        public void Randomize(Random random)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = random.NextDouble(activation.Min, activation.Max);
            }
            Bias = random.NextDouble(activation.Min, activation.Max);
        }

        public double Compute(params double[] inputs)
        {
            double output = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * Weights[i];
            }
            output = activation.Activate(output + Bias);
            Output = output;
            return output;
        }
    }
}
