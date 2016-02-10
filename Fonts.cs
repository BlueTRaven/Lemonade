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
        public static SpriteFont munro12;
        public static SpriteFont munro24;
        public static SpriteFont munro24Italic;
        public static SpriteFont papyrus;

        /// <summary>
        /// Initializes all fonts used in the game. Called in loadcontent, once.
        /// </summary>
        public static void LoadFonts(ContentManager content)
        {
            munro12 = content.Load<SpriteFont>("fonts/munro-12");
            munro24 = content.Load<SpriteFont>("fonts/munro-24");
            munro24Italic = content.Load<SpriteFont>("fonts/munro-24-italic");
            papyrus = content.Load<SpriteFont>("fonts/papyrus-12");
        }
    }
}
