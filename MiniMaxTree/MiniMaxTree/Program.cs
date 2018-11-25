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
            C4();

            Console.ReadKey();
        }

        static void C4()
        {
            Connect4GM manager = new Connect4GM();
            MiniMaxTree<Connect4GS> C4Tree = new MiniMaxTree<Connect4GS>(manager, false, new Connect4GS());

            while (!C4Tree.Root.gameState.gameFinished)
            {
                Console.WriteLine(C4Tree.Root.gameState);
                ReadOnlySpan<char> charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                int input = int.Parse(charStore);
                Console.Clear();

                int x = input;
                
                Thread.Sleep(300);

                Connect4GS currentC4GS = C4Tree.Root.gameState;
                int y = 0;
                for (; currentC4GS.marks[x, y] != '\0'; y++) ;

                currentC4GS.marks[x, y] = 'O';

                manager.GenerateTree(C4Tree.Root, true, 3);
                manager.CalculateTree(C4Tree.Root, true);

                if (C4Tree.Root.children.Length != 0)
                {
                    int i = 0;
                    for (; C4Tree.Root.children[i].Value != C4Tree.Root.Value; i++) ;

                    C4Tree.Root = C4Tree.Root.children[i];
                }

            }
            Console.WriteLine(C4Tree.Root.gameState);
        }

        static void TicTac()
        {
            TicTacGameManager manager = new TicTacGameManager();
            MiniMaxTree<TicTacGameState> ticTacTree = new MiniMaxTree<TicTacGameState>(manager, false, new TicTacGameState());

            while (!ticTacTree.Root.gameState.gameFinished)
            {
                Console.WriteLine(ticTacTree.Root.gameState);
                ReadOnlySpan<char> charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                int input = int.Parse(charStore) - 1;
                Console.Clear();

                int x = input % 3;
                int y = 2 - input / 3;
                Thread.Sleep(300);

                TicTacGameState currentTicTacGameState = ticTacTree.Root.gameState;
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
