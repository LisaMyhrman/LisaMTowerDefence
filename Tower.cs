using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//MAKE UNDERCLASSES DEPENDING ON WHAT KIND OF TOWER

namespace LisaMTowerDefence
{
    internal class Tower : GameObject
    {
        //private bool isActive;
        private float timer;
        //private List<Bullet> bullets;
        private bool shooting;
        private Bullet bulletType;
        private Vector2 midPos;
        private Vector2 closestEnemyPos;


        public Tower(Texture2D texture, Vector2 position, Rectangle hitbox) : base(texture, position, hitbox)
        {
            //isActive = false;
            timer = 0;
            //bullets = new List<Bullet>();
            shooting = false;
            //bulletType = new Bullet(tex, pos, hitbox, new Vector2(1, 1), 1);
            midPos = new Vector2(position.X + texture.Width /2, position.Y + texture.Height / 2);
            closestEnemyPos = new Vector2(0, 0);
        }


        public void Update(GameTime gameTime)
        {
            Shooting();
            timer += gameTime.ElapsedGameTime.Milliseconds;
      
        }

        private void Shooting()
        {
            if(timer >= 2000)
            {
//SPAWN NEW BULLET, GIVE OUT POSITION FROM TOWER HERE?
                System.Diagnostics.Debug.WriteLine("shot");
                timer = 0;
                shooting = true;
                //get tower information(speed of bullets, etc)
                //send true boolean to gamemanager
            }
            else
            {
                shooting = false;
            }
        }

        private Vector2 GetDirection()
        {
            float X;
            float Y;
            float distX = midPos.X - closestEnemyPos.X;
            //float X = distX < 0 ? X = -1 : X = 1;
            if(distX < 20) { X = 1; } else if(distX > -20) { X = -1; } else { X = 0; }
            float distY = midPos.Y - closestEnemyPos.Y;
            if (distY < 20) { Y = 1; } else if (distY > -20) { Y = -1; } else { Y = 0; }
            //float Y = distY < 0 ? Y = -1 : Y = 1;
            System.Diagnostics.Debug.WriteLine("x " + X +" y " + Y);
            return new Vector2(X,Y);
        }

        public bool isShooting
        {
            get { return shooting; }
            set { shooting = value; }
        }

        public Bullet typeOfBullet
        {
            get { return new Bullet(Assets.TinyCatTex, midPos, hitbox, GetDirection(), 2); }
        }

        public Vector2 GetTowerPos
        {
            get { return midPos; }
        }

        public Vector2 ClosestEnemyPos
        {
            //get { return closestEnemyPos; }
            set { closestEnemyPos = value; }
        }


    }
}
