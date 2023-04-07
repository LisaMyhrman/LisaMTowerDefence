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
using SharpDX.Direct3D9;

//MAKE UNDERCLASSES DEPENDING ON WHAT KIND OF TOWER

namespace LisaMTowerDefence
{
    internal class Tower : GameObject
    {
        
        //STANDARDTOWER

        private float timer;
        private bool shooting;
        private Bullet bulletType;
        private Vector2 midPos;
        private Vector2 closestEnemyPos;
        private int cost;
        private int reloadShotMiliSec;
        private int reach;
        private int towerType;


        public Tower(Texture2D texture, Vector2 position, Rectangle hitbox, int towerType) : base(texture, position, hitbox)
        {
            timer = 0;
            shooting = false;
            midPos = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            closestEnemyPos = new Vector2(0, 0);
            //SPECIFIK FOR EACH TOWERTYPE
            //this.cost = cost;
            //this.reloadShotMiliSec = reloadShotMiliSec;
            //this.reach = reach;
            //int cost, int reloadShotMiliSec, int reach
            this.towerType = towerType;

            //SWITCHCASE
            if(towerType == 1)
            {
                cost = 1;
                reach = 300;
                reloadShotMiliSec = 2000;
            }
            else if(towerType == 2)
            {
                cost = 2;
                reach = 600;
                reloadShotMiliSec = 500;
            }




            //cost = 1;
            //reloadShotMiliSec = 2000;
            //cost, reloadShotmilisec, bulletType?

        }


        public void Update(GameTime gameTime)
        {
            Shooting();
            timer += gameTime.ElapsedGameTime.Milliseconds;
      
        }

        private void Shooting()
        {
//INTE SNYGGAST ATT HA MIDPOS HÄR
            midPos.X = (pos.X + tex.Width / 2);
            midPos.Y = (pos.Y + tex.Height / 2);
            if (timer >= reloadShotMiliSec)
            {
                System.Diagnostics.Debug.WriteLine("shot");
                timer = 0;
                shooting = true;
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
            if(distX < 20) { X = 1; } else if(distX > -20) { X = -1; } else { X = 0; }
            float distY = midPos.Y - closestEnemyPos.Y;
            if (distY < 20) { Y = 1; } else if (distY > -20) { Y = -1; } else { Y = 0; }
            System.Diagnostics.Debug.WriteLine("x " + X +" y " + Y);
            return new Vector2(X,Y);
        }

        public bool isShooting
        {
            get { return shooting; }
            set { shooting = value; }
        }


//MAKE LIST OF TOWERS + THEIR TYPE OF BULLET
        public Bullet typeOfBullet
        {
            get { return new Bullet(Assets.TinyCatTex, midPos, new Rectangle((int)midPos.X, (int)midPos.Y ,Assets.TinyCatTex.Width, Assets.TinyCatTex.Height), GetDirection(), 2, 1); }
        }

        public Vector2 GetTowerPos
        {
            get { return midPos; }
            set { pos = value; }
        }

        public Vector2 ClosestEnemyPos
        {
            get { return closestEnemyPos; }
            set { closestEnemyPos = value; }
        }

        public int Cost
        {
            get { return cost; }
        }

        public int Reach
        {
            get { return reach; }
        }
    }
}
