using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetDialogue : GuiWidget
    {
        SpriteFont font;
        
        string[] text;
        string wrappedText;
        char[] wrappedChars;
        StringBuilder finalText;

        public int currentLine = 0;

        int textSpeed;  //How fast a letter appears in ticks, 60 = 1 sec
        int letterTimer;    //timer to count up that amount
        int count;

        public bool finished = false;

        Color[] colors;
        public GuiWidgetDialogue(Rectangle setBounds, Tuple<WidgetType, int> id, string forKey, Color textColor, SpriteFont setFont, int textSpeed, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;

            outlineWidth = 4;
            interiorBounds = new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2));

            font = setFont;

            this.textSpeed = textSpeed;

            this.colors = colors;

            text = Utilities.ReadFile("Content\\strings\\dialogue.txt", forKey);//.Split(':');

            wrappedText = Utilities.WrapText(font, text[0], interiorBounds.Width);
            wrappedChars = wrappedText.ToCharArray();

            finalText = new StringBuilder();
        }

        public void Update()
        {
            if (active)
            {
                letterTimer++;

                if (letterTimer >= textSpeed)
                {
                    letterTimer = 0;
                    count++;
                }
                base.update();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
            PrimiviteDrawing.DrawRectangle(null, batch, interiorBounds, colors[0]);

            if (count <= wrappedChars.Length && !finished)
            {
                finalText.Clear();

                for (int i = 0; i < wrappedChars.Length; i++)
                {
                    if (i == 0 && Regex.IsMatch(wrappedChars[i].ToString(), @"^\d+$"))
                    {   //if the first char (and only first) is a number - sets the text speed to that.
                        textSpeed = (int)char.GetNumericValue(wrappedChars[i]);
                        continue;
                    }
                    if (i <= count)
                    {
                        finalText.Append(wrappedChars[i].ToString());
                    }
                }
            }
            else
            {
                finished = true;
                count = 0;
            }

            batch.DrawString(font, finalText.ToString(), new Vector2(interiorBounds.X, interiorBounds.Y), Color.Black);
        }

        /// <summary>
        /// Changes the text to the new line index and speed.
        /// </summary>
        /// <param name="newSpeed">Rate at which a character appears.</param>
        /// <param name="newLine">line of "text" array to set to. if left at default, it will simply increase the text array index.</param>
        /// <returns>Whether or not there is any more text in the array. if there isn't, it's done.</returns>
        public bool ChangeText(int newSpeed = -1, int newLine = -1)
        {
            finished = false;

            count = 0;
            if (newLine >= 0)
                currentLine = newLine;
            else if (currentLine + 1 < text.Length)
            {
                wrappedText = Utilities.WrapText(font, text[currentLine + 1], interiorBounds.Width);
                wrappedChars = wrappedText.ToCharArray();
                currentLine++;
            }
            else return true;

            if (newSpeed != -1)
                textSpeed = newSpeed;
            return false;
        }
    }
}
