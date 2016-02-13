using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    public class Enemy : EntityLiving
    {
        public Enemy(Vector2 setPosition, int setLayer, int type)
        {
            position = setPosition;
            aiType = type;

            layer = setLayer;

            Name = "Enemy";
        }

        public override void Initialize(World setWorld, Camera2D setCamera)
        {
            camera = setCamera;
            //world = setWorld;

            if (aiType == 0 || aiType == 1)
            {
                texture = Assets.GetTexture("entity_enemy1");
                damage = 12;
                maxSpeed = 3;
            }

            maxHealth = 20;

            health = maxHealth;

            initialized = true;
        }

        public override void Update()
        {
            hitTimer--;
            if (hitTimer <= 0)
                isHit = false;
            if (aiType == 0)
            {
                MoveTo(Game1.playerPosition);
            }

            //position += velocity;

            capVelocity();
        }

        public override void DealDamage(EntityLiving dealTo)
        {
            dealTo.DealtDamage(this);
        }

        public override void DealtDamage(EntityLiving dealtBy)
        {
            takeDamage(dealtBy);
            //SetHit(dealtBy);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            batch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            PrimiviteDrawing.DrawRectangle(null, batch, hitbox, 1, Color.Red);
            batch.End();
        }
    }
}
