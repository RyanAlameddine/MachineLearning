using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    class TicTacGameState : GameState
    {
        public override GameState BlankState {
            get
            {
                return new TicTacGameState();
            }
        }

        public char[,] marks = new char[3, 3];
        public bool XerVictory = false;
        public bool Tie = false;

        public override GameState[] getPossibleStates(bool Xer)
        {
            LinkedList<TicTacGameState> states = new LinkedList<TicTacGameState>();

            for(int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (marks[x, y] != '\0')
                    {
                        continue;
                    }

                    states.AddFirst(new TicTacGameState());
                    for (int ix = 0; ix < 3; ix++)
                    {
                        for (int iy = 0; iy < 3; iy++)
                        {
                            states.First.Value.marks[ix, iy] = marks[ix, iy];
                        }
                    }

                    if (Xer)
                    {
                        states.First.Value.marks[x, y] = 'X';
                    }
                    else
                    {
                        states.First.Value.marks[x, y] = 'O';
                    }
                    states.First.Value.gameFinished = EvaluateVictory(states.First.Value, x, y);
                    states.First.Value.XerVictory = Xer;
                    
                }
            }
            if(!gameFinished && states.Count == 0)
            {
                gameFinished = true;
                Tie = true;
            }

            return states.ToArray();
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
