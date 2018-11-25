using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MiniMaxTree
{
    struct Linq
    {
        public int X;


        public static implicit operator Linq(int x)
        {
            Linq l = new Linq();
            l.X = x;
            return l;
        }
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            List<Linq> LinqedList = Enumerable.Range(0, 100).Select(x => (Linq)x).ToList();
            TicTac();

            Console.ReadKey();
        }

        static void TicTac()
        {
            TicTacGameManager manager = new TicTacGameManager();
            MiniMaxTree ticTacTree = new MiniMaxTree(manager, false);

            while (!ticTacTree.Root.gameState.gameFinished)
            {
                Console.WriteLine(ticTacTree.Root.gameState);
                ReadOnlySpan<char> charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                int input = int.Parse(charStore) - 1;
                Console.Clear();

                int x = input % 3;
                int y = 2 - input / 3;
                Thread.Sleep(300);

                TicTacGameState currentTicTacGameState = (TicTacGameState)ticTacTree.Root.gameState;
                currentTicTacGameState.marks[x, y] = 'O';

                manager.GenerateTree(ticTacTree.Root, true);
                manager.CalculateTree(ticTacTree.Root, true);

                if (ticTacTree.Root.children.Length != 0)
                {
                    int i = 0;
                    for (; ticTacTree.Root.children[i].Value != ticTacTree.Root.Value; i++) ;

                    ticTacTree.Root = ticTacTree.Root.children[i];
                }

            }
            Console.WriteLine(ticTacTree.Root.gameState);
        }
    }
}
