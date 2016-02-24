using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.utility;
using Lemonade.item;

namespace Lemonade
{
    public class GameMouse
    {
        Texture2D texture;

        public MouseState currentState, previousState;

        public Vector2 positionRelativeCamera { get { return currentState.Position.ToVector2(); } set { Mouse.SetPosition((int)value.X, (int)value.Y); } }  //The position of the mouse RELATIVE TO THE CAMERA
        public Vector2 positionRelativeWorld { get { return Vector2.Transform(positionRelativeCamera, World.camera.inverseTransform); } }//currentState.Position.ToVector2() - Game1.cameraPosition; } 
        public Vector2 center { get { return new Vector2(currentState.Position.X + texture.Width / 2, currentState.Position.Y + texture.Height / 2); }
            set { center = new Vector2(value.X - texture.Width / 2, value.Y - texture.Height / 2); } }

        public ItemStack heldItem;
        public Item hoveredItem;

        private bool drawMouse = true;
        public GameMouse()
        {
        }

        public void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("textures/cursor");
        }

        public void Update()
        {
            previousState = currentState;

            currentState = Mouse.GetState();
            //position = currentState.Position.ToVector2() - Utilities.GetOriginRectangle(game.world.cameraRect).Item1;
        }

        /// <summary>
        /// Gets the angle to the mouse, in radians.
        /// </summary>
        /// <returns></returns>
        public float GetAngleToMouse(Vector2 testPosition)
        {
            Vector2 dPos = (positionRelativeWorld - testPosition);
            return (float)Math.Atan2(dPos.X, dPos.Y);
        }

        public bool LeftClick()
        {
            return (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released);
        }

        public bool RightClick()
        {
            return (currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released);
        }

        public void Draw(SpriteBatch batch)
        {
            if (drawMouse)
                batch.Draw(texture, positionRelativeCamera, Color.White);

            if (currentState.LeftButton == ButtonState.Pressed)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(positionRelativeCamera.ToPoint(), new Point(texture.Bounds.Width, texture.Bounds.Height)), Color.Red * 0.5f);
            }

            drawMouse = true;
            if (heldItem != null)
            {
                if (heldItem.item.texture != null)
                {
                    batch.Draw(heldItem.item.texture, center, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    drawMouse = false;
                }
            }

            if (hoveredItem != null)
            {
                batch.DrawString(Assets.GetFont(Assets.munro12), hoveredItem.name, Game1.mouse.positionRelativeCamera, Color.Black);
                for (int i = 0; i < hoveredItem.description.Length; i++)
                {
                    if (hoveredItem.description[i] != null)
                        batch.DrawString(Assets.GetFont(Assets.munro12), hoveredItem.description[i], new Vector2(Game1.mouse.positionRelativeCamera.X, Game1.mouse.positionRelativeCamera.Y + 12 * (i + 1)), Color.Black);
                }
                drawMouse = false;
            }
            hoveredItem = null;
        }
    }
}
