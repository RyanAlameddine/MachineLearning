using System;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            BinaryStep binaryStep = new BinaryStep();
            NeuralNetwork neuralNetwork = new NeuralNetwork(3, new IActivation[][]
            {
                new IActivation[] { binaryStep, binaryStep },
                new IActivation[] { binaryStep },
                new IActivation[] { binaryStep, binaryStep, binaryStep },
            });
            neuralNetwork.Randomize(random);
            neuralNetwork.Compute(new double[] { 0, 1, 2 } );

            foreach(double output in neuralNetwork.outputs)
            {
                Console.WriteLine(output);
            }

            Console.ReadKey();
        }
    }
}
