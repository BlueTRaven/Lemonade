using System;
using System.Text;
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

    public class GuiWidgetDialogue : GuiWidget
    {
        SpriteFont font;
        string[] text;
        public int currentLine = 0;

        int textSpeed;  //How fast a letter appears in ticks, 60 = 1 sec
        int letterTimer;    //timer to count up that amount
        int count;

        public bool finished = false;

        Color[] colors;
        public GuiWidgetDialogue(Rectangle setBounds, Tuple<string, int> id, string forKey, Color textColor, SpriteFont setFont, int textSpeed, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            
            outlineWidth = 4;
            interiorBounds = new Rectangle(bounds.X + outlineWidth, bounds.Y + outlineWidth, bounds.Width - (int)(outlineWidth * 2), bounds.Height - (int)(outlineWidth * 2));

            font = setFont;

            this.textSpeed = textSpeed;

            this.colors = colors;

            text = Utilities.ReadFile("Content\\strings\\dialogue.txt", forKey).Split(':');
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                letterTimer++;

                if (letterTimer >= textSpeed)
                {
                    letterTimer = 0;
                    count++;
                }
                base.update(gMouse);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            PrimiviteDrawing.DrawRectangle(null, batch, bounds, colors[1]);
            PrimiviteDrawing.DrawRectangle(null, batch, interiorBounds, colors[0]);

            string wrappedText = Utilities.WrapText(font, text[currentLine], interiorBounds.Width);
            char[] s = wrappedText.ToCharArray();

            string final = "";

            if (count <= s.Length && !finished)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (i <= count)
                    {
                        final = final + s[i];
                    }
                }
            }
            else
            { 
                final = wrappedText;
                finished = true;
                count = 0;
            }
            batch.DrawString(font, final, new Vector2(interiorBounds.X, interiorBounds.Y), Color.Black);
        }

        /// <summary>
        /// Changes the text to the new line index and speed.
        /// </summary>
        /// <param name="newSpeed">Rate at which a character appears.</param>
        /// <param name="newLine">line of "text" array to set to. if left at default, it will simply increase the text array index.</param>
        /// <returns>Whether or not there is any more text in the array. if there isn't, it's done.</returns>
        public bool ChangeText(int newSpeed, int newLine = -1)
        {
            finished = false;

            count = 0;
            if (newLine >= 0)
                currentLine = newLine;
            else if (currentLine + 1 < text.Length)
            {
                currentLine++;
            }
            else return true;
            textSpeed = newSpeed;
            return false;
        }
    }

    public class GuiWidgetButtonString : GuiWidget
    {
        public enum Alignment 
        { 
            Center, 
            Left, 
            Right, 
            Top, 
            Bottom 
        }
        Alignment alignment;
        SpriteFont font;
        string text;

        Color[] colors;

        public GuiWidgetButtonString(Rectangle setBounds, Tuple<string, int> id, string setText, Alignment align, Color textColor, SpriteFont setFont, Color[] colors)
        {
            this.id = id;
            bounds = setBounds;
            font = setFont;
            text = setText;//Utilities.WrapText(font, setText, bounds.Width);

            alignment = align;

            this.colors = colors;
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                base.update(gMouse);
            }
        }
            
        public override void Draw(SpriteBatch batch)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = bounds.Center.ToVector2();
            Vector2 origin = size * 0.5f;

            Color color = colors[0];
            if (currentState == State.None)
            {
                color = colors[0];
            }
            if (currentState == State.Hot)
            {
                color = colors[1];
            }
            if (currentState == State.Active || currentState == State.Done)
            {
                color = colors[2];
            }

            if (alignment == Alignment.Left)
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }
            if (alignment == Alignment.Right)
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }
            if (alignment == Alignment.Top)
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }
            if (alignment == Alignment.Bottom)
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            batch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
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

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                base.update(gMouse);
            }
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
        private ItemStack item;
        bool drawToolTip = false;

        Color[] colors;

        GameMouse gMouse;
        Player player;

        public GuiWidgetItemSlot(Rectangle setBounds, Tuple<string, int> id, Color[] colors, GameMouse gMouse, Player player)
        {
            this.id = id;
            bounds = setBounds;

            this.colors = colors;

            this.gMouse = gMouse;
            this.player = player;
        }

        public void Update(GameMouse gMouse)
        {
            if (active)
            {
                item = player.inventory[this.id.Item2];

                if (currentState == State.Done)
                {
                    if (gMouse.heldItem != null)
                    {
                        if (item == null)
                        {
                            AddItemToEmptySlot();
                        }
                        else if (item.item.GetType() == gMouse.heldItem.item.GetType() && item.item.id == gMouse.heldItem.item.id)
                        {
                            AddItemToFilledSlot();
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            if (item == gMouse.heldItem)
                                RemoveItemToFilledMouse();
                            else RemoveItemToEmptyMouse();
                        }
                    }
                }
                base.update(gMouse);
            }
        }

        public void AddItemToEmptySlot()
        {
            player.inventory[this.id.Item2] = gMouse.heldItem;
            gMouse.heldItem = null;
        }

        public void AddItemToFilledSlot()
        {
            player.inventory[this.id.Item2].stackSize += gMouse.heldItem.stackSize;
            gMouse.heldItem = null;
        }

        public void RemoveItemToEmptyMouse()
        {
            gMouse.heldItem = player.inventory[this.id.Item2];
            player.inventory[this.id.Item2] = null;
        }
        public void RemoveItemToFilledMouse()
        {
            gMouse.heldItem.stackSize += player.inventory[this.id.Item2].stackSize;
            player.inventory[this.id.Item2] = null;
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


            if (item != null && item.item != null && item.item.texture != null)
            {
                batch.Draw(item.item.texture, bounds, Color.White);

                if (item.stackSize > 1)
                    batch.DrawString(Assets.fonts["munro12"], item.stackSize.ToString(), new Vector2(bounds.Right, bounds.Bottom), Color.White);
            }
        }

        public void DrawToolTip(SpriteBatch batch)
        {
            if (item != null && item.item.texture != null)
            {
                if (currentState == State.Hot)
                {
                    batch.DrawString(Assets.fonts["munro12"], item.item.name, gMouse.position, Color.White);
                    for (int i = 0; i < item.item.description.Count(); i++)
                    {
                        if (item.item.description[i] != null)
                            batch.DrawString(Assets.fonts["munro12"], item.item.description[i], new Vector2(gMouse.position.X, gMouse.position.Y + 12 * (i + 1)), Color.White);
                    }

                }
            }
        }
    }
}
