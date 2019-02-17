using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkovChain
{
    class Program
    {
        static void Main(string[] args)
        {
            //format("episodes.txt");

            Console.WriteLine("Enter path to output:");
            string name = Console.ReadLine();
            MarkovChain chain = new MarkovChain();
            chain.ReadText(File.ReadAllText("genesis.txt"));
            File.WriteAllText(name, chain.GetText("In", "\n\nENDCHAPTER"));
        }

        static void format(string path)
        {
            string[] lines = File.ReadAllLines(path);
            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Length != 0)
                {
                    lines[i] = "[" + lines[i] + "]";
                }
            }

            File.WriteAllLines(path, lines);

            string text = File.ReadAllText(path);

            text = Regex.Replace(text, "^[^\n]+:.+", (m) =>
            {
                return m.Value.Substring(1, m.Value.Length - 3);
            }, RegexOptions.Multiline);

            File.WriteAllText(path, text);
        }
    }
}
