using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public class MiniMaxTree
    {
        public GameManager GameManager;

        public MiniMaxNode Root;

        public MiniMaxTree(GameManager GameManager, bool maximizerFirst)
        {
            this.GameManager = GameManager;

            Root = new MiniMaxNode(new TicTacGameState());
            GameManager.GenerateTree(Root, maximizerFirst);
            GameManager.CalculateTree(Root, true);
        }
    }

    public class MiniMaxNode
    {
        public MiniMaxNode[] children;
        public GameState gameState;
        public int Value;

        public MiniMaxNode(GameState gameState)
        {
            this.gameState = gameState;
        }


    }
}



