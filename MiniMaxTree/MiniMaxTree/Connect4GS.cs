using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    class Connect4GS : GameState<Connect4GS>
    {
        public override Connect4GS BlankState {
            get
            {
                return new Connect4GS();
            }
        }

        public char[,] marks = new char[7, 6];
        public bool XerVictory = false;
        public bool Tie = false;

        public override void GetPossibleStates(bool Xer, List<Connect4GS> states)
        {
            states.Clear();
            for(int x = 0; x < 7; x++)
            {
                if (marks[x, 5] != '\0')
                {
                    continue;
                }

                Connect4GS state = new Connect4GS();
                states.Add(state);
                int targetY = 0;
                for (int ix = 0; ix < 7; ix++)
                {
                    for (int iy = 0; iy < 6; iy++)
                    {
                        if(ix == x)
                        {
                            if (marks[ix, iy] != '\0')
                            {
                                targetY = iy + 1;
                            }
                        }
                        state.marks[ix, iy] = marks[ix, iy];
                    }
                }

                if (Xer)
                {
                    state.marks[x, targetY] = 'X';
                }
                else
                {
                    state.marks[x, targetY] = 'O';
                }
                state.gameFinished = EvaluateVictory(state, x, targetY);
                state.XerVictory = Xer;
                
            }
            if(!gameFinished && states.Count == 0)
            {
                gameFinished = true;
                Tie = true;
            }
            ;
        }

        bool EvaluateVictory(Connect4GS gameState, int x, int y)
        {

            //-
            //-
            //-
            //-
            if (y >= 4 && gameState.marks[x, y] == gameState.marks[x, y - 1] && gameState.marks[x, y] == gameState.marks[x, y - 2] && gameState.marks[x, y] == gameState.marks[x, y - 4]) return true;

            //-----
            int matchXL = x;
            int matchXR = x;
            while (matchXL - 1 >= 0 && gameState.marks[matchXL - 1, y] == gameState.marks[x, y]) matchXL--;
            while (matchXR + 1 >  7 && gameState.marks[matchXR + 1, y] == gameState.marks[x, y]) matchXR++;
            if (matchXR - matchXL >= 4) return true;

            //-
            // -
            //  -
            //   -
            int matchYL = y;
            int matchYR = y;
            matchXL = x;
            matchXR = x;
            while (matchXL - 1 >= 0 && matchYL + 1 < 6 && gameState.marks[matchXL - 1, matchYL + 1] == gameState.marks[x, y])
            {
                matchXL--;
                matchYL++;
            }
            while (matchXR + 1 < 7 && matchYR - 1 >= 0 && gameState.marks[matchXR + 1, matchYR - 1] == gameState.marks[x, y])
            {
                matchXR++;
                matchYR--;
            }
            if (matchXR - matchXL >= 4) return true;

            //-
            // -
            //  -
            //   -
            matchYL = y;
            matchYR = y;
            matchXL = x;
            matchXR = x;
            while (matchXL - 1 >= 0 && matchYL - 1 >= 0 && gameState.marks[matchXL - 1, matchYL - 1] == gameState.marks[x, y])
            {
                matchXL--;
                matchYL--;
            }
            while (matchXR + 1 < 7 && matchYR + 1 < 6 && gameState.marks[matchXR + 1, matchYR + 1] == gameState.marks[x, y])
            {
                matchXR++;
                matchYR++;
            }
            if (matchXR - matchXL >= 4) return true;

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; marks[i, j] != '\0'; j++)
                {
                    stringBuilder.Append(marks[i, j]);
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
    }
}
