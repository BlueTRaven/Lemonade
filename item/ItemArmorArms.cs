using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public class ItemArmorArms : Item
    {
        public ItemArmorArms(int id)
        {
            this.id = id;
            this.type = Type.ArmorArms;
        }

        public override void Initialize()
        {
            this.initialized = true;
        }

        public static ItemArmorArms CreateItemArmorArms(int id)
        {
            ItemArmorArms arms = new ItemArmorArms(id);
            arms.Initialize();
            return arms;
        }
    }
}
