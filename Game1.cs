﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Spline;
//using System.IO;

namespace LisaMTowerDefence
{
    public class Game1 : Game
    {

        //på 5 e och f

        //reg-instances
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //windowsize
        private int windowWidth = 850;
        private int windowHeight = 650;

        //spline
        private SimplePath path;

        //textures
        private Texture2D catTex;
        private float catPos;

        //rendertarget
        private Texture2D transTex;
        private RenderTarget2D renderTest;

        //test place object
        private Vector2 mousePos;
        private MouseState mouseState;

        private GameObject test;
        List<GameObject> placedObjects = new List<GameObject>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            path = new SimplePath(graphics.GraphicsDevice);
           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            path = new SimplePath(graphics.GraphicsDevice);
            path.generateDefaultPath();

            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();


            spriteBatch = new SpriteBatch(GraphicsDevice);


            catTex = Content.Load<Texture2D>("fatcat");
            transTex = Content.Load<Texture2D>("transparentSquareBackground");

            renderTest = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);

            
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            test = new GameObject(catTex, new Vector2(100,100), new Rectangle(0, 0, catTex.Width, catTex.Height));

            //TÄNK PÅ RÄTT ORDNING
            DrawOnRenderTarget();

            catPos = path.beginT;
            path.SetPos(0,Vector2.Zero);
            path.AddPoint(new Vector2(windowWidth, windowHeight));

           

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            System.Diagnostics.Debug.WriteLine(mousePos);
            test.pos = mousePos;

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if(CanPlace(test))
                {
                    placedObjects.Add(test);
                }
            }

            catPos++;

            base.Update(gameTime);
        }

      

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //path.Draw(spriteBatch);
            //path.DrawPoints(spriteBatch);

            spriteBatch.Draw(renderTest, Vector2.Zero, Color.White);

            test.Draw(spriteBatch);
            foreach(var obj in placedObjects)
            {
                obj.Draw(spriteBatch);
            }

            //if(catPos<path.endT)
            //{
            //    spriteBatch.Draw(catTex, path.GetPos(catPos), new Rectangle(0, 0, catTex.Width, catTex.Height), Color.White, 0f, new Vector2(catTex.Width / 2, catTex.Height / 2), 1f, SpriteEffects.None, 0f);
            //    spriteBatch.End();
            //}
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawOnRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(renderTest);

            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            //GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Draw(transTex, Vector2.Zero, Color.White);
            //spriteBatch.Draw(catTex, new Vector2(100,100), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }


        //KOLLA CANPLACE PÅ ETT LAGER MED INGET; RITA SEDAN TILL RENDERTARGET
        public bool CanPlace(GameObject g)
        {
            Color[] pixels = new Color[g.tex.Width * g.tex.Height];
            Color[] pixels2 = new Color[g.tex.Height * g.tex.Width];
            g.tex.GetData<Color>(pixels2);
            renderTest.GetData(0, g.hitbox, pixels, 0, pixels.Length);
            for(int i = 0; i < pixels.Length; i++)
                {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
                }
            return true;
        }
    }
}