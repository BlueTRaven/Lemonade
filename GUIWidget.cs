using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade;

namespace Lemonade
{
    public abstract class GuiWidget
    {
        public enum State
        {
            None,   //No state
            Hot,    //Being moused over
            Active, //Being left clicked
            Active2,//Being right clicked
            Done,   //Finished left click
            Done2   //Finished right click
        }

        public Tuple<string, int> id;   //Tuple containing the type "button", "inventoryItem", etc and an id to identify by.

        public Vector2 mousePos;
        public State currentState;
        public State previousState;

        protected int outlineWidth =0;

        protected Rectangle bounds;

        public Vector2 center { get { return new Vector2(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2); } set { bounds.X = (int)value.X - bounds.Width; bounds.Y = (int)value.Y - bounds.Height; } }

        public void Update(MouseState mState)
        {
            mousePos = mState.Position.ToVector2();
            currentState = GetState(mState);

            previousState = currentState;
        }

        public abstract void Draw(SpriteBatch batch);

        public State GetState(MouseState mState)
        {
            if (bounds.Contains(mState.Position))
            {
                currentState = State.Hot;
            }
            else
            {
                currentState = State.None;
            }

            if (currentState == State.Hot)
            {
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    currentState = State.Active;
                }
                if (mState.RightButton == ButtonState.Pressed)
                {
                    currentState = State.Active2;
                }
            }

            if (previousState == State.Active && mState.LeftButton == ButtonState.Released)
            {
                currentState = State.Done;
            }
            if (previousState == State.Active2 && mState.RightButton == ButtonState.Released)
            {
                currentState = State.Done2;
            }

            return currentState;
        }
    }

    public class GuiWidgetButton : GuiWidget
    {
        SpriteFont font;
        string text;

        Color[] colors;

        public GuiWidgetButton(Rectangle setBounds, Tuple<string, int> id, string setText, Color textColor, SpriteFont setFont, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            text = setText;
            font = setFont;

            this.colors = colors;
        }

        public void Update(MouseState mState)
        {
            base.Update(mState);   
        }

        public override void Draw(SpriteBatch batch)
        {
            if (currentState == State.None)
            {
                outlineWidth = 2;
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[0]);
            }
            if (currentState == State.Hot)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[0]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[1]);
            }
            if (currentState == State.Active || currentState == State.Active2)//|| currentState == State.Done)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
            if (currentState == State.Done || currentState == State.Done2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
        }
    }

    public class GuiWidgetItemSlot : GuiWidget
    {
        public ItemStack itemInSlot;

        bool drawToolTip = false;

        Color[] colors;

        public GuiWidgetItemSlot(Rectangle setBounds, Tuple<string, int> id, Color[] colors, Game1 game)
        {
            this.id = id;
            bounds = setBounds;

            this.colors = colors;

            //itemInSlot = item;
        }

        public void Update(MouseState mState)
        {
            base.Update(mState);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (currentState == State.None)
            {
                outlineWidth = 2;
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[0]);
            }
            if (currentState == State.Hot)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[0]);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2)), colors[1]);
            }
            if (currentState == State.Active || currentState == State.Active2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);
            if (currentState == State.Done || currentState == State.Done2)
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[2]);

            if (itemInSlot != null && itemInSlot.item != null && itemInSlot.item.texture != null)
            {
                batch.Draw(itemInSlot.item.texture, bounds, Color.White);

                if (itemInSlot.stackSize > 1)
                    batch.DrawString(Fonts.munro, itemInSlot.stackSize.ToString(), new Vector2(bounds.Right, bounds.Bottom), Color.White);
            }
        }

        public void DrawToolTip(SpriteBatch batch)
        {
            if (itemInSlot != null && itemInSlot.item.texture != null)
            {
                if (currentState == State.Hot)
                {
                    batch.DrawString(Fonts.munro, itemInSlot.item.name, new Vector2(mousePos.X, mousePos.Y), Color.White);
                    for (int i = 0; i < itemInSlot.item.description.Count(); i++)
                    {
                        if (itemInSlot.item.description[i] != null)
                            batch.DrawString(Fonts.munro, itemInSlot.item.description[i], new Vector2(mousePos.X, mousePos.Y + 12 * (i + 1)), Color.White);
                    }

                }
            }
        }
    }
}
