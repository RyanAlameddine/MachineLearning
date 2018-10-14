using System;

namespace Perceptron
{
    class Program
    {
        static void Main(string[] args)
        {
            // AND Gate
            // Or Gate
            // Nand Gate
            // Xor (will be big no possible)

            Perceptron orGate = new Perceptron(2);
            Perceptron andGate = new Perceptron(2);
            Perceptron nandGate = new Perceptron(2);


            double[][] inputs =
            {
                new double[]{ 0,0 },
                new double[]{ 0,1 },
                new double[]{ 1,0 },
                new double[]{ 1,1 }
            };

            double[] outputs = { 0, 1, 1, 0 };

            trainPerceptron(orGate,   inputs, new double[] { 0, 1, 1, 1 });
            trainPerceptron(andGate,  inputs, new double[] { 0, 0, 0, 1 });
            trainPerceptron(nandGate, inputs, new double[] { 1, 1, 1, 0 });
            Console.ReadKey();
        }

        static double xor(Perceptron nand, params double[] inputs)
        {
            return nand.Compute(nand.Compute(inputs[0], nand.Compute(inputs[0], inputs[1])), nand.Compute(inputs[1], nand.Compute(inputs[1], inputs[0])));
        }

        static void trainPerceptron(Perceptron perceptron, double[][] inputs, double[] outputs)
        {
            double mae = 0;
            int epoch = 0;
            do
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    perceptron.Train(inputs[i], outputs[i], .5);
                }

                Console.SetCursorPosition(0, 0);

                mae = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    mae += Math.Abs(outputs[i] - perceptron.Compute(inputs[i]));
                    Console.WriteLine($"{inputs[i][0]},{inputs[i][1]}:{outputs[i]} -> {perceptron.Output}");
                }
                mae /= inputs.Length;


                epoch++;
                Console.WriteLine($"Error: {mae:0.00}\nEpoch: {epoch}");

            } while (mae > 0);
        }
    }
}
