﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Lemonade
{
    public static class Logger
    {
        private static readonly string path = "logs\\log.txt";

        public static void CreateNewLogFile()
        {
            Directory.CreateDirectory("logs\\");
            File.Create("logs\\log.txt").Close();
            if (File.Exists(path))
            {
                File.Copy("logs\\log.txt", "logs\\logOld.txt", true);
            }
        }
        public static void Log(string text, bool print)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(text);
            }

            if (print)
                Console.WriteLine("Logged: " + text);
        }
    }
}
