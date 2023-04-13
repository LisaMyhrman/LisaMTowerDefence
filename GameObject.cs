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

        public GameObject(Texture2D standardTex, Vector2 position)
        {
            this.tex = standardTex;
            this.pos = position;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
        }

    }
}
