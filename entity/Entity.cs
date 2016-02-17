using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Lemonade.entity
{
    [DataContract]
    public abstract class Entity
    {
        public Camera2D camera;

        [DataMember]
        public string Name = "";
        [DataMember]
        public Vector2 position, velocity;

        public Vector2 center { get{ return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);} }

        public Rectangle hitbox { get{ return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);} }
        public Rectangle overlap;

        public Texture2D texture;

        public World world;

        //public List<Entity> recentCollision = new List<Entity>(); //list of entities that the entity has recently been hit by.
        public List<HurtBox> hurtboxes = new List<HurtBox>(); //extra hitboxes outside player area, used for damaging

        public float maxSpeed = 5;
        public float speed = 1;
        
        [DataMember]
        public float scale;

        public int aiType;
        
        public int damage;

        [DataMember]
        public int layer;   //NOTE entities will never be on layer 0

        [DataMember]
        public bool dead, friendly = false;

        public bool falling;
        public int fallingTime;

        protected bool initialized = false;

        public DirectionCardinal facing;
        public bool directionNorth { get { facing = DirectionCardinal.North; return velocity.Y < 0; } }
        public bool directionSouth { get { facing = DirectionCardinal.South; return velocity.Y > 0; } }
        public bool directionEast { get { facing = DirectionCardinal.East; return velocity.X < 0; } }
        public bool directionWest { get { facing = DirectionCardinal.West; return velocity.X > 0; } }

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Draw(SpriteBatch batch);

        public void MoveTo(Vector2 toLocation)
        {

            Vector2 dir = toLocation - position;
            if (dir != Vector2.Zero)
                dir.Normalize();

            position += dir * maxSpeed;
        }
        
        public void capVelocity()
        {
            int newMax = (int) maxSpeed - 1;
            if (velocity.X > newMax)
                velocity.X = newMax;
            if (velocity.X < -newMax)
                velocity.X = -newMax;
            if (velocity.Y > newMax)
                velocity.Y = newMax;
            if (velocity.Y < -newMax)
                velocity.Y = -newMax;
        }

        /// <summary>
        /// Creates a "hurtbox", which are used to determine locations of damage
        /// </summary>
        /// <param name="bounds">The size of the rectangle to be created.</param>
        /// <param name="duration">how long the hurtbox persists.</param>
        /// <returns></returns>
        public HurtBox createHurtBox(Entity creator, Rectangle bounds, Vector2 origin, int damage, int duration, float rotation = 0f)
        {
            HurtBox h = new HurtBox(creator, bounds, origin, damage, duration, rotation);

            hurtboxes.Add(h);
            return h;
        }

        //public abstract void collide(Entity entityHit);
    }

    public abstract class EntityLiving : Entity
    {
        public int health, maxHealth;
        public int timeSinceCombat, healTickTime;   //May not use, may make player heal manually
        public int defensePhys, defenseIce, defenseFire, defenseElec;

        protected int hitTimerMax = 200;
        protected int hitTimer = 200;
        protected bool isHit = false;

        public Entity prevHit;

        public abstract void DealDamage(EntityLiving dealTo);
        public abstract void DealtDamage(EntityLiving dealtBy);

        public void SetHit(EntityLiving hitBy)
        {
            if (!isHit)
            {
                Console.WriteLine("Hit by " + hitBy.GetType());
                isHit = true;
                hitTimer = hitTimerMax;
            }

            int[] test = new int[300];
        }

        /// <summary>
        /// Drops an item on top of the entity.
        /// </summary>
        /// <param name="dropItem">the item to drop.</param>
        public void dropItem(ItemStack dropItem)
        {
            ItemEntity.CreateItemEntity(center, velocity, dropItem, layer);
        }

        public void takeDamage(EntityLiving dealtBy, Element damageType = Element.PHYSICAL)
        {
            if (!isHit)
            {
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

        public int calculateDefenseDamage(int amount, Element damageType)
        {
            int finalDamage;

            if (damageType == Element.PHYSICAL)
            {   //damage decreased by half of phys defense.
                finalDamage = amount - (defensePhys / 2);
            }
            else if (damageType == Element.ICE)
            {
                //damage decreased by 1/3 of ice defense
                finalDamage = amount - (defenseIce / 3);
            }
            else if (damageType == Element.FIRE)
            {   //full defense subtracted
                finalDamage = amount - defenseFire;
            }
            else if (damageType == Element.ELECTRIC)
            {   //damage decreased by 2/3 of electric defense
                finalDamage = amount - ((defenseElec / 3) * 2);
            }
            else
            {   //ether damage ignores defense
                finalDamage = amount;
            }
            return finalDamage;
        }
    }

    public abstract class EntityBoss : EntityLiving
    {
        public string bossmusic;
        List<Enemy> minions = new List<Enemy>();
    }
}
