using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Enemy : GameObject
    {
        private float speed;
        private float posOnPath;
        private int health;
        private int startHealth;
        private int value;
        private int enemyType;
        private float animateTimer;
        private float animationInterval;
        private Vector2 midPos;
        GameManager manager;


        public Enemy(Texture2D texture, Vector2 position, int enemyType) : base(texture, position)
        {
            manager = new GameManager();
            //posOnPath = 0;
            posOnPath = position.X;
            animateTimer = 0;
            animationInterval = 1000;
            this.enemyType = enemyType;


            if (enemyType == 1)
            {
                speed = 1.0f;
                health = 2;
                value = 2;
                tex = Assets.tinyCatTex;
            }
            else if (enemyType == 2)
            {
                speed = 2.0f;
                health = 3;
                value = 3;
                tex = Assets.cat2;
            }
            startHealth = health;

        }

        //MAKE HITBOX SLIGHTLY SMALER FOR EASE?, PIXELPERFECT COLLISIONS?

        public void Update(GameTime gameTime)
        {
            animateTimer += gameTime.ElapsedGameTime.Milliseconds;
            posOnPath = posOnPath + speed;
            hitbox.X = (int)pos.X;
            hitbox.Y = (int)pos.Y;
            midPos.X = pos.X + tex.Width / 2;
            midPos.Y = pos.Y + tex.Height / 2;
            Animate();
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            //bars need to be bigger, draw new/scale them
            spriteBatch.Draw(Assets.confusion, new Rectangle((int)pos.X, (int)pos.Y - Assets.confusion.Height * 2, (Assets.confusion.Width / startHealth) * health * 2, Assets.confusion.Height * 2), new Rectangle(0, 0, (Assets.confusion.Width / startHealth) * health, Assets.confusion.Height), Color.White);
        }

        public float positionOnPath
        {
            get { return posOnPath; }
            set { posOnPath = value; }
        }

        //slowing effect works, more powers here?
        public void HealthPowerMaster(int damage, bool slowing)
        {
            health = health - damage;
            if (slowing)
            {
                speed = speed / 2;
            }
        }

        public int Health
        {
            get { return health; }
        }

        public int Value
        {
            get { return value; }
        }

        public void Animate()
        {
            //fix animation for different sprites
            if (animateTimer >= animationInterval)
            {
                if (enemyType == 1)
                { tex = Assets.catTex2; }
                else if(enemyType == 2)
                { tex = Assets.cat2; }

            }
            else
            {
                //tex = Assets.tinyCatTex;
                if (enemyType == 1)
                { tex = Assets.tinyCatTex; }
                else if(enemyType == 2)
                { tex = Assets.cat2_2; }
            }

            if (animateTimer >= animationInterval * 2)
            {
                animateTimer = 0;
            }
        }

        public Vector2 GetMidPos
        {
            get { return midPos; }
        }
    }
}
