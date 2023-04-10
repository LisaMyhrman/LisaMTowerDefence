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

        public Color tint { get; set; }


        public Tower(Texture2D texture, Vector2 position, Rectangle hitbox, int towerType) : base(texture, position, hitbox)
        {
            timer = 0;
            shooting = false;
            midPos = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            closestEnemyPos = new Vector2(0, 0);
            this.towerType = towerType;
            tint = Color.White;

            //SWITCHCASE??
            if(towerType == 1)
            {
                cost = 1;
                reach = 300;
                reloadShotMiliSec = 2000;
            }
            else if(towerType == 2)
            {
                cost = 3;
                reach = 600;
                reloadShotMiliSec = 2000;
            }
        }


        public void Update(GameTime gameTime)
        {
            Shooting();
            timer += gameTime.ElapsedGameTime.Milliseconds;
      
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, tint);
        }

        //hoppa fram o tillbaka naj
        private void Shooting()
        {
//INTE SNYGGAST ATT HA MIDPOS HÄR

//FIX GRANNY SHOOTING ANIMATION
            midPos.X = (pos.X + tex.Width / 2);
            midPos.Y = (pos.Y + tex.Height / 2);
            if (timer >= reloadShotMiliSec)
            {
                //System.Diagnostics.Debug.WriteLine("shot");
                timer = 0;
                shooting = true;
                //tex = Assets.granny1_2;
            }
            else
            {
                //tex = Assets.granny1;
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
            //System.Diagnostics.Debug.WriteLine("x " + X +" y " + Y);
            return new Vector2(X,Y);
        }

        public bool isShooting
        {
            get { return shooting; }
            set { shooting = value; }
        }


//MAKE LIST OF TOWERS + THEIR TYPE OF BULLET
//new bullet method
        public Bullet typeOfBullet
        {
            get { return new Bullet(Assets.yarn1, midPos, new Rectangle((int)midPos.X, (int)midPos.Y ,Assets.yarn1.Width, Assets.yarn1.Height), GetDirection(), 1); }
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
