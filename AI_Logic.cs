using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    public partial class _2048
    {
        // list that stores all possible future moves
        List<int[,]> Future_moves = new();

        // simulates an up move and stores every possible result into the list
        private void Simulate_Up(int[,] curr_board, int Curr_board_space)
        {
            int[,] Move = new int[4, 4];

            // copies the present board to a new one for simulating
            Array.Copy(curr_board, Move, 16);

            if (!Is_up_valid(Move))
            {
                return;
            }

            // moves all the tiles
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (Move[i, j] != 0 && Move[i, j - 1] == 0)
                    {
                        Move[i, j - 1] = Move[i, j];
                        Move[i, j] = 0;
                        j = 0;
                    }
                }
            }

            //merger
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (Move[i, 0] == Move[i, 1] && Move[i, 0] != 0)
                {
                    Move[i, 0] *= 2;
                    Move[i, 1] = Move[i, 2];
                    Move[i, 2] = Move[i, 3];
                    Move[i, 3] = 0;
                    Curr_board_space += 1;
                }
                // merges middle two
                if (Move[i, 1] == Move[i, 2] && Move[i, 1] != 0)
                {
                    Move[i, 1] *= 2;
                    Move[i, 2] = Move[i, 3];
                    Move[i, 3] = 0;
                    Curr_board_space += 1;
                }
                // merges last two
                if (Move[i, 2] == Move[i, 3] && Move[i, 2] != 0)
                {
                    Move[i, 2] *= 2;
                    Move[i, 3] = 0;
                    Curr_board_space += 1;
                }
            }

            // simulates the various possible new tile spawns
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Move[i,j] == 0)
                    {
                        int[,] Move2 = new int[4, 4];
                        Array.Copy(Move, Move2, 16);
                        //int[,] Move4 = new int[4, 4];
                        //Array.Copy(Move, Move4, 16);

                        Move2[i, j] = 2;
                        //Move4[i, j] = 4;
                        Future_moves.Add(Move2);
                        //Future_moves.Add(Move4);
                    }
                }
            }
        }

        // simulates a down move and stores every possible result into the list
        private void Simulate_Down(int[,] curr_board, int Curr_board_space)
        {
            int[,] Move = new int[4, 4];

            // copies the present board to a new one for simulating
            Array.Copy(curr_board, Move, 16);

            if (!Is_down_valid(Move))
            {
                return;
            }

            // shifts every tile
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j > -1; j--)
                {
                    if (Move[i, j] != 0 && Move[i, j + 1] == 0)
                    {
                        Move[i, j + 1] = Move[i, j];
                        Move[i, j] = 0;
                        j = 3;
                    }
                }
            }

            //merge
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (Move[i, 3] == Move[i, 2] && Move[i, 3] != 0)
                {
                    Move[i, 3] *= 2;
                    Move[i, 2] = Move[i, 1];
                    Move[i, 1] = Move[i, 0];
                    Move[i, 0] = 0;
                    Curr_board_space += 1;
                }
                // merges middle two
                if (Move[i, 2] == Move[i, 1] && Move[i, 2] != 0)
                {
                    Move[i, 2] *= 2;
                    Move[i, 1] = Move[i, 0];
                    Move[i, 0] = 0;
                    Curr_board_space += 1;
                }
                // merges last two
                if (Move[i, 1] == Move[i, 0] && Move[i, 1] != 0)
                {
                    Move[i, 1] *= 2;
                    Move[i, 0] = 0;
                    Curr_board_space += 1;
                }
            }

            // simulates the various possible new tile spawns
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Move[i, j] == 0)
                    {
                        int[,] Move2 = new int[4, 4];
                        Array.Copy(Move, Move2, 16);
                        //int[,] Move4 = new int[4, 4];
                        //Array.Copy(Move, Move4, 16);

                        Move2[i, j] = 2;
                        //Move4[i, j] = 4;
                        Future_moves.Add(Move2);
                        //Future_moves.Add(Move4);
                    }
                }
            }
        }

        // simulates a left move and stores every possible result into the list
        private void Simulate_Left(int[,] curr_board, int Curr_board_space)
        {
            int[,] Move = new int[4, 4];

            // copies the present board to a new one for simulating
            Array.Copy(curr_board, Move, 16);

            if (!Is_left_valid(Move))
            {
                return;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (Move[i, j] != 0 && Move[i - 1, j] == 0)
                    {
                        Move[i - 1, j] = Move[i, j];
                        Move[i, j] = 0;
                        i = 0;
                    }
                }
            }

            // merges
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (Move[0, i] == Move[1, i] && Move[0, i] != 0)
                {
                    Move[0, i] *= 2;
                    Move[1, i] = Move[2, i];
                    Move[2, i] = Move[3, i];
                    Move[3, i] = 0;
                    Curr_board_space += 1;
                }
                // merges middle two
                if (Move[1, i] == Move[2, i] && Move[1, i] != 0)
                {
                    Move[1, i] *= 2;
                    Move[2, i] = Move[3, i];
                    Move[3, i] = 0;
                    Curr_board_space += 1;
                }
                // merges last two
                if (Move[2, i] == Move[3, i] && Move[2, i] != 0)
                {
                    Move[2, i] *= 2;
                    Move[3, i] = 0;
                    Curr_board_space += 1;
                }
            }

            // simulates the various possible new tile spawns
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Move[i, j] == 0)
                    {
                        int[,] Move2 = new int[4, 4];
                        Array.Copy(Move, Move2, 16);
                        //int[,] Move4 = new int[4, 4];
                        //Array.Copy(Move, Move4, 16);

                        Move2[i, j] = 2;
                        //Move4[i, j] = 4;
                        Future_moves.Add(Move2);
                        //Future_moves.Add(Move4);
                    }
                }
            }
        }

        // simulates a right move and stores every possible result into the list
        private void Simulate_Right(int[,] curr_board, int Curr_board_space)
        {
            
            int[,] Move = new int[4, 4];

            // copies the present board to a new one for simulating
            Array.Copy(curr_board, Move, 16);

            if (!Is_right_valid(Move))
            {
                return;
            }


                for (int j = 0; j < 4; j++)
            {
                for (int i = 2; i > -1; i--)
                {
                    if (Move[i, j] != 0 && Move[i + 1, j] == 0)
                    {
                        Move[i + 1, j] = Move[i, j];
                        Move[i, j] = 0;
                        i = 3;
                    }
                }
            }

            // merges any valid tiles
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (Move[3, i] == Move[2, i] && Move[3, i] != 0)
                {
                    Move[3, i] *= 2;
                    Move[2, i] = Move[1, i];
                    Move[1, i] = Move[0, i];
                    Move[0, i] = 0;
                    Curr_board_space += 1;
                }
                // merges middle two
                if (Move[2, i] == Move[1, i] && Move[2, i] != 0)
                {
                    Move[2, i] *= 2;
                    Move[1, i] = Move[0, i];
                    Move[0, i] = 0;
                    Curr_board_space += 1;
                }
                // merges last two
                if (Move[1, i] == Move[0, i] && Move[1, i] != 0)
                {
                    Move[1, i] *= 2;
                    Move[0, i] = 0;
                    Curr_board_space += 1;
                }
            }

            // simulates the various possible new tile spawns
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Move[i, j] == 0)
                    {
                        int[,] Move2 = new int[4, 4];
                        Array.Copy(Move, Move2, 16);
                        //int[,] Move4 = new int[4, 4];
                        //Array.Copy(Move, Move4, 16);

                        Move2[i, j] = 2;
                        //Move4[i, j] = 4;
                        Future_moves.Add(Move2);
                        //Future_moves.Add(Move4);
                    }
                }
            }
        }

        // returns the position of the highest value in the array in 1d form since I didn't want to make a struct
        private int Find_max(int[,] curr_board)
        {
            int max = curr_board[0, 0];
            int pos = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (curr_board[i,j] > max)
                    {
                        max = curr_board[i, j];
                        pos = 4 * j + i;
                    }
                }
            }
            return pos;
        }

        //calculates empty spaces on the board
        private int Find_empty_space(int[,] curr_board)
        {
            int space = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (curr_board[i,j] == 0)
                    {
                        space++;
                    }
                }
            }
            return space;
        }

        private int Assess()
        {
            int score = 0;

            for (int i = 0; i < Future_moves.Count(); i++)
            {
                // rewards the top score being in the top left corner
                if (Find_max(Future_moves[i]) == 0)
                {
                    score += 16 * Future_moves[i][Find_max(Future_moves[i])%4, Find_max(Future_moves[i])/4];
                }

                // rewards organizing tiles in a snake formation
                for (int j = 0; j < 3; j++)
                {
                    if (Future_moves[i][j, 0] == Future_moves[i][j+1, 0] * 2)
                    {
                        score += Future_moves[i][j+1, 0];
                    }
                }
                if (Future_moves[i][3,0] == Future_moves[i][3,1] * 2)
                {
                    score += Future_moves[i][3, 1];
                }

                for (int j = 0; j < 3; j++)
                {
                    if (Future_moves[i][j, 1] == Future_moves[i][j + 1, 1] / 2)
                    {
                        score += Future_moves[i][j, 1];
                    }
                }


            }

            Future_moves.Clear();
            return score;
        }

        private int Command()
        {
            // 1 is up
            // 2 is left
            // 3 is right
            // 4 is down
            int Left_score;
            int Right_score;
            int Up_score;
            int Down_score;

            // assesses all board states unless that respective move is invalid
            if (!Is_up_valid(board))
            {
                Up_score = -1;
            }
            
            else
            {
                // simulates up for next turn
                Simulate_Up(board, Board_space);

                // simulates next for all possible future moves
                int x = Future_moves.Count();
                for (int i = 0; i < x; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }

                int y = Future_moves.Count();
                for (int i = x; i < y; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }


                Up_score = Assess();
            }

            if (!Is_right_valid(board))
            {
                Right_score = -1;
            }

            else
            {
                Simulate_Right(board, Board_space);
                int x = Future_moves.Count();
                for (int i = 0; i < x; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }
                int y = Future_moves.Count();
                for (int i = x; i < y; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }


                Right_score = Assess();
            }

            if (!Is_left_valid(board))
            {
                Left_score = -1;
            }

            else
            {
                Simulate_Left(board, Board_space);
                int x = Future_moves.Count();
                for (int i = 0; i < x; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }

                int y = Future_moves.Count();
                for (int i = x; i < y; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }

                Left_score = Assess();
            }

            if (Is_down_valid(board))
            {
                Down_score = -1;
            }

            else
            {
                Simulate_Down(board, Board_space);
                int x = Future_moves.Count();
                for (int i = 0; i < x; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }
                int y = Future_moves.Count();
                for (int i = x; i < y; i++)
                {
                    if (Is_up_valid(Future_moves[i]))
                    {
                        Simulate_Up(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_left_valid(Future_moves[i]))
                    {
                        Simulate_Left(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_right_valid(Future_moves[i]))
                    {
                        Simulate_Right(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                    if (Is_down_valid(Future_moves[i]))
                    {
                        Simulate_Down(Future_moves[i], Find_empty_space(Future_moves[i]));
                    }
                }

                Down_score = Assess();
            }

            // highest score is up
            if ((Up_score >= Left_score) && (Up_score >= Right_score) && (Up_score >= Down_score))
            {
                return 1;
            }

            // highest score is left
            else if ((Left_score >= Right_score) && (Left_score >= Down_score))
            {
                return 2;
            }
        
            //highest score is right
            else if (Right_score >= Down_score && Right_score > Up_score && Right_score > Left_score)
            {
                return 3;
            }

            // highest score is down
            else
            {
                return 4;
            }
        }
    }
}
