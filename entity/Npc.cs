using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.entity
{
    /// <summary>
    /// Used for any entity that will not attack the player; Quest givers, characters, etc...
    /// </summary>
    class Npc : Entity
    {
        public string dialogueKey = "<default>";
        public Npc()
        {
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch batch)
        {
            
        }
    }
}
