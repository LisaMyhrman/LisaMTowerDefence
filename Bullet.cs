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
        int damage;

        public Bullet(Texture2D texture, Vector2 position, Rectangle hitbox, Vector2 direction, int speed, int damage) : base (texture,position,hitbox)
        {
            this.direction = direction;
            this.speed = speed;
            this.damage = damage;
        }

        public void Update()
        {
            //LOGISTICS REGARDING DIRECTION AND MOVEMENT, working prehaps? hmmm good enough

            pos.X += direction.X * speed;
            pos.Y += direction.Y * speed;
            hitbox.X = (int)pos.X;
            hitbox.Y = (int)pos.Y;

        }
        

        public bool IsColliding(Rectangle otherHitbox)
        {
            return hitbox.Intersects(otherHitbox);
        }

        public int Damage
        {
            get { return damage; }
        }
    
    }
}
