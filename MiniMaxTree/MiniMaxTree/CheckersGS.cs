using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMaxTree
{

    struct Checker
    {
        public byte X;
        public byte Y;
        public bool Xer;
        public bool Upgraded;

        public Checker(byte X, byte Y, bool Xer)
        {
            this.X = X;
            this.Y = Y;
            this.Xer = Xer;
            Upgraded = false;
        }

        public bool CheckUpLeft(int[,] checkerMarks)
        {
            if (!Xer)
            {
                if(X > 0 && Y < 7)
                {
                    if (checkerMarks[X - 1, Y + 1] == 0)
                    {
                        return true;
                    }
                }
            }else if (Upgraded)
            {
                if (X > 0 && Y < 7)
                {
                    if (checkerMarks[X - 1, Y + 1] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckUpRight(int[,] checkerMarks)
        {
            if (!Xer)
            {
                if (X < 7 && Y < 7)
                {
                    if (checkerMarks[X + 1, Y + 1] == 0)
                    {
                        return true;
                    }
                }
            }
            else if (Upgraded)
            {
                if (X < 7 && Y < 7)
                {
                    if (checkerMarks[X + 1, Y + 1] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckDownRight(int[,] checkerMarks)
        {
            if (Xer)
            {
                if (X < 7 && Y > 0)
                {
                    if (checkerMarks[X + 1, Y - 1] == 0)
                    {
                        return true;
                    }
                }
            }
            else if (Upgraded)
            {
                if (X < 7 && Y > 0)
                {
                    if (checkerMarks[X + 1, Y - 1] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckDownLeft(int[,] checkerMarks)
        {
            if (Xer)
            {
                if (X > 0 && Y > 0)
                {
                    if (checkerMarks[X - 1, Y - 1] == 0)
                    {
                        return true;
                    }
                }
            }
            else if (Upgraded)
            {
                if (X > 0 && Y > 0)
                {
                    if (checkerMarks[X - 1, Y - 1] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class CheckersGS : GameState<CheckersGS>
    {
        public override CheckersGS BlankState {
            get
            {
                return new CheckersGS();
            }
        }

        public List<Checker> checkers = new List<Checker>();
        int[,] checkerMarks = new int[8, 8];
        public bool XerVictory = false;
        public bool Tie = false;
        public bool Xer = false;


        public override void GetPossibleStates(bool Xer, List<CheckersGS> states)
        {
            this.Xer = !Xer;
            states.Clear();
            //TODO DELETE THIS 
            if(checkers.Count == 0)
            {
                throw new Exception("Failed");
            }

            foreach(Checker checker in checkers)
            {
                checkerMarks[checker.X, checker.Y] = checker.Xer ? 1 : -1;
            }
            for(int i = 0; i < checkers.Count; i++)
            {
                if(checkers[i].Xer != Xer)
                {
                    continue;
                }
                if (checkers[i].CheckUpLeft(checkerMarks))
                {
                    CheckersGS state = new CheckersGS();
                    states.Add(state);

                    state.checkers.AddRange(checkers);
                    Checker checker = state.checkers[i];
                    checker.X--;
                    checker.Y++;
                    state.checkers[i] = checker;

                    state.Xer = Xer;
                    state.gameFinished = EvaluateVictory(state);
                    state.XerVictory = Xer;
                }
                if (checkers[i].CheckUpRight(checkerMarks))
                {
                    CheckersGS state = new CheckersGS();
                    states.Add(state);

                    state.checkers.AddRange(checkers);
                    Checker checker = state.checkers[i];
                    checker.X++;
                    checker.Y++;
                    state.checkers[i] = checker;

                    state.Xer = Xer;
                    state.gameFinished = EvaluateVictory(state);
                    state.XerVictory = Xer;
                }
                if (checkers[i].CheckDownLeft(checkerMarks))
                {
                    CheckersGS state = new CheckersGS();
                    states.Add(state);

                    state.checkers.AddRange(checkers);
                    Checker checker = state.checkers[i];
                    checker.X--;
                    checker.Y--;
                    state.checkers[i] = checker;

                    state.Xer = Xer;
                    state.gameFinished = EvaluateVictory(state);
                    state.XerVictory = Xer;
                }
                if (checkers[i].CheckDownRight(checkerMarks))
                {
                    CheckersGS state = new CheckersGS();
                    states.Add(state);

                    state.checkers.AddRange(checkers);
                    Checker checker = state.checkers[i];
                    checker.X++;
                    checker.Y--;
                    state.checkers[i] = checker;

                    state.Xer = Xer;
                    state.gameFinished = EvaluateVictory(state);
                    state.XerVictory = Xer;
                }
            }
            if(!gameFinished && states.Count == 0)
            {
                gameFinished = true;
                Tie = true;
            }
        }

        public bool EvaluateVictory(CheckersGS gameState)
        {
            int Xers = 0;
            int nXers = 0;
            foreach(Checker checker in gameState.checkers)
            {
                if (checker.Xer)
                {
                    Xers++;
                    continue;
                }
                nXers++;
            }

            if(Xers == 0 || nXers == 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            
            return stringBuilder.ToString();
        }

        public void ConsoleWrite(bool test = false)
        {
            char[,] marks = new char[8, 8];
            foreach(Checker checker in checkers)
            {
                marks[checker.X, checker.Y] = checker.Xer ? 'X' : 'O';
            }
            if (!test)
            {
                for (int y = 7; y >= 0; y--)
                {
                    for (int x = 0; x < 8; x++)
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
                    
                }
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
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
                
            }
        }
    }
}
