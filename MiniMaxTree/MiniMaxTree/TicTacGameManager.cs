using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    class TicTacGameManager : GameManager
    {
        public override void CalculateTree(MiniMaxNode root, bool maximizer)
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
            foreach(MiniMaxNode child in root.children)
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

        public override void GenerateTree(MiniMaxNode root, bool maximizer)
        {
            Stack<(bool maximizer, MiniMaxNode node)> miniMaxNodes = new Stack<(bool maximizer, MiniMaxNode node)>();

            miniMaxNodes.Push((maximizer, root));
            while (miniMaxNodes.Count > 0)
            {
                (bool maximizer, MiniMaxNode node) currentNode = miniMaxNodes.Pop();
                GameState[] states = currentNode.node.gameState.getPossibleStates(currentNode.maximizer);
                currentNode.node.children = new MiniMaxNode[states.Length];
                for (int i = 0; i < states.Length; i++)
                {
                    currentNode.node.children[i] = new MiniMaxNode(states[i]);
                    if (!states[i].gameFinished)
                    {
                        miniMaxNodes.Push((!currentNode.maximizer, currentNode.node.children[i]));
                    }
                }
            }
        }
    }
}
