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


                for (int x = 0; x < 16; x++)
                {
                    createInventorySlot(new Rectangle(x * (64 + 16), 256, 64, 64), new Tuple<string, int>("invslot", x), button1Colors);
                }
                for (int y = 16; y < 32; y++)
                {
                    createInventorySlot(new Rectangle(y * (64 + 16), 256 + 64, 64, 64), new Tuple<string, int>("invslot", y), button1Colors);
                }
                for (int z = 32; z < 48; z++)
                {
                    createInventorySlot(new Rectangle(z * (64 + 16), 256 + 64 + 64 + 16, 64, 64), new Tuple<string, int>("invslot", z), button1Colors);
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
                    if (widget.currentState != GuiWidget.State.None)
                    {
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
                                GuiWidgetItemSlot widgetInvSlot = (GuiWidgetItemSlot)widget;
                                if (widget.currentState == GuiWidget.State.Hot)
                                {
                                }

                                if (widget.currentState == GuiWidget.State.Done)//&& widget.previousState == GuiWidget.State.Hot)
                                {
                                    if (widgetInvSlot.itemInSlot != null)       //Has an item in slot
                                    {
                                        if (gMouse.heldItem == null)            //Has no item in mouse slot
                                        {
                                            gMouse.heldItem = widgetInvSlot.itemInSlot; //Set the item in mouse slot to inventory slot
                                            widgetInvSlot.itemInSlot = null;            //Set inventory slot nothing (really only visual, not sure if this is really neccessary)
                                            game.world.player.inventory[widget.id.Item2] = null;    //Also sets the player's inventory slot to null.
                                        }
                                        else if (widgetInvSlot.itemInSlot.item.id == gMouse.heldItem.item.id)    //Else, if the item in slot's id is the same as the item in the mouse slot's id
                                        {
                                            widgetInvSlot.itemInSlot.stackSize += gMouse.heldItem.stackSize;    //Increases the item in slot's stack count by the stacksize of the mouse's held item
                                            gMouse.heldItem = null; //Sets the held item to nothing
                                        }
                                    }
                                    else  //Otherwise, if there is no item in the slot
                                    {
                                        widgetInvSlot.itemInSlot = gMouse.heldItem; //Sets the item in slot to that of the mouse's held item
                                        gMouse.heldItem = null;                     //Sets mouse held item to nothing
                                        game.world.player.inventory[widget.id.Item2] = widgetInvSlot.itemInSlot;    //And sets the inventory slot to the item
                                    }
                                }

                                if (widget.currentState == GuiWidget.State.Done2)
                                {
                                    if (widgetInvSlot.itemInSlot != null)       //Has an item in slot
                                    {
                                        if (gMouse.heldItem == null)    //held item is null, so can pick up one item
                                        {
                                            if (widgetInvSlot.itemInSlot.stackSize - 1 != 0)      //if the stacksize - 1 is not equal to 1
                                            {
                                                widgetInvSlot.itemInSlot.stackSize -= 1;    //Decreases the item in slot's stack count by one
                                                gMouse.heldItem = widgetInvSlot.itemInSlot; //Sets the held item to the same as the item in the slot, so it's picked up.
                                                gMouse.heldItem.stackSize = 1;
                                            }
                                            else if (widgetInvSlot.itemInSlot.stackSize - 1 <= 0)    //Otherwise if the stacksize is less than or equal to 0, set the item to null
                                            {
                                                widgetInvSlot.itemInSlot.stackSize += 1;    //Increases the item in slot stack size by one
                                                gMouse.heldItem = null;                     //and sets the item to null, as it now has a stacksize of null.
                                            }
                                        }
                                        else
                                        {

                                        }
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
