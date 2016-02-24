using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;
using Lemonade.gui.guiwidget;

namespace Lemonade.gui
{
    public abstract class Gui
    {
        //protected Game1 game;

        protected Gui oldPriorityGui;

        protected Rectangle bounds;
        protected Vector2 center { get { return bounds.Center.ToVector2(); } }

        protected Texture2D background;
        protected Color backgroundColor;

        public List<GuiWidget> widgets = new List<GuiWidget>();

        public abstract void Update();

        public abstract void Draw(SpriteBatch batch);

        protected bool firstOpen = true;
        public bool active;
        protected bool recieveInput = false;

        protected int type;

        /// <summary>
        /// Creates a dialogue box.
        /// </summary>
        /// <param name="position">Bounds of the created dialogue box.</param>
        /// <param name="text">The key of the dialogue widget. see Content/strings/dialogue.txt for a list of keys.</param>
        /// <param name="color">the color of the text.</param>
        /// <param name="font">The font to use</param>
        /// <param name="textSpeed"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public GuiWidgetDialogue createDialogue(Rectangle position, Tuple<WidgetType, int> id, Color color, SpriteFont font, Color[] colors, string text = "<default>")
        {
            GuiWidgetDialogue widget;
            widget = new GuiWidgetDialogue(position, id, text, color, font, colors);

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

        public GuiWidgetButton createButton(Rectangle position, Tuple<WidgetType, int> id, string text, GuiWidget.Alignment alignment, Color color, SpriteFont font, Color[] colors)
        {
            GuiWidgetButton widget;
            widget = new GuiWidgetButton(position, id, text, alignment, color, font, colors);

            widgets.Add(widget);
            return widget;
        }

        public GuiWidgetItemSlot createInventorySlot(Rectangle setBounds, Tuple<WidgetType, int> id, ItemRestriction type, Color[] colors, Player player)
        {
            GuiWidgetItemSlot widget;
            widget = new GuiWidgetItemSlot(setBounds, id, type, colors, player);

            widgets.Add(widget);
            return widget;
        }

        /// <summary>
        /// Removes all widgets of a type.
        /// </summary>
        /// <param name="type">The type to remove.</param>
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
