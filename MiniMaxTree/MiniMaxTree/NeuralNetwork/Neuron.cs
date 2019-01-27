using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NeuralNetwork
{
    class Neuron
    {
        public double Bias;
        public double[] Weights;
        public double Output;
        
        [JsonIgnore]
        public IActivation activation;
        public ActivationTypes activationType;

        public double Delta = 0;
        public double[] WeightsUpdate;
        public double BiasUpdate = 0;

        public double[] PreviousWeightsUpdate;
        public double PreviousBiasUpdate = 0;

        public Neuron()
        {

        }

        [OnDeserialized]
        internal void CreateActivation(StreamingContext context)
        {
            activation = IActivation.CreateActivation(activationType);
        }


        public Neuron(int inputCount, IActivation activation)
        {
            this.activation = activation;
            this.activationType = activation.ActivationType;
            Weights = new double[inputCount];
            WeightsUpdate = new double[inputCount];
            PreviousWeightsUpdate = new double[inputCount];
        }

        public void Randomize(Random random)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = random.NextDouble(activation.Min, activation.Max);
            }
            Bias = random.NextDouble(activation.Min, activation.Max);
        }

        public double Compute(ReadOnlySpan<double> inputs)
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
