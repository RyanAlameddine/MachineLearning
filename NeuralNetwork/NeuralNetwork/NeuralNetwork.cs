using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        Layer[] layers;
        public double[] outputs;

        public NeuralNetwork(int inputCount, IActivation[][] neurons)
        {
            layers = new Layer[neurons.Length];
            layers[0] = new Layer(inputCount, neurons[0]);
            for(int i = 1; i < layers.Length; i++)
            {
                layers[i] = new Layer(layers[i - 1].Neurons.Length, neurons[i]);
            }
        }

        public double[] Compute(double[] inputs)
        {
            double[] outputs = new double[layers[layers.Length - 1].Neurons.Length];
            for(int i = 0; i < layers.Length; i++)
            {
                outputs = layers[i].Compute(inputs);
                inputs = outputs;
            }
            this.outputs = outputs;
            return outputs;
        }

        public void Randomize(Random random)
        {
            foreach(Layer layer in layers)
            {
                layer.Randomize(random);
            }
        }

        //Mutate
        //Train
    }
}
