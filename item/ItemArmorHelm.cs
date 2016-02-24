using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public class ItemArmorHelm : ItemArmor
    {
        public ItemArmorHelm(int id)
        {
            this.id = id;
            this.type = Type.ArmorHelm;
        }

        public override void Initialize()
        {
            this.initialized = true;
        }

        public static ItemArmorHelm CreateItemArmorHelm(int id)
        {
            ItemArmorHelm helm = new ItemArmorHelm(id);
            helm.Initialize();
            return helm;
        }
    }
}
