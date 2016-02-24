using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.item
{
    public abstract class ItemArmor : Item
    {
        public int defensePhys, defenseIce, defenseFire, defenseElec;

        public override abstract void Initialize();
    }
}
