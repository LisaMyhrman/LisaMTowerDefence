using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    //kolla accesability
    public class GameObject
    {

        //ändra standard efter inistiering
        public Vector2 pos;
        public Texture2D tex;
        public Rectangle hitbox;

        public GameObject(Texture2D texture, Vector2 position, Rectangle hitbox)
        {
            this.tex = texture;
            this.pos = position;
            this.hitbox = hitbox;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
        }

    }
}
