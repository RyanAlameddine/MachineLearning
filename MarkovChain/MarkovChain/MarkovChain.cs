using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovChain
{
    class MarkovChain
    {
        Dictionary<string, MarkovNode> nodes = new Dictionary<string, MarkovNode>();
        
        public void ReadText(string text)
        {
            nodes.Clear();
            text = text.Replace("\r\n\r\n", " \n\n");
            //text = text.ToLower();
            string[] words = text.Split(' ');
            nodes.Add(words[0], new MarkovNode(new Dictionary<string, int>()));
            for(int i = 1; i < words.Length; i++)
            {
                if (!nodes.ContainsKey(words[i]))
                {
                    nodes.Add(words[i], new MarkovNode(new Dictionary<string, int>()));
                }
                if (!nodes[words[i - 1]].weights.ContainsKey(words[i]))
                {
                    nodes[words[i - 1]].weights.Add(words[i], 1);
                }
                else
                {
                    nodes[words[i - 1]].weights[words[i]] += 1;
                }
            }
        }

        public string GetText(string currentWord, string end = "\n\nEPISODECOMPLETE")
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            stringBuilder.Append(currentWord);
            stringBuilder.Append(' ');

            MarkovNode currentNode = nodes[currentWord];

            while(currentWord != end)
            {
                List<string> wordOptions = new List<string>(currentNode.weights.Count);
                foreach(string word in currentNode.weights.Keys)
                {
                    for(int i = 0; i < currentNode.weights[word]; i++)
                    {
                        wordOptions.Add(word);
                    }
                }

                int index = random.Next(0, wordOptions.Count);
                currentWord = wordOptions[index];
                currentNode = nodes[currentWord];
                stringBuilder.Append(currentWord);
                stringBuilder.Append(' ');
            }
            


            return stringBuilder.ToString();
        }
    }

    struct MarkovNode
    {
        public Dictionary<string, int> weights;

        public MarkovNode(Dictionary<string, int> weights)
        {
            this.weights = weights;
        }
    }
}
