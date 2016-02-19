using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.tile;
using Lemonade.utility;

namespace Lemonade.tile
{
    public class TileDynamic1 : Tile
    {
        private bool triggered = false;
        public TileDynamic1(Rectangle setSize, int setID)
        {
            rect = setSize;

            id = setID;

            solid = false;
            draw = true;
        }

        public override void Initialize()
        {
            if (id == 0 || id == 1)
            {
                solid = false;  //World loader tile
                texture = Assets.GetTexture(Assets.tile_test);
            }
        }

        public override void Update(World world)
        {
            if (rect.Intersects(world.player.hitbox))
            {
                if (!triggered)
                {
                    triggered = true;
                    Trigger(world);
                }
            }
            else
            {
                triggered = false;
            }

            /*if (id == 0)
            {
                rect.X += 1;
                if (rect.Intersects(world.player.hitbox))
                {
                    world.LoadWorld(1);
                }
            }

            if (id == 1)
            {
            }*/
        }

        public void Trigger(World world)
        {
            if (id == 0)
            {
                world.LoadWorld(1);
            }

            if (id == 1)
            {
                //world.player.OpenDialogue(new Vector2(0, 720-128), "<intro_1>");
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (draw)
            {
                //batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
                //batch.Draw(texture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, rect.Width, rect.Height), Color.White);
                PrimiviteDrawing.DrawRectangle(null, batch, rect, 1, Color.Red);
                //batch.End();
            }
        }

        /// <summary>
        /// Create a new dynamic tile.
        /// Dynamic tiles always run the update function every tick.
        /// </summary>
        /// <param name="brush">Rectangle of the position and size.</param>
        /// <param name="id">id of the dynamic tile to create.</param>
        /// <returns>instance of the created tile.</returns>
        public static Tile CreateTileDynamic(Rectangle brush, int id, int layer)
        {
            TileDynamic1 t = new TileDynamic1(brush, id);
            t.Initialize();
            World.tiles.Add(t);
            return t;
        }
    }
}
