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
        private List<Bullet> bullets;


        public Tower(Texture2D texture, Vector2 position, Rectangle hitbox) : base(texture, position, hitbox)
        {
            //isActive = false;
            timer = 0;
            bullets = new List<Bullet>();
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
            }
        }
    }
}
