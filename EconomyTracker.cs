﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LisaMTowerDefence
{
    internal static class EconomyTracker
    {
        public static int currentCoins = 2;

        public static void AlterCoins(int value)
        {
            currentCoins += value;
        }
    }
}
