using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lemonade.gui;

namespace Lemonade
{
    [DataContract]
    public class Player : EntityLiving
    {
        bool isFriction = true;    //Should the player slow down
        float friction = 0.85f;     //Speed at which the player slows down. 

        public int damage;

        public int defensePhys, defenseIce, defenseFire, defenseElec;

        public ItemStack[] inventory = new ItemStack[300];
        //public static ItemStack[] inventory = new ItemStack[300];
        
        public static ItemWeapon equppedWeapon;
        public static ItemArmorHelm equippedHelmet;
        public static ItemArmorChest equippedChestPiece;
        public static ItemArmorLegs equppedLeggings;
        public static ItemArmorArms equippedArms;

        public GuiHud guiHUD;
        public GuiInventory guiInventory;

        public Player(Vector2 setPosition, int setLayer)
        {
            position = setPosition;

            layer = setLayer;
        }

        public override void Initialize(World setWorld, Camera2D setCamera)
        {
            camera = setCamera;

            world = setWorld;

            texture = Assets.GetTexture(Assets.entity_player);
            //texture = world.game.
            //Texture2D>("textures/player");

            maxHealth = 100;
            health = maxHealth;

            defensePhys = 0; defenseIce = 0; defenseFire = 0; defenseElec = 0;

            guiHUD = new GuiHud(world.game);
            guiInventory = new GuiInventory(world.game);

            world.game.priorityGui = guiHUD;
        }
        
        [DataMember]
        public string location = "test_map";

        public void Control()
        {   //Handles input
            bool keyW = Keyboard.GetState().IsKeyDown(Keys.W);
            bool keyS = Keyboard.GetState().IsKeyDown(Keys.S);
            bool keyA = Keyboard.GetState().IsKeyDown(Keys.A);
            bool keyD = Keyboard.GetState().IsKeyDown(Keys.D);

            if (keyW)
            {
                velocity.Y += -speed;
            }

            if (keyS)
            {
                velocity.Y += speed;
            }

            if (keyA)
            {
                velocity.X += -speed;
            }

            if (keyD)
            {
                velocity.X += speed;
            }

            if (world.game.keyPress(Keys.I))
            {
                if (guiInventory.active)
                    guiInventory.Close();
                else guiInventory.Open();
            }

            if (world.game.keyPress(Keys.L))
            {
                world.Save();
            }
            if (world.game.keyPress(Keys.K))
            {
                world.LoadWorldFromFile("map1_test.json");
            }
            if (world.game.keyPress(Keys.F))//Keyboard.GetState().IsKeyDown(Keys.F))
            {
                world.createItemEntity(center, Vector2.Zero, new ItemStack(world.game.CreateItemWeapon(1), 1), 1);
            }
            if (world.game.keyPress(Keys.G))
            {
                guiHUD.OpenDialogue("<default>", Assets.GetFont(Assets.munro24));
            }
            if (world.game.keyPress(Keys.OemTilde))
            {
                world.drawTileDEBUG = !world.drawTileDEBUG;
            }

            if (world.game.mouse.LeftClick())
            {
                Vector2 dPos = (world.game.mouse.center - center);

                float angleToMouse = (float)Math.Atan2(dPos.X, dPos.Y);

                Vector2 positionToRotation = Vector2.Transform(new Vector2(center.X, center.Y + 32) - center, Matrix.CreateRotationZ(-angleToMouse)) + center;

                createHurtBox(new Rectangle((int)positionToRotation.X, (int)positionToRotation.Y, 64, 32), 10, 5, -angleToMouse);
            }

            if ((keyW && keyS && keyA && keyD) == false)
            {   //if the player is holding none of the movement keys down
                isFriction = true;  //turn off friction
            }
            else
                isFriction = false;
        }

        public override void Update()
        {
            //Console.WriteLine(inventory.Count);
            fallingTime--;
            hitTimer--;
            //Console.WriteLine(fallingTime);
            if (hitTimer <= 0)
                isHit = false;
            if (fallingTime <= 0)
                falling = false;
            for (int h = 0; h < hurtboxes.Count; h++)
            {
                if (hurtboxes[h].active)
                {
                    //hurtboxes[h].rect.Origin = center;
                    hurtboxes[h].Update();
                }
                else
                    hurtboxes.RemoveAt(h);
            }

            //Console.WriteLine(layer);

            position += velocity;

            if (isFriction)
                velocity *= friction;

            capVelocity();

            guiHUD.Update(world.game.mouse);
            guiInventory.Update(world.game.mouse);
        }

        public override void DealDamage(EntityLiving dealTo)
        {
            dealTo.DealtDamage(this);
        }

        public override void DealtDamage(EntityLiving dealtBy)
        {
            takeDamage(dealtBy);
        }

        public bool PickupItem(ItemEntity item)
        {
            for (int i = 0; i < inventory.GetLength(0); i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item.itemStack;
                    return true;
                }
                else if (inventory[i].item.id == item.item.id)
                {
                    inventory[i].stackSize += item.itemStack.stackSize;
                    return true;
                }
            }
            return false;
        }

        public void CreateItem(int type)
        {
            //inventory.Add();
        }

        float rot = 0;
        public override void Draw(SpriteBatch batch)
        {
            rot += 0.01f;
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());

            //double angleDegrees = (angleRadians * 180)/Math.PI;

            foreach (HurtBox box in hurtboxes)
            {
                //batch.Draw(texture, new Vector2(box.rect.CollisionRectangle.X, box.rect.CollisionRectangle.Y), box.rect.CollisionRectangle, Color.White, box.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                PrimiviteDrawing.DrawRectangle(null, batch, box.rect.CollisionRectangle, Color.Red, box.rotation, box.rect.center - box.rect.UpperLeftCorner());
            }


            batch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //batch.Draw(texture, position, null, Color.White, 30f, center - position, 1f, SpriteEffects.None, 0f);
            //PrimiviteDrawing.DrawRectangle(null, batch, hitbox, Color.Red, -angleToMouse, center - position);
            batch.DrawString(Assets.GetFont(Assets.munro12), ("Layer:" + layer + "\nHealth: " + health), new Vector2(position.X, position.Y - 32), Color.Black);

            if (isHit)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, hitbox, new Color(1, 0, 0, 0.5f));
            }

            PrimiviteDrawing.DrawRectangle(null, batch, hitbox, 1, Color.Red);

            batch.End();
        }
    }
}
