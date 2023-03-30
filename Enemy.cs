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



        public Enemy(Texture2D texture, Vector2 position, Rectangle hitbox, float speed, int health) : base(texture, position, hitbox)
        {
            this.speed = speed;
            posOnPath = 0;
            this.health = health;
        }

        //MAKE HITBOX SLIGHTLY SMALER FOR EASE?, PIXELPERFECT COLLISIONS?

        public void Update()
        {
            posOnPath = posOnPath + speed;
            hitbox.X = (int)pos.X;
            hitbox.Y = (int)pos.Y;

            //if(health < 2)
            //{
            //    //change based on health :)
            //}
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

        //WORKS WITHOUT SPECIFIC DRAW, ONLY NEEDS ORIGIN CHANGED TO MOVE CORRECTLY
    }
}
