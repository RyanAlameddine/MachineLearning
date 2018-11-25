using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public abstract class GameManager<T> where T : GameState<T>
    {
        public abstract void GenerateTree(MiniMaxNode<T> root, bool maximizer);

        public abstract void CalculateTree(MiniMaxNode<T> root, bool maximizer);
    }
}
