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
        private int speed;
        public float posOnPath;

        public Enemy(Texture2D texture, Vector2 position, Rectangle hitbox) : base(texture,position,hitbox)
        {
            speed = 2;
            posOnPath = 0;
//SET TILL PATH START
        }

        public void Update()
        {
            posOnPath = posOnPath + speed;
        }

        public float positionOnPath
        {
            get { return posOnPath; }
            set { posOnPath = value; }
        }

        //WORKS WITHOUT SPECIFIC DRAW, ONLY NEEDS ORIGIN CHANGED TO MOVE CORRECTLY
    }
}
