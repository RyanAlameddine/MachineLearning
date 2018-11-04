using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        Layer[] layers;
        public double[] Outputs => layers[layers.Length - 1].Outputs;

        public NeuralNetwork(int inputCount, IActivation[][] neurons)
        {
            layers = new Layer[neurons.Length];
            layers[0] = new Layer(inputCount, neurons[0]);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i] = new Layer(layers[i - 1].Neurons.Length, neurons[i]);
            }
        }

        public double[] Compute(double[] inputs)
        {
            double[] outputs = inputs;
            for (int i = 0; i < layers.Length; i++)
            {
                outputs = layers[i].Compute(outputs);
            }
            return outputs;
        }

        public void Randomize(Random random)
        {
            foreach (Layer layer in layers)
            {
                layer.Randomize(random);
            }
        }

        //Mutate -> Unsupervised Learning -> Genetics
        public void Mutate(Random random, double rate)
        { 
            for(int l = 0; l < layers.Length; l++)
            {
                for(int i = 0; i < layers[l].Neurons.Length; i++)
                {
                    for(int j = 0; j < layers[l].Neurons[i].Weights.Length; j++)
                    {
                        if(random.NextDouble() < rate)
                        {
                            layers[l].Neurons[i].Weights[j] *= random.NextDouble(0.5, 1.5) * random.RandSign();
                        }
                    }

                    if (random.NextDouble() < rate)
                    {
                        layers[l].Neurons[i].Bias *= random.NextDouble(0.5, 1.5) * random.RandSign();
                    }
                }
            }
        }

        public void Crossover(Random random, NeuralNetwork other)
        {
            for(int l = 0; l < other.layers.Length; l++)
            {
                int cutPoint = random.Next(0, layers[l].Neurons.Length);

                int startIndex;
                int endIndex;
                if(random.NextDouble() < .5)
                {
                    startIndex = 0;
                    endIndex = cutPoint;
                }
                else
                {
                    startIndex = cutPoint;
                    endIndex = layers[l].Neurons.Length;
                }
                for(int i = startIndex; i < endIndex; i++)
                {
                    layers[l].Neurons[i].Bias = other.layers[l].Neurons[i].Bias;
                    for(int j = 0; j < layers[l].Neurons[i].Weights.Length; j++)
                    {
                        layers[l].Neurons[i].Weights[j] = other.layers[l].Neurons[i].Weights[j];
                    }
                }
            }
        }

        //Train -> Supervised Learning
    }
}
