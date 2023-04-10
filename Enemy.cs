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

        public Enemy(Texture2D texture, Vector2 position, Rectangle hitbox, int enemyType) : base(texture, position, hitbox)
        {
            posOnPath = 0;
            animateTimer = 0;
            animationInterval = 1000;

            if(enemyType == 0)
            {
                speed = 1.0f;
                health = 2;
                value = 2;
            }
            else if(enemyType == 1)
            {
                speed = 2.0f;
                health = 3;
                value = 3;
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
            //System.Diagnostics.Debug.WriteLine(animateTimer);
            Animate();
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
//bars need to be bigger, draw new/scale them
            spriteBatch.Draw(Assets.confusion, new Rectangle((int)pos.X, (int)pos.Y - Assets.confusion.Height, (Assets.confusion.Width / startHealth) * health, Assets.confusion.Height), new Rectangle(0, 0, (Assets.confusion.Width / startHealth) * health, Assets.confusion.Height), Color.White);
        }

        public float positionOnPath
        {
            get { return posOnPath; }
            set { posOnPath = value; }
        }

        public void HealthMaster(int damage)
        {
            health = health - damage;
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
            if(animateTimer >= animationInterval)
            {
                tex = Assets.catTex2;
            }
            else
            {
                tex = Assets.tinyCatTex;
            }

            if(animateTimer >= animationInterval*2)
            {
                animateTimer = 0;
            }
        }

        //WORKS WITHOUT SPECIFIC DRAW, ONLY NEEDS ORIGIN CHANGED TO MOVE CORRECTLY
    }
}
