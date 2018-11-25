using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    class TicTacGameManager : GameManager<TicTacGameState>
    {
        public override void CalculateTree(MiniMaxNode<TicTacGameState> root, bool maximizer)
        {

            if(root.children == null || root.children.Length == 0)
            {
                if(((TicTacGameState)root.gameState).Tie)
                {
                    root.Value = 0;
                }
                else if (((TicTacGameState)root.gameState).XerVictory)
                {
                    root.Value = 1;
                }
                else
                {
                    root.Value = -1;
                }
                return;
            }

            CalculateTree(root.children[0], !maximizer);
            root.Value = root.children[0].Value;
            foreach(MiniMaxNode<TicTacGameState> child in root.children)
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

        public override void GenerateTree(MiniMaxNode<TicTacGameState> root, bool maximizer)
        {
            Stack<(bool maximizer, MiniMaxNode<TicTacGameState> node)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode<TicTacGameState> node)>();

            List<TicTacGameState> states = new List<TicTacGameState>();

            miniMaxNodes.Push((maximizer, root));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode<TicTacGameState> node) currentNode = miniMaxNodes.Pop();
                currentNode.node.gameState.GetPossibleStates(currentNode.maximizer, states);
                currentNode.node.children = new MiniMaxNode<TicTacGameState>[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode<TicTacGameState>(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i]));
                    }
                }
            }
        }
    }
}
