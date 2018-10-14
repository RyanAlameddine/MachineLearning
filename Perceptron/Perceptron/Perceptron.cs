using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron
{
    class Perceptron
    {
        public double Bias;
        public double[] Weights;
        public double Output { get; set; }

        public Perceptron(int inputCount)
        {
            Weights = new double[inputCount];
        }

        public void Randomize(Random random)
        {

            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = random.NextDouble(-1, 1);
            }
        }

        public void Mutate(Random random, double rate, double learningRate)
        {
            for(int i = 0; i < Weights.Length; i++)
            {
                if (random.NextDouble() < rate)
                {
                    Weights[i] += random.NextDouble(-learningRate, learningRate);
                }
            }

            if (random.NextDouble() < rate)
            {
                Bias += random.NextDouble(-learningRate, learningRate);
            }

        }


        public void Train(double[] inputs, double desiredOutput, double learningRate)
        {
            learningRate = Math.Clamp(learningRate, 0.0, 1.0);

            Compute(inputs);

            double error = desiredOutput - Output;
            for (int i = 0; i < inputs.Length; i++)
            {
                Weights[i] += error * inputs[i] * learningRate;
            }
            Bias += error * learningRate;
        }

        public double BinaryStep(double x)
        {
            return x < 0 ? 0 : 1;
        }

        public double Compute(params double[] inputs)
        {
            double output = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * Weights[i];
            }
            output = BinaryStep(output + Bias);
            Output = output;
            return output;
        }
    }
}
