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
    public static class Assets
    {
        public static Dictionary<string, Texture2D> textures = new Dictionary<string,Texture2D>();
        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static void Load(ContentManager content)
        {
            textures.Add("player", content.Load<Texture2D>("textures/player"));
            textures.Add("enemy1", content.Load<Texture2D>("textures/enemy1"));
            textures.Add("tile_grass1", content.Load<Texture2D>("textures/tiles/tile_grass1"));
            textures.Add("tile_test", content.Load<Texture2D>("textures/tiles/tile_test"));

            textures.Add("BG_sky1", content.Load<Texture2D>("textures/bgSky"));
            textures.Add("BG_debuggrid", content.Load<Texture2D>("textures/bgTest"));

            fonts.Add("munro12", content.Load<SpriteFont>("fonts/munro-12"));
            fonts.Add("munro24", content.Load<SpriteFont>("fonts/munro-24"));
            fonts.Add("munro24italic", content.Load<SpriteFont>("fonts/munro-24-italic"));
            fonts.Add("papyrus12", content.Load<SpriteFont>("fonts/papyrus-12"));
        }

        internal static void Unload()
        {
            foreach (KeyValuePair<string, Texture2D> kvp in textures)
                textures[kvp.Key].Dispose();

            textures.Clear();
        }
    }
}
