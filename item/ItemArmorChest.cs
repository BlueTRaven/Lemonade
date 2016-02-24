using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public class ItemArmorChest : Item
    {
        public ItemArmorChest(int id)
        {
            this.id = id;
            this.type = Type.ArmorChest;
        }

        public override void Initialize()
        {
            this.initialized = true;
        }

        public static ItemArmorChest CreateItemArmorChest(int id) 
        {
            ItemArmorChest chest = new ItemArmorChest(id);
            chest.Initialize();
            return chest;
        }
    }
}
