using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetDialogue : GuiWidget
    {
        SpriteFont font;
        string[] text;
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

            text = Utilities.ReadFile("Content\\strings\\dialogue.txt", forKey).Split(':');
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                letterTimer++;

                if (letterTimer >= textSpeed)
                {
                    letterTimer = 0;
                    count++;
                }
                base.update(gMouse);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
            PrimiviteDrawing.DrawRectangle(null, batch, interiorBounds, colors[0]);

            string wrappedText = Utilities.WrapText(font, text[currentLine], interiorBounds.Width);
            char[] s = wrappedText.ToCharArray();

            string final = null;

            if (count <= s.Length && !finished)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (i <= count)
                    {
                        final = final + s[i];
                    }
                }
            }
            else
            {
                final = wrappedText;
                finished = true;
                count = 0;
            }
            batch.DrawString(font, final, new Vector2(interiorBounds.X, interiorBounds.Y), Color.Black);
        }

        /// <summary>
        /// Changes the text to the new line index and speed.
        /// </summary>
        /// <param name="newSpeed">Rate at which a character appears.</param>
        /// <param name="newLine">line of "text" array to set to. if left at default, it will simply increase the text array index.</param>
        /// <returns>Whether or not there is any more text in the array. if there isn't, it's done.</returns>
        public bool ChangeText(int newSpeed, int newLine = -1)
        {
            finished = false;

            count = 0;
            if (newLine >= 0)
                currentLine = newLine;
            else if (currentLine + 1 < text.Length)
            {
                currentLine++;
            }
            else return true;
            textSpeed = newSpeed;
            return false;
        }
    }
}
