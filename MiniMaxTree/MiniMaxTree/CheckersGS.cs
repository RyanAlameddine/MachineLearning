using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{
    unsafe struct ChxMarks
    {
        fixed char data[8 * 8];

        public char this[int x, int y]
        {
            get
            {
                return data[(x * 8) + y];
            }
            set
            {
                data[(x * 8) + y] = value;
            }
        }

        public char this[int i]
        {
            get
            {
                return data[i];
            }
            set
            {
                data[i] = value;
            }
        }
    }

    class Checker
    {
        public Checker next;
        public int x;
        public int y;
        public bool upgraded;
        public char mark;

        public Checker(int x, int y, char mark)
        {
            this.x = x;
            this.y = y;
            upgraded = mark == 'X' || mark == 'O';
            this.mark = mark;
        }
    }

    class CheckersGS : GameState<CheckersGS>
    {
        public override CheckersGS BlankState
        {
            get
            {
                return new CheckersGS();
            }
        }

        public ChxMarks marks = new ChxMarks();
        public bool XerVictory = false;
        public bool Tie = false;
        public bool Xer = false;
        public bool doubleJ;

        public CheckersGS()
        {

        }

        public CheckersGS(bool Xer)
        {
            for (int x = 1; x < 8; x += 2)
            {
                marks[x, 0] = 'o';
            }
            for (int x = 0; x < 8; x += 2)
            {
                marks[x, 1] = 'o';
            }
            for (int x = 1; x < 8; x += 2)
            {
                marks[x, 2] = 'o';
            }

            for (int x = 0; x < 8; x += 2)
            {
                marks[x, 7] = 'x';
            }
            for (int x = 1; x < 8; x += 2)
            {
                marks[x, 6] = 'x';
            }
            for (int x = 0; x < 8; x += 2)
            {
                marks[x, 5] = 'x';
            }
        }


        public override void GetPossibleStates(bool Xer, List<CheckersGS> states)
        {
            //Invert Xer
            //this.Xer = !Xer;
            states.Clear();
            //For each possible turn, copy marks into a new state and evaluate victory

            Checker root = null;
            Checker currentChecker = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if(marks[x, y] == (Xer ? 'x' : 'o') || marks[x, y] == (Xer ? 'X' : 'O'))
                    {
                        if(root == null)
                        {
                            root = new Checker(x, y, marks[x, y]);
                            currentChecker = root;
                            continue;
                        }
                        currentChecker.next = new Checker(x, y, marks[x, y]);
                        currentChecker = currentChecker.next;
                    }
                }
            }

            currentChecker = root;
            
            for (; currentChecker != null; currentChecker = currentChecker.next)
            {
                states.AddRange(GetMoves(Xer, currentChecker));
            }
            //check ties?
        }
        
        public List<CheckersGS> GetMoves(bool Xer, Checker checker)
        {
            List<CheckersGS> states = new List<CheckersGS>();

            int currentX = checker.x;
            int currentY = checker.y;

            //UpRight
            if ((!Xer || checker.upgraded) && currentX + 1 < 8 && currentY + 1 < 8)
            {
                states.AddRange(GetMove(checker, marks, currentX, currentY, currentX + 1, currentY + 1, Xer));
            }

            //UpLeft
            if ((!Xer || checker.upgraded) && currentX - 1 >= 0 && currentY + 1 < 8)
            {
                states.AddRange(GetMove(checker, marks, currentX, currentY, currentX - 1, currentY + 1, Xer));
            }

            //DownRight
            if ((Xer || checker.upgraded) && currentX + 1 < 8 && currentY - 1 >= 0)
            {
                states.AddRange(GetMove(checker, marks, currentX, currentY, currentX + 1, currentY - 1, Xer));
            }

            //DownLeft
            if ((Xer || checker.upgraded) && currentX - 1 >= 0 && currentY - 1 >= 0)
            {
                states.AddRange(GetMove(checker, marks, currentX, currentY, currentX - 1, currentY - 1, Xer));
            }

            return states;
        }

        public double[] toDoubles()
        {
            double[] markdoubles = new double[33];
            string compact = ToCompact();
            
            markdoubles = new double[33];
            for (int j = 0; j < compact.Length; j++)
            {
                char c = compact[j];
                if      (c == 'O') markdoubles[j] = -2;
                else if (c == 'o') markdoubles[j] = -1;
                else if (c == ' ') markdoubles[j] = 0;
                else if (c == 'x') markdoubles[j] = 1;
                else if (c == 'X') markdoubles[j] = 2;
            }
            return markdoubles;
        }

        public List<CheckersGS> GetMove(Checker checker, ChxMarks chxMrkx, int currentX, int currentY, int newX, int newY, bool isXer)
        {
            List<CheckersGS> states = new List<CheckersGS>();

            //Friendly piece
            if (Char.ToLower(chxMrkx[newX, newY]) == Char.ToLower(checker.mark))
            {
                return states;
            }

            //No piece
            if (chxMrkx[newX, newY] == '\0')
            {
                CheckersGS state = new CheckersGS();
                states.Add(state);
                //copy marks
                state.marks = chxMrkx;

                state.Xer = isXer;
                state.marks[newX, newY] = state.marks[currentX, currentY];
                state.marks[currentX, currentY] = '\0';
                state.gameFinished = EvaluateVictory(state, newX, newY);
                return states;
            }

            int doubleX = newX - currentX + newX;
            int doubleY = newY - currentY + newY;

            if(doubleX < 0 || doubleX > 7 || doubleY < 0 || doubleY > 7)
            {
                return states;
            }

            //Enemy's piece
            if (Char.ToLower(chxMrkx[doubleX, doubleY]) != '\0')
            {
                return states;
            }
            else
            {
                CheckersGS state = new CheckersGS();
                state.doubleJ = true;
                states.Add(state);
                doubleJ = true;
                //copy marks
                state.marks = chxMrkx;

                state.Xer = isXer;
                state.marks[doubleX, doubleY] = state.marks[currentX, currentY];
                state.marks[currentX, currentY] = '\0';
                state.marks[newX, newY] = '\0';
                state.gameFinished = EvaluateVictory(state, newX, newY);


                currentX = doubleX;
                currentY = doubleY;
                isXer = !isXer;
                //UpRight
                if ((!isXer || checker.upgraded) && currentX + 1 < 8 && currentY + 1 < 8)
                {
                    states.AddRange(GetMove(checker, state.marks, currentX, currentY, currentX + 1, currentY + 1, isXer));
                }

                //UpLeft
                if ((!isXer || checker.upgraded) && currentX - 1 >= 0 && currentY + 1 < 8)
                {
                    states.AddRange(GetMove(checker, state.marks, currentX, currentY, currentX - 1, currentY + 1, isXer));
                }

                //DownRight
                if ((isXer || checker.upgraded) && currentX + 1 < 8 && currentY - 1 >= 0)
                {
                    states.AddRange(GetMove(checker, state.marks, currentX, currentY, currentX + 1, currentY - 1, isXer));
                }

                //DownLeft
                if ((isXer || checker.upgraded) && currentX - 1 >= 0 && currentY - 1 >= 0)
                {
                    states.AddRange(GetMove(checker, state.marks, currentX, currentY, currentX - 1, currentY - 1, isXer));
                }

                for(int i = 0; i < states.Count; i++)
                {
                    if (!states[i].doubleJ)
                    {
                        states.RemoveAt(i);
                        i--;
                    }
                }

                return states;
            }
        }

        public bool EvaluateVictory(CheckersGS gameState, int x, int y)
        {
            int xCount = 0;
            int oCount = 0;
            for (int ix = 0; ix < 8; ix++)
            {
                for (int iy = 0; iy < 8; iy++)
                {
                    char chr = Char.ToLower(gameState.marks[ix, iy]);
                    if (chr == 'x')
                    {
                        if(iy == 0)
                        {
                            gameState.marks[ix, iy] = 'X';
                        }
                        xCount++;
                    }
                    else if (chr == 'o')
                    {
                        if (iy == 7)
                        {
                            gameState.marks[ix, iy] = 'O';
                        }
                        oCount++;
                    }
                }
            }

            if(xCount == 0)
            {
                XerVictory = false;
                gameFinished = true;
                return true;
            }
            if(oCount == 0)
            {
                XerVictory = true;
                gameFinished = true;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (marks[i, j] == '\0') stringBuilder.Append(' ');
                    stringBuilder.Append(marks[i, j]);
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
        public string ToCompact()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (Xer)
            {
                stringBuilder.Append('x');
            }
            else
            {
                stringBuilder.Append('o');
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 1; x < 8; x += 2)
                {
                    if (marks[x, y] == '\0')
                    {
                        stringBuilder.Append(' ');
                        continue;
                    }
                    stringBuilder.Append(marks[x, y]);
                }
                y++;
                for (int x = 0; x < 8; x += 2)
                {
                    if (marks[x, y] == '\0')
                    {
                        stringBuilder.Append(' ');
                        continue;
                    }
                    stringBuilder.Append(marks[x, y]);
                }
            }
            return stringBuilder.ToString();
        }

        public void ConsoleWrite(bool test = false)
        {
            if (!test)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("------------------");
                for (int y = 7; y >= 0; y--)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('|');
                        if (marks[x, y] == 'X' || marks[x, y] == 'x')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(marks[x, y]);
                        } 
                        else if (marks[x, y] == 'O' || marks[x, y] == 'o')
                        {

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(marks[x, y]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(' ');
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('|');
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(y + 1);
                    Console.Write('|');
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('\n');

                    Console.WriteLine("------------------");
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("|1|2|3|4|5|6|7|8|");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("------------------");
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

                Console.WriteLine("------------------");
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("|1|2|3|4|5|6|7|8|");
        }
    }
}
