using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.Guis
{
    public class GuiInventory : Gui
    {
        public GuiInventory(Game1 game)
        {
            this.game = game;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = false;
        }

        public override void Update(GameMouse gMouse)
        {
            if (active)
            {
                if (firstOpen)
                {   //Create widgets and such
                    CreateWidgets();
                    firstOpen = false;
                }

                if (game.priorityGui == this)
                {
                    List<GuiWidget> removeList = new List<GuiWidget>();
                    GuiWidgetButtonString widgetButtonString = null;
                    GuiWidgetItemSlot widgetInvSlot = null;

                    foreach (GuiWidget widget in widgets)
                    {
                        if (widget.id.Item1 == "butstring")
                        {
                            widgetButtonString = (GuiWidgetButtonString)widget;
                            widgetButtonString.Update(gMouse);
                        }

                        if (widget.id.Item1 == "invslot")
                        {
                            widgetInvSlot = (GuiWidgetItemSlot)widget;
                            widgetInvSlot.Update(gMouse);
                        }
                    }
                }
            }
        }

        public void Open()
        {
            oldPriorityGui = game.priorityGui;
            game.priorityGui = this;
            active = true;
        }

        public void Close()
        {
            game.priorityGui = oldPriorityGui;
            active = false;
        }


        public void CreateWidgets()
        {
            Color[] button1Colors = new Color[] { Color.Salmon, Color.DarkSalmon, Color.Orange };

            for (int x = 0; x < 16; x++)
            {
                createInventorySlot(new Rectangle(8 + x * (64 + 16), 256, 64, 64), new Tuple<string, int>("invslot", x), button1Colors);
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
                    if(widget.id.Item1 == "invslot")
                    {
                        widgetInvSlot = (GuiWidgetItemSlot)widget;

                        widgetInvSlot.DrawToolTip(batch);
                    }
                }
                foreach (GuiWidget widget in widgets)
                {
                    if (widget.id.Item1 == "invslot")
                    {
                        widgetInvSlot = (GuiWidgetItemSlot)widget;

                        widgetInvSlot.DrawToolTip(batch);
                    }
                }
            }
        }
    }
}
