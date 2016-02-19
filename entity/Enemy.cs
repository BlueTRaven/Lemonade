using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;

namespace Lemonade.entity
{
    public class Enemy : EntityLiving
    {
        float healthRatio = 0;
        int healthBarWidth = 0;
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

                deadTime = 60;
            }

            maxHealth = 20;

            health = maxHealth;
            healthBarWidth = health;

            initialized = true;
        }

        public override void Update()
        {
            if (dying)
            {
                Dying();
                return;//PAUSE. EVERYTHING.
            }
            hitTimer--;
            if (hitTimer <= 0)
                isHit = false;
            if (aiType == 0)
            {
                MoveTo(Game1.playerPosition);
            }

            capVelocity();
        }

        public override void DealDamage(EntityLiving dealTo)
        {
            dealTo.DealtDamage(this);
        }

        public override void DealtDamage(EntityLiving dealtBy)
        {
            if (!isHit)
            {
                takeDamage(dealtBy);

                healthRatio = ((float)health / (float)maxHealth);
                healthBarWidth = (int)((healthRatio) * 32);
            }
        }

        public override void Dying()
        {
            deadTimer++;
            if (deadTimer >= deadTime)
                dead = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            //batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            batch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            PrimiviteDrawing.DrawRectangle(null, batch, hitbox, 1, Color.Red);

            if (health < maxHealth)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle((int)position.X, (int)position.Y - 16, 32, 8), Color.Gray);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle((int)position.X, (int)position.Y - 16, healthBarWidth, 8), Color.Red);
                batch.DrawString(Assets.GetFont(Assets.munro12), health + "/" + maxHealth, new Vector2(position.X, position.Y - 16), Color.White);
            }
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
