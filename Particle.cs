using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Particle
    {
        private Vector2 position;
        private int age;
        private Texture2D texture;
        private Vector2 direction;
        

        public Particle(Vector2 position, Texture2D texture, Vector2 direction)
        {
            this.position = position;
            //this.color = color;
            this.texture = texture;
            this.direction = direction;
        }

        public void Update()
        {
            position += direction;
            age++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public int Age
        {
            get { return age; }
        }
    }
}
