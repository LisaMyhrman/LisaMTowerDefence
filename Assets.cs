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
        public static Texture2D tinyCatTex, catTex, catTex2, yarn1, level1Background, granny1, granny1_2, particle, confusion;

        public static void LoadTextures(ContentManager cm)
        {
            tinyCatTex = cm.Load<Texture2D>("enemy1");
            catTex = cm.Load<Texture2D>("fatcat");
            catTex2 = cm.Load<Texture2D>("enemy1.2");
            yarn1 = cm.Load<Texture2D>("yarn1");
            level1Background = cm.Load<Texture2D>("level_path_test");
            granny1 = cm.Load<Texture2D>("tant1");
            granny1_2 = cm.Load<Texture2D>("tant1.2");
            particle = cm.Load<Texture2D>("particle");
            confusion = cm.Load<Texture2D>("confusionbar");
        }
    }
}
