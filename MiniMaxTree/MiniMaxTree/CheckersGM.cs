using System;
using System.Collections.Generic;
using NeuralNetwork;

namespace MiniMaxTree
{
    class CheckersGM : GameManager<CheckersGS>
    {
        public NeuralNetwork.NeuralNetwork monteNet = null;

        public override void CalculateTree(MiniMaxNode<CheckersGS> root, bool maximizer)
        {
            if (root.children == null || root.children.Length == 0)
            {
                //if game over set values
                if (root.gameState.gameFinished)
                {
                    if (root.gameState.Tie)
                    {
                        root.Value = 0.5;
                    }
                    else if (root.gameState.XerVictory)
                    {
                        root.Value = int.MaxValue;
                    }
                    else
                    {
                        root.Value = int.MinValue;
                    }
                }
                return;
            }

            CalculateTree(root.children[0], !maximizer);
            root.Value = root.children[0].Value;
            foreach (MiniMaxNode<CheckersGS> child in root.children)
            {
                CalculateTree(child, !maximizer);
                if (child.pruned)
                {
                    continue;
                }
                if (maximizer && child.Value > root.Value)
                {
                    root.Value = child.Value;
                }
                else if (!maximizer && child.Value < root.Value)
                {
                    root.Value = child.Value;
                }
            }
        }

        public override void GenerateTree(MiniMaxNode<CheckersGS> root, bool maximizer)
        {
            Stack<(bool maximizer, MiniMaxNode<CheckersGS> node)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode<CheckersGS> node)>();

            List<CheckersGS> states = new List<CheckersGS>();

            miniMaxNodes.Push((maximizer, root));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode<CheckersGS> node) currentNode = miniMaxNodes.Pop();
                currentNode.node.gameState.GetPossibleStates(currentNode.maximizer, states);
                currentNode.node.children = new MiniMaxNode<CheckersGS>[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode<CheckersGS>(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i]));
                    }
                }
            }
        }

        public void GenerateTree(MiniMaxNode<CheckersGS> root, bool maximizer, int layerDepth)
        {
            Stack<(bool maximizer, MiniMaxNode<CheckersGS> node, int depth)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode<CheckersGS> node, int depth)>();
            List<CheckersGS> states = new List<CheckersGS>(layerDepth * 8);

            miniMaxNodes.Push((maximizer, root, 0));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode<CheckersGS> node, int depth) currentNode = miniMaxNodes.Pop();
                if (currentNode.depth == layerDepth)
                {
                    continue;
                }
                currentNode.node.gameState.GetPossibleStates(currentNode.maximizer, states);
                currentNode.node.children = new MiniMaxNode<CheckersGS>[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode<CheckersGS>(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i], currentNode.depth + 1));
                    }
                }
            }
        }
        
        private double AlphaBetaMonteCarlo(MiniMaxNode<CheckersGS> root, bool maximizer, double alpha, double beta)
        {
            double bestVal = maximizer ? double.MinValue : double.MaxValue;
            if (root.children == null || root.children.Length == 0)
            {
                if (monteNet == null)
                {
                    MonteCarlo(root, maximizer);
                }
                else
                {
                    root.Value = monteNet.Compute(root.gameState.toDoubles())[0];
                }
                return root.Value;
            }
            else
            {
                for (int i = 0; i < root.children.Length; i++)
                {
                    double value = AlphaBetaMonteCarlo(root.children[i], !maximizer, alpha, beta);
                    if (maximizer)
                    {
                        bestVal = dMax(bestVal, value);
                        alpha = dMax(alpha, value);
                    }
                    else
                    {
                        bestVal = dMin(bestVal, value);
                        beta = dMin(beta, value);
                    }
                    if (beta <= alpha)
                    {
                        root.pruned = true;
                        break;
                    }
                }
            }
            return bestVal;
        }

        public double AlphaBetaMonteCarlo(MiniMaxNode<CheckersGS> root, bool maximizer)
        {
            return AlphaBetaMonteCarlo(root, maximizer, double.MinValue, double.MaxValue);
        }

        private double dMax(double first, double second)
        {
            if (first > second)
            {
                return first;
            }
            return second;
        }

        private double dMin(double first, double second)
        {
            if (first < second)
            {
                return first;
            }
            return second;
        }

        public static void MonteCarlo(MiniMaxNode<CheckersGS> leaf, bool maximizer)
        {
            if (leaf.gameState.gameFinished)
            {
                if (leaf.gameState.Tie)
                {
                    leaf.Value = 0;
                    return;
                }
                if (leaf.gameState.XerVictory)
                {
                    leaf.Value = double.MaxValue;
                    return;
                }
                else
                {
                    leaf.Value = double.MinValue;
                    return;
                }
            }


            List<MiniMaxNode<CheckersGS>> roots = new List<MiniMaxNode<CheckersGS>>();
            roots.Add(leaf);

            Random random = new Random();
            var carloNodes = new (int wins, MiniMaxNode<CheckersGS> node)[1000];
            var unvisitedNodes = new List<(MiniMaxNode<CheckersGS> node, int parentIndex)>();

            for (int i = 0; i < roots.Count; i++)
            {
                carloNodes[i] = (0, roots[i]);

                //For each possible turn, create a MiniMaxNode, copy roots[i].gameState into the node with said move, then evaluate victory and add to unvisitedNodes

                Checker root = null;
                Checker currentChecker = null;

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (roots[i].gameState.marks[x, y] == (roots[i].gameState.Xer ? 'x' : 'o') || roots[i].gameState.marks[x, y] == (roots[i].gameState.Xer ? 'X' : 'O'))
                        {
                            if (root == null)
                            {
                                root = new Checker(x, y, roots[i].gameState.marks[x, y]);
                                currentChecker = root;
                                continue;
                            }
                            currentChecker.next = new Checker(x, y, roots[i].gameState.marks[x, y]);
                            currentChecker = currentChecker.next;
                        }
                    }
                }

                currentChecker = root;
                List<CheckersGS> states = new List<CheckersGS>();
                for (; currentChecker != null; currentChecker = currentChecker.next)
                {
                    states.AddRange(roots[i].gameState.GetMoves(roots[i].gameState.Xer, currentChecker));
                }

                foreach(CheckersGS state in states)
                {
                    MiniMaxNode<CheckersGS> miniMaxNode = new MiniMaxNode<CheckersGS>(state);
                    unvisitedNodes.Add((miniMaxNode, i));
                }
            }

            for (int i = roots.Count; i < carloNodes.Length; i++)
            {
                //Picks random unvisited node
                int rand = random.Next(0, unvisitedNodes.Count);
                if (unvisitedNodes.Count == 0)
                {
                    break;
                }
                (MiniMaxNode<CheckersGS> node, int parentIndex) currentNode = unvisitedNodes[rand];
                unvisitedNodes.RemoveAt(rand);
                CheckersGS currentState = currentNode.node.gameState.BlankState;
                //copy currentNode.node.gameState into currentState
                currentState.marks = currentNode.node.gameState.marks;

                //Simulation
                int noChangeCount = 0;
                int simCount = 0;
                while (!currentState.gameFinished)
                {
                    //Pick random moves until game Finished
                    Checker root = null;
                    Checker currentChecker = null;
                    int count = 0;

                    int xMax = random.Next(1, 9);
                    int yMax = random.Next(1, 9);

                    for (int x = 0; x < xMax; x++)
                    {
                        for (int y = 0; y < yMax; y++)
                        {
                            if (currentState.marks[x, y] == (currentState.Xer ? 'x' : 'o') || currentState.marks[x, y] == (currentState.Xer ? 'X' : 'O'))
                            {
                                if (root == null)
                                {
                                    root = new Checker(x, y, currentState.marks[x, y]);
                                    currentChecker = root;
                                    count++;
                                    continue;
                                }
                                currentChecker.next = new Checker(x, y, currentState.marks[x, y]);
                                currentChecker = currentChecker.next;
                                count++;
                            }
                        }
                    }
                    if(root == null)
                    {
                        simCount++;
                        if(simCount > 1000)
                        {
                            currentState.EvaluateVictory(currentState, 0, 0);
                        }
                        continue;
                    }
                    //Console.SetCursorPosition(0, 0);
                    //currentState.ConsoleWrite();
                    List<CheckersGS> states = new List<CheckersGS>();

                    int currentX = currentChecker.x;
                    int currentY = currentChecker.y;
                    //UpRight
                    if ((!currentState.Xer || currentChecker.upgraded) && currentX + 1 < 8 && currentY + 1 < 8)
                    {
                        states.AddRange(currentState.GetMove(currentChecker, currentState.marks, currentX, currentY, currentX + 1, currentY + 1, !currentState.Xer));
                    }

                    //UpLeft
                    if ((!currentState.Xer || currentChecker.upgraded) && currentX - 1 >= 0 && currentY + 1 < 8)
                    {
                        states.AddRange(currentState.GetMove(currentChecker, currentState.marks, currentX, currentY, currentX - 1, currentY + 1, !currentState.Xer));
                    }

                    //DownRight
                    if ((currentState.Xer || currentChecker.upgraded) && currentX + 1 < 8 && currentY - 1 >= 0)
                    {
                        states.AddRange(currentState.GetMove(currentChecker, currentState.marks, currentX, currentY, currentX + 1, currentY - 1, !currentState.Xer));
                    }

                    //DownLeft
                    if ((currentState.Xer || currentChecker.upgraded) && currentX - 1 >= 0 && currentY - 1 >= 0)
                    {
                        states.AddRange(currentState.GetMove(currentChecker, currentState.marks, currentX, currentY, currentX - 1, currentY - 1, !currentState.Xer));
                    }

                    if (states.Count > 0)
                    {
                        int index = random.Next(0, states.Count);
                        currentState = states[index];
                        noChangeCount = 0;
                        simCount++;
                        continue;
                    }
                    else
                    {
                        noChangeCount++;
                        if(noChangeCount > 100)
                        {
                            currentState.gameFinished = true;
                            currentState.XerVictory = !currentState.Xer;
                        }
                    }
                }

                //Fake Nodes

                //For each possible move, create a MiniMaxNode, copy currentNode.node.gameState into createdNode, play a game, and add to unvisited.
                Checker Froot = null;
                Checker FcurrentChecker = null;

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (currentNode.node.gameState.marks[x, y] == (currentNode.node.gameState.Xer ? 'x' : 'o') || currentNode.node.gameState.marks[x, y] == (currentNode.node.gameState.Xer ? 'X' : 'O'))
                        {
                            if (Froot == null)
                            {
                                Froot = new Checker(x, y, currentNode.node.gameState.marks[x, y]);
                                FcurrentChecker = Froot;
                                continue;
                            }
                            FcurrentChecker.next = new Checker(x, y, currentNode.node.gameState.marks[x, y]);
                            FcurrentChecker = FcurrentChecker.next;
                        }
                    }
                }

                FcurrentChecker = Froot;

                List<CheckersGS> Fstates = new List<CheckersGS>();

                for (; FcurrentChecker != null; FcurrentChecker = FcurrentChecker.next)
                {
                    Fstates.AddRange(currentNode.node.gameState.GetMoves(currentNode.node.gameState.Xer, FcurrentChecker));
                    for(int l = 0; l < Fstates.Count; l++)
                    {
                        unvisitedNodes.Add((new MiniMaxNode<CheckersGS>(Fstates[l]), currentNode.parentIndex));
                        Fstates.RemoveAt(l);
                        l--;
                    }
                }

                //Calculate Wins/Losses/Ties
                currentState.XerVictory = !currentState.Xer;
                int winCount = currentState.Tie ? 0 : currentState.XerVictory ? 1 : -1;
                carloNodes[i] = (winCount, currentNode.node);
                carloNodes[currentNode.parentIndex].wins += winCount;
            }

            for (int i = 0; i < roots.Count; i++)
            {
                roots[i].Value = carloNodes[i].wins;
            }
        }
    }
}
