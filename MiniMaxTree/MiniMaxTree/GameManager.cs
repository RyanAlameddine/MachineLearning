using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public abstract class GameManager
    {
        public abstract void GenerateTree(MiniMaxNode root, bool maximizer);

        public abstract void CalculateTree(MiniMaxNode root, bool maximizer);
    }
}
