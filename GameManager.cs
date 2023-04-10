using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Spline;
using Microsoft.Xna.Framework.Content;
using WindowsForm;
using System.Drawing.Text;

namespace LisaMTowerDefence
{
    internal class GameManager
    {
       //FIX WINDOWSIZE FROM GAME1?
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
        private bool changedRender = false;
        private Tower test;
        private Tower chosenTower = null;

        //ska listan innehålla almänna towers, beroende på olika typer osvosv?
        List<Tower> placedObjects = new List<Tower>();

        //shootingtests
        private List<Bullet> bullets = new List<Bullet>();


        //partikeltest
        private ParticleEmitter particleEmitter;

        //waves
        private int waveNumber;
        private int prevWaveNumber;
        private float waveTimer;
        
    



        public void LoadGame(GraphicsDevice graphics, ContentManager Content, SpriteBatch spriteBatch)
        {

            path = new SimplePath(graphics);
            path.Clean();

            waveNumber = 1;
            Assets.LoadTextures(Content);

            //behövs inte senare
            tinyCatTex = Assets.tinyCatTex;
            catTex = Assets.catTex;

            renderTest = new RenderTarget2D(graphics, windowWidth, windowHeight);
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            DrawOnRenderTarget(graphics, spriteBatch);
            MakePath();
            enemyStartPos = path.beginT;

            foreach (Enemy e in enemies)
            {
                e.positionOnPath = path.beginT;
            }
        }

   

        public void Update(GameTime gameTime)
        {
            WaveHandler(gameTime);

            //inputs
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            if (chosenTower == null)
            {
                //FIX COST OF TOWERS
                //PICKING TOWERS
                if (keyState.IsKeyDown(Keys.D1))
                {
                    chosenTower = new Tower(Assets.granny1, mousePos, new Rectangle((int)mousePos.X, (int)mousePos.Y, Assets.granny1.Width, Assets.granny1.Height), 1);
                    CheckAffordTower();
                }
                else if (keyState.IsKeyDown(Keys.D2))
                {
                    chosenTower = new Tower(Assets.granny1, mousePos, new Rectangle((int)mousePos.X, (int)mousePos.Y, Assets.granny1.Width, Assets.granny1.Height), 2);
                    CheckAffordTower();
                }

                //POSSIBILITY FOR DESIGN YOUR OWN TOWER

            }
            else
            {
                chosenTower.pos = mousePos;
                chosenTower.hitbox.X = (int)mousePos.X;
                chosenTower.hitbox.Y = (int)mousePos.Y;

                PlaceTowerMode();
            }


            if (particleEmitter != null)
            particleEmitter.Update(gameTime);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].pos = path.GetPos(enemies[i].positionOnPath);
                enemies[i].Update(gameTime);
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
                if (CheckDistance(t))
                {
                    //System.Diagnostics.Debug.WriteLine("in distance");
                    if (t.isShooting)
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
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            

            if (changedRender == true)
            {
                DrawOnRenderTarget(graphics, spriteBatch);
                changedRender = false;
            }

            spriteBatch.Begin();
            graphics.Clear(Color.White);
            //ta bort efter map är made
            spriteBatch.Draw(Assets.level1Background, Vector2.Zero, Color.White);

            path.Draw(spriteBatch);

            spriteBatch.Draw(renderTest, Vector2.Zero, Color.White);

            if (particleEmitter != null)
                particleEmitter.Draw(spriteBatch);

            if (chosenTower != null)
                chosenTower.Draw(spriteBatch);

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

            foreach (Enemy e in enemies)
            {
                if (enemyStartPos < path.endT)
                {
                    e.Draw(spriteBatch);
                    e.DrawHealthBar(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        private void DrawOnRenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTest);

            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            foreach (var obj in placedObjects)
            {
                obj.Draw(spriteBatch);
            }

            //GraphicsDevice.Clear(Color.Transparent);

            //spriteBatch.Draw(transTex, Vector2.Zero, Color.White);
            //spriteBatch.Draw(catTex, new Vector2(100,100), Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        private void MakePath()
        {
            //default level
            path.AddPoint(Vector2.Zero);
            path.SetPos(0, Vector2.Zero);
            path.AddPoint(new Vector2(160, 200));
            path.AddPoint(new Vector2(500, 150));
            path.AddPoint(new Vector2(550, 200));
            path.AddPoint(new Vector2(750, 600));
            path.AddPoint(new Vector2(650, 700));
            path.AddPoint(new Vector2(250, 700));
            path.AddPoint(new Vector2(250, 550));
            path.AddPoint(new Vector2(300, 400));
            path.AddPoint(new Vector2(700, 300));
            path.AddPoint(new Vector2(windowWidth, windowHeight));
        }

        public bool CanPlace(GameObject g)
        {
            if (mousePos.X > 0 && mousePos.X + g.tex.Width < windowWidth && mousePos.Y > 0 && mousePos.Y + g.tex.Height < windowHeight)
            {
                Color[] pixels = new Color[g.tex.Width * g.tex.Height];
                Color[] pixels2 = new Color[g.tex.Height * g.tex.Width];
                g.tex.GetData<Color>(pixels2);
                renderTest.GetData(0, g.hitbox, pixels, 0, pixels.Length);
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

        private bool CheckDistance(Tower t)
        {
            foreach (Enemy e in enemies)
            {
                if (GetDistance(t.GetTowerPos, e.pos) < t.Reach)
                {
                    t.ClosestEnemyPos = e.pos;
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
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }

        private void BulletCollision(Bullet b)
        {
            foreach (Enemy e in enemies)
            {
                if (b.IsColliding(e.hitbox))
                {
                    System.Diagnostics.Debug.WriteLine("HIT");
                    e.HealthMaster(b.Damage);
                    bullets.Remove(b);
                }
            }
        }

        private void WaveHandler(GameTime gameTime)
        {
            switch (waveNumber)
            {
                case 0:
                    if (enemies.Count <= 0)
                    {
                        waveTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (waveTimer >= 10000)
                        {
                            waveNumber = prevWaveNumber + 1;
                        }
                    }
                    break;
                case 1:
                    //method för spawning enemies
                    enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 0));
                    enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 1));
                    prevWaveNumber = waveNumber;
                    waveNumber = 0;
                    break;

                case 2:
                    enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 0));
                    enemies.Add(new Enemy(tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), new Rectangle(0, 0, tinyCatTex.Height, tinyCatTex.Width), 1));
                    prevWaveNumber = waveNumber;
                    waveNumber = 0;
                    break;


                    //SET WINCASE
            }
        }

        private void CheckAffordTower()
        {
            if (chosenTower.Cost > EconomyTracker.GetCoins())
            {
                chosenTower = null;
            }
        }

        private void PlaceTowerMode()
        {
            chosenTower.tint = Color.Red;
            if (CanPlace(chosenTower) == true)
            {
                chosenTower.tint = Color.White;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    particleEmitter = new ParticleEmitter(mousePos, 5000f, 10);
                    chosenTower.GetTowerPos = mousePos;
                    placedObjects.Add(chosenTower);
                    EconomyTracker.AlterCoins(-chosenTower.Cost);
                    changedRender = true;
                    chosenTower = null;
                }
            }
        }

    }
}
