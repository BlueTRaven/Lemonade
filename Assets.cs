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
{   //not sure if I should use enums...
    public static class Assets
    {
        public static readonly string entityPrefix = "entity_";
        public static readonly string entity_player = "entity_player";
        public static readonly string entity_enemy1 = "entity_enemy1";

        public static readonly string tilePrefix = "tile_";
        public static readonly string tile_grass1 = "tile_grass1";
        public static readonly string tile_test = "tile_test";
        public static readonly string tile_rockwall = "tile_rockwall";

        public static readonly string bgPrefix = "bg_";
        public static readonly string bg_sky1 = "bg_sky1";
        public static readonly string bg_debuggrid = "bg_debuggrid";

        public static readonly string itemPrefix = "item_";
        public static readonly string item_knifeRust = "item_knifeRust";
        public static readonly string item_knife = "item_knife";

        public static readonly string munro12 = "munro12";
        public static readonly string munro24 = "munro24";
        public static readonly string munro24italic = "munro24italic";
        public static readonly string papyrus12 = "papyrus12";

        public static Dictionary<string, Texture2D> textures = new Dictionary<string,Texture2D>();
        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Texture2D GetTexture(string name)
        {
            try
            {
                return textures[name];
            }
            catch(KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load texture file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public static SpriteFont GetFont(string name)
        {
            try
            {
                return fonts[name];
            }
            catch(KeyNotFoundException e)
            {
                Logger.Log("Couldn't find and/or load spritefont file '" + name + "'!\n" + e.ToString(), true);
                return null;
            }
        }

        public static void Load(ContentManager content)
        {   
            //-- Entity textures --//
            textures.Add("entity_player", content.Load<Texture2D>("textures/player"));
            textures.Add("entity_enemy1", content.Load<Texture2D>("textures/enemy1"));

            //-- Item textures --//
            textures.Add("item_knifeRust", content.Load<Texture2D>("textures/knifeRust"));
            textures.Add("item_knife", content.Load<Texture2D>("textures/knife"));

            //-- Tile textures --//
            textures.Add("tile_grass1", content.Load<Texture2D>("textures/tiles/tile_grass1"));
            textures.Add("tile_test", content.Load<Texture2D>("textures/tiles/tile_test"));
            textures.Add("tile_rockwall", content.Load<Texture2D>("textures/tiles/tile_rockwall"));

            //-- BG textures --//
            textures.Add("bg_sky1", content.Load<Texture2D>("textures/bgSky"));
            textures.Add("bg_debuggrid", content.Load<Texture2D>("textures/bgTest"));

            //-- Fonts --//
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
