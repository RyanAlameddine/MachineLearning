using System;
using System.Collections.Generic;

namespace MiniMaxTree
{
    class CheckersGM : GameManager<CheckersGS>
    {
        public override void CalculateTree(MiniMaxNode<CheckersGS> root, bool maximizer)
        {

            if(root.children == null || root.children.Length == 0)
            {
                if (root.gameState.gameFinished)
                {
                    if (root.gameState.Tie)
                    {
                        root.Value = 0;
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
            foreach(MiniMaxNode<CheckersGS> child in root.children)
            {
                CalculateTree(child, !maximizer);
                if(maximizer && child.Value > root.Value)
                {
                    root.Value = child.Value;
                }else if(!maximizer && child.Value < root.Value)
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
                if(currentNode.depth == layerDepth)
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

        private int loops = 0;

        private double AlphaBetaMonteCarlo(MiniMaxNode<CheckersGS> root, bool maximizer, double alpha, double beta)
        {
            loops++;
            if (loops > 20)
            {
                Console.SetCursorPosition(0, 0);
                root.gameState.ConsoleWrite(true);
                loops = 0;
            }
            double bestVal = maximizer ? double.MinValue : double.MaxValue;
            if (root.children == null || root.children.Length == 0)
            {
                MonteCarlo(root, maximizer);
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
                        alpha   = dMax(alpha,   value);
                    }
                    else
                    {
                        bestVal = dMin(bestVal, value);
                        beta    = dMin(beta,    value);
                    }
                    if(beta <= alpha)
                    {
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
            if(first > second)
            {
                return first;
            }
            return second;
        }

        private double dMin(double first, double second)
        {
            if(first < second)
            {
                return first;
            }
            return second;
        }

        public void MonteCarlo(MiniMaxNode<CheckersGS> leaf, bool maximizer)
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

            for(int i = 0; i < roots.Count; i++)
            {
                carloNodes[i] = (0, roots[i]);

                int[,] checkerMarks = new int[8, 8];
                foreach (Checker checker in roots[i].gameState.checkers)
                {
                    checkerMarks[checker.X, checker.Y] = checker.Xer ? 1 : -1;
                }

                //Add all moves to unvisitedNodes
                for (int j = 0; j < roots[i].gameState.checkers.Count; j++)
                {
                    if (roots[i].gameState.checkers[i].CheckUpLeft(checkerMarks))
                    {
                        MiniMaxNode<CheckersGS> createdNode = new MiniMaxNode<CheckersGS>(roots[i].gameState.BlankState);

                        createdNode.gameState.checkers.AddRange(roots[i].gameState.checkers);
                        Checker checker = createdNode.gameState.checkers[i];
                        checker.X--;
                        checker.Y++;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = roots[i].gameState.Xer;
                        createdNode.gameState.gameFinished = roots[i].gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = roots[i].gameState.Xer;

                        unvisitedNodes.Add((createdNode, i));
                    }
                    if (roots[i].gameState.checkers[i].CheckUpRight(checkerMarks))
                    {
                        MiniMaxNode<CheckersGS> createdNode = new MiniMaxNode<CheckersGS>(roots[i].gameState.BlankState);

                        createdNode.gameState.checkers.AddRange(roots[i].gameState.checkers);
                        Checker checker = createdNode.gameState.checkers[i];
                        checker.X++;
                        checker.Y++;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = roots[i].gameState.Xer;
                        createdNode.gameState.gameFinished = roots[i].gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = roots[i].gameState.Xer;

                        unvisitedNodes.Add((createdNode, i));
                    }
                    if (roots[i].gameState.checkers[i].CheckDownLeft(checkerMarks))
                    {
                        MiniMaxNode<CheckersGS> createdNode = new MiniMaxNode<CheckersGS>(roots[i].gameState.BlankState);

                        createdNode.gameState.checkers.AddRange(roots[i].gameState.checkers);
                        Checker checker = createdNode.gameState.checkers[i];
                        checker.X--;
                        checker.Y--;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = roots[i].gameState.Xer;
                        createdNode.gameState.gameFinished = roots[i].gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = roots[i].gameState.Xer;

                        unvisitedNodes.Add((createdNode, i));
                    }
                    if (roots[i].gameState.checkers[i].CheckDownRight(checkerMarks))
                    {
                        MiniMaxNode<CheckersGS> createdNode = new MiniMaxNode<CheckersGS>(roots[i].gameState.BlankState);

                        createdNode.gameState.checkers.AddRange(roots[i].gameState.checkers);
                        Checker checker = createdNode.gameState.checkers[i];
                        checker.X++;
                        checker.Y--;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = roots[i].gameState.Xer;
                        createdNode.gameState.gameFinished = roots[i].gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = roots[i].gameState.Xer;

                        unvisitedNodes.Add((createdNode, i));
                    }
                }
            }

            for (int i = roots.Count; i < carloNodes.Length; i++)
            {
                int rand = random.Next(0, unvisitedNodes.Count);
                if(unvisitedNodes.Count == 0)
                {
                    break;
                }
                (MiniMaxNode<CheckersGS> node, int parentIndex) currentNode = unvisitedNodes[rand];
                unvisitedNodes.RemoveAt(rand);
                CheckersGS currentState = currentNode.node.gameState.BlankState;
                currentState.checkers.AddRange(currentNode.node.gameState.checkers);


                int[,] checkerMarks = new int[8, 8];
                foreach (Checker checker in currentState.checkers)
                {
                    checkerMarks[checker.X, checker.Y] = checker.Xer ? 1 : -1;
                }
                //Simulation
                while (!currentState.gameFinished)
                {
                    //play random moves on currentState
                    int randIndex = random.Next(0, currentState.checkers.Count);
                    if (currentState.checkers[randIndex].Xer != currentState.Xer) { continue; }

                    randIndex = random.Next(0, 4);

                    if(randIndex == 0)
                    {
                        if (currentState.checkers[i].CheckUpLeft(checkerMarks))
                        {
                            Checker checker = currentState.checkers[i];
                            checker.X--;
                            checker.Y++;
                            currentState.checkers[i] = checker;
                            
                            currentState.gameFinished = currentState.EvaluateVictory(currentState);
                            currentState.XerVictory = roots[i].gameState.Xer;

                            currentState.Xer = !currentState.Xer;
                        }
                    }
                    else if(randIndex == 1)
                    {
                        if (currentState.checkers[i].CheckUpRight(checkerMarks))
                        {
                            Checker checker = currentState.checkers[i];
                            checker.X++;
                            checker.Y++;
                            currentState.checkers[i] = checker;

                            currentState.gameFinished = currentState.EvaluateVictory(currentState);
                            currentState.XerVictory = roots[i].gameState.Xer;

                            currentState.Xer = !currentState.Xer;
                        }
                    }
                    else if(randIndex == 2)
                    {
                        if (currentState.checkers[i].CheckDownRight(checkerMarks))
                        {
                            Checker checker = currentState.checkers[i];
                            checker.X++;
                            checker.Y--;
                            currentState.checkers[i] = checker;

                            currentState.gameFinished = currentState.EvaluateVictory(currentState);
                            currentState.XerVictory = roots[i].gameState.Xer;

                            currentState.Xer = !currentState.Xer;
                        }
                    }
                    else
                    {
                        if (currentState.checkers[i].CheckDownLeft(checkerMarks))
                        {
                            Checker checker = currentState.checkers[i];
                            checker.X--;
                            checker.Y--;
                            currentState.checkers[i] = checker;

                            currentState.gameFinished = currentState.EvaluateVictory(currentState);
                            currentState.XerVictory = roots[i].gameState.Xer;

                            currentState.Xer = !currentState.Xer;
                        }
                    }
                }


                //Fake Nodes
                checkerMarks = new int[8, 8];
                foreach (Checker checker in currentNode.node.gameState.checkers)
                {
                    checkerMarks[checker.X, checker.Y] = checker.Xer ? 1 : -1;
                }
                for (int c = 0; c < currentNode.node.gameState.checkers.Count; c++)
                {
                    if (currentNode.node.gameState.checkers[c].Xer != currentNode.node.gameState.Xer) continue;

                    MiniMaxNode<CheckersGS> createdNode = new MiniMaxNode<CheckersGS>(currentState.BlankState);
                    createdNode.gameState.checkers.AddRange(currentNode.node.gameState.checkers);

                    if (roots[i].gameState.checkers[i].CheckUpLeft(checkerMarks))
                    {
                        Checker checker = createdNode.gameState.checkers[c];
                        checker.X--;
                        checker.Y++;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = !currentNode.node.gameState.Xer;

                        createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                        unvisitedNodes.Add((createdNode, currentNode.parentIndex));
                    }
                    if (roots[i].gameState.checkers[i].CheckUpRight(checkerMarks))
                    {
                        Checker checker = createdNode.gameState.checkers[c];
                        checker.X++;
                        checker.Y++;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = !currentNode.node.gameState.Xer;

                        createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                        unvisitedNodes.Add((createdNode, currentNode.parentIndex));
                    }
                    if (roots[i].gameState.checkers[i].CheckDownRight(checkerMarks))
                    {
                        Checker checker = createdNode.gameState.checkers[c];
                        checker.X++;
                        checker.Y--;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = !currentNode.node.gameState.Xer;

                        createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                        unvisitedNodes.Add((createdNode, currentNode.parentIndex));
                    }
                    if (roots[i].gameState.checkers[i].CheckDownLeft(checkerMarks))
                    {
                        Checker checker = createdNode.gameState.checkers[c];
                        checker.X--;
                        checker.Y--;
                        createdNode.gameState.checkers[i] = checker;

                        createdNode.gameState.Xer = !currentNode.node.gameState.Xer;

                        createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState);
                        createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                        unvisitedNodes.Add((createdNode, currentNode.parentIndex));
                    }
                }


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
