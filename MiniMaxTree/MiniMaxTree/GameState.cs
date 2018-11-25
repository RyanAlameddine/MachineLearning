using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaxTree
{
    public abstract class GameState<T>  where T : GameState<T>
    {
        public bool gameFinished = false;
        public abstract T BlankState { get; }

        public abstract void GetPossibleStates(bool maximizer, List<T> store);

        public override abstract string ToString();
    }
}
