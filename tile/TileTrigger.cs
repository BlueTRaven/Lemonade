using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.tile
{
    class TileTrigger : Tile
    {
        string dialogueKey = "<default>";
        int dialogueSpeed = 2;
        public TileTrigger(Rectangle bounds, string dialogueKey, int dialogueSpeed)
        {
            this.rect = bounds;
            this.dialogueKey = dialogueKey;
            this.dialogueSpeed = dialogueSpeed;

            this.solid = false;
        }

        public override void Initialize()
        {
            
        }

        public override void Update(World world)
        {
            
        }

        public void OnCollision(World world)
        {
            world.player.OpenDialogue(new Vector2(0, 720 - 128), dialogueSpeed, dialogueKey);
        }

        public override void Draw(SpriteBatch batch)
        {   //Noop
            //TileTriggers do not draw anything, aside from debug outlines
            PrimiviteDrawing.DrawRectangle(null, batch, rect, 1, Color.Red);
        }


        public static Tile CreateTileTrigger(Rectangle bounds, int speed, string dialogueKey)
        {
            TileTrigger t = new TileTrigger(bounds, dialogueKey, speed);
            t.Initialize();
            World.tiles.Add(t);

            return t;
        }
    }
}
