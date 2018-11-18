using System;
using System.Linq;
using System.Threading;

namespace MiniMaxTree
{
    class Program
    {
        static void Main(string[] args)
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
                Thread.Sleep(1000);

                TicTacGameState currentTicTacGameState = (TicTacGameState)ticTacTree.Root.gameState;
                currentTicTacGameState.marks[x, y] = 'O';

                manager.GenerateTree(ticTacTree.Root, true);
                manager.CalculateTree(ticTacTree.Root, true);

                int i = 0;
                for (; ticTacTree.Root.children[i].Value != ticTacTree.Root.Value; i++) ;

                ticTacTree.Root = ticTacTree.Root.children[i];

            }
            Console.ReadKey();
        }
    }
}
