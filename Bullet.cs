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
        int bulletType;
        float rotation;
        float rotationSpeed;

        public Bullet(Texture2D texture, Vector2 position, Rectangle hitbox, Vector2 direction, int bulletType) : base (texture,position,hitbox)
        {
            this.direction = direction;
            this.bulletType = bulletType;
            rotation = 0f;

            if(bulletType == 1)
            {
                speed = 1;
                damage = 1;
                rotationSpeed = 0.1f;
            }
            else if(bulletType == 2)
            {
                speed = 2;
                damage = 1;
                rotationSpeed = 0.5f;
            }
            
        }

        public void Update()
        {
            //LOGISTICS REGARDING DIRECTION AND MOVEMENT, working prehaps? hmmm good enough

            pos.X += direction.X * speed;
            pos.Y += direction.Y * speed;
            hitbox.X = (int)pos.X;
            hitbox.Y = (int)pos.Y;

            rotation += rotationSpeed;
        }
        

        public bool IsColliding(Rectangle otherHitbox)
        {
            return hitbox.Intersects(otherHitbox);
        }

        public int Damage
        {
            get { return damage; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, null ,Color.White, rotation, new Vector2(tex.Width/2, tex.Height/2), 1f, SpriteEffects.None, 0f);
        }

    }
}
