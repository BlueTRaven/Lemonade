using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lemonade.entity;
using Lemonade.utility;

namespace Lemonade
{
    public class Cutscene
    {
        public int playingID;

        object[] actors;
        Player player;
        List<EntityLiving> enemies = new List<EntityLiving>();
        List<Npc> npcs = new List<Npc>();

        public bool playing = false;

        private bool[] node;

        private int pauseTimer;
        private bool setPause, firstPauseFrame;
        /// <summary>
        /// Starts a new cutscene.
        /// </summary>
        /// <param name="id">Id of the cutscene to play.</param>
        /// <param name="actors">Actors of the cutscene; Player, npcs, enemies, etc.</param>
        public void StartCutscene(int id, params object[] actors)
        {
            this.playingID = id;
            this.actors = actors;

            SetDefaults();
            ResolveActors();

            this.playing = true;
        }

        private void SetDefaults()
        {
            if (playingID == 0)
            {
                this.node = new bool[4];
                this.node[0] = true;    //Start the first node playing
            }
        }

        private void ResolveActors()
        {
            for (int i = 0; i < actors.Length; i++)
            {   //Resolve actors to their own lists.
                if (actors[i] is Player)
                {
                    Player castPlayer = (Player)actors[i];
                    this.player = castPlayer;
                }

                if (actors[i] is Enemy)
                {
                    Enemy castEnemy = (Enemy)actors[i];
                    this.enemies.Add(castEnemy);
                }

                if (actors[i] is Npc)
                {
                    Npc castNpc = (Npc)actors[i];
                    npcs.Add(castNpc);
                }
            }
        }

        public void Initialize()
        {
            
        }

        public void Play(World world)
        {
            if (playingID == 0)
            {
                for (int i = 0; i < node.Length; i++)
                {
                    if (setPause)
                    {
                        pauseTimer--;
                        if (pauseTimer <= 0)
                        {
                            setPause = false;
                        }
                    }
                    else
                    {
                        if (node[i] == true)
                        {
                            bool isalreadyplaying = world.player.OpenDialogue(new Vector2(0, 720 - 128), "<intro_" + (i + 1) + ">");

                            if (!isalreadyplaying)
                            {
                                Pause(240);
                                CheckAndProceedCutscene(i, node.Length);
                                break;
                            }
                            firstPauseFrame = false;
                        }
                    }
                }
            }
        }
        
        public void CheckAndProceedCutscene(int index, int nodeLength)
        {
            if (index + 1 < nodeLength)
            {
                node[index] = false;
                node[index + 1] = true;
            }
            else if (index + 1 >= nodeLength)
            {
                playing = false;
            }
        }

        public void Pause(int time)
        {
            firstPauseFrame = true;
            setPause = true;
            pauseTimer = time;
        }
    }
}
