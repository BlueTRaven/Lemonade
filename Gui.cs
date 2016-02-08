using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    /// <summary>
    /// TODO:
    /// gui full (takes up full screen. Healthbar, mana, etc will be on this type)
    /// gui static (takes up a portion of the screen. Inventory, map, etc will be this type)
    /// gui draggable (takes up a portion of the screen, and can be relocated from the top.)
    /// </summary>
    public abstract class Gui
    {
        public enum WidgetType
        {
            Button
        }
        protected Keys keyToOpen;
        protected Game1 game;

        protected Rectangle bounds;
        protected Vector2 center { get { return bounds.Center.ToVector2(); } }

        protected Texture2D background;
        protected Color backgroundColor;

        public List<GuiWidget> widgets = new List<GuiWidget>();

        public abstract void Update(GameMouse gMouse);

        public abstract void Draw(SpriteBatch batch);

        public bool active;

        protected int type;

        public GuiWidgetButton createButton(Rectangle position, Tuple<string, int> id, string text, Color color, SpriteFont font, Color[] colors)
        {
            GuiWidgetButton widget;
            widget = new GuiWidgetButton(position, id, text, color, font, colors);

            widgets.Add(widget);
            return widget;
        }

        public GuiWidgetItemSlot createInventorySlot(Rectangle setBounds, Tuple<string, int> id, Color[] colors)
        {
            GuiWidgetItemSlot widget;
            widget = new GuiWidgetItemSlot(setBounds, id, colors, game);

            widgets.Add(widget);
            return widget;
        }

        public void RemoveWidgetType(string name)
        {
            int i = 0;
            widgets.ForEach(x =>
            {
                if (x.id.Item1 == name)
                {
                    widgets.RemoveAt(i);
                }
                ++i;
            });
        }
    }

    public class GuiFull : Gui
    {
        public GuiFull(Game1 game, Rectangle screenSize, int type, Color color)
        {
            this.game = game;
            bounds = screenSize;

            backgroundColor = color;

            this.type = type;

            if (type == 0)
            {
                keyToOpen = Keys.H;
                Color[] button1Colors = new Color[] { Color.Salmon, Color.DarkSalmon, Color.Orange };
                createButton(new Rectangle((int)center.X - 32, (int)center.Y - 16, 64, 32), new Tuple<string, int>("button", 0), "Test", Color.White, Fonts.munro, button1Colors);

                for (int x = 0; x < 5; x++)
                {
                    createInventorySlot(new Rectangle(x * 64, 256, 64, 64), new Tuple<string, int>("invslot", x), button1Colors);
                }
            }

            if (type == 1)
            {
                Color[] button1Colors = new Color[] { Color.Red, Color.Red, Color.DarkRed };
            }
        }

        public GuiFull(Game1 game, Rectangle screenSize, int type, Texture2D bgTex)
        {
            bounds = screenSize;

            background = bgTex;
            
            this.type = type;

            Color[] button1Colors = new Color[] { };
            //createButton(new Rectangle((int)center.X, (int)center.Y, 64, 32), 0, "Test", Color.White, Fonts.munro, button1Colors);
        }

        public override void Update(GameMouse gMouse)
        {
            /*if (game.keyPress(keyToOpen))
            {
                if (!active)
                    Open();
                else
                    Close();
            }*/
            if (active)
            {
                foreach (GuiWidget widget in widgets)
                {
                    widget.Update(gMouse.currentState);
                    if (type == 0)
                    {
                        if (widget.id.Item1 == "button")
                        {
                            if (widget.id.Item2 == 0)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    Close(); 
                                }
                            }
                        }

                        if (widget.id.Item1 == "invslot")
                        {
                            GuiWidgetItemSlot widgetInvSlot = (GuiWidgetItemSlot) widget;
                            if (widget.currentState == GuiWidget.State.Hot)
                            {
                            }

                            if (widget.currentState == GuiWidget.State.Done)
                            {
                                if (widgetInvSlot.itemInSlot != null)
                                {
                                    if (gMouse.heldItem == null)
                                    {
                                        gMouse.heldItem = widgetInvSlot.itemInSlot;
                                        widgetInvSlot.itemInSlot = null;
                                        game.world.player.inventory[widget.id.Item2] = null;
                                    }
                                    else if(widgetInvSlot.itemInSlot.item.id == gMouse.heldItem.item.id)
                                    {
                                        widgetInvSlot.itemInSlot.stackSize += gMouse.heldItem.stackSize;
                                        gMouse.heldItem.stackSize = 0;
                                        gMouse.heldItem = null;
                                    }
                                }
                                else
                                {
                                    widgetInvSlot.itemInSlot = gMouse.heldItem;
                                    gMouse.heldItem = null;
                                    game.world.player.inventory[widget.id.Item2] = widgetInvSlot.itemInSlot;
                                }
                            }
                        }
                    }

                    if (type == 1)
                    {
                        //if (widget.id == 1)
                        {

                        }
                    }
                }
            }
        }

        public void Open()
        {
            active = true;
            if (type == 0)
            {
                Game1.paused = true;

                //RemoveWidgetType("invslot");
                //RemoveWidgetType("equipslot");

                Color[] button1Colors = new Color[] { Color.Salmon, Color.DarkSalmon, Color.Orange };

                //Player.inventory.Insert(0, null);


                int index = 0;
                foreach (GuiWidget widget in widgets)
                {
                    if (widget.id.Item1 == "invslot")
                    {
                        GuiWidgetItemSlot widgetInvSlot = (GuiWidgetItemSlot) widget;

                        widgetInvSlot.itemInSlot = game.world.player.inventory[widgetInvSlot.id.Item2];
                    }
                    ++index;
                }
            }
        }

        public void Close()
        {
            active = false;
            if (type == 0)
            {
                Game1.paused = false;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                if (backgroundColor != null)
                {
                    PrimiviteDrawing.DrawRectangle(null, batch, bounds, Color.Black);
                }
                if (background != null)
                {

                }

                foreach (GuiWidget widget in widgets)
                {
                    widget.Draw(batch);
                }

                foreach (GuiWidget widget in widgets)
                {
                    if (widget is GuiWidgetItemSlot)
                    {
                        GuiWidgetItemSlot itemSlot = (GuiWidgetItemSlot)widget;
                        itemSlot.DrawToolTip(batch);
                    }
                }
            }
        }
    }

    public class GuiStatic : Gui
    {
        public GuiStatic(int type)
        {

        }

        public override void Update(GameMouse gMouse)
        {
            
        }

        public override void Draw(SpriteBatch batch)
        {
            
        }
    }

    /*public class GuiDraggable : Gui
    {

    }*/
}
