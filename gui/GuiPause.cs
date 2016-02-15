using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.gui.guiwidget;

namespace Lemonade.gui
{
    class GuiPause : Gui
    {
        Game1 game;
        private GuiConfirm reallyExit;
        public GuiPause(Game1 game)
        {
            this.game = game;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = false;

            reallyExit = new GuiConfirm(new Rectangle((int)center.X - 256, (int)center.Y - 256, 512, 512), new string[] { "Are you sure you want to exit?", "Yes", "No"});
        }

        public override void Update()
        {
            if (active)
            {
                if (firstOpen)
                {   //Create widgets and such
                    CreateWidgets();
                    firstOpen = false;
                }

                if (Game1.priorityGui == this)
                {
                    GuiWidgetButtonString widgetButtonString = null;

                    for (int i = widgets.Count - 1; i >= 0; i--)
                    {
                        if (widgets[i].id.Item1 == WidgetType.ButtonString)
                        {
                            widgetButtonString = (GuiWidgetButtonString)widgets[i];
                            widgetButtonString.Update();

                            if (widgets[i].id.Item2 == 0)
                            {
                                if (widgets[i].currentState == GuiWidget.State.Done)
                                {
                                    Close();
                                }
                            }
                            if (widgets[i].id.Item2 == 1)
                            {
                                if (widgets[i].currentState == GuiWidget.State.Done)
                                {
                                    reallyExit.Open();
                                }
                            }
                        }

                        if (!widgets[i].active)
                            widgets.RemoveAt(i--);
                    }
                }
                reallyExit.Update();
                if (reallyExit.clicked[0])
                {
                    game.Exit();
                }
            }
        }

        public void Open()
        {
            oldPriorityGui = Game1.priorityGui;
            Game1.priorityGui = this;
            active = true;
            game.paused = true;
        }

        public void Close()
        {
            Game1.priorityGui = oldPriorityGui;
            active = false;
            game.paused = false;
        }

        public void CreateWidgets()
        {
            createButtonString(new Rectangle((int)center.X - 64, (int)center.Y - 8, 128, 16), new Tuple<WidgetType, int>(WidgetType.ButtonString, 0), "Resume game", GuiWidgetButtonString.Alignment.Center, Color.White, Assets.GetFont(Assets.munro12), new Color[] { Color.White, Color.DarkGray, Color.Gray });
            createButtonString(new Rectangle((int)center.X - 64, (int)center.Y + 16, 128, 16), new Tuple<WidgetType, int>(WidgetType.ButtonString, 1), "Exit game", GuiWidgetButtonString.Alignment.Center, Color.White, Assets.GetFont(Assets.munro12), new Color[] { Color.White, Color.DarkGray, Color.Gray });
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
