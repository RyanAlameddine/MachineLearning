using System;
using System.Threading;

namespace HillClimber
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = Console.ReadLine();
            Random random = new Random();

            char[] value = new char[target.Length];
            for(int i = 0; i < target.Length; i++)
            {
                value[i] = (char) random.Next(32, 127);
            }

            float error;

            do
            {
                error = Error(value, target);
                Mutate(error, value, target, random);
                error = Error(value, target);
                Console.Write(new string(value) + " => ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(target);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" (");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{error:#0.00}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(")");
                Thread.Sleep(10);
            } while (error > 0);

            Console.ReadKey();
        }

        static void Mutate(float currentError, char[] value, string target, Random random)
        {
            
            float updatedError;
            char prevChar;
            do
            {
                int index = random.Next(0, value.Length);
                prevChar = value[index];
                if (random.Next(0, 2) > 0)
                {
                    value[index] = (char)(prevChar + 1);
                }
                else
                {
                    value[index] = (char)(prevChar - 1);
                }
                updatedError = Error(value, target);
                if(updatedError > currentError)
                {
                    value[index] = prevChar;
                }
            } while (updatedError > currentError);
        }

        static float Error(char[] value, string target)
        {
            float error = 0;
            for(int i = 0; i < target.Length; i++)
            {
                error += Math.Abs(value[i] - target[i]);
            }
            error /= target.Length;

            return error;
        }
    }
}
