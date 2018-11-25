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
                    return;
                }

                
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
    }
}
