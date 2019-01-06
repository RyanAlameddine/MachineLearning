using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    unsafe struct C4Marks
    {
        fixed char data[7 * 6];

        public char this[int x, int y]
        {
            get
            {
                return data[(x * 7) + y];
            }
            set
            {
                data[(x * 7) + y] = value;
            }
        }
    }


    class Connect4GS : GameState<Connect4GS>
    {
        public override Connect4GS BlankState {
            get
            {
                return new Connect4GS();
            }
        }

        public C4Marks marks = new C4Marks();
        public bool XerVictory = false;
        public bool Tie = false;
        public bool Xer = false;

        public override void GetPossibleStates(bool Xer, List<Connect4GS> states)
        {
            this.Xer = !Xer;
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

                state.Xer = Xer;
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
        }

        public bool EvaluateVictory(Connect4GS gameState, int x, int y)
        {
            //-
            //-
            //-
            //-
            if (y >= 3 && gameState.marks[x, y] == gameState.marks[x, y - 1] && gameState.marks[x, y] == gameState.marks[x, y - 2] && gameState.marks[x, y] == gameState.marks[x, y - 3]) return true;

            //-----
            int matchXL = x;
            int matchXR = x;
            while (matchXL - 1 >= 0 && gameState.marks[matchXL - 1, y] == gameState.marks[x, y]) matchXL--;
            while (matchXR + 1 <  7 && gameState.marks[matchXR + 1, y] == gameState.marks[x, y]) matchXR++;
            if (matchXR - matchXL >= 3) return true;

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
            if (matchXR - matchXL >= 3) return true;

            //   -
            //  -
            // -
            //-
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
            if (matchXR - matchXL >= 3) return true;

            if (gameState.marks[0, 5] != '\0' && gameState.marks[1, 5] != '\0' && gameState.marks[2, 5] != '\0' && gameState.marks[3, 5] != '\0' && gameState.marks[4, 5] != '\0' && gameState.marks[5, 5] != '\0' && gameState.marks[6, 5] != '\0')
            {
                //Tie
                gameState.Tie = true;
                gameState.gameFinished = true;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    if (marks[i, j] == '\0') continue;
                    stringBuilder.Append(marks[i, j]);
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }

        public void ConsoleWrite(bool test = false)
        {
            if (!test)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("---------------");
                for (int y = 5; y >= 0; y--)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('|');
                        if (marks[x, y] == 'X')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('X');
                        }
                        else if (marks[x, y] == 'O')
                        {

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write('O');
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(' ');
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('|');
                    Console.Write('\n');

                    Console.WriteLine("---------------");
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("|1|2|3|4|5|6|7|");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("---------------");
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write('|');
                    if (marks[x, y] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write('X');
                    }
                    else if (marks[x, y] == 'O')
                    {

                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write('O');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(' ');
                    }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write('|');
                Console.Write('\n');

                Console.WriteLine("---------------");
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("|1|2|3|4|5|6|7|");
        }
    }
}
