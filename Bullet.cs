using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Bullet : GameObject
    {
        Vector2 direction;
        int speed;

        public Bullet(Texture2D texture, Vector2 position, Rectangle hitbox, Vector2 direction, int speed) : base (texture,position,hitbox)
        {
            this.direction = direction;
            this.speed = speed;
        }

        public void Update()
        {
//LOGISTICS REGARDING DIRECTION AND MOVEMENT
            this.pos.X += direction.X * speed;
            this.pos.Y += direction.Y * speed;
        }
    }
}
