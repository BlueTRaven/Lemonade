using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;

namespace Lemonade.tile
{
    /// <summary>
    /// Triggers various functions upon being touched. To have it only activate once, set triggerOnce to true.
    /// </summary>
    public class TileTrigger : TileDynamic
    {
        public enum TriggerAction
        {
            OpenDialogue,
            DamageBox,
            KillBox
        }

        //--OpenDialogue--//
        string dialogueKey = "<default>";

        //--DamageBox--//
        public int damage;

        bool triggerOnce;

        TriggerAction runFunction;

        public TileTrigger(Rectangle bounds, bool triggerOnce)
        {
            this.rect = bounds;

            this.triggerOnce = triggerOnce;

            this.solid = false;
        }

        public void SetType(string dialogueKey)
        {
            this.dialogueKey = dialogueKey;

            this.runFunction = TriggerAction.OpenDialogue;
        }

        public void SetType(int damage)
        {
            this.damage = damage;

            this.runFunction = TriggerAction.DamageBox;
        }

        public void SetType()
        {
            this.runFunction = TriggerAction.KillBox;
        }

        public override void Initialize()
        {
            
        }

        public override void Update(World world)
        {
            
        }

        public override void OnCollision(World world)
        {
            if (runFunction == TriggerAction.OpenDialogue)
            {
                world.player.OpenDialogue(new Vector2(0, 720 - 128), dialogueKey);
            }
            else if (runFunction == TriggerAction.DamageBox)
            {
                if (!world.player.isHit)
                {
                    world.player.DealtDamage(this);
                }
            }
            else if (runFunction == TriggerAction.KillBox)
            {
                world.player.dead = true;
            }

            if (triggerOnce)
            {
                this.dead = true;
            }
        }

        public void OpenDialogue(World world)
        {
        }

        public override void Draw(SpriteBatch batch)
        {   //Noop
            //TileTriggers do not draw anything, aside from debug outlines
            PrimiviteDrawing.DrawRectangle(null, batch, rect, 1, Color.Red);
        }


        public static Tile CreateTileTriggerDialogue(Rectangle bounds, bool triggerOnce, string dialogueKey)
        {
            TileTrigger t = new TileTrigger(bounds, triggerOnce);
            t.SetType(dialogueKey);
            t.Initialize();
            World.tiles.Add(t);

            return t;
        }

        public static Tile CreateTileTriggerDamageBox(Rectangle bounds, bool triggerOnce, int damage)
        {
            TileTrigger t = new TileTrigger(bounds, triggerOnce);
            t.SetType(damage);
            t.Initialize();
            World.tiles.Add(t);

            return t;
        }

        public static Tile CreateTileTriggerKillBox(Rectangle bounds, bool triggerOnce)
        {
            TileTrigger t = new TileTrigger(bounds, triggerOnce);
            t.SetType();
            t.Initialize();
            World.tiles.Add(t);

            return t;
        }
    }
}
