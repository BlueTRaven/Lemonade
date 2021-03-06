﻿using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;
using Lemonade.item;

namespace Lemonade.entity
{
    public class ItemEntity : Entity
    {
        public ItemStack itemStack;
        public Item item;
        public ItemEntity(Vector2 setPosition, Vector2 setVelocity, ItemStack droppedItem)
        {
            position = setPosition;
            velocity = setVelocity;
            itemStack = droppedItem;
            item = itemStack.item;
        }

        public override void Initialize()
        {
            texture = item.texture;

            position = center;
        }

        public override void Draw(SpriteBatch batch)
        {
            //batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            batch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            PrimiviteDrawing.DrawRectangle(null, batch, hitbox, 1, Color.Red);
            //batch.End();
        }

        public override void Update()
        {
            position += velocity;
        }


        public static ItemEntity CreateItemEntity(Vector2 position, Vector2 velocity, ItemStack droppedItem, int layer)
        {
            ItemEntity i = new ItemEntity(position, velocity, droppedItem);
            i.Initialize();
            i.layer = layer;
            World.itemEntities.Add(i);

            return i;
        }
    }
}
