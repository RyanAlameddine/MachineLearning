using System;
using System.Collections.Generic;

namespace MiniMaxTree
{
    class Connect4GM : GameManager<Connect4GS>
    {
        public override void CalculateTree(MiniMaxNode<Connect4GS> root, bool maximizer)
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
            foreach(MiniMaxNode<Connect4GS> child in root.children)
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

        public override void GenerateTree(MiniMaxNode<Connect4GS> root, bool maximizer)
        {
            Stack<(bool maximizer, MiniMaxNode<Connect4GS> node)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode<Connect4GS> node)>();

            List<Connect4GS> states = new List<Connect4GS>(); 

            miniMaxNodes.Push((maximizer, root));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode<Connect4GS> node) currentNode = miniMaxNodes.Pop();
                currentNode.node.gameState.GetPossibleStates(currentNode.maximizer, states);
                currentNode.node.children = new MiniMaxNode<Connect4GS>[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode<Connect4GS>(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i]));
                    }
                }
            }
        }

        public void GenerateTree(MiniMaxNode<Connect4GS> root, bool maximizer, int layerDepth)
        {
            Stack<(bool maximizer, MiniMaxNode<Connect4GS> node, int depth)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode<Connect4GS> node, int depth)>();
            List<Connect4GS> states = new List<Connect4GS>(layerDepth * 8);

            miniMaxNodes.Push((maximizer, root, 0));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode<Connect4GS> node, int depth) currentNode = miniMaxNodes.Pop();
                if(currentNode.depth == layerDepth)
                {
                    continue;
                }
                currentNode.node.gameState.GetPossibleStates(currentNode.maximizer, states);
                currentNode.node.children = new MiniMaxNode<Connect4GS>[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode<Connect4GS>(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i], currentNode.depth + 1));
                    }
                }
            }
        }

        private int loops = 0;

        private double AlphaBetaMonteCarlo(MiniMaxNode<Connect4GS> root, bool maximizer, double alpha, double beta)
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

        public double AlphaBetaMonteCarlo(MiniMaxNode<Connect4GS> root, bool maximizer)
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

        public void MonteCarlo(MiniMaxNode<Connect4GS> leaf, bool maximizer)
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
            

            List<MiniMaxNode<Connect4GS>> roots = new List<MiniMaxNode<Connect4GS>>();
            roots.Add(leaf);

            Random random = new Random();
            var carloNodes = new (int wins, MiniMaxNode<Connect4GS> node)[1000];
            var unvisitedNodes = new List<(MiniMaxNode<Connect4GS> node, int parentIndex)>();

            for(int i = 0; i < roots.Count; i++)
            {
                carloNodes[i] = (0, roots[i]);


                for (int slot = 0; slot < 7; slot++)
                {
                    if (roots[i].gameState.marks[slot, 5] != '\0')
                    {
                        continue;
                    }
                    else
                    {
                        MiniMaxNode<Connect4GS> createdNode = new MiniMaxNode<Connect4GS>(roots[i].gameState.BlankState);
                        for (int x = 0; x < 7; x++)
                        {
                            for (int y = 0; y < 6; y++)
                            {
                                createdNode.gameState.marks[x, y] = roots[i].gameState.marks[x, y];
                            }
                        }

                        for (int j = 0; j < 6; j++)
                        {
                            if (createdNode.gameState.marks[slot, j] != '\0')
                            {
                                continue;
                            }
                            createdNode.gameState.marks[slot, j] = !roots[i].gameState.Xer ? 'X' : 'O';
                            createdNode.gameState.Xer = !roots[i].gameState.Xer;

                            createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState, slot, j);
                            createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                            break;
                        }

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
                (MiniMaxNode<Connect4GS> node, int parentIndex) currentNode = unvisitedNodes[rand];
                unvisitedNodes.RemoveAt(rand);
                Connect4GS currentState = currentNode.node.gameState.BlankState;
                for(int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        currentState.marks[x, y] = currentNode.node.gameState.marks[x, y];
                    }
                }

                //Simulation
                while (!currentState.gameFinished)
                {
                    int slot = random.Next(0, 7);
                    if (currentState.marks[slot, 5] != '\0')
                    {
                        if(currentState.marks[0, 5] != '\0' && currentState.marks[1, 5] != '\0' && currentState.marks[2, 5] != '\0' && currentState.marks[3, 5] != '\0' && currentState.marks[4, 5] != '\0' && currentState.marks[5, 5] != '\0' && currentState.marks[6, 5] != '\0')
                        {
                            currentState.gameFinished = true;
                            currentState.Tie = true;
                        }
                        continue;
                    }

                    for (int j = 0; j < 6; j++)
                    {
                        if (currentState.marks[slot, j] != '\0')
                        {
                            continue;
                        }
                        currentState.marks[slot, j] = currentState.Xer ? 'X' : 'O';

                        currentState.gameFinished = currentState.EvaluateVictory(currentState, slot, j);
                        currentState.XerVictory = currentState.Xer;
                        currentState.Xer = !currentState.Xer;

                        break;
                    }
                }

                //Fake Nodes
                for(int slot = 0; slot < 7; slot++)
                {
                    if (currentNode.node.gameState.marks[slot, 5] != '\0')
                    {
                        continue;
                    }
                    else
                    {
                        MiniMaxNode<Connect4GS> createdNode = new MiniMaxNode<Connect4GS>(currentState.BlankState);
                        for (int x = 0; x < 7; x++)
                        {
                            for (int y = 0; y < 6; y++)
                            {
                                createdNode.gameState.marks[x, y] = currentNode.node.gameState.marks[x, y];
                            }
                        }

                        for (int j = 0; j < 6; j++)
                        {
                            if (createdNode.gameState.marks[slot, j] != '\0')
                            {
                                continue;
                            }
                            createdNode.gameState.marks[slot, j] = !currentNode.node.gameState.Xer ? 'X' : 'O';
                            createdNode.gameState.Xer = !currentNode.node.gameState.Xer;

                            createdNode.gameState.gameFinished = createdNode.gameState.EvaluateVictory(createdNode.gameState, slot, j);
                            createdNode.gameState.XerVictory = createdNode.gameState.Xer;

                            break;
                        }

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
