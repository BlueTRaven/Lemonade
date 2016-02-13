using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
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
    }

    #region Item
    public abstract class Item
    {
        public Texture2D texture;
        public string textureName;

        protected Game1 game;

        public int id;

        public string name;
        public string[] description = new string[8];   //Describes the item. new line = array

        public abstract void Initialize(Game1 game);
        //public abstract void CreateItem(int id);
    }
    #endregion

    #region ItemArmor
    public class ItemArmor : Item
    {
        public int defensePhys, defenseIce, defenseFire, defenseElec;

        public override void Initialize(Game1 game)
        {

        }
    }
    #endregion

    #region ItemArmorHelm
    public class ItemArmorHelm : Item
    {
        public ItemArmorHelm(int id)
        {

        }

        public override void Initialize(Game1 game)
        {

        }
    }
    #endregion

    #region ItemArmorChest
    public class ItemArmorChest : Item
    {
        public ItemArmorChest(int id)
        {

        }

        public override void Initialize(Game1 game)
        {

        }
    }
    #endregion

    #region ItemArmorLegs
    public class ItemArmorLegs : Item
    {
        public ItemArmorLegs(int id)
        {

        }

        public override void Initialize(Game1 game)
        {

        }
    }
    #endregion

    #region ItemArmorArms
    public class ItemArmorArms : Item
    {
        public ItemArmorArms(int id)
        {

        }

        public override void Initialize(Game1 game)
        {

        }
    }
    #endregion

    #region ItemWeapon
    public class ItemWeapon : Item
    {
        public enum Names : int
        {
            TEST
        }

        public enum WeaponType   //Determines attack pattern.
        {
            KNIFE,
            LONGSWORD,
            DUALSWORD,
            SPEAR,
            HAMMER
        }
        WeaponType type;
        Element element;

        int damage; //Amount of damage to deal.


        public ItemWeapon(int id)
        {
            this.id = id;
            if (id == 0)
            {
                element = Element.ETHER;
                type = WeaponType.KNIFE;

                textureName = "knife";
                name = "Test Weapon";
                description[0] = "A test weapon";
                description[1] = "You shouldn't have this.";
            }

            if (id == 1)
            {
                textureName = "knifeRust";
                name = "Test Weapon 2";
                description[0] = "THE second test weapon.";
                description[1] = "You shouldn't have this.";
                description[2] = "although you probably can later.";
            }
        }

        public override void Initialize(Game1 game)
        {
            if (this.id == 0)
            {
                texture = game.Content.Load<Texture2D>("textures/" + textureName);
            }
            if (this.id == 1)
            {
                texture = game.Content.Load<Texture2D>("textures/" + textureName);
            }
        }


    }
    #endregion

    #region ItemMaterial
    public class ItemMaterial : Item
    {
        public ItemMaterial(int id)
        {

        }

        public override void Initialize(Game1 game)
        {
            this.game = game;
        }
    }
    #endregion
}