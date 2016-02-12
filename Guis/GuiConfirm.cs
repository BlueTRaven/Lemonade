using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.Guis
{
    class GuiConfirm : Gui
    {
        private string[] texts = new string[3];

        //Widget clicked. 0 = <id 1> clicked, 1 = <id 2> clicked;
        public bool[] clicked = new bool[] { false, false };
        /// <param name="texts">0 = Top text, 1 = "Yes" text, 2 = "No" text</param>
        public GuiConfirm(Game1 game, Rectangle size, string[] texts)
        {
            this.game = game;
            this.bounds = size;
            this.active = false;
            this.texts = texts;
        }

        public override void Update(GameMouse gMouse)
        {
            if (active)
            {
                if (firstOpen)
                {   //Create widgets and such
                    CreateWidgets();
                    firstOpen = false;
                }

                if (game.priorityGui == this)
                {
                    List<GuiWidget> removeList = new List<GuiWidget>();
                    GuiWidgetButtonString widgetButtonString = null;

                    foreach (GuiWidget widget in widgets)
                    {
                        if (widget.id.Item1 == "butstring")
                        {
                            widgetButtonString = (GuiWidgetButtonString)widget;
                            widgetButtonString.Update(gMouse);

                            if (widget.id.Item2 == 1)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    clicked[0] = true;
                                }
                            }

                            if (widget.id.Item2 == 2)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    clicked[1] = true;
                                    Close();
                                }
                            }
                        }

                        if (widget.active == false)
                        {
                            removeList.Add(widget);
                        }
                    }

                    foreach (GuiWidget delete in removeList)
                    {
                        widgets.Remove(delete);
                    }
                }
            }
        }

        public void Open()
        {
            oldPriorityGui = game.priorityGui;
            game.priorityGui = this;
            active = true;
        }

        public void Close()
        {
            game.priorityGui = oldPriorityGui;
            active = false;
            clicked[0] = false;
            clicked[1] = false;
        }

        public void CreateWidgets()
        {
            createButtonString(new Rectangle((int)center.X - 64, (int)center.Y, 128, 16), new Tuple<string, int>("butstring", 0), texts[0], GuiWidgetButtonString.Alignment.Center, Color.White, Fonts.munro24, new Color[] { Color.White, Color.White, Color.White });

            createButtonString(new Rectangle((int)center.X - 192, (int)center.Y + 16, 48, 16), new Tuple<string, int>("butstring", 1), texts[1], GuiWidgetButtonString.Alignment.Center, Color.White, Fonts.munro12, new Color[] { Color.White, Color.DarkGray, Color.Gray });
            createButtonString(new Rectangle((int)center.X + 192, (int)center.Y + 16, 48, 16), new Tuple<string, int>("butstring", 2), texts[2], GuiWidgetButtonString.Alignment.Center, Color.White, Fonts.munro12, new Color[] { Color.White, Color.DarkGray, Color.Gray });
        }

        public override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, Color.White);
                PrimiviteDrawing.DrawRectangle(null, batch, new Rectangle(bounds.X + 4, bounds.Y + 4, bounds.Width - 4 * 2, bounds.Height - 4 * 2), Color.Black);
                foreach (GuiWidget widget in widgets)
                {
                    PrimiviteDrawing.DrawRectangle(null, batch, widget.bounds, 1, Color.Red);
                    widget.Draw(batch);
                }
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, 1, Color.Red);
            }
        }
    }
}
