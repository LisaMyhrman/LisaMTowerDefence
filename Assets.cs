using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LisaMTowerDefence
{
    internal class Assets
    {
        public static Texture2D tinyCatTex, catTex, catTex2, cat2, cat2_2, yarn1, yarn2, yarn3, level1Background, granny1, granny1_2, granny2, granny2_2, granny3, granny3_2, particle, confusion, renderTargetBackground, fishTex, cookie;
        public static SpriteFont font;
        public static Song song;


        public static void Load(ContentManager cm)
        {
            tinyCatTex = cm.Load<Texture2D>("enemy1");
            catTex = cm.Load<Texture2D>("fatcat");
            catTex2 = cm.Load<Texture2D>("enemy1.2");
            cat2 = cm.Load<Texture2D>("enemy2");
            cat2_2 = cm.Load<Texture2D>("enemy2.2");
            yarn1 = cm.Load<Texture2D>("yarn1");
            yarn2 = cm.Load<Texture2D>("yarn2");
            yarn3 = cm.Load<Texture2D>("yarn3");
            level1Background = cm.Load<Texture2D>("background_level1");
            granny1 = cm.Load<Texture2D>("tant1");
            granny1_2 = cm.Load<Texture2D>("tant1.2");
            granny2 = cm.Load<Texture2D>("tant2");
            granny2_2 = cm.Load<Texture2D>("tant2_2.");
            granny3 = cm.Load<Texture2D>("tant3");
            granny3_2 = cm.Load<Texture2D>("tant3_2");
            particle = cm.Load<Texture2D>("particle");
            confusion = cm.Load<Texture2D>("confusionbar");
            renderTargetBackground = cm.Load<Texture2D>("rendertargetbackground");
            fishTex = cm.Load<Texture2D>("fish1");
            cookie = cm.Load<Texture2D>("cookie");
            font = cm.Load<SpriteFont>("File");
            song = cm.Load<Song>("td_music");

        }
    }
}
