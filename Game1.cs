using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Spline;


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

        //spline
        private SimplePath path;
        

        //textures
        private Texture2D catTex;
        private float catPos;
        private Texture2D tinyCatTex;

        //rendertarget
        private Texture2D transTex;
        private RenderTarget2D renderTest;

        //object following mouse(currently selected object)
        private Vector2 mousePos;
        private MouseState mouseState;

        //enemies
        private Enemy enemy;

        //towers
        private Tower test;

        //ska listan innehålla almänna towers, beroende på olika typer osvosv?
        List<Tower> placedObjects = new List<Tower>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
         
            path = new SimplePath(graphics.GraphicsDevice);
            path.Clean();
    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();


            spriteBatch = new SpriteBatch(GraphicsDevice);


            catTex = Content.Load<Texture2D>("fatcat");
            transTex = Content.Load<Texture2D>("transparentSquareBackground");
            tinyCatTex = Content.Load<Texture2D>("cat");


            renderTest = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);

            
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            test = new Tower(catTex, new Vector2(0,0), new Rectangle(0, 0, catTex.Width, catTex.Height));
            
            DrawOnRenderTarget();


//MAKE PRETTIER PATH
//MAKE METHOD FOR PATH

            path.AddPoint(Vector2.Zero);
            path.SetPos(0,Vector2.Zero);
            path.AddPoint(new Vector2(100, 100));
            path.AddPoint(new Vector2(200, 100));
            path.AddPoint(new Vector2(200, 200));
            path.AddPoint(new Vector2(windowWidth - 100, windowHeight));
            path.AddPoint(new Vector2(windowWidth, windowHeight));
            

            catPos = path.beginT;
            enemy = new Enemy(tinyCatTex, new Vector2(catPos, catPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width));

        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            
            //KOM IHÅG ATT ÄVEN FLYTTA HITBOXEN
            test.pos = mousePos;
            test.hitbox.X = (int)mousePos.X;
            test.hitbox.Y = (int)mousePos.Y;


            //System.Diagnostics.Debug.WriteLine(CanPlace(test));
            //System.Diagnostics.Debug.WriteLine(mouseState.LeftButton.ToString());

            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                if (CanPlace(test) == true)
                {
                    placedObjects.Add(new Tower(catTex, mousePos, new Rectangle(0, 0, catTex.Width, catTex.Height)));
                    DrawOnRenderTarget();
                    //SÄTT TOWER SOM AKTIVT? AUTOMATISKT

                }


            }

            //splinemovement
            //speed, fix individual
            catPos = catPos + 2;
            //make foreach enemy, get their speed(count upwards in class itself)
            enemy.pos = path.GetPos(catPos);

            foreach(Tower t in placedObjects)
            {
                t.Update(gameTime);
            }

            base.Update(gameTime);
        }

      

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            path.Draw(spriteBatch);
            //path.DrawPoints(spriteBatch);

            spriteBatch.Draw(renderTest, Vector2.Zero, Color.White);

            test.Draw(spriteBatch);


            if (catPos < path.endT)
            {
//BEHÖVER DENNA VARA SÅ GODDAMN LONG?
    //ENDA SOM ANVÄNDS ÄR ORIGIN
                enemy.Draw(spriteBatch);
                //spriteBatch.Draw(tinyCatTex, path.GetPos(catPos), new Rectangle(0, 0, tinyCatTex.Width, tinyCatTex.Height), Color.White, 0f, new Vector2(tinyCatTex.Width / 2, tinyCatTex.Height / 2), 1f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawOnRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(renderTest);

            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            foreach (var obj in placedObjects)
            {
                obj.Draw(spriteBatch);
            }

            //GraphicsDevice.Clear(Color.Transparent);

            //spriteBatch.Draw(transTex, Vector2.Zero, Color.White);
            //spriteBatch.Draw(catTex, new Vector2(100,100), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        public bool CanPlace(GameObject g)
        {
//BUG: CLICKA UTANFÖR WINDOW = CRASH, LÖST
            if (mousePos.X > 0 && mousePos.X + g.tex.Width < windowWidth && mousePos.Y > 0 && mousePos.Y + g.tex.Height < windowHeight)
            {
                Color[] pixels = new Color[g.tex.Width * g.tex.Height];
                Color[] pixels2 = new Color[g.tex.Height * g.tex.Width];
                g.tex.GetData<Color>(pixels2);
                renderTest.GetData(0, g.hitbox, pixels, 0, pixels.Length);
                //System.Diagnostics.Debug.WriteLine("pixels" + pixels[0].A.ToString());
                //System.Diagnostics.Debug.WriteLine("pixels2" + pixels2[0].A.ToString());
                for (int i = 0; i < pixels.Length; i++)
                {
                    if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}