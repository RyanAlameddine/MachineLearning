using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public class MiniMaxTree<T> where T : GameState<T>
    {
        public GameManager<T> GameManager;

        public MiniMaxNode<T> Root;

        public MiniMaxTree(GameManager<T> GameManager, bool maximizerFirst, T state)
        {
            this.GameManager = GameManager;

            Root = new MiniMaxNode<T>(state);
        }
    }

    public class MiniMaxNode<T> where T : GameState<T>
    {
        public MiniMaxNode<T>[] children;
        public T gameState;
        public double Value;
        public bool pruned = false;

        public MiniMaxNode(T gameState)
        {
            this.gameState = gameState;
        }


    }
}



