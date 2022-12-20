using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace _2048
{
    // used to force a single keypress
    //blatantly stolen from 
    // https://community.monogame.net/t/one-shot-key-press/11669
    public class Controls
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        public static KeyboardState CheckKey()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }

        public static bool CheckKeyPress(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool CheckKeyRelease(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }


    } // end Controls class

    public partial class _2048
    {
        // resets the board to 0
        private void Clear_board()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = 0;
                    previous_turn[i, j] = board[i, j];
                }
            }
            Board_space = 16;
        }

        private void Fill_board()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = (int)Math.Pow(2, (j * 4) + i + 1);
                    Board_space--;
                }
            }

        }
        
        // returns color based on the value in the box
        // Colors chosen arbitrarily based on what black text showed up visibly on

        private static Color Identify_Color(int input)
        {
            Color cl1;

            // new Color(red, green, blue)

            //2
            if (input == Math.Pow(2, 1))
            {
                cl1 = new Color(242, 108, 79);
                return cl1;
            }

            //4
            else if (input == Math.Pow(2, 2))
            {
                cl1 = new Color(246, 142, 86);
                return cl1;
            }

            //8
            else if (input == Math.Pow(2, 3))
            {
                cl1 = new Color(251, 175, 93);
                return cl1;
            }

            //16
            else if (input == Math.Pow(2, 4))
            {
                cl1 = new Color(255, 245, 104);
                return cl1;
            }

            //32
            else if (input == Math.Pow(2, 5))
            {
                cl1 = new Color(172, 211, 115);
                return cl1;
            }

            //64
            else if (input == Math.Pow(2, 6))
            {
                cl1 = new Color(124, 197, 118);
                return cl1;
            }

            //128
            else if (input == Math.Pow(2, 7))
            {
                cl1 = new Color(60, 184, 120);
                return cl1;
            }

            //256
            else if (input == Math.Pow(2, 8))
            {
                cl1 = new Color(28, 187, 180);
                return cl1;
            }

            //512
            else if (input == Math.Pow(2, 9))
            {
                cl1 = new Color(0, 191, 243);
                return cl1;
            }

            //1024
            else if (input == Math.Pow(2, 10))
            {
                cl1 = new Color(68, 140, 203);
                return cl1;
            }

            //2048
            else if (input == Math.Pow(2, 11))
            {
                cl1 = new Color(86, 116, 185);
                return cl1;
            }

            //4096
            else if (input == Math.Pow(2, 12))
            {
                cl1 = new Color(96, 92, 168);
                return cl1;
            }

            //8192
            else if (input == Math.Pow(2, 13))
            {
                cl1 = new Color(133, 96, 168);
                return cl1;
            }

            //16384
            else if (input == Math.Pow(2, 14))
            {
                cl1 = new Color(168, 100, 168);
                return cl1;
            }

            //32768
            else if (input == Math.Pow(2, 15))
            {
                cl1 = new Color(240, 110, 170);
                return cl1;
            }

            //65536
            else if (input == Math.Pow(2,16))
            {
                cl1 = new Color(242, 209, 125);
                return cl1;
            }
            else
            {
                return Color.LightGray;
            }
        }


        private void New_tile()
        {
            // doesn't run if there's no empty spaces left
            if (Board_space == 0)
            {
                return;
            }
            // 10% chance to be 4; otherwise it's 2
            int Two_or_four = r.Next() % 10;
            if (Two_or_four == 0)
            {
                Two_or_four = 4;
            }
            else
            {
                Two_or_four = 2;
            }

            // chooses a random number based on how many spaces are left
            int Pos = r.Next() % Board_space;

            // loops through, subtracting 1 each time an empty space is encountered
            // when it reaches 0, it will fill the next empty space with either 2 or 4
            // depending on the roll
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i,j] == 0 && Pos == 0)
                    {
                        board[i, j] = Two_or_four;
                        Pos--;
                        Board_space -= 1;
                    }
                    else if (board[i,j] == 0)
                    {
                        Pos--;
                    }
                }
            }   
        }

        // resets the game
        private void New_game()
        {
            Clear_board();          // REMOVE LATER DUMBASS
                                    Future_moves.Clear();
                                    curr_position = 0;
            Score = 0;
            New_tile();
            New_tile();
        }

        // shifts the board up
        private void Up()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (board[i,j] != 0 && board[i, j-1] == 0)
                    {
                        board[i, j - 1] = board[i, j];
                        board[i, j] = 0;
                        j = 0;
                    }
                }
            }

            //merger
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (board[i, 0] == board[i, 1] && board[i, 0] != 0)
                {
                    board[i, 0] *= 2;
                    Score += board[i, 0];
                    board[i, 1] = board[i, 2];
                    board[i, 2] = board[i, 3];
                    board[i, 3] = 0;
                    Board_space += 1;
                }
                // merges middle two
                if (board[i, 1] == board[i, 2] && board[i, 1] != 0)
                {
                    board[i, 1] *= 2;
                    Score += board[i, 1];
                    board[i, 2] = board[i, 3];
                    board[i, 3] = 0;
                    Board_space += 1;
                }
                // merges last two
                if (board[i, 2] == board[i, 3] && board[i, 2] != 0)
                {
                    board[i, 2] *= 2;
                    Score += board[i, 2];
                    board[i, 3] = 0;
                    Board_space += 1;
                }
            }

            New_tile();
        }

        // checks if up is a valid move
        public bool Is_up_valid(int[,] board_test)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j > 0; j--)
                {
                    if (board_test[i, j] != 0 && board_test[i, j - 1] == 0)
                    {
                        return true;
                    }
                    else if (board_test[i, j] == board_test[i, j - 1] && board_test[i, j] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // shifts the board down
        private void Down()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j > -1; j--)
                {
                    if (board[i, j] != 0 && board[i, j + 1] == 0)
                    {
                        board[i, j + 1] = board[i, j];
                        board[i, j] = 0;
                        j = 3;
                    }
                }
            }

            //merge
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (board[i, 3] == board[i, 2] && board[i, 3] != 0)
                {
                    board[i, 3] *= 2;
                    Score += board[i, 3];
                    board[i, 2] = board[i, 1];
                    board[i, 1] = board[i, 0];
                    board[i, 0] = 0;
                    Board_space += 1;
                }
                // merges middle two
                if (board[i, 2] == board[i, 1] && board[i, 2] != 0)
                {
                    board[i, 2] *= 2;
                    Score += board[i, 2];
                    board[i, 1] = board[i, 0];
                    board[i, 0] = 0;
                    Board_space += 1;
                }
                // merges last two
                if (board[i, 1] == board[i, 0] && board[i, 1] != 0)
                {
                    board[i, 1] *= 2;
                    Score += board[i, 1];
                    board[i, 0] = 0;
                    Board_space += 1;
                }
            }
            New_tile();
        }

        // checks if down is a valid move
        public bool Is_down_valid(int[,] board_test)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board_test[i,j] != 0 && board_test[i,j+1] == 0)
                    {
                        return true;
                    }
                    else if (board_test[i, j] == board_test[i, j + 1] && board_test[i,j] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // shifts board left
        private void Left()
        {
            
            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (board[i, j] != 0 && board[i - 1, j] == 0)
                    {
                        board[i - 1, j] = board[i, j];
                        board[i, j] = 0;
                        i = 0;
                    }
                }
            }

            // merges
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (board[0, i] == board[1, i] && board[0, i] != 0)
                {
                    board[0, i] *= 2;
                    Score += board[0, i];
                    board[1, i] = board[2, i];
                    board[2, i] = board[3, i];
                    board[3, i] = 0;
                    Board_space += 1;
                }
                // merges middle two
                if (board[1, i] == board[2, i] && board[1, i] != 0)
                {
                    board[1, i] *= 2;
                    Score += board[1, i];
                    board[2, i] = board[3, i];
                    board[3, i] = 0;
                    Board_space += 1;
                }
                // merges last two
                if (board[2, i] == board[3, i] && board[2, i] != 0)
                {
                    Score += board[2, i];
                    board[2, i] *= 2;
                    board[3, i] = 0;
                    Board_space += 1;
                }
            }
            New_tile();
        }

        // checks if left is a valid move
        public bool Is_left_valid(int[,] board_test)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j > 0; j--)
                {
                    if (board_test[j, i] != 0 && board_test[j - 1, i] == 0)
                    {
                        return true;
                    }
                    else if (board_test[j, i] == board_test[j - 1, i] && board_test[j, i] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // shifts board right
        private void Right()
        {
            for (int j = 0; j < 4;j++)
            {
                for (int i = 2; i > -1; i--)
                {
                    if (board[i, j] != 0 && board[i + 1, j] == 0)
                    {
                        board[i + 1, j] = board[i, j];
                        board[i, j] = 0;
                        i = 3;
                    }
                }
            }

            // merges any valid tiles
            for (int i = 0; i < 4; i++)
            {
                // merges first two
                if (board[3, i] == board[2, i] && board[3, i] != 0)
                {
                    board[3, i] *= 2;
                    Score += board[3, i];
                    board[2, i] = board[1, i];
                    board[1, i] = board[0, i];
                    board[0, i] = 0;
                    Board_space += 1;
                }
                // merges middle two
                if (board[2, i] == board[1, i] && board[2, i] != 0)
                {
                    board[2, i] *= 2;
                    Score += board[2, i];
                    board[1, i] = board[0, i];
                    board[0, i] = 0;
                    Board_space += 1;
                }
                // merges last two
                if (board[1, i] == board[0, i] && board[1, i] != 0)
                {
                    board[1, i] *= 2;
                    Score += board[1, i];
                    board[0, i] = 0;
                    Board_space += 1;
                }
            }

            New_tile();
        }

        // checks if right is a valid move
        public bool Is_right_valid(int[,] board_test)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board_test[j, i] != 0 && board_test[j + 1, i] == 0)
                    {
                        return true;
                    }
                    else if (board_test[j, i] == board_test[j + 1, i] && board_test[j, i] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // checks if the game has any valid moves; if not, returns true
        public bool Game_over(int[,] curr_board)
        {
            if (Board_space > 0)
            {
                return false;
            }

            else if (!Is_down_valid(curr_board) && !Is_left_valid(curr_board) && !Is_right_valid(curr_board) && !Is_up_valid(curr_board))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    } // end _2048 class
} // end namespace
