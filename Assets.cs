using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal class Assets
    {
        public static Texture2D TinyCatTex, catTex;

        public static void LoadTextures(ContentManager cm)
        {
            TinyCatTex = cm.Load<Texture2D>("cat");
            catTex = cm.Load<Texture2D>("fatcat");
            
        }
    }
}
