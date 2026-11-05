using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame_Assignment_2___Loops_And_Lists //Maxym F.
{

    enum Screen 
    {
        intoScreen,
        gameScreen
    }


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen screen;

        Random generator;
        Rectangle window;
        Texture2D introBackgroundTexture;
        Texture2D battleBackgroundTexture;

        List<Rectangle> tankRects;

        List<Texture2D> tankTextures;
        List<Texture2D> textures;

        MouseState mouseState, prevMouseSate;
        KeyboardState keyboardState;


        float seconds;
        float respawnTime;





        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            screen = Screen.intoScreen; 

            window = new Rectangle(0, 0, 800, 600);
            generator = new Random();
            textures = new List<Texture2D>();
            tankTextures = new List<Texture2D>();
            tankRects = new List<Rectangle>();

            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            seconds = 0f;
            respawnTime = 2f;

            base.Initialize();

            for (int i = 0; i < 5; i++)
            {
                int size = generator.Next(100, 261);

                tankRects.Add(new Rectangle(generator.Next(window.Width - size), generator.Next(window.Height - size), size, size));
                tankTextures.Add(textures[generator.Next(textures.Count)]);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            battleBackgroundTexture = Content.Load<Texture2D>("Images/fieldBackground");
            introBackgroundTexture = Content.Load<Texture2D>("Images/introbackground");

            for (int i = 1; i <= 4; i++)
            {
                textures.Add(Content.Load<Texture2D>("Images/tank" + i));
            }


        }

        protected override void Update(GameTime gameTime)
        {

            prevMouseSate = mouseState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            if (screen == Screen.intoScreen)
            {
                Console.WriteLine("Press ENTER To Continue");
                if (keyboardState.IsKeyDown(Keys.Enter)) 
                {
                    screen = Screen.gameScreen;
                }
            }

            else if (screen == Screen.gameScreen)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > respawnTime)
                {
                    int size = generator.Next(100, 260); //Size range

                    tankRects.Add(new Rectangle(generator.Next(window.Width - size), generator.Next(window.Height - size), size, size));
                    tankTextures.Add(textures[generator.Next(textures.Count)]);
                    seconds = 0; //Without this, a new planet would spawn every single frame!
                }


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                // TODO: Add your update logic here


                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseSate.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < tankRects.Count; i++)
                    {
                        if (tankRects[i].Contains(mouseState.Position))
                        {
                            tankRects.RemoveAt(i);
                            tankTextures.RemoveAt(i);

                            i--;
                        }
                    }
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            if (screen == Screen.intoScreen)
            {
                _spriteBatch.Draw(introBackgroundTexture, window, Color.White);
            }

            else if (screen == Screen.gameScreen)
            {
                _spriteBatch.Draw(battleBackgroundTexture, window, Color.White);

                for (int i = 0; i < tankRects.Count; i++)
                {
                    _spriteBatch.Draw(tankTextures[i], tankRects[i], Color.White);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
