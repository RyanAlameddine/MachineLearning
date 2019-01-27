using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    static class Extensions
    {
        public static double NextDouble(this Random gen, double min, double max)
        {
            return gen.NextDouble() * (max - min) + min;
        }

        public static double MAE(this NeuralNetwork neuralNetwork, double[][] inputs, double[][] expectedOutputs)
        {
            //if the output layer is not size 1 throw

            double mae = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                double output = neuralNetwork.Compute(inputs[i])[0];
                mae += Math.Abs(output - expectedOutputs[i][0]);
            }
            mae /= inputs.Length;

            return mae;
        }
    }
}
