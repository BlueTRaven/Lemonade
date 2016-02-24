using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lemonade.utility;

namespace Lemonade.item
{
    public class ItemWeapon : Item
    {
        public enum WeaponType   //Determines attack pattern.
        {
            KNIFE,
            LONGSWORD,
            DUALSWORD,
            SPEAR,
            HAMMER
        }
        WeaponType weaponType;
        Element element;

        int damage; //Amount of damage to deal.

        public ItemWeapon(int id)
        {
            this.id = id;
            this.type = Type.Weapon;

            if (id == 0)
            {
                element = Element.ETHER;
                weaponType = WeaponType.KNIFE;

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

        public override void Initialize()
        {
            texture = Assets.GetTexture(Assets.itemPrefix + textureName);

            this.initialized = true;
        }

        public static ItemWeapon CreateItemWeapon(int id)
        {
            ItemWeapon iWep = new ItemWeapon(id);
            iWep.Initialize();
            return iWep;
        }
    }
}
