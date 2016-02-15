using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Lemonade
{
    public static class Utilities
    {
        public static readonly string playerSave = "player.json";

        private static readonly CultureInfo DecimalParsingInfo = CultureInfo.InvariantCulture;
        private static readonly Random _rnd = new Random();

        public static Vector2 ToVector2(this string[] args, int offset = 0)
        {
            ReplaceCommasWithDots(args);

            if (args.Length == 1)
                return new Vector2(float.Parse(args[0 + offset], DecimalParsingInfo));
            
            return new Vector2(
                float.Parse(args[0 + offset], DecimalParsingInfo),
                float.Parse(args[1 + offset], DecimalParsingInfo));
        }

        private static void ReplaceCommasWithDots(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                args[i] = args[i].Replace(',', '.');
            }
        }

        /// <summary>
        /// Gets the origin points of the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to get the values from.</param>
        /// <returns>A tuple containing the top left and bottom right points of the rectangle.</returns>
        public static Tuple<Vector2, Vector2> GetOriginRectangle(Rectangle rect)
        {
            return new Tuple<Vector2, Vector2>(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
        }

        public static string ReadFile(string path, string tag)
        {
            var paneContent = new StringBuilder();
            bool lineFound = false;
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains(tag))
                {
                    lineFound = !lineFound;
                }
                else
                {
                    if (lineFound)
                    {
                        paneContent.Append(line);
                    }
                }
            }
            return paneContent.ToString();
        }

        public static string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == "")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                        else
                        {
                            sb.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
