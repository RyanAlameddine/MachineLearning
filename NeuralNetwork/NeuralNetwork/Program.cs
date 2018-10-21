using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            BinaryStep binaryStep = new BinaryStep();
            Sigmoid sigmoid = new Sigmoid();

            // XOR
            // 3 ? 1
            // bit bit gate

            double[][] inputs = new double[][]
            {
                //Nand
                new double[] { 0, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 1, 0, 0 },
                new double[] { 1, 1, 0 },

                //Or
                new double[] { 0, 0, 1 },
                new double[] { 0, 1, 1 },
                new double[] { 1, 0, 1 },
                new double[] { 1, 1, 1 },

                //And
                new double[] { 0, 0, 2 },
                new double[] { 0, 1, 2 },
                new double[] { 1, 0, 2 },
                new double[] { 1, 1, 2 },

                //Nor
                new double[] { 0, 0, 3 },
                new double[] { 0, 1, 3 },
                new double[] { 1, 0, 3 },
                new double[] { 1, 1, 3 },
            };

            double[] outputs = new double[]
            {
                1, 1, 1, 0,
                0, 1, 1, 1,
                0, 0, 0, 1,
                1, 0, 0, 0,
            };

            // population: 1000 nn
            (NeuralNetwork network, double mae)[] population = new(NeuralNetwork network, double mae)[1000];

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = (new NeuralNetwork(3, new IActivation[][]
                {
                    Enumerable.Repeat(sigmoid, 5).ToArray(),
                    Enumerable.Repeat(sigmoid, 5).ToArray(),
                    new IActivation[] { sigmoid }
                }), 0);
                population[i].network.Randomize(random);
            }

            double top = double.MaxValue;
            
            do
            {
                // run the population
                //double[] mae = new double[population.Length];
                for (int i = 0; i < population.Length; i++)
                {
                    population[i].mae = population[i].network.MAE(inputs, outputs);
                }

                Array.Sort(population, (a, b) => a.mae.CompareTo(b.mae));

                // sort them by their error (MAE)
                // keep the top guy the same, mutate the rest (mutation rate should be const) (scale is affected by error)
                top = population[0].mae;
                StringBuilder stringBuilder = new StringBuilder("{ ");
                for (int i = 0; i < inputs.Length; i++)
                {
                    stringBuilder.Append($"{population[0].network.Compute(inputs[i])[0]:0.00}");
                    stringBuilder.Append(' ');
                }
                stringBuilder.Append($"}} : {top:0.00}");
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("{ 1.00 1.00 1.00 0.00 0.00 1.00 1.00 1.00 0.00 0.00 0.00 1.00 1.00 0.00 0.00 0.00 }");
                Console.WriteLine(stringBuilder.ToString());
                for (int i = 1; i < population.Length; i++)
                {
                    population[i].network.Mutate(random, .2, population[i].mae);
                }
            } while (top != 0);

            Console.ReadKey();
        }


    }
}
