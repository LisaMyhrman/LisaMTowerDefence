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
    internal class ParticleEmitter
    {
        private Vector2 position;
        private List<Particle> particles = new List<Particle>();
        private Texture2D particleTexture = Assets.particle;
        private float duration;
        private float timer;
        private int amount;
        private bool active;
        private Random rand = new Random();
   

        public ParticleEmitter(Vector2 position, float durationMS, int amount)
        {
            this.position = position;   
            this.duration = durationMS;
            this.amount = amount;
            timer = 0;
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if(timer <= duration)
            {
                if(amount >= 0)
                {
//ADD SPACING
                    particles.Add(new Particle(position, Assets.particle, getDirection()));
                    amount--;
                }
            }
            else
            {
                active = false;
            }

            foreach (Particle p in particles)
            {
                p.Update();
                if (p.Age >= 75)
                {
                    particles.Remove(p);
                    break;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Particle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public bool IsActive
        {
            get { return active; }
        }

        private Vector2 getDirection()
        {
            float dirX = rand.Next(-1, 2);
            float dirY = rand.Next(-1, 2);
            return new Vector2(dirX, dirY);
        }
    }
}
