using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lemonade.Guis
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
                    CreateWidgets();
                    firstOpen = false;
                }

                if (game.priorityGui == this)
                {
                    List<GuiWidget> removeList = new List<GuiWidget>();
                    GuiWidgetDialogue widgetDialogue = null;
                    GuiWidgetButton widgetButton = null;

                    foreach (GuiWidget widget in widgets)
                    {
                        if (widget.id.Item1 == "dialogue")
                        {
                            widgetDialogue = (GuiWidgetDialogue)widget;
                            widgetDialogue.Update(gMouse);
                        }

                        if (widget.id.Item1 == "button")
                        {
                            widgetButton = (GuiWidgetButton)widget;
                            widgetButton.Update(gMouse);
                        }

                        if (widget.id.Item1 == "dialogue")
                        {
                            if (widget.id.Item2 == 0)
                            {
                                if (widget.currentState == GuiWidget.State.Done)
                                {
                                    if (widgetDialogue.ChangeText(2))
                                        widgetDialogue.active = false;
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

        public void CreateWidgets()
        {
            createDialogue(new Rectangle(0, 720 - 128, 1280, 128), new Tuple<string, int>("dialogue", 0), "<test>", Color.White, Fonts.munro24, 2, new Color[] { Color.White, Color.DarkGray });
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
