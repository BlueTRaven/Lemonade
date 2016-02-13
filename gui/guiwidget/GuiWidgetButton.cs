using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetButton : GuiWidget
    {
        SpriteFont font;
        string text;

        Color[] colors;

        public GuiWidgetButton(Rectangle setBounds, Tuple<WidgetType, int> id, string setText, Color textColor, SpriteFont setFont, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            text = setText;
            font = setFont;

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
            if (currentState == State.None)
            {
                outlineWidth = 2;
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[0]);
            }
            if (currentState == State.Hot)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[0]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[1]);
            }
            if (currentState == State.Active || currentState == State.Active2)//|| currentState == State.Done)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
            if (currentState == State.Done || currentState == State.Done2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
        }
    }
}
