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
    }
}
