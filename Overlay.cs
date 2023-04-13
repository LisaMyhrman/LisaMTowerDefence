using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Overlay
    {
        private Color tint = Color.White;

        public void Update()
        {
            if(GameManager.playerHealth <= 1)
            {
                tint = Color.Red;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //playerhealth
            for(int i = 0; i <= GameManager.playerHealth; i++)
            {
                spriteBatch.Draw(Assets.fishTex, new Vector2(Game1.windowWidth - Assets.fishTex.Width * 2 * i, 20) , tint);
            }

            //wavetimer
            if(GameManager.waveTimer > 0)
            {
                spriteBatch.DrawString(Assets.font, "Time until next Cattack: " + (int)((GameManager.waveCoolDown - GameManager.waveTimer)/1000), new Vector2(300, Game1.windowHeight / 2), Color.Black);
            }

            //coins
            spriteBatch.DrawString(Assets.font, EconomyTracker.currentCoins.ToString(), new Vector2(20, Game1.windowHeight - 70), Color.Black);
            spriteBatch.Draw(Assets.cookie, new Vector2(60, Game1.windowHeight - 60), Color.White);

            //menu
            spriteBatch.DrawString(Assets.font, "Menu: press 'p'", new Vector2(25,0), Color.Black);

            //wavenumber
            spriteBatch.DrawString(Assets.font, "Wave : " + GameManager.prevWaveNumber, new Vector2(Game1.windowWidth - 200, Game1.windowHeight - 50), Color.Black);
        }

    }
}
