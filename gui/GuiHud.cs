using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;
using Lemonade.gui.guiwidget;
using Lemonade.utility;

namespace Lemonade.gui
{
    public class GuiHud : Gui
    {
        Player player;
        public bool dialogueOpen = false;
        public GuiHud(Player player)
        {
            this.player = player;
            this.bounds = new Rectangle(0, 0, 1280, 720);
            this.active = true;

            Game1.defaultPriorityGui = this;
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
                    GuiWidgetDialogue widgetDialogue = null;
                    GuiWidgetButton widgetButton = null;

                    for (int i = widgets.Count - 1; i >= 0; i--)
                    {
                        if (widgets[i].id.Item1 == WidgetType.Dialogue)
                        {
                            widgetDialogue = (GuiWidgetDialogue)widgets[i];
                            widgetDialogue.Update();

                            //Can only be 1 dialogue widget, always index of 0
                            if (widgets[i].id.Item2 == 0)
                            {
                                if (widgets[i].currentState == GuiWidget.State.Done)
                                {
                                    if (widgetDialogue.ChangeText(-1))
                                        widgetDialogue.active = false;
                                }
                            }
                        }

                        if (widgets[i].id.Item1 == WidgetType.Button)
                        {
                            widgetButton = (GuiWidgetButton)widgets[i];
                            widgetButton.Update();
                        }

                        if (!widgets[i].active)
                            widgets.RemoveAt(i--);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the health bar's text and size. Only run on player hit, and only modifies the health bar button widget (Type button, index 0)
        /// </summary>
        public void UpdateHealthBar()
        {
            GuiWidgetButton button = (GuiWidgetButton)widgets.Find(x => {
                return x.id.Item1 == WidgetType.Button && x.id.Item2 == 0;
            });

            if (button != null)
            {
                button.text = (player.health.ToString("0000") + " / " + player.maxHealth.ToString("0000"));

                float playerHealthRatio = ((float)player.health / (float)player.maxHealth);
                button.interiorBounds.Width = (int)((playerHealthRatio) * (button.bounds.Width - button.outlineWidth));
            }
        }

        public void CreateWidgets()
        {
            GuiWidgetButton b = createButton(new Rectangle(0, 0, 448, 64), new Tuple<WidgetType, int>(WidgetType.Button, 0), player.health.ToString(), GuiWidget.Alignment.Center, Color.White, Assets.GetFont(Assets.munro12), new Color[] { Color.Red * 0.8f, Color.DarkRed * 0.8f, Color.Orange * 0.8f });
            b.drawText = false;
            //createDialogue(new Rectangle(0, 720 - 128, 1280, 128), new Tuple<WidgetType, int>(WidgetType.Dialogue, 0), Color.White, Assets.GetFont(Assets.munro24), new Color[] { Color.White, Color.DarkGray }, 2, "<test>");
        }

        public void OpenDialogue(Vector2 position, int speed, string key, SpriteFont font)
        {
            bool hasDialogue = false;
            foreach(GuiWidget widget in widgets)
            {
                if (widget is GuiWidgetDialogue)
                {
                    hasDialogue = true;
                    break;
                }
            }

            if (!hasDialogue)
                createDialogue(new Rectangle((int)position.X, (int)position.Y/*720 - 128*/, 1280, 128), new Tuple<WidgetType, int>(WidgetType.Dialogue, 0), Color.White, font, new Color[] { Color.White, Color.DarkGray }, speed, key);
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (GuiWidget widget in widgets)
            {
                if (widget.id.Item1 == WidgetType.Button)
                {
                    GuiWidgetButton button = (GuiWidgetButton)widget;
                    if (widget.id.Item2 == 0)
                    {
                        if (player.health == player.maxHealth)
                            widget.draw = false;
                        else widget.draw = true; 
                        if (widget.GetState() == GuiWidget.State.Hot || widget.GetState() == GuiWidget.State.Active)
                        {
                            button.drawText = true;
                        }
                        else
                        {
                            button.drawText = false; 
                        }
                        if (widget.draw)
                            PrimiviteDrawing.DrawRectangle(null, batch, button.bounds, Color.LightGray * 0.5f);
                    }
                }
                widget.Draw(batch);
            }
        }
    }
}
