using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade
{
    public class GameMouse
    {
        Texture2D texture;

        public MouseState currentState, previousState;

        public Vector2 center { get { return new Vector2(currentState.Position.X + texture.Width / 2, currentState.Position.Y + texture.Height / 2); }
            set { center = new Vector2(value.X - texture.Width / 2, value.Y - texture.Height / 2); } }

        public ItemStack heldItem;

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
            //Do things between here
            currentState = Mouse.GetState();
        }

        public bool LeftClick()
        {
            return (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released);
            /*if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;*/
        }

        public bool RightClick()
        {
            return (currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, center, Color.White);

            if (currentState.LeftButton == ButtonState.Pressed)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(center.ToPoint(), new Point(texture.Bounds.Width, texture.Bounds.Height)), Color.Red * 0.5f);
            }

            if (heldItem != null)
            {
                if (heldItem.item.texture != null)
                {
                    batch.Draw(heldItem.item.texture, new Vector2(currentState.Position.X - texture.Width / 2, currentState.Position.Y - texture.Height / 2), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }
        }
    }
}
