using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            // bit bit AND OR XOR NOT
            double[][] inputs = new double[][]
            {                                                                            
                //Nand                                                                   
                new double[] { 0, 0, 1, 0, 0, 1 },
                new double[] { 0, 1, 1, 0, 0, 1 },
                new double[] { 1, 0, 1, 0, 0, 1 },
                new double[] { 1, 1, 1, 0, 0, 1 },                                       
                                                                                         
                //Or                                                                     
                new double[] { 0, 0, 0, 1, 0, 0 },
                new double[] { 0, 1, 0, 1, 0, 0 },
                new double[] { 1, 0, 0, 1, 0, 0 },
                new double[] { 1, 1, 0, 1, 0, 0 },

                //And
                new double[] { 0, 0, 1, 0, 0, 0 },
                new double[] { 0, 1, 1, 0, 0, 0 },
                new double[] { 1, 0, 1, 0, 0, 0 },
                new double[] { 1, 1, 1, 0, 0, 0 },

                //Nor
                new double[] { 0, 0, 0, 1, 0, 1 },
                new double[] { 0, 1, 0, 1, 0, 1 },
                new double[] { 1, 0, 0, 1, 0, 1 },
                new double[] { 1, 1, 0, 1, 0, 1 },

                //XOR
                new double[] { 0, 0, 0, 0, 1, 0 },
                new double[] { 0, 1, 0, 0, 1, 0 },
                new double[] { 1, 0, 0, 0, 1, 0 },
                new double[] { 1, 1, 0, 0, 1, 0 },

                //NXOR
                new double[] { 0, 0, 0, 0, 1, 1 },
                new double[] { 0, 1, 0, 0, 1, 1 },
                new double[] { 1, 0, 0, 0, 1, 1 },
                new double[] { 1, 1, 0, 0, 1, 1 },
            };

            double[] outputs = new double[]
            {
                1, 1, 1, 0,
                0, 1, 1, 1,
                0, 0, 0, 1,
                1, 0, 0, 0,
                0, 1, 1, 0,
                1, 0, 0, 1,
            };

            int epochs = 0;

            #region Genetic
            /*
            // population: 1000 nn
            (NeuralNetwork network, double mae)[] population = new(NeuralNetwork network, double mae)[1000];

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = (new NeuralNetwork(6, new IActivation[][]
                {
                    Enumerable.Repeat(binaryStep, 10).ToArray(),
                    new IActivation[] { binaryStep }
                }), 1);
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
                StringBuilder topValues = new StringBuilder("{ ");
                for (int i = 0; i < inputs.Length; i++)
                {
                    topValues.Append($"{population[0].network.Compute(inputs[i])[0]:0.00}");
                    topValues.Append(' ');
                }
               
                topValues.Append($"}} : {top:0.00}");
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("{ 1.00 1.00 1.00 0.00 0.00 1.00 1.00 1.00 0.00 0.00 0.00 1.00 1.00 0.00 0.00 0.00 0.00 1.00 1.00 0.00 1.00 0.00 0.00 1.00 }");
                Console.WriteLine(topValues.ToString());

                //10% keep
                //80% crossover with a random net from the top 10% AND mutate
                //10% randomize
                int crossStart = (int) (population.Length * .05);
                int randomStart = (int)(population.Length * .7);
                for (int i = crossStart; i < randomStart; i++)
                {
                    NeuralNetwork toCross = population[random.Next(0, crossStart)].network;
                    population[i].network.Crossover(random, toCross);
                    population[i].network.Mutate(random, .2, population[i].mae);
                }
                for(int i = randomStart; i < population.Length; i++)
                {
                    population[i].network.Randomize(random);
                }
            } while (top != 0);
            */
            #endregion

            #region Backprop

            Chx chx = new Chx(Path.Combine(@"\\GMRDC1\Folder Redirection\Ryan.Alameddine\Documents\Visual Studio 2017\Projects\NeuralNet\MiniMaxTree\MiniMaxTree\bin\Debug\netcoreapp2.1", "CHX.txt"));

            NeuralNetwork neuralNetwork = new NeuralNetwork(33, new IActivation[][]
                {
                    Enumerable.Repeat(sigmoid, 15).ToArray(),
                    Enumerable.Repeat(sigmoid, 15).ToArray(),
                    new IActivation[] { sigmoid }
                });
            neuralNetwork.Randomize(random);

            StringBuilder topValues = new StringBuilder();
            while (true)
            {
                epochs++;

                if (Console.KeyAvailable)
                {
                    char keyChar = Console.ReadKey().KeyChar;
                    if (keyChar == 's')
                    {
                        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                        File.WriteAllLines(Path.Combine(desktop, "trained.json"), new string[] { JsonConvert.SerializeObject(neuralNetwork, Formatting.Indented , new JsonSerializerSettings() { ContractResolver = new Resolver() }) });
                    }
                    else if(keyChar == 'x')
                    {
                        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                        var lines = File.ReadAllText(Path.Combine(desktop, "trained.json"));
                        
                        neuralNetwork = JsonConvert.DeserializeObject<NeuralNetwork>(lines, new JsonSerializerSettings() { ContractResolver = new Resolver() });
                    }
                }
                
                double MAE = neuralNetwork.Train(chx.Spaces, chx.Outputs, 1, .0125, 0.2) / chx.Spaces.Length;

                if (epochs % 1000 == 0)
                {
                    /*double sumError = 0;
                    for (int i = 0; i < chx.TestSpaces.Length; i++)
                    {
                        neuralNetwork.Compute(chx.TestSpaces[i]);
                        sumError += neuralNetwork.CalculateError(chx.TestOutputs[i]);
                    }*/
                    Console.WriteLine(MAE);
                    Console.WriteLine(neuralNetwork.MAE(chx.TestSpaces, chx.TestOutputs));
                    ///Console.WriteLine(sumError / chx.TestSpaces.Length);
                    Console.WriteLine($"Epochs: {epochs}");
                }
            }
            Console.ReadKey();
            
            #endregion

            #region MNIST
            /*MNIST mnist = new MNIST();

            // 784,400,10
            NeuralNetwork neuralNetwork = new NeuralNetwork(784, new IActivation[][]
                {
                    Enumerable.Repeat(new Sigmoid(), 600).ToArray(),
                    Enumerable.Repeat(new Sigmoid(), 300).ToArray(),
                    Enumerable.Repeat(new Sigmoid(), 10).ToArray(),
                });
            neuralNetwork.Randomize(random);

            double mae = 0;
            do
            {
                mae = neuralNetwork.Train(mnist.Pixels, mnist.Outputs, 1, .3, .9);
                Console.WriteLine($"{mae}");
            }
            while (mae > 0);
            ;*/
            #endregion MNIST
        }
    }
}

public class Resolver : Newtonsoft.Json.Serialization.DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(p => base.CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                               .Select(f => base.CreateProperty(f, memberSerialization)))
                    .ToList();
        props.ForEach(p => { p.Writable = true; p.Readable = true; });
        return props;
    }
}