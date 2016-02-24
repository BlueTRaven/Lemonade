using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public class ItemMaterial : Item
    {
        public ItemMaterial(int id)
        {
            this.id = id;
            this.type = Type.Material;
        }

        public override void Initialize()
        {
            this.initialized = true;
        }

        public static ItemMaterial CreateItemMaterial(int id)
        {
            ItemMaterial material = new ItemMaterial(id);
            material.Initialize();
            return material;
        }
    }
}
