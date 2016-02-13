using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.gui.guiwidget;

namespace Lemonade.gui
{
    public abstract class Gui
    {
        protected Game1 game;

        protected Gui oldPriorityGui;

        protected Rectangle bounds;
        protected Vector2 center { get { return bounds.Center.ToVector2(); } }

        protected Texture2D background;
        protected Color backgroundColor;

        public List<GuiWidget> widgets = new List<GuiWidget>();

        public abstract void Update(GameMouse gMouse);

        public abstract void Draw(SpriteBatch batch);

        protected bool firstOpen = true;
        public bool active;
        protected bool recieveInput = false;

        protected int type;

        public GuiWidgetDialogue createDialogue(Rectangle position, Tuple<WidgetType, int> id, string text, Color color, SpriteFont font, int textSpeed, Color[] colors)
        {
            GuiWidgetDialogue widget;
            widget = new GuiWidgetDialogue(position, id, text, color, font, textSpeed, colors);

            widgets.Add(widget);
            return widget;
        }

        public GuiWidgetButtonString createButtonString(Rectangle position, Tuple<WidgetType, int> id, string text, GuiWidgetButtonString.Alignment align, Color color, SpriteFont font, Color[] colors)
        {
            GuiWidgetButtonString widget;
            widget = new GuiWidgetButtonString(position, id, text, align, color, font, colors);

            widgets.Add(widget);
            return widget;
        }

        public GuiWidgetButton createButton(Rectangle position, Tuple<WidgetType, int> id, string text, Color color, SpriteFont font, Color[] colors)
        {
            GuiWidgetButton widget;
            widget = new GuiWidgetButton(position, id, text, color, font, colors);

            widgets.Add(widget);
            return widget;
        }

        public GuiWidgetItemSlot createInventorySlot(Rectangle setBounds, Tuple<WidgetType, int> id, Color[] colors)
        {
            GuiWidgetItemSlot widget;
            widget = new GuiWidgetItemSlot(setBounds, id, colors, game.mouse, game.world.player);

            widgets.Add(widget);
            return widget;
        }

        public void RemoveWidgetType(WidgetType type)
        {
            int i = 0;
            widgets.ForEach(x =>
            {
                if (x.id.Item1 == type)
                {
                    widgets.RemoveAt(i);
                }
                ++i;
            });
        }
    }
}
