using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    class TicTacGameState : GameState<TicTacGameState>
    {
        public override TicTacGameState BlankState {
            get
            {
                return new TicTacGameState();
            }
        }

        public char[,] marks = new char[3, 3];
        public bool XerVictory = false;
        public bool Tie = false;

        public override void GetPossibleStates(bool Xer, List<TicTacGameState> states)
        {
            states.Clear();

            for(int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (marks[x, y] != '\0')
                    {
                        continue;
                    }

                    var state = new TicTacGameState();
                    states.Add(state);
                    for (int ix = 0; ix < 3; ix++)
                    {
                        for (int iy = 0; iy < 3; iy++)
                        {
                            state.marks[ix, iy] = marks[ix, iy];
                        }
                    }

                    if (Xer)
                    {
                        state.marks[x, y] = 'X';
                    }
                    else
                    {
                        state.marks[x, y] = 'O';
                    }
                    state.XerVictory = Xer;
                    state.gameFinished = EvaluateVictory(state, x, y);
                    
                }
            }
            if(!gameFinished && states.Count == 0)
            {
                gameFinished = true;
                Tie = true;
            }
        }

        bool EvaluateVictory(TicTacGameState gameState, int x, int y)
        {
            if (gameState.marks[x, 0] == gameState.marks[x, 1] && gameState.marks[x, 1] == gameState.marks[x, 2]) return true;
            if (gameState.marks[0, y] == gameState.marks[1, y] && gameState.marks[1, y] == gameState.marks[2, y]) return true;
            if (gameState.marks[0, 0] == gameState.marks[1, 1] && gameState.marks[1, 1] == gameState.marks[2, 2] && gameState.marks[2, 2] != '\0') return true;
            if (gameState.marks[2, 0] == gameState.marks[1, 1] && gameState.marks[1, 1] == gameState.marks[0, 2] && gameState.marks[0, 2] != '\0') return true;

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    stringBuilder.Append(marks[x, y] == '\0' ? "# " : marks[x, y] + " ");
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
    }
}
