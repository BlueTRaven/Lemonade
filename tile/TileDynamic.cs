using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemonade.tile
{
    /// <summary>
    /// Tile dynamic: abstract class for TileTrigger, TileMoving, etc. Behaves somewhat like an entity; can die.
    /// </summary>
    public abstract class TileDynamic : Tile
    {
        public bool dead = false;

        public abstract void OnCollision(World world);
    }
}
