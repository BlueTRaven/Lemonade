using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;
using Lemonade.gui.guiwidget;
using Lemonade.utility;

namespace Lemonade.gui
{
    public class GuiInventory : Gui
    {
        Player player;
        public GuiInventory(Player player)
        {
            this.player = player;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = false;
        }

        public override void Update()
        {
            if (active)
            {
                if (firstOpen)
                {   //Create widgets and such
                    CreateWidgets();
                    firstOpen = false;
                }

                if (Game1.priorityGui == this)
                {
                    GuiWidgetButtonString widgetButtonString = null;
                    GuiWidgetItemSlot widgetInvSlot = null;

                    for (int i = widgets.Count - 1; i >= 0; i--)
                    {
                        if (widgets[i].id.Item1 == WidgetType.ButtonString)
                        {
                            widgetButtonString = (GuiWidgetButtonString)widgets[i];
                            widgetButtonString.Update();
                        }

                        if (widgets[i].id.Item1 == WidgetType.ItemSlot)
                        {
                            widgetInvSlot = (GuiWidgetItemSlot)widgets[i];
                            widgetInvSlot.Update();
                        }

                        if (!widgets[i].active)
                            widgets.RemoveAt(i--);
                    }
                }
            }
        }

        public void Open()
        {
            oldPriorityGui = Game1.priorityGui;
            Game1.priorityGui = this;
            active = true;
        }

        public void Close()
        {
            Game1.priorityGui = oldPriorityGui;
            active = false;
        }


        public void CreateWidgets()
        {
            Color[] button1Colors = new Color[] { Color.Salmon, Color.DarkSalmon, Color.Orange };

            for (int x = 0; x < 16; x++)
            {
                createInventorySlot(new Rectangle(8 + x * (64 + 16), 256, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, x), button1Colors, player);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                GuiWidgetItemSlot widgetInvSlot = null;

                PrimiviteDrawing.DrawRectangle(null, batch, bounds, Color.Black);
                foreach (GuiWidget widget in widgets)
                {
                    widget.Draw(batch);
                    if (widget.id.Item1 == WidgetType.ItemSlot)
                    {
                        widgetInvSlot = (GuiWidgetItemSlot)widget;

                        widgetInvSlot.DrawToolTip(batch);
                    }
                }
                foreach (GuiWidget widget in widgets)
                {
                    if (widget.id.Item1 == WidgetType.ItemSlot)
                    {
                        widgetInvSlot = (GuiWidgetItemSlot)widget;

                        widgetInvSlot.DrawToolTip(batch);
                    }
                }
            }
        }
    }
}
