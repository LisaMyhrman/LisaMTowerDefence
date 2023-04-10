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
        //standard-instances
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Form1 form;

//MAKE PRETTIER
        //windowsize
        private int windowWidth = 1200;
        private int windowHeight = 900;

        GameManager gameManager;

        //states
        private enum GameState { menu, playing, ending };
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

            gameManager.LoadGame(GraphicsDevice, Content, spriteBatch);

          

            //Window.IsBorderless = true;
            //ADD AFTER EXIT BUTTON
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            switch(currentState)
            {
                case GameState.menu:
                if(form.gameStarted)
                    {
                        currentState = GameState.playing;
                    }
                    break;

                case GameState.playing:
                    gameManager.Update(gameTime);
                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch(currentState)
            {
                case GameState.menu:

                    break;

                case GameState.playing:

                gameManager.Draw(spriteBatch, GraphicsDevice);
                    break;

            }


            base.Draw(gameTime);
        }
    }
}