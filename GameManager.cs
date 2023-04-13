using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Spline;
using Microsoft.Xna.Framework.Content;
using WindowsForm;
using System.Drawing.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace LisaMTowerDefence
{
    internal class GameManager
    {
        int windowWidth;
        int windowHeight;

        //spline
        SimplePath path;

        public static int playerHealth { get; private set; }

        //rendertarget
        RenderTarget2D renderTest;

        //inputs
        Vector2 mousePos;
        MouseState mouseState;
        KeyboardState keyState;

        //enemies
        float enemyStartPos;
        List<Enemy> enemies = new List<Enemy>();
        float enemyOffSet;

        //towers
        Tower chosenTower = null;

        //ska listan innehålla almänna towers, beroende på olika typer osvosv?
        List<Tower> placedObjects = new List<Tower>();

        //shootingtests
        List<Bullet> bullets = new List<Bullet>();


        //partikeltest
        List<ParticleEmitter> particleEmitters = new List<ParticleEmitter>();

        //waves
        int waveNumber;
        public static int prevWaveNumber { get; private set; }
        public static float waveTimer { get; private set; }
        public static float waveCoolDown { get; private set; }
        List<Enemy> waves = new List<Enemy>();

        //overlay
        Overlay overlay;

        //win/lose
        WinForm winForm;
        LoseForm loseForm;
        PausForm pausForm;
        public bool lost { get; private set; }
        public bool paused { get; set; }

        public void LoadGame(GraphicsDevice graphics, ContentManager Content)
        {
            windowWidth = Game1.windowWidth;
            windowHeight = Game1.windowHeight;

            //path
            path = new SimplePath(graphics);
            path.Clean();


            playerHealth = 3;
            waveCoolDown = 5000;

            waveNumber = 1;

            Assets.Load(Content);

            winForm = new WinForm();
            loseForm = new LoseForm();
            pausForm = new PausForm();
            paused = false;

            //music
            MediaPlayer.Play(Assets.song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.4f;
            //isMusicOn = true;

            overlay = new Overlay();
            renderTest = new RenderTarget2D(graphics, windowWidth, windowHeight);
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            MakePath();
            enemyStartPos = path.beginT;
            enemyOffSet = 200f;
        }

   

        public void Update(GameTime gameTime)
        {
            WaveHandler(gameTime);

            //inputs
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            //paus
            if (keyState.IsKeyDown(Keys.P))
            {
                paused = true;
            }

            PickTower();

            if(playerHealth <= 0)
            {
                loseForm.Show();
                lost = true;
            }

            //update all objects

            for(int i = 0; i < particleEmitters.Count; i++)
            {
                particleEmitters[i].Update(gameTime);
                if (particleEmitters[i].IsActive == false)
                {
                    particleEmitters.Remove(particleEmitters[i]);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].pos = path.GetPos(enemies[i].positionOnPath);
                enemies[i].Update(gameTime);
                System.Diagnostics.Debug.WriteLine(i + " " + enemies[i].GetMidPos);
                if (enemies[i].Health <= 0)
                {
                    EconomyTracker.AlterCoins(enemies[i].Value);
                    particleEmitters.Add(new ParticleEmitter(enemies[i].GetMidPos, 5000f, 30));
                    enemies.RemoveAt(i);
                    break;
                }
                CheckEnemyInBounds(enemies[i]);
            }

            foreach (Tower t in placedObjects)
            {
                t.Update(gameTime);
                if (CheckDistance(t))
                {
                    //t.Animator();
                    bullets.Add(t.Shooting());
                }
            }

            //forloop instead of foreach where collection is edited while looping
            for (int i = 0; i < bullets.Count; i++)
            {
                if(bullets[i] != null)
                {
                    bullets[i].Update();
                    if(!CheckDistanceBullets(bullets[i]))
                    {
                        BulletCollision(bullets[i]);
                    }
                }
            }

            overlay.Update();
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //rendertargetmethod first
            DrawOnRenderTarget(graphics, spriteBatch);

            //rendertarget changed, new spritebatch begins
            spriteBatch.Begin();
            graphics.Clear(Color.White);
            spriteBatch.Draw(Assets.level1Background, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderTest, Vector2.Zero, Color.White);

            //drawing all objects

            for (int i = 0; i < particleEmitters.Count; i++)
            {
                particleEmitters[i].Draw(spriteBatch);
            }

            if (chosenTower != null)
                chosenTower.Draw(spriteBatch);

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i]!=null)
                bullets[i].Draw(spriteBatch);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
                enemies[i].DrawHealthBar(spriteBatch);
            }

            overlay.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawOnRenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            //drawing on the rendertarget for placed towers
            graphicsDevice.SetRenderTarget(renderTest);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (var obj in placedObjects)
            {
                obj.Draw(spriteBatch);
            }
            spriteBatch.Draw(Assets.renderTargetBackground, Vector2.Zero, Color.White);
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

        private void PickTower()
        {
            if (chosenTower == null)
            {
                if (keyState.IsKeyDown(Keys.D1))
                {
                    chosenTower = new Tower(Assets.granny1, mousePos, 1);
                    CheckAffordTower();
                }
                else if (keyState.IsKeyDown(Keys.D2))
                {
                    chosenTower = new Tower(Assets.granny3, mousePos, 2);
                    CheckAffordTower();
                }
                else if (keyState.IsKeyDown(Keys.D3))
                {
                    chosenTower = new Tower(Assets.granny2, mousePos, 3);
                    CheckAffordTower();
                }
            }
            else
            {
                chosenTower.pos = mousePos;
                chosenTower.hitbox.X = (int)mousePos.X;
                chosenTower.hitbox.Y = (int)mousePos.Y;
                PlaceTowerMode();
                if (keyState.IsKeyDown(Keys.E))
                {
                    chosenTower = null;
                }
            }
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
            //checks if an enemy is in reach for the tower
            foreach (Enemy e in enemies)
            {
                if (GetDistance(t.GetTowerPos, e.GetMidPos) < t.Reach)
                {
                    t.ClosestEnemyPos = e.GetMidPos;
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        private bool CheckDistanceBullets(Bullet b)
        {
            if (GetDistance(b.spawnPos, b.pos) >= 1000)
            {
                bullets.Remove(b);
                return true;
            }
            else return false;
        }

        private float GetDistance(Vector2 one, Vector2 two)
        {
            //used in checkdistance
            float X = one.X - two.X;
            float Y = one.Y - two.Y;
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }

        private void CheckEnemyInBounds(Enemy e)
        {
            //checks if an enemy has left the window, takes off health(fish) from player
            if (e.GetMidPos.X >= windowWidth && e.GetMidPos.Y >= windowHeight)
            {
                playerHealth--;
                enemies.Remove(e);
            }
        }

        private void BulletCollision(Bullet b)
        {
            foreach (Enemy e in enemies)
            {
                if (b.IsColliding(e.hitbox))
                {
                    e.HealthPowerMaster(b.Damage, b.slowing, b.stunning);
                    bullets.Remove(b);
                    
                }
            }
        }

        private async void WaveHandler(GameTime gameTime)
        {
            //switch for waves, changes to case 0 in order to not spawn multiple waves more than once.
            switch (waveNumber)
            {
                case 0:
                    if (enemies.Count <= 0)
                    {
                        //if wave is last and all enemies are killed, win
                        if(prevWaveNumber == 4)
                        {
                            winForm.Show();
                        }
                        else
                        {
                        //wait for next wave
                            waveTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (waveTimer >= waveCoolDown)
                            {
                                waveNumber = prevWaveNumber + 1;
                            }
                        }
                    }
                    break;
                case 1:
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet, enemyStartPos), 2));
                    AfterWave();
                    break;

                case 2:
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet, enemyStartPos), 1));
                    AfterWave();
                    break;
                case 3:
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet, enemyStartPos), 1));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet*2, enemyStartPos), 2));
                    AfterWave();
                    break;
                case 4:
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet*2, enemyStartPos), 2));
                    enemies.Add(new Enemy(Assets.tinyCatTex, new Vector2(enemyStartPos - enemyOffSet*3, enemyStartPos), 2));
                    AfterWave();
                    break;
            }
        }

        private void AfterWave()
        {
            prevWaveNumber = waveNumber;
            waveNumber = 0;
            waveTimer = 0;
        }

        private void CheckAffordTower()
        {
            if (chosenTower.Cost > EconomyTracker.currentCoins)
            {
                chosenTower = null;
            }
        }

        private void PlaceTowerMode()
        {
            //if tower cannot be placed, it is red
            chosenTower.tint = Color.Red;
            if (CanPlace(chosenTower) == true)
            {
            //if placeable, turns white
                chosenTower.tint = Color.White;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
            //player purchases tower and it gets placed
                    chosenTower.GetTowerPos = mousePos;
                    placedObjects.Add(chosenTower);
                    EconomyTracker.AlterCoins(-chosenTower.Cost);
                    chosenTower = null;
                }
            }
        }

    }
}
