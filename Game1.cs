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

        //spline
        private SimplePath path;
        

        //textures
        private Texture2D catTex;
        private float enemyStartPos;
        private Texture2D tinyCatTex;

        //rendertarget
        private Texture2D transTex;
        private RenderTarget2D renderTest;

        //object following mouse(currently selected object)
        private Vector2 mousePos;
        private MouseState mouseState;
        private KeyboardState keyState;

        //enemies
        private Enemy enemy;
        private List<Enemy> enemies = new List<Enemy>();

        //towers
        private Tower test;
        private Tower chosenTower = null;

        //ska listan innehålla almänna towers, beroende på olika typer osvosv?
        List<Tower> placedObjects = new List<Tower>();

        //shootingtests
        private List<Bullet> bullets = new List<Bullet>();

        

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

            Assets.LoadTextures(Content);

            //behövs inte senare
            tinyCatTex = Assets.TinyCatTex;
            catTex = Assets.catTex;

            spriteBatch = new SpriteBatch(GraphicsDevice);


            //catTex = Content.Load<Texture2D>("fatcat");
            //transTex = Content.Load<Texture2D>("transparentSquareBackground");
            //tinyCatTex = Content.Load<Texture2D>("cat");

           


            renderTest = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);

            
            mousePos = new Vector2(mouseState.X, mouseState.Y);

//SKAPA ENDAST DÅ TORN VALTS
            //test = new Tower(catTex, new Vector2(0,0), new Rectangle(0, 0, catTex.Width, catTex.Height));

            
            
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



            //FIX LISTED ENEMIES

            enemyStartPos = path.beginT;
            enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 2.0f, 2, 3));
            enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 1.0f, 2, 1));
            //enemy = new Enemy(tinyCatTex, new Vector2(catPos, catPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width));

            foreach (Enemy e in enemies)
            {
                e.positionOnPath = path.beginT;
            }

        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            System.Diagnostics.Debug.WriteLine(EconomyTracker.GetCoins());




            //if 1,2,3,4(corresponding to towertypes) is pressed, spawn new tower ( MAKE METHOD)

            if(chosenTower == null)
            {
//FIX COST OF TOWERS
                if (keyState.IsKeyDown(Keys.E))// && chosenTower.Cost < EconomyTracker.GetCoins())
                {
                    //standardtower
                    //chosenTower = new Tower(catTex, new Vector2(0, 0), new Rectangle(0, 0, catTex.Width, catTex.Height));
                    chosenTower = new Tower(catTex, mousePos, new Rectangle((int)mousePos.X, (int)mousePos.Y, catTex.Width, catTex.Height),1, 4000, 300);
                }
                else if(keyState.IsKeyDown(Keys.F))
                {
                    chosenTower = new Tower(catTex, mousePos, new Rectangle((int)mousePos.X, (int)mousePos.Y, catTex.Width, catTex.Height),2, 1000, 500);
                }

//POSSIBILITY FOR DESIGN YOUR OWN TOWER
                
            }
            else
            {
                chosenTower.pos = mousePos;
                chosenTower.hitbox.X = (int)mousePos.X;
                chosenTower.hitbox.Y = (int)mousePos.Y;
            }
            

            //METHOD?
            if (mouseState.LeftButton == ButtonState.Pressed && chosenTower != null)
            {
                if (CanPlace(chosenTower) == true)
                {
//MIGHT BE A PROBLEM WITH LIST NULLING?
                    //placedObjects.Add(new Tower(catTex, mousePos, new Rectangle((int)mousePos.X, (int)mousePos.Y, catTex.Width, catTex.Height)));
                    placedObjects.Add(chosenTower);
                    DrawOnRenderTarget();
                    chosenTower = null;
                  
                }
            }
                


            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].pos = path.GetPos(enemies[i].positionOnPath);
                enemies[i].Update();
                //System.Diagnostics.Debug.WriteLine(enemies[i].Health);
                if (enemies[i].Health <= 0)
                {
                    EconomyTracker.AlterCoins(enemies[i].Value);
                    enemies.RemoveAt(i);
                }
            }

            foreach (Tower t in placedObjects)
            {
                t.Update(gameTime);
                if(CheckDistance(t))
                {
                    //System.Diagnostics.Debug.WriteLine("in distance");
                    if(t.isShooting)
                    {
                        bullets.Add(t.typeOfBullet);
                    }
                }
                
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                BulletCollision(bullets[i]);
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

            if(chosenTower != null)
            {chosenTower.Draw(spriteBatch);}
            


            //DRAW BULLETS NOT WORKING

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

            //foreach(Bullet b in bullets)
            //{
            //    b.Draw(spriteBatch);
            //}

            foreach (Enemy e in enemies)
            {
                if (enemyStartPos < path.endT)
                {
                    //BEHÖVER DENNA VARA SÅ GODDAMN LONG?
                    //ENDA SOM ANVÄNDS ÄR ORIGIN
                    e.Draw(spriteBatch);
                //enemy.Draw(spriteBatch);
                //spriteBatch.Draw(tinyCatTex, path.GetPos(catPos), new Rectangle(0, 0, tinyCatTex.Width, tinyCatTex.Height), Color.White, 0f, new Vector2(tinyCatTex.Width / 2, tinyCatTex.Height / 2), 1f, SpriteEffects.None, 0f);
                }
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
//BUG: CLICKA UTANFÖR WINDOW = CRASH, SOVLED, TOO LONG STATEMENT?
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

        //private void ShootBullet()
        //{

        //}

        private bool CheckDistance(Tower t)
        {
                //Check through list of enemies
                //else-statement? none

            foreach(Enemy e in enemies)
            {
//PICK DISTANCE HERE
                if(GetDistance(t.GetTowerPos, e.pos) < t.Reach)
                {
                    t.ClosestEnemyPos = e.pos;
                    //System.Diagnostics.Debug.WriteLine(enemy.pos);
                    return true;

                }
                else
                {
                    continue;
                }
            }

            return false;

        }

        private float GetDistance(Vector2 one, Vector2 two)
        {
            float X = one.X - two.X;
            float Y = one.Y - two.Y;
            return (float)Math.Sqrt((X*X) + (Y*Y));
        }

        private void BulletCollision(Bullet b)
        {
            foreach(Enemy e in enemies)
            {
                if(b.IsColliding(e.hitbox))
                {
                    System.Diagnostics.Debug.WriteLine("HIT");
                    e.HealthMaster(b.Damage);
                    bullets.Remove(b);
                }
            }
        }
    }
}