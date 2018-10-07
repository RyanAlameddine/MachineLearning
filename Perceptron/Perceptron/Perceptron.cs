using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron
{
    class Perceptron
    {
        double bias; //Threshold
        double[] weights;
        Random random;

        public Perceptron(int inputCount, Random random)
        {
            this.random = random;
            weights = new double[inputCount];
            Randomize();
        }

        public void Randomize()
        {
            //set w0 = 1, w1 = 1
            //randomize the weights between 0 - 2

            for(int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() * 2;
            }
        }

        public void Mutate(double scale)
        {
            int index = random.Next(0, weights.Length);

        }

        // 1 2 = 3
        // 5 7 = 13
        public double MAE(double[][] inputs, double[][] outputs)
        {
            double error = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                for(int j = 0; j < inputs[i].Length; j++)
                {
                    error += Math.Abs(outputs[i][j] - inputs[i][j]);
                }
            }
            error /= inputs.Length;
            return error;
        }

        public double Compute(double[] inputs)
        {
            // z = i1 * w1 + i2 * w2
            double output = 0;

            for(int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }

            return output;
        }
    }
}
