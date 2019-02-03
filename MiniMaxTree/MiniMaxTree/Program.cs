using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
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
            Chx();

            Console.ReadKey();
        }

        static void ChxMonteCarloTrain()
        {
            using (StreamWriter fs = new StreamWriter(Path.Combine(@"\\GMRDC1\Folder Redirection\Ryan.Alameddine\Documents\Visual Studio 2017\Projects\NeuralNet\MiniMaxTree\MiniMaxTree\bin\Debug\netcoreapp2.1", "CHX.txt"), true))
            {               

                Random rand = new Random();
                for (int i = 0; i < 10000; i++)
                {
                    CheckersGS gs = new CheckersGS();
                    gs.Xer = rand.Next(0, 2) == 1;
                    int xCount = rand.Next(1, 13);
                    int oCount = rand.Next(1, 13);
                    for (int x = 0; x < xCount; x++)
                    {
                        int f = rand.Next(0, 32) * 2;
                        if (f % 16 < 8)
                        {
                            f++;
                        }
                        if (gs.marks[f] != '\0')
                        {
                            x--;
                            continue;
                        }
                        else
                        {
                            if (rand.Next(0, 10) == 0)
                            {
                                gs.marks[f] = 'X';
                            }
                            else
                            {
                                gs.marks[f] = 'x';
                            }
                        }
                    }

                    for (int o = 0; o < oCount; o++)
                    {
                        int f = rand.Next(0, 32) * 2;
                        if (f % 16 < 8)
                        {
                            f++;
                        }
                        if (gs.marks[f] != '\0')
                        {
                            o--;
                            continue;
                        }
                        else
                        {
                            if (rand.Next(0, 10) == 0)
                            {
                                gs.marks[f] = 'O';
                            }
                            else
                            {
                                gs.marks[f] = 'o';
                            }
                        }
                    }
                    var node = new MiniMaxNode<CheckersGS>(gs);
                    Console.SetCursorPosition(0, 0);
                    gs.ConsoleWrite();
                    Console.WriteLine(i);
                    CheckersGM.MonteCarlo(node, gs.Xer);
                    string ln = node.Value + ":" + gs.ToCompact();
                    fs.WriteLine(ln);
                    fs.Flush();
                }
            }
            //File.AppendAllLines("", lines);
        }


        static void C4()
        {
            Connect4GM manager = new Connect4GM();
            MiniMaxTree<Connect4GS> C4Tree = new MiniMaxTree<Connect4GS>(manager, false, new Connect4GS());

            while (!C4Tree.Root.gameState.gameFinished)
            {
                Console.SetCursorPosition(0, 0);
                C4Tree.Root.gameState.ConsoleWrite();
                ReadOnlySpan<char> charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                int input = int.Parse(charStore);

                int x = input - 1;

                Connect4GS currentC4GS = C4Tree.Root.gameState;
                int y = 0;
                for (; currentC4GS.marks[x, y] != '\0'; y++) ;

                currentC4GS.marks[x, y] = 'O';
                List<MiniMaxNode<Connect4GS>> leafList = new List<MiniMaxNode<Connect4GS>>();
                
                manager.GenerateTree(C4Tree.Root, true, 7);
                manager.AlphaBetaMonteCarlo(C4Tree.Root, true);
                manager.CalculateTree(C4Tree.Root, true);

                if (C4Tree.Root.children.Length != 0)
                {
                    int i = 0;
                    for (; C4Tree.Root.children[i].Value != C4Tree.Root.Value; i++) ;

                    C4Tree.Root = C4Tree.Root.children[i];
                }
            }
            Console.Clear();
            C4Tree.Root.gameState.ConsoleWrite();
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

        static void Chx()
        {
            CheckersGM manager = new CheckersGM();
            MiniMaxTree<CheckersGS> C4Tree = new MiniMaxTree<CheckersGS>(manager, false, new CheckersGS(true));

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trained.json");

            manager.monteNet = JsonConvert.DeserializeObject<NeuralNetwork.NeuralNetwork>(File.ReadAllText(path));

            while (!C4Tree.Root.gameState.gameFinished)
            {
                Console.SetCursorPosition(0, 0);
                C4Tree.Root.gameState.ConsoleWrite();

                ReadOnlySpan<char> charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                int input = int.Parse(charStore);
                int x = input - 1;

                charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                input = int.Parse(charStore);
                int y = input - 1;

                charStore = stackalloc char[] { Console.ReadKey().KeyChar };
                input = int.Parse(charStore);

                CheckersGS currentGS = C4Tree.Root.gameState;

                (int x, int y) d = (0, 0);
                if      (input == 9)
                {
                    d = (1, 1);
                }else if(input == 7)
                {
                    d = (-1, 1);
                }else if(input == 1)
                {
                    d = (-1, -1);
                }else if(input == 3)
                {
                    d = (1, -1);
                }

                if(currentGS.marks[x + d.x, y + d.y] == '\0')
                {
                    currentGS.marks[x + d.x, y + d.y] = currentGS.marks[x, y];
                    currentGS.marks[x, y] = '\0';
                }
                else
                {
                    currentGS.marks[x + d.x, y + d.y] = '\0';
                    currentGS.marks[x + d.x + d.x, y + d.y + d.y] = currentGS.marks[x, y];
                    currentGS.marks[x, y] = '\0';
                }

                List<MiniMaxNode<CheckersGS>> leafList = new List<MiniMaxNode<CheckersGS>>();

                manager.GenerateTree(C4Tree.Root, true, 6);
                Console.Clear();
                manager.AlphaBetaMonteCarlo(C4Tree.Root, true);
                manager.CalculateTree(C4Tree.Root, true);

                if (C4Tree.Root.children.Length != 0)
                {
                    int i = 0;
                    for (; C4Tree.Root.children[i].Value != C4Tree.Root.Value; i++) ;

                    C4Tree.Root = C4Tree.Root.children[i];
                    C4Tree.Root.pruned = false;
                }
            }
            Console.Clear();
            C4Tree.Root.gameState.ConsoleWrite();
        }
    }
}
