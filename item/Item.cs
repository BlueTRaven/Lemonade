using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;

namespace Lemonade.item
{
    public enum Element  //Damage element the weapon will deal.
    {
        PHYSICAL,
        FIRE,
        ICE,
        ELECTRIC,
        ETHER
    }

    public class ItemStack
    {
        public Item item;
        public int stackSize;

        public ItemStack(Item setItem, int setStackSize)
        {
            item = setItem;
            stackSize = setStackSize;
        }

        public static ItemStack CreateItemStack(Item item, int stackSize)
        {
            if (!item.initialized)
            {
                item.Initialize();
            }
            return new ItemStack(item, stackSize);
        }
    }

    #region Item
    public abstract class Item
    {
        public enum Type
        {
            ArmorArms,
            ArmorChest,
            ArmorHelm,
            ArmorLegs,
            Material,
            Weapon
        }

        public Texture2D texture;
        public string textureName;

        public int id;

        public string name;
        public string[] description = new string[8];   //Describes the item. new line = next line down

        public bool initialized = false;

        public Type type;

        public abstract void Initialize();
    }
    #endregion
}