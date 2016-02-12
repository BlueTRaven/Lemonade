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
            world = setWorld;

            if (aiType == 0 || aiType == 1)
            {
                texture = Assets.textures["enemy1"];
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
                MoveTo(world.player.position);
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


        public void takeDamage(EntityLiving dealtBy, string damageType = "physical")
        {
            if (!isHit)
            {
                Console.WriteLine("agck");
                if (health - calculateDefenseDamage(dealtBy.damage, damageType) <= 0)
                {
                    health = 0;
                    dead = true;
                }
                else
                {
                    health -= calculateDefenseDamage(dealtBy.damage, damageType);
                }
            }
            SetHit(dealtBy);
        }

        public int calculateDefenseDamage(int amount, string damageType)
        {
            int finalDamage;

            if (damageType == "physical")
            {   //damage decreased by half of phys defense.
                finalDamage = amount - (defensePhys / 2);
            }
            else if (damageType == "ice")
            {
                //damage decreased by 1/3 of ice defense
                finalDamage = amount - (defenseIce / 3);
            }
            else if (damageType == "fire")
            {   //full defense subtracted
                finalDamage = amount - defenseFire;
            }
            else if (damageType == "electric")
            {   //damage decreased by 2/3 of electric defense
                finalDamage = amount - ((defenseElec / 3) * 2);
            }
            else
            {   //ether damage ignores defense
                finalDamage = amount;
            }
            return finalDamage;
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
