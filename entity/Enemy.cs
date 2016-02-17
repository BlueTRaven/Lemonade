using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.entity
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

        public override void Initialize()
        {
            if (aiType == 0 || aiType == 1)
            {
                texture = Assets.GetTexture(Assets.entity_enemy1);
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
            //batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            batch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            PrimiviteDrawing.DrawRectangle(null, batch, hitbox, 1, Color.Red);
            //batch.End();
        }

        /// <summary>
        /// Creates a new enemy.
        /// </summary>
        /// <param name="position">Position where the enemy is created.</param>
        /// <param name="id">Id of enemy to create.</param>
        /// <returns>instance of the created enemy.</returns>
        public static Enemy CreateEnemy(Vector2 position, int layer, int id)
        {
            Enemy e = new Enemy(position, layer, id);
            e.Initialize();
            World.entityLivings.Add(e);

            return e;
        }
    }
}
