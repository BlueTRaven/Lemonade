using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.gui.guiwidget
{
    public class GuiWidgetItemSlot : GuiWidget
    {
        private ItemStack item;
        bool drawToolTip = false;

        Color[] colors;

        GameMouse gMouse;
        Player player;

        public GuiWidgetItemSlot(Rectangle setBounds, Tuple<WidgetType, int> id, Color[] colors, GameMouse gMouse, Player player)
        {
            this.id = id;
            bounds = setBounds;

            this.colors = colors;

            this.gMouse = gMouse;
            this.player = player;
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                item = player.inventory[this.id.Item2];

                if (currentState == State.Done)
                {
                    if (gMouse.heldItem != null)
                    {
                        if (item == null)
                        {
                            AddItemToEmptySlot();
                        }
                        else if (item.item.GetType() == gMouse.heldItem.item.GetType() && item.item.id == gMouse.heldItem.item.id)
                        {
                            AddItemToFilledSlot();
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            if (item == gMouse.heldItem)
                                RemoveItemToFilledMouse();
                            else RemoveItemToEmptyMouse();
                        }
                    }
                }
                base.update(gMouse);
            }
        }

        public void AddItemToEmptySlot()
        {
            player.inventory[this.id.Item2] = gMouse.heldItem;
            gMouse.heldItem = null;
        }

        public void AddItemToFilledSlot()
        {
            player.inventory[this.id.Item2].stackSize += gMouse.heldItem.stackSize;
            gMouse.heldItem = null;
        }

        public void RemoveItemToEmptyMouse()
        {
            gMouse.heldItem = player.inventory[this.id.Item2];
            player.inventory[this.id.Item2] = null;
        }
        public void RemoveItemToFilledMouse()
        {
            gMouse.heldItem.stackSize += player.inventory[this.id.Item2].stackSize;
            player.inventory[this.id.Item2] = null;
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
            if (currentState == State.Active || currentState == State.Active2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
            if (currentState == State.Done || currentState == State.Done2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);


            if (item != null && item.item != null && item.item.texture != null)
            {
                batch.Draw(item.item.texture, bounds, Color.White);

                if (item.stackSize > 1)
                    batch.DrawString(Assets.GetFont(Assets.munro12), item.stackSize.ToString(), new Vector2(bounds.Right, bounds.Bottom), Color.White);
            }
        }

        public void DrawToolTip(SpriteBatch batch)
        {
            if (item != null && item.item.texture != null)
            {
                if (currentState == State.Hot)
                {
                    batch.DrawString(Assets.GetFont(Assets.munro12), item.item.name, gMouse.position, Color.White);
                    for (int i = 0; i < item.item.description.Count(); i++)
                    {
                        if (item.item.description[i] != null)
                            batch.DrawString(Assets.GetFont(Assets.munro12), item.item.description[i], new Vector2(gMouse.position.X, gMouse.position.Y + 12 * (i + 1)), Color.White);
                    }

                }
            }
        }
    }
}
