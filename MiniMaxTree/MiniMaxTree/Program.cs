﻿using System;
using System.Collections.Generic;
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
                
                manager.GenerateTree(C4Tree.Root, true, 4);
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
            MiniMaxTree<CheckersGS> ChxTree = new MiniMaxTree<CheckersGS>(manager, false, new CheckersGS());

            ChxTree.Root.gameState.checkers.AddRange(new Checker[] { new Checker(0, 0, false), new Checker(2, 0, false), new Checker(4, 0, false), new Checker(6, 0, false), new Checker(1, 1, false), new Checker(3, 1, false), new Checker(5, 1, false), new Checker(7, 1, false), new Checker(0, 2, false), new Checker(2, 2, false), new Checker(4, 2, false), new Checker(6, 2, false),
                                                                     new Checker(7-0, 7-0, true), new Checker(7-2, 7-0, true), new Checker(7-4, 7-0, true), new Checker(7-6, 7-0, true), new Checker(7-1, 7-1, true), new Checker(7-3, 7-1, true), new Checker(7-5, 7-1, true), new Checker(7-7, 7-1, true), new Checker(7-0, 7-2, true), new Checker(7-2, 7-2, true), new Checker(7-4, 7-2, true), new Checker(7-6, 7-2, true)});

            while (!ChxTree.Root.gameState.gameFinished)
            {
                Console.SetCursorPosition(0, 0);
                ChxTree.Root.gameState.ConsoleWrite();
                byte x = byte.Parse(Console.ReadLine());
                byte y = byte.Parse(Console.ReadLine());
                byte dx = byte.Parse(Console.ReadLine());
                byte dy = byte.Parse(Console.ReadLine());

                CheckersGS checkersGS = ChxTree.Root.gameState;

                for(int i = 0; i < checkersGS.checkers.Count; i++)
                {
                    Checker checker = checkersGS.checkers[i];
                    if (checker.X == x && checker.Y == y)
                    {
                        checker.X += dx;
                        checker.Y += dy;
                        checkersGS.checkers[i] = checker;
                        break;
                    }
                }
                List<MiniMaxNode<CheckersGS>> leafList = new List<MiniMaxNode<CheckersGS>>();

                manager.GenerateTree(ChxTree.Root, true, 4);
                manager.AlphaBetaMonteCarlo(ChxTree.Root, true);
                manager.CalculateTree(ChxTree.Root, true);

                if (ChxTree.Root.children.Length != 0)
                {
                    int i = 0;
                    for (; ChxTree.Root.children[i].Value != ChxTree.Root.Value; i++) ;

                    ChxTree.Root = ChxTree.Root.children[i];
                }
            }
            Console.Clear();
            ChxTree.Root.gameState.ConsoleWrite();
        }
    }
}
