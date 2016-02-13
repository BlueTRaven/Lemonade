using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.gui.guiwidget
{
    public enum WidgetType
    {
        Dialogue,
        Button,
        ButtonString,
        ItemSlot
    }

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

        public Tuple<WidgetType, int> id;   //Tuple containing the type "button", "inventoryItem", etc and an id to identify by.

        public Vector2 mousePos;
        public State currentState;
        public State previousState;

        protected int outlineWidth = 0;

        public Rectangle bounds;
        protected Rectangle interiorBounds;

        public bool active = true;

        public Vector2 center { get { return new Vector2(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2); } set { bounds.X = (int)value.X - bounds.Width; bounds.Y = (int)value.Y - bounds.Height; } }

        public void update(GameMouse gMouse)
        {
            //mousePos = mState.Position.ToVector2();
            currentState = GetState(gMouse);

            previousState = currentState;
        }

        public abstract void Draw(SpriteBatch batch);

        public State GetState(GameMouse gMouse)
        {
            if (bounds.Contains(gMouse.position))
            {
                currentState = State.Hot;
            }
            else
            {
                currentState = State.None;
            }

            if (currentState == State.Hot)
            {
                if (gMouse.currentState.LeftButton == ButtonState.Pressed)
                {
                    currentState = State.Active;
                }
                if (gMouse.currentState.RightButton == ButtonState.Pressed)
                {
                    currentState = State.Active2;
                }
            }

            if (previousState == State.Active && gMouse.currentState.LeftButton == ButtonState.Released)
            {
                currentState = State.Done;
            }
            if (previousState == State.Active2 && gMouse.currentState.RightButton == ButtonState.Released)
            {
                currentState = State.Done2;
            }

            return currentState;
        }
    }
}
