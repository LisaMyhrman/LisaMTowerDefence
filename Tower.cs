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


        public Tower(Texture2D texture, Vector2 position, Rectangle hitbox) : base(texture, position, hitbox)
        {
            //isActive = false;
            timer = 0;
            //bullets = new List<Bullet>();
            shooting = false;
            //bulletType = new Bullet(tex, pos, hitbox, new Vector2(1, 1), 1);
            midPos = new Vector2(position.X + texture.Width /2, position.Y + texture.Height / 2);   
        }


        public void Update(GameTime gameTime)
        {
            Shooting();
            timer += gameTime.ElapsedGameTime.Milliseconds;
      
        }

        private void Shooting()
        {
            if(timer >= 5000)
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
            return new Vector2(1,1);
        }

        public bool isShooting
        {
            get { return shooting; }
            set { shooting = value; }
        }

        public Bullet typeOfBullet
        {
            get { return new Bullet(Assets.TinyCatTex, midPos, hitbox, GetDirection(), 3); }
        }

        public Vector2 GetTowerPos
        {
            get { return midPos; }
        }


    }
}
