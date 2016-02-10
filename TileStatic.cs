using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    [DataContract(Name="TileStatic")]
    public class TileStatic : Tile
    {
        [DataMember]
        string textureName;

        World world;

        public TileStatic(Rectangle setSize, string setTexture, int setLayer, World setWorld)
        {
            rect = setSize;

            textureName = setTexture;

            layer = setLayer;

            world = setWorld;

            solid = false;
            draw = true;

            mass = 1;
            inv_mass = 1 / mass;
            restitution = 1;
        }

        public override void Initialize(ContentManager content)
        {
            Logger.Log(String.Format("Created tile.\nIndex: {0}\nLayer: {1}\nFacing Direction: {2}", index, layer, facing), true);

            solid = false;//This is grass bgtile
            texture = content.Load<Texture2D>("textures/tiles/" + textureName);
            blend = content.Load<Texture2D>("textures/blend");
        }

        public override void Update(World world) { return; }   //Static tiles never update

        public override void Draw(SpriteBatch batch, Camera2D camera)
        {
            if (draw)
            {
                batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
                batch.Draw(texture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, rect.Width, rect.Height), Color.White);
                float layerAlpha = 1f / 10f * (float)layer;
                PrimiviteDrawing.DrawRectangle(null, batch, rect, new Color(0, 0, 0, layerAlpha));

                if (wall)
                {
                    float percentLeft = 0.09f;
                    if (facing == Directions.West)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X + newWidth, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                    }
                    if (facing == Directions.East)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                    }
                    if (facing == Directions.North)
                    {
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y + newHeight, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                    if (facing == Directions.South)
                    {
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                    if (facing == Directions.NorthWest)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X + newWidth, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y + newHeight, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                    if (facing == Directions.NorthEast)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y + newHeight, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                    if (facing == Directions.SouthWest)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X + newWidth, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                    if (facing == Directions.SouthEast)
                    {
                        int newWidth = (int)(rect.Width * percentLeft);
                        int newHeight = (int)(rect.Height * percentLeft);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width - newWidth, rect.Height), Color.Black);
                        PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - newHeight), Color.Black);
                    }
                }
                
                batch.End();
            }
        }

        public void DrawDEBUG(SpriteBatch batch, Camera2D camera)
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            PrimiviteDrawing.DrawRectangle(null, batch, rect, 1, Color.Red);
            PrimiviteDrawing.DrawLineSegment(null, batch, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), Color.Blue, 1);
            batch.DrawString(Fonts.munro, "layer:" + layer, new Vector2(position.X + 32, position.Y + 32), Color.Black);
            batch.End();
        }
    }
}

