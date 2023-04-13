using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsForm;

namespace LisaMTowerDefence
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Form1 form;
        PausForm pausForm;

        public static int windowWidth { get; private set; }
        public static int windowHeight { get; private set; }

        GameManager gameManager;

        private enum GameState { menu, playing, pausMenu };
        private GameState currentState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameManager = new GameManager();
            currentState = GameState.menu;
            windowWidth = 1200;
            windowHeight = 900;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            form = new Form1();
            form.Show();
            pausForm = new PausForm();

            gameManager.LoadGame(GraphicsDevice, Content);

            Window.IsBorderless = true;
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || Form1.closeGame == true)
                Exit();

            switch(currentState)
            {
                case GameState.menu:
                    if(form.gameStarted && gameManager.lost != true)
                    {
                        currentState = GameState.playing;
                    }
                    break;

                case GameState.playing:
                    
                    if(gameManager.lost == true)
                    {
                        currentState = GameState.menu;
                    }
                    else if(gameManager.paused)
                    {
                        currentState = GameState.pausMenu;
                        pausForm = new PausForm();
                        pausForm.Show();
                    }

                    gameManager.Update(gameTime);

                    break;

                case GameState.pausMenu:

                    if (pausForm.closedPausForm)
                    {
                        gameManager.paused = false;
                        currentState = GameState.playing;
                    }
                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch(currentState)
            {
                case GameState.playing:
                    gameManager.Draw(spriteBatch, GraphicsDevice);
                    break;

                default:
                    break;
            }
            base.Draw(gameTime);
        }
    }
}