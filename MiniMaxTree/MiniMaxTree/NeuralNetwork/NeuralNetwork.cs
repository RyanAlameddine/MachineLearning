using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        public Layer[] layers { get; set; }

        [JsonIgnore]
        public double[] Outputs => layers[layers.Length - 1].Outputs;

        public NeuralNetwork()
        {

        }

        public NeuralNetwork(int inputCount, IActivation[][] neurons)
        {
            layers = new Layer[neurons.Length];
            layers[0] = new Layer(inputCount, neurons[0]);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i] = new Layer(layers[i - 1].Neurons.Length, neurons[i]);
            }
        }

        public ReadOnlySpan<double> Compute(ReadOnlySpan<double> inputs)
        {
            ReadOnlySpan<double> outputs = inputs;
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
        public void Mutate(Random random, double rate, double scale)
        { 
            for(int l = 0; l < layers.Length; l++)
            {
                for(int i = 0; i < layers[l].Neurons.Length; i++)
                {
                    for(int j = 0; j < layers[l].Neurons[i].Weights.Length; j++)
                    {
                        if(random.NextDouble() < rate)
                        {
                            layers[l].Neurons[i].Weights[j] += random.NextDouble(-scale, scale);
                        }
                    }

                    if (random.NextDouble() < rate)
                    {
                        layers[l].Neurons[i].Bias += random.NextDouble(-scale, scale);
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
        public double Train(double[][] inputs, double[][] desiredOutputs, int batchSize, double learningRate = 1, double momentum = 0.5)
        {
            batchSize = Math.Clamp(batchSize, 1, inputs.Length);
            double sumError = 0;
            for (int i = 0; i < inputs.Length/batchSize; i++)
            {
                ReadOnlySpan<double[]> spanInputs = inputs;
                spanInputs = spanInputs.Slice(i * batchSize, batchSize);

                ReadOnlySpan<double[]> spanOutputs = desiredOutputs;
                spanOutputs = spanOutputs.Slice(i * batchSize, batchSize);

                sumError += Train(spanInputs, spanOutputs, learningRate, momentum);
            }
            return sumError;
        }

        public double Train(ReadOnlySpan<double[]> inputs, ReadOnlySpan<double[]> desiredOutputs, double learningRate = 1, double momentum = 0.5)
        {
            ClearUpdates();
            double sumError = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                Compute(inputs[i]);
                sumError += CalculateError(desiredOutputs[i]);
                CalculateUpdates(inputs[i], learningRate);
            }
            UpdateWeights(momentum);
            momentum = Math.Clamp(momentum, 0, 1);
         

            //calculate mae
            return sumError;
        }

        private void ClearUpdates()
        {
            foreach (Layer layer in layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.WeightsUpdate.Length; i++)
                    {
                        neuron.WeightsUpdate[i] = 0;
                    }
                    neuron.BiasUpdate = 0;
                }
            }
        }

        public double CalculateError(double[] desiredOutputs)
        {
            Layer outputLayer = layers[layers.Length - 1];

            //output
            double sumError = 0;
            for(int i = 0; i < outputLayer.Neurons.Length; i++)
            {
                Neuron outputNeuron = outputLayer.Neurons[i];
                double outputError = desiredOutputs[i] - outputNeuron.Output;
                outputNeuron.Delta = outputError * outputNeuron.activation.Derivative(outputNeuron.Output);
                sumError += Math.Abs(outputError);
            }

            for(int l = layers.Length - 2; l >= 0; l--)
            {
                Layer layer = layers[l];
                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    Neuron neuron = layer.Neurons[i];
                    double error = 0;
                    foreach(Neuron target in layers[l + 1].Neurons)
                    {
                        error += target.Delta * target.Weights[i];
                    }
                    neuron.Delta = error * neuron.activation.Derivative(neuron.Output);
                }
            }

            return sumError;
        }

        private void CalculateUpdates(double[] inputs, double learningRate)
        {
            Layer inputLayer = layers[0];
            for (int i = 0; i < inputLayer.Neurons.Length; i++)
            {
                Neuron neuron = inputLayer.Neurons[i];
                for (int j = 0; j < neuron.Weights.Length; j++)
                {
                    neuron.WeightsUpdate[j] += learningRate * neuron.Delta * inputs[j];
                }
                neuron.BiasUpdate += learningRate * neuron.Delta * 1;
            }

            for (int i = 1; i < layers.Length; i++)
            {
                Layer hiddenLayer = layers[i];
                Layer prevLayer = layers[i - 1];

                for (int j = 0; j < hiddenLayer.Neurons.Length; j++)
                {
                    Neuron neuron = hiddenLayer.Neurons[j];
                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        neuron.WeightsUpdate[k] += learningRate * neuron.Delta * prevLayer.Neurons[k].Output;
                    }
                    neuron.BiasUpdate += learningRate * neuron.Delta * 1; //FIXED
                }
            }
        }

        public void UpdateWeights(double momentum)
        {
            foreach (Layer layer in layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.Weights.Length; i++)
                    {
                        neuron.PreviousWeightsUpdate[i] = neuron.WeightsUpdate[i] + momentum * neuron.PreviousWeightsUpdate[i];
                        neuron.Weights[i] += neuron.PreviousWeightsUpdate[i];
                        
                    }
                    neuron.PreviousBiasUpdate = neuron.BiasUpdate + momentum * neuron.PreviousBiasUpdate;
                    neuron.Bias += neuron.PreviousBiasUpdate;
                }
            }
        }
    }
}
