using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    public class TileDynamic : Tile
    {
        World world;

        public TileDynamic(Rectangle setSize, int setID, World setWorld)
        {
            rect = setSize;

            id = setID;

            solid = false;
            draw = true;

            //world = setWorld;
        }

        public override void Initialize(ContentManager content)
        {
            if (id == 0)
            {
                solid = false;  //World loader tile
                texture = content.Load<Texture2D>("textures/tiles/tile_test");
            }
        }

        public override void Update(World world)
        {
            if (id == 0)
            {
                rect.X += 1;
                if (rect.Intersects(world.player.hitbox))
                {
                    world.LoadWorld(1);
                }
            }
        }

        public override void Draw(SpriteBatch batch, Camera2D camera)
        {
            if (draw)
            {
                batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
                batch.Draw(texture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, rect.Width, rect.Height), Color.White);
                PrimiviteDrawing.DrawRectangle(null, batch, rect, 1, Color.Red);
                batch.End();
            }
        }
    }
}
