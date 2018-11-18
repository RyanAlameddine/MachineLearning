using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public abstract class GameState
    {
        public bool gameFinished = false;
        public abstract GameState BlankState { get; }

        public abstract GameState[] getPossibleStates(bool maximizer);

        public override abstract string ToString();
    }
}
