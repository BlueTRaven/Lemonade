﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
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

using Lemonade.utility;

namespace Lemonade.utility
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

        /// <summary>
        /// Reads a file for text contatined in the tag.
        /// </summary>
        /// <param name="path">The path of the file to read</param>
        /// <param name="tag">The tag used to locate text.</param>
        /// <returns>The whole of the text between the tag.</returns>
        public static string[] ReadFile(string path, string tag)
        {
            List<string> taggedLines = new List<string>();
            bool everFound = false;
            bool lineFound = false;
            //int index = 0;
            foreach (string line in File.ReadLines("Content\\" + path))
            {
                if (line.Contains(tag))
                {
                    lineFound = !lineFound;
                    everFound = true;
                    continue;
                }
                else
                {
                    if (lineFound)
                    {
                        taggedLines.Add(line);
                        //taggedLines[index] = line;
                        //taggedLines.Append(line);
                        //++index;
                    }
                }
            }

            if (!everFound)
            {
                Logger.Log("Could not find specified tag '" + tag + "' in file '" + path + "'", true);
                return new string[] {""};
            }
            return taggedLines.ToArray();//taggedLines.ToString().Split(new char[] {':'});
        }

        /// <summary>
        /// Creates a rectangle from a string array.
        /// </summary>
        /// <param name="parse">Array of the strings to parse.</param>
        /// <returns></returns>
        public static Rectangle CreateRectangleFromStrings(string[] parse)
        {
            int[] parsedStrings = new int[4];

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    parsedStrings[i] = Int32.Parse(parse[i]);
                }
                return new Rectangle(parsedStrings[0], parsedStrings[1], parsedStrings[2], parsedStrings[3]);
            }
            catch (Exception e)
            {
                Logger.Log("Couldn't create Rectangle from array.\n" + e, true);
                return Rectangle.Empty;
            }
        }

        public static Vector2 CreateVector2FromStrings(string[] parse)
        {
            int[] parsedStrings = new int[2];

            try
            {
                for (int i = 0; i < 2; i++ )
                {
                    parsedStrings[i] = Int32.Parse(parse[i]);
                }
                return new Vector2(parsedStrings[0], parsedStrings[1]);
            }
            catch (Exception e)
            {
                Logger.Log("Couldn't create Vector2 from array.\n" + e, true);
                return Vector2.Zero;
            }
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
