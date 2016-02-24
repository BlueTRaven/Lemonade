using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public class ItemArmorLegs : Item
    {
        public ItemArmorLegs(int id)
        {
            this.id = id;
            this.type = Type.ArmorLegs;
        }

        public override void Initialize()
        {
            this.initialized = true;
        }

        public static ItemArmorLegs CreateItemArmorLegs(int id)
        {
            ItemArmorLegs legs = new ItemArmorLegs(id);
            legs.Initialize();
            return legs;
        }
    }
}
