using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetButton : GuiWidget
    {
        public bool drawText = true;
        SpriteFont font;
        public string text;

        Color textColor;

        Color[] colors;

        public GuiWidgetButton(Rectangle setBounds, Tuple<WidgetType, int> id, string setText, Alignment align, Color textColor, SpriteFont setFont, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            text = setText;

            this.align = align;
            this.textColor = textColor;
            font = setFont;

            this.colors = colors;

            outlineWidth = 2;

            interiorBounds = new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - outlineWidth * 2, bounds.Height - outlineWidth * 2);
        }

        public void Update()
        {
            if (active)
            {
                base.update();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (draw)
            {
                Vector2 size = font.MeasureString(text);
                Vector2 pos = bounds.Center.ToVector2();
                Vector2 origin = size * 0.5f;

                if (currentState == State.None)
                {
                    PrimiviteDrawing.DrawRectangle(null, batch, bounds, outlineWidth, colors[1]);
                    PrimiviteDrawing.DrawRectangle(null, batch, interiorBounds, colors[0]);
                }
                if (currentState == State.Hot)
                {
                    PrimiviteDrawing.DrawRectangle(null, batch, bounds, outlineWidth, colors[0]);
                    PrimiviteDrawing.DrawRectangle(null, batch, interiorBounds, colors[1]);
                }
                if (currentState == State.Active || currentState == State.Active2)
                    PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
                if (currentState == State.Done || currentState == State.Done2)
                    PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);

                if (align == Alignment.Left)
                {
                    origin.X += bounds.Width / 2 - size.X / 2;
                }
                if (align == Alignment.Right)
                {
                    origin.X -= bounds.Width / 2 - size.X / 2;
                }
                if (align == Alignment.Top)
                {
                    origin.Y += bounds.Height / 2 - size.Y / 2;
                }
                if (align == Alignment.Bottom)
                {
                    origin.Y -= bounds.Height / 2 - size.Y / 2;
                }

                if (drawText)
                    batch.DrawString(font, text, pos, textColor, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
