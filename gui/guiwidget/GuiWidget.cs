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
        Dialogue,       //Dialogue box, shows text. Click on it to go to next page. The text inside is decided by the used 'key'. See Content/strings/dialogue.txt.
        Button,         //Button. Simple box, can have text inside. does things when clicked.
        ButtonString,   //Same thing as button, only does not draw the outline, only the text.
        ItemSlot        //Item slot. Used primarily for inventory.
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

        public enum Alignment
        {
            Center,
            Left,
            Right,
            Top,
            Bottom
        }

        public Tuple<WidgetType, int> id;   //Tuple containing the WidgetType and an id to identify by.

        public Vector2 mousePos;
        public State currentState;
        public State previousState;

        public Alignment align;

        public int outlineWidth = 0;

        public Rectangle bounds;
        public Rectangle interiorBounds;

        //public Rectangle InteriorBounds { get { return  } set { interiorBounds = value; } }

        public bool active = true;
        public bool draw = true;

        public Vector2 center { get { return new Vector2(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2); } set { bounds.X = (int)value.X - bounds.Width; bounds.Y = (int)value.Y - bounds.Height; } }

        public void update()
        {
            //mousePos = mState.Position.ToVector2();
            currentState = GetState();

            previousState = currentState;
        }

        public abstract void Draw(SpriteBatch batch);

        public State GetState()
        {
            State finalState;
            if (bounds.Contains(Game1.mouse.positionRelativeCamera))
            {
                finalState = State.Hot;
            }
            else
            {
                finalState = State.None;
            }

            if (finalState == State.Hot)
            {
                if (Game1.mouse.currentState.LeftButton == ButtonState.Pressed)
                {
                    finalState = State.Active;
                }
                if (Game1.mouse.currentState.RightButton == ButtonState.Pressed)
                {
                    finalState = State.Active2;
                }
            }

            if (previousState == State.Active && Game1.mouse.currentState.LeftButton == ButtonState.Released)
            {
                finalState = State.Done;
            }
            if (previousState == State.Active2 && Game1.mouse.currentState.RightButton == ButtonState.Released)
            {
                finalState = State.Done2;
            }

            return finalState;
        }
    }
}
