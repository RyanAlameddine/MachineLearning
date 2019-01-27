using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class Layer
    {
        public Neuron[] Neurons { get; set; }
        [JsonIgnore]
        public double[] Outputs => Neurons.Select(n => n.Output).ToArray();
        //public double[] Outputs
        //{
        //    get
        //    {
        //        double[] outputs = new double[Neurons.Length];
        //        for(int i = 0; i < Neurons.Length; i++)
        //        {
        //            outputs[i] = Neurons[i].Output;
        //        }
        //        return outputs;
        //    }
        //}

        public Layer() { }
        public Layer(int inputCount, IActivation[] activations)
        {
            Neurons = new Neuron[activations.Length];
            for (int i = 0; i < activations.Length; i++)
            {
                Neurons[i] = new Neuron(inputCount, activations[i]);
            }
        }

        public void Randomize(Random random)
        {
            foreach (Neuron neuron in Neurons)
            {
                neuron.Randomize(random);
            }
        }

        public ReadOnlySpan<double> Compute(ReadOnlySpan<double> inputs)
        {
            double[] outputs = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                outputs[i] = Neurons[i].Compute(inputs);
            }
            return outputs;
        }
    }
}
