using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.gui.guiwidget;

namespace Lemonade.gui
{
    public class GuiHud : Gui
    {
        public GuiHud(Game1 game)
        {
            this.game = game;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = true;
        }

        public override void Update(GameMouse gMouse)
        {
            if (active)
            {
                if (firstOpen)
                {   //Create widgets and such
                    //CreateWidgets();
                    firstOpen = false;
                }

                if (game.priorityGui == this)
                {
                    GuiWidgetDialogue widgetDialogue = null;
                    GuiWidgetButton widgetButton = null;

                    for (int i = widgets.Count - 1; i >= 0; i--)
                    {
                        if (widgets[i].id.Item1 == WidgetType.Dialogue)
                        {
                            widgetDialogue = (GuiWidgetDialogue)widgets[i];
                            widgetDialogue.Update(gMouse);

                            if (widgets[i].id.Item2 == 0)
                            {
                                if (widgets[i].currentState == GuiWidget.State.Done)
                                {
                                    if (widgetDialogue.ChangeText(2))
                                        widgetDialogue.active = false;
                                }
                            }
                        }

                        if (widgets[i].id.Item1 == WidgetType.Button)
                        {
                            widgetButton = (GuiWidgetButton)widgets[i];
                            widgetButton.Update(gMouse);
                        }

                        if (widgets[i].id.Item1 == WidgetType.Dialogue)
                        {

                        }

                        if (!widgets[i].active)
                            widgets.RemoveAt(i);
                    }
                }
            }
        }

        public void CreateWidgets()
        {
            createDialogue(new Rectangle(0, 720 - 128, 1280, 128), new Tuple<WidgetType, int>(WidgetType.Dialogue, 0), "<test>", Color.White, Assets.GetFont("munro24"), 2, new Color[] { Color.White, Color.DarkGray });
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (GuiWidget widget in widgets)
            {
                widget.Draw(batch);
            }
        }
    }
}
