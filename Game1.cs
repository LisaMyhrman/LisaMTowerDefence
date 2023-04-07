using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Spline;
using System.Linq;

namespace LisaMTowerDefence
{
    public class Game1 : Game
    {
        //standard-instances
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //windowsize
        private int windowWidth = 1200;
        private int windowHeight = 900;

        GameManager gameManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameManager = new GameManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameManager.LoadGame(GraphicsDevice, Content, spriteBatch);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameManager.Draw(spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}