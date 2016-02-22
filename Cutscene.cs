using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /*public Cutscene(params object[] actors)
        {
            this.actors = actors;
        }*/

        /// <summary>
        /// Starts a new cutscene.
        /// </summary>
        /// <param name="id">Id of the cutscene to play.</param>
        /// <param name="actors">Actors of the cutscene; Player, npcs, enemies, etc.</param>
        public void StartCutscene(int id, params object[] actors)
        {
            this.playingID = id;
            this.actors = actors;

            ResolveActors();

            this.playing = true;
        }

        public void ResolveActors()
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

        public void Update()
        {
            if (playing)
            {

            }
        }
    }
}
