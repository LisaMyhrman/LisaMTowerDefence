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
using SharpDX.DirectWrite;

namespace LisaMTowerDefence
{
    internal class Tower : GameObject
    {
        
        //STANDARDTOWER

        private float timer;
        private bool shooting;
        private Vector2 midPos;
        private Vector2 closestEnemyPos;
        private int cost;
        private int reloadShotMiliSec;
        private int reach;
        private int towerType;
        private Texture2D idleTex;
        private Texture2D shootTex;
 
        public Color tint { get; set; }


        public Tower(Texture2D texture, Vector2 position, int towerType) : base(texture, position)
        {
            timer = 0;
            shooting = false;
            this.towerType = towerType;
            tint = Color.White;

            //SWITCHCASE??
            if(towerType == 1)
            {
                cost = 1;
                reach = 300;
                reloadShotMiliSec = 2000;
                idleTex = Assets.granny1;
                shootTex = Assets.granny1_2;
            }
            else if(towerType == 2)
            {
                cost = 3;
                reach = 600;
                reloadShotMiliSec = 2000;
                idleTex = Assets.granny3;
                shootTex = Assets.granny3_2;
            }
            else if(towerType == 3)
            {
                cost = 4;
                reach = 500;
                reloadShotMiliSec = 1000;
                idleTex = Assets.granny2;
                shootTex = Assets.granny2_2;
            }
        }


        public void Update(GameTime gameTime)
        {
            midPos.X = (pos.X + tex.Width / 2);
            midPos.Y = (pos.Y + tex.Height / 2);
            Animator();
            timer += gameTime.ElapsedGameTime.Milliseconds;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, tint);
        }

        public Bullet Shooting()
        {
            if (timer >= reloadShotMiliSec)
            {
                timer = 0;
                shooting = true;
                return new Bullet(Assets.yarn1, midPos, GetDirection(), towerType);
            }
            else
            {
               return null;
            }
        }

        private Vector2 GetDirection()
        {
            float x = closestEnemyPos.X - midPos.X;
            float y = closestEnemyPos.Y - midPos.Y;
            Vector2 delta = new Vector2(x,y);
            delta.Normalize();
            return delta;
        }

        //isneeded?
        public bool isShooting
        {
            get { return shooting; }
            set { shooting = value; }
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

//FIX PROPERTIES
        public int Cost
        {
            get { return cost; }
        }

        public int Reach
        {
            get { return reach; }
        }

        public void Animator()
        {
            if (shooting == true)
            {
                tex = shootTex;
                if(timer >= reloadShotMiliSec / 2 )
                {
                    tex = idleTex;
                    shooting = false;
                }
            }

        }
    }
}
