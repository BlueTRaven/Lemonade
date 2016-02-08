using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lemonade
{
    public static class Fonts
    {
        public static SpriteFont munro;
        public static SpriteFont papyrus;

        /// <summary>
        /// Initializes all fonts used in the game. Called in loadcontent, once.
        /// </summary>
        public static void LoadFonts(ContentManager content)
        {
            munro = content.Load<SpriteFont>("fonts/munro-12");
            papyrus = content.Load<SpriteFont>("fonts/papyrus-12");
        }
    }
}
