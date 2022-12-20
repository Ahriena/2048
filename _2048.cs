using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace _2048
{
    public partial class _2048 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private SpriteFont Game_over_font;
        readonly int[,] board = new int[4, 4];
        readonly int[,] previous_turn = new int[4, 4];

        public int Board_space { get; set; }
        public int Score { get; set; }
        readonly Random r = new();
        Texture2D box_texture;

        bool toggleAI = false;
        int curr_position = 0;

        public _2048()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 415,
                PreferredBackBufferHeight = 500
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            base.Initialize();
            Clear_board();
            New_game();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Tile_number");
            Game_over_font = Content.Load<SpriteFont>("Game_Over");
        }


        protected override void Update(GameTime gameTime)
        {

            // save some writing
            var kstate = Keyboard.GetState();
            Controls.CheckKey();

            // exit keybind of escape / f5
            if (kstate.IsKeyDown(Keys.Escape) || kstate.IsKeyDown(Keys.F5) || kstate.IsKeyDown(Keys.F6))
                Exit();

            // T to clear
            if (Controls.CheckKeyRelease(Keys.T))
            {
                New_game();
            }

            // up
            if (Controls.CheckKeyRelease(Keys.Up) || Controls.CheckKeyRelease(Keys.W))
            {
                if (Is_up_valid(board))
                {
                    Up();
                }
            }

            // down
            if (Controls.CheckKeyRelease(Keys.Down) || Controls.CheckKeyRelease(Keys.S))
            {
                if (Is_down_valid(board))
                {
                    Down();
                }
            }

            // left
            if (Controls.CheckKeyRelease(Keys.Left) || Controls.CheckKeyRelease(Keys.A))
            {
                if (Is_left_valid(board))
                {
                    Left();
                }
            }

            // right
            if (Controls.CheckKeyRelease(Keys.Right) || Controls.CheckKeyRelease(Keys.D))
            {
                if (Is_right_valid(board))
                {
                    Right();
                }
            }

            // simulates move for testing
            if (Controls.CheckKeyRelease(Keys.D1))
            {
                Simulate_Right(board, Board_space);
            }

            // cycles through array of possible outcomes
            if (Controls.CheckKeyRelease(Keys.D2))
            {
                curr_position++;
                if (curr_position == Future_moves.Count)
                {
                    curr_position = 0;
                }
            }

            // fills the board for testing
            if (Controls.CheckKeyRelease(Keys.R))
            {
                Fill_board();
                Score = 0;
            }

            // runs AI
            if (Controls.CheckKeyRelease(Keys.Space))
            {
                toggleAI = !toggleAI;
            }


            if (toggleAI == true)
            {
                int command = Command();
                
                if (Game_over(board))
                {
                    New_game();
                }
                if (command == 1)
                {
                    Up();
                }
                else if (command == 2)
                {
                    Left();

                }
                else if (command == 3)
                {
                    Right();
                }
                else
                {
                    Down();
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // draw all the rectangles
            box_texture = new Texture2D(GraphicsDevice, 1, 1);
            box_texture.SetData(new Color[] { Color.White });

            //background grid
            _spriteBatch.Draw(box_texture, new Rectangle(5, 5, 405, 405), Color.DarkSlateGray);

            // draws the 16 game board base
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _spriteBatch.Draw(box_texture, new Rectangle(10 + i * 100, 10 + j * 100, 95, 95), Color.LightGray);
                }
            }

            // draws the tiles 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] != 0)
                    {
                        Color Tile_color = Identify_Color(board[i, j]);
                        _spriteBatch.Draw(box_texture, new Rectangle(10 + i * 100, 10 + j * 100, 95, 95), Tile_color);
                        _spriteBatch.DrawString(font, board[i, j].ToString(), new Vector2(62 + i * 100 - 9 * (board[i, j].ToString()).Length, 47 + j * 100), Color.Black);
                    }
                }
            }

            // score text
            _spriteBatch.DrawString(font, Score.ToString(), new Vector2(200 + 4 - 4 * Score.ToString().Length, 440), Color.Black);


            // game over text
            if (Game_over(board))
            {
                _spriteBatch.DrawString(Game_over_font, "GAME", new Vector2(60, 103), Color.Black);
                _spriteBatch.DrawString(Game_over_font, "OVER", new Vector2(60, 193), Color.Black);
            }

            _spriteBatch.End();
            base.Draw(gameTime);

        }
    } // end _2048 class
} // end namespace