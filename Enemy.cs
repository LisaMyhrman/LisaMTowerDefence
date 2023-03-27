using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Enemy : GameObject
    {
        public Enemy(Texture2D texture, Vector2 position, Rectangle hitbox) : base(texture,position,hitbox)
        {

        }


        //WORKS WITHOUT SPECIFIC DRAW, ONLY NEEDS ORIGIN CHANGED TO MOVE CORRECTLY
    }
}
