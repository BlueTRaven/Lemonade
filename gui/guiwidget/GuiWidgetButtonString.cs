using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetButtonString : GuiWidget
    {
        public enum Alignment
        {
            Center,
            Left,
            Right,
            Top,
            Bottom
        }
        Alignment alignment;
        SpriteFont font;
        string text;

        Color[] colors;

        public GuiWidgetButtonString(Rectangle setBounds, Tuple<WidgetType, int> id, string setText, Alignment align, Color textColor, SpriteFont setFont, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            font = setFont;
            text = setText;//Utilities.WrapText(font, setText, bounds.Width);

            alignment = align;

            this.colors = colors;
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                base.update(gMouse);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = bounds.Center.ToVector2();
            Vector2 origin = size * 0.5f;

            Color color = colors[0];
            if (currentState == State.None)
            {
                color = colors[0];
            }
            if (currentState == State.Hot)
            {
                color = colors[1];
            }
            if (currentState == State.Active || currentState == State.Done)
            {
                color = colors[2];
            }

            if (alignment == Alignment.Left)
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }
            if (alignment == Alignment.Right)
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }
            if (alignment == Alignment.Top)
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }
            if (alignment == Alignment.Bottom)
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            batch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
