using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.Guis
{
    class GuiPause : Gui
    {
        private GuiConfirm reallyExit;
        public GuiPause(Game1 game)
        {
            this.game = game;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = false;

            reallyExit = new GuiConfirm(game, new Rectangle((int)center.X - 256, (int)center.Y - 256, 512, 512), new string[] { "Are you sure you want to exit?", "Yes", "No"});
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

                            if (widget.id.Item2 == 0)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    Close();
                                }
                            }
                            if (widget.id.Item2 == 1)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    reallyExit.Open();
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
                reallyExit.Update(gMouse);
                if (reallyExit.clicked[0])
                {
                    game.Exit();
                }
            }
        }

        public void Open()
        {
            oldPriorityGui = game.priorityGui;
            game.priorityGui = this;
            active = true;
            game.paused = true;
        }

        public void Close()
        {
            game.priorityGui = oldPriorityGui;
            active = false;
            game.paused = false;
        }

        public void CreateWidgets()
        {
            createButtonString(new Rectangle((int)center.X - 64, (int)center.Y - 8, 128, 16), new Tuple<string, int>("butstring", 0), "Resume game", GuiWidgetButtonString.Alignment.Center, Color.White, Fonts.munro12, new Color[] { Color.White, Color.DarkGray, Color.Gray });
            createButtonString(new Rectangle((int)center.X - 64, (int)center.Y + 16, 128, 16), new Tuple<string, int>("butstring", 1), "Exit game", GuiWidgetButtonString.Alignment.Center, Color.White, Fonts.munro12, new Color[] { Color.White, Color.DarkGray, Color.Gray });
        }

        public override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                PrimiviteDrawing.DrawRectangle(null, batch, bounds, Color.Black);
                foreach (GuiWidget widget in widgets)
                {
                    widget.Draw(batch);
                   // PrimiviteDrawing.DrawRectangle(null, batch, widget.bounds, 1, Color.Red);
                }

                if (reallyExit.active)
                {
                    reallyExit.Draw(batch);
                }
            }
        }
    }
}
