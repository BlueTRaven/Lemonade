using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;
using Lemonade.gui.guiwidget;
using Lemonade.utility;
using Lemonade.item;

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

            if (firstOpen)
            {   //Create widgets and such
                CreateWidgets();
                firstOpen = false;
            }

            LoadInventory();
        }

        public void Close()
        {
            Game1.priorityGui = oldPriorityGui;
            active = false;

            SaveInventory();
        }

        public void LoadInventory()
        {
            for (int i = 0; i < 69; i++)
            {
                GuiWidgetItemSlot islot = (GuiWidgetItemSlot)widgets[i];

                if (i < 64)
                    islot.item = player.inventory[i];
                else
                {
                    if (islot.item != null && islot.item.item != null)
                    {
                        if (i == 64)
                        {
                            islot.item.item = player.equippedHelmet;
                        }
                        else if (i == 65)
                        {
                            islot.item.item = player.equippedArms;
                        }
                        else if (i == 66)
                        {
                            islot.item.item = player.equippedChestPiece;
                        }
                        else if (i == 67)
                        {
                            islot.item.item = player.equppedLeggings;
                        }
                        else if (i == 68)
                        {
                            islot.item.item = player.equppedWeapon;
                        }
                    }
                }
            }
        }

        public void SaveInventory()
        {
            for (int i = 0; i < 69; i++)
            {
                GuiWidgetItemSlot islot = (GuiWidgetItemSlot)widgets[i];

                if (i < 64)
                    player.inventory[i] = islot.item;
                else
                {
                    if (islot.item != null && islot.item.item != null)
                    {
                        if (i == 64)
                        {
                            player.equippedHelmet = (ItemArmorHelm)islot.item.item;
                        }
                        else if (i == 65)
                        {
                            player.equippedArms = (ItemArmorArms)islot.item.item;
                        }
                        else if (i == 66)
                        {
                            player.equippedChestPiece = (ItemArmorChest)islot.item.item;
                        }
                        else if (i == 67)
                        {
                            player.equppedLeggings = (ItemArmorLegs)islot.item.item;
                        }
                        else if (i == 68)
                        {
                            player.equppedWeapon = (ItemWeapon)islot.item.item;
                        }
                    }
                }
            }

            /*GuiWidgetItemSlot helmSlot = (GuiWidgetItemSlot)widgets[65];
            player.equippedHelmet = (ItemArmorHelm)helmSlot.item.item;
            GuiWidgetItemSlot armSlot = (GuiWidgetItemSlot)widgets[66];
            player.equippedArms = (ItemArmorArms)armSlot.item.item;
            GuiWidgetItemSlot chestSlot = (GuiWidgetItemSlot)widgets[67];
            player.equippedChestPiece = (ItemArmorChest)chestSlot.item.item;
            GuiWidgetItemSlot legSlot = (GuiWidgetItemSlot)widgets[68];
            player.equppedLeggings = (ItemArmorLegs)legSlot.item.item;
            GuiWidgetItemSlot weaponSlot = (GuiWidgetItemSlot)widgets[69];
            player.equppedWeapon = (ItemWeapon)weaponSlot.item.item;*/

        }

        public void CreateWidgets()
        {
            Color[] colors = new Color[] { Color.White, Color.LightGray, Color.Gray };

            //Create the main inventory slots
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    createInventorySlot(new Rectangle(8 + x * 80, 400 + 80 * y, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, x + (16 * y)), ItemRestriction.Any, colors, player);
                }
            }
            //Todo: create armor/weapon/etc slots here. Can only hold items of their specific type.

            createInventorySlot(new Rectangle(8, 64, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, 65), ItemRestriction.Helm, colors, player);
            createInventorySlot(new Rectangle(88, 64, 64, 64),new Tuple<WidgetType, int>(WidgetType.ItemSlot, 66), ItemRestriction.Arms, colors, player);
            createInventorySlot(new Rectangle(168, 64, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, 67), ItemRestriction.Chest, colors, player);
            createInventorySlot(new Rectangle(248, 64, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, 68), ItemRestriction.Legs, colors, player);
            createInventorySlot(new Rectangle(328, 64, 64, 64), new Tuple<WidgetType, int>(WidgetType.ItemSlot, 69), ItemRestriction.Weapon, colors, player);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, Color.CadetBlue);
                foreach (GuiWidget widget in widgets)
                {
                    widget.Draw(batch);
                }
            }
        }
    }
}
