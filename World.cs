using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Lemonade
{
    public class World
    {
        ContentManager content;
        public Rectangle worldRect;
        public Rectangle cameraRect;
        Rectangle drawRect;

        Texture2D[] layerTextures;
        RenderTarget2D tileRenderTarget;
        bool[] drawLayer;
        bool[] tileLayer;

        public Player player;
        //TODO put all entityliving here
        public List<EntityLiving> entityLivings = new List<EntityLiving>();
        //public List<EntityLiving> enemies = new List<EntityLiving>();
        public List<ItemEntity> itemEntities = new List<ItemEntity>();
        public List<Particle> particles = new List<Particle>();
        public List<TileStatic> tilesStatic = new List<TileStatic>();
        public List<TileDynamic> tilesDynamic = new List<TileDynamic>();

        public int worldIndex;
        public bool pause, drawnTileRenderTarget = false, drawTileDEBUG = false;

        public int worldCountSecond;
        public int worldCountMinute;    //One second = one minute, IN GAME NOT IN CODE
        public int worldCountHour;
        public int worldCountDay;   //May not use

        public Color ambientColor;

        private Camera2D camera;
        public Game1 game;
    
        public void Initialize(Game1 setGame, ContentManager setContent, Rectangle setWorldRect, Rectangle setCameraRect, GraphicsDevice graphics)
        {
            game = setGame;
            worldRect = setWorldRect;
            cameraRect = setCameraRect;

            content = setContent;

            drawRect = new Rectangle(0, 0, 0, 0);

            layerTextures = new Texture2D[8];
            
            tileRenderTarget = new RenderTarget2D(graphics, worldRect.Width, worldRect.Height);

            drawLayer = new bool[8];
            tileLayer = new bool[8];

            player = createPlayer(Vector2.Zero, 1);

            worldIndex = 0;

            LoadWorld(worldIndex);
        }

        public void calculateWorldAlpha()
        {
            if (worldCountHour >= 18 && worldCountHour <= 22)
            {   //if it's evening: 6pm - 10pm
                if (ambientColor.A <= 100)
                {   //Increase alpha of overlay - once every hour in evening - 4hr
                    ambientColor.A += 25;
                }
            }
            else if (worldCountHour >= 22 || worldCountHour <= 2)
            {   //if it's midnight: 10pm - 2am
                ambientColor.A = 100;   //Set the color to full 100 alpha (full nightime)
            }   //pretty much just backup
            else if (worldCountHour >= 2 && worldCountHour <= 8)
            {   //if it's morning: 2am - 8am
                if (ambientColor.A >= 0)
                {   //decrease alpha of overlay by 17 each hour - 6hrs
                    ambientColor.A -= 17;
                }
            }
            else
                ambientColor.A = 0;

            if (ambientColor.A <= 0)
                ambientColor.A = 0;
            if (ambientColor.A >= 100)
                ambientColor.A = 100;
        }

        public void Update()
        {
            worldCountSecond++;

            int index = 0;
            foreach (EntityLiving el in entityLivings)
            {
                if (el is Player)
                {
                    if (!player.falling)
                        player.Control();
                    player.Update();
                }

                if (el is Enemy)
                {
                    el.Update();

                    if (el.dead)
                    {
                        entityLivings.RemoveAt(index);
                    }
                }
                ++index;
            }

            for (int p = 0; p < particles.Count; p++)
            {
                particles[p].Update();

                if (particles[p].dead)
                    particles.RemoveAt(p);
            }

            for (int ie = 0; ie < itemEntities.Count; ie++)
            {
                itemEntities[ie].Update();
                if (itemEntities[ie].dead)
                    itemEntities.RemoveAt(ie);
            }

            for (int t = 0; t < tilesDynamic.Count; t++)
            {
                tilesDynamic[t].Update(this);
            }

            camera.MoveTo(player.center, new Vector2(0, 0), true);

            CheckCollision();


            if (worldCountSecond >= 60)
            {
                worldCountMinute++;

                if (worldCountMinute >= 60)
                {
                    worldCountHour++;
                    calculateWorldAlpha();

                    if (worldCountHour >= 24)
                    {
                        worldCountDay++;

                        worldCountHour = 0;
                    }

                    worldCountMinute = 0;
                }

                worldCountSecond = 0;
            }
        }

        public void CheckCollision()
        {
            List<Tile> collidedTiles = new List<Tile>();

            int index = 0;
            //Check collisions with enemies
            //Handles damage dealing with player
            foreach (EntityLiving living in entityLivings)
            {
                //---- handle tile collisions ----//
                foreach (Tile tile in tilesStatic)
                {
                    if (living.hitbox.Intersects(tile.rect))
                    {
                        Vector2 centerDistance = tile.center - living.center;

                        Rectangle overlap = Rectangle.Intersect(tile.rect, living.hitbox);
                        if (tile.layer >= living.layer || tile.wall)
                        {
                            if (overlap.Width > overlap.Height)
                            {
                                if (centerDistance.Y > 0)
                                {
                                    living.position.Y = living.hitbox.Y - overlap.Height;
                                }
                                if (centerDistance.Y < 0)
                                {
                                    living.position.Y = living.hitbox.Y + overlap.Height;
                                }
                                
                            }
                            else
                            {
                                if (centerDistance.X > 0)
                                {
                                    living.position.X = living.hitbox.X - overlap.Width;
                                }
                                if (centerDistance.X < 0)
                                {
                                    living.position.X = living.hitbox.X + overlap.Width;
                                }
                            }
                        }
                        collidedTiles.Add(tile);
                    }
                }

                List<Tile> Sorted = collidedTiles.OrderByDescending(o =>
                {   //Sort by intersection area (larger = higher) - MAY BE REDUNDANT. REMOVE?
                    Rectangle overlap = Rectangle.Intersect(o.rect, living.hitbox);
                    return overlap.Width * overlap.Height;
                }).ToList();

                List<Tile>  Sorted2 = Sorted.OrderByDescending(o =>
                {   //Sort by layer, so only the tile under the player is calculated for falling.
                    return o.layer;
                }).ToList();

                Tile biggestIntersection = null;
                if (Sorted.Count != 0)
                    biggestIntersection = Sorted2[0];
                if (biggestIntersection != null && biggestIntersection.layer + 1 < living.layer)
                {
                    if (!living.falling)
                    {
                        living.falling = true;
                        living.fallingTime = 7;
                        living.layer--;
                    }
                }

                //---- handle entity collisions ----//
                if (living is Player)
                {
                    foreach (ItemEntity iEnt in itemEntities)
                    {
                        if (living.hitbox.Intersects(iEnt.hitbox))
                        {
                            if (player.PickupItem(iEnt))
                            {
                                iEnt.dead = true;
                            }
                        }
                    }
                    
                }

                if (living is Enemy)
                {
                    if (living.hitbox.Intersects(player.hitbox))
                    {
                        player.DealtDamage(living);
                    }

                    foreach (HurtBox hurtbox in player.hurtboxes)
                    {
                        if (hurtbox.rect.Intersects(living.hitbox))
                        {
                            player.DealDamage(living);
                        }
                    }
                }
                ++index;
            }
        }

        public Player createPlayer(Vector2 position, int layer)
        {
            Player p = new Player(position, 1);
            p.Initialize(this, camera);
            camera = new Camera2D(new Viewport(cameraRect), worldRect);
            entityLivings.Insert(0, p);

            return p;
        }

        /// <summary>
        /// Creates a new enemy.
        /// </summary>
        /// <param name="position">Position where the enemy is created.</param>
        /// <param name="id">Id of enemy to create.</param>
        /// <returns>instance of the created enemy.</returns>
        public Enemy createEnemy(Vector2 position, int layer, int id)
        {
            Enemy e = new Enemy(position, layer, id);
            e.Initialize(this, camera);
            entityLivings.Add(e);

            return e;
        }

        public ItemEntity createItemEntity(Vector2 position, Vector2 velocity, ItemStack droppedItem, int layer)
        {
            ItemEntity i = new ItemEntity(position, velocity, droppedItem);
            i.Initialize(this, camera);
            i.layer = layer;
            itemEntities.Add(i);

            return i;
        }

        /// <summary>
        /// Creates a new particle.
        /// </summary>
        /// <param name="position">Position where the particle is created.</param>
        /// <param name="velocity">Initial velocity of particle.</param>
        /// <param name="ai">AI type of particle. SEE: Particle.AiType for individual explanations.</param>
        /// <param name="color">Color of the particle.</param>
        /// <param name="timeLeft">NULLABLE. How long the particle lasts (60 = 1s)</param>
        /// <returns>instance of created particle.</returns>
        public Particle createParticle(Vector2 position, Vector2 velocity, Particle.AiType ai, Color color, int? timeLeft = null)
        {
            Particle p;

            if (timeLeft == null)
            {
                p = new Particle(position, velocity, ai, color);
            }
            else
            {
                p = new Particle(position, velocity, ai, (int)timeLeft, color);
            }

            p.Initialize(this, camera);

            particles.Add(p);

            return p;
        }

        /// <summary>
        /// Create a new static tile. 
        /// Static tiles do NOT UPDATE.
        /// If you want a tile that spawns projectiles/enemies, or moves, use a tileDynamic.
        /// </summary>
        /// <param name="brush">Rectangle of the position and size.</param>
        /// <param name="textureName">name of the texture to apply to the tile.</param>
        /// <param name="layer">layer on which to create the tile. 0-8</param>
        /// <returns>instance of the created tile.</returns>
        public Tile createTileStatic(Rectangle brush, string textureName, int layer, bool setWall = false, Directions setFacing = Directions.North)
        {
            TileStatic t = new TileStatic(brush, textureName, layer, this);
            t.Initialize(content);
            t.wall = setWall;
            t.facing = setFacing;
            tilesStatic.Add(t);

            return t;
        }

        /// <summary>
        /// Create a new dynamic tile.
        /// Dynamic tiles always run the update function every tick.
        /// </summary>
        /// <param name="brush">Rectangle of the position and size.</param>
        /// <param name="id">id of the dynamic tile to create.</param>
        /// <returns>instance of the created tile.</returns>
        public Tile createTileDynamic(Rectangle brush, int id, int layer)
        {
            TileDynamic t = new TileDynamic(brush, id, this);
            t.Initialize(content);
            tilesDynamic.Add(t);

            return t;
        }

        public void LoadWorld(int id)
        {
            Console.WriteLine("Loading world id " + id);
            //Clear ALL enemies/projectiles/tiles/etc
            entityLivings.Clear();
            tilesStatic.Clear();
            tilesDynamic.Clear();

            if (id == 0)
            {
                player = createPlayer(new Vector2(0, 0), 1);                
                //createEnemy(new Vector2(500, 300), 0);
                //createEnemy(new Vector2(300, 500), 1);

                createTileStatic(new Rectangle(0, 0, 512, 512), "tile_grass1", 0);
                createTileStatic(new Rectangle(512, 0, 64, 512), "tile_grass1", 4, true, Directions.West);
                createTileStatic(new Rectangle(0, 512, 512, 64), "tile_grass1", 4, true, Directions.North);
                createTileStatic(new Rectangle(512, 512, 64, 64), "tile_grass1", 4, true, Directions.NorthWest);

                for (int i = 8; i >= 0; i--)
                {
                    createTileStatic(new Rectangle(576 + (i * 64), 64, 64, 64), "tile_grass1", i);
                }
                createTileStatic(new Rectangle(256, 256, 64, 64), "tile_test", 4);

                createEnemy(new Vector2(576, 576), 2, 0);

                createItemEntity(new Vector2(576 + 64, 576), Vector2.Zero, new ItemStack(game.CreateItemWeapon(0), 1), 1);
                createItemEntity(new Vector2(576 + 128, 576 - 36), Vector2.Zero, new ItemStack(game.CreateItemWeapon(1), 58), 1);

                ambientColor = new Color(25, 25, 50, 100);

                //createTileDynamic(new Rectangle(128, 256, 64, 64), 0, 0);
            }  

            if (id == 1)
            {
                player = createPlayer(Vector2.Zero, 1);
                createEnemy(new Vector2(500, 300), 1, 0);
                createEnemy(new Vector2(300, 500), 1, 1);

                createTileStatic(new Rectangle(128, 128, 64, 64), "tile_grass1", 0);
                createTileStatic(new Rectangle(128, 360, 128, 64), "tile_grass1", 0);
            }
            drawnTileRenderTarget = false;
        }
        
        public void DrawTileRenderTarget(GraphicsDeviceManager graphics, SpriteBatch batch)
        {   //More efficent to do this way, as static tiles never move and so never need to be redrawn
            graphics.GraphicsDevice.SetRenderTarget(tileRenderTarget);
            graphics.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            
            foreach (TileStatic tile in tilesStatic)
            {
                tile.Draw(batch, camera);
            }

            graphics.GraphicsDevice.SetRenderTarget(null);

            drawnTileRenderTarget = true;
        }

        public void Draw(GraphicsDeviceManager graphics, GraphicsDevice device, SpriteBatch batch)
        {
            drawBGLayers(batch);

            if (!drawnTileRenderTarget)
                DrawTileRenderTarget(graphics, batch);

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            batch.Draw((Texture2D)tileRenderTarget, Vector2.Zero, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            batch.End();

            foreach (TileDynamic tile in tilesDynamic)
            {
                tile.Draw(batch, camera);
            }

            foreach (TileStatic tile in tilesStatic)
            {
                if (drawTileDEBUG)
                {
                    tile.DrawDEBUG(batch, camera);
                }
            }

            foreach (EntityLiving living in entityLivings)
            {
                living.Draw(batch);
            }

            foreach (Particle particle in particles)
            {
                particle.Draw(batch);
            }

            foreach (ItemEntity iEnt in itemEntities)
            {
                iEnt.Draw(batch);
            }

/*            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            PrimiviteDrawing.DrawRectangle(null, batch, worldRect, ambientColor);
            batch.End();*/

            //player.Draw(batch);
        }

        public void drawBGLayers(SpriteBatch batch)
        {
            for (int i = 0; i <= 7; i++)
            {   //loops through all the textures in layers.
                if (layerTextures[i] != null)   //makes sure it's not null, because that would cause errors
                {
                    //DEBUG
                    drawLayer[i] = true;
                    tileLayer[i] = true;

                    if (i == 1)
                        tileLayer[i] = false;
                    //

                    if (drawLayer[i])   //Makes sure the layer wants to draw
                    {
                        if (!tileLayer[i])
                        {   //Stupid things to make sure texture doesn't tile.
                            drawRect = layerTextures[i].Bounds; //sets drawRect to size of the current layer's texture.
                            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
                            batch.Draw(layerTextures[i], drawRect, drawRect, Color.White);
                        }
                        else
                        {
                            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
                            batch.Draw(layerTextures[i], worldRect, worldRect, Color.White);
                        }

                        batch.End();
                    }
                }   
            }
        }

        public void loadContent()
        {
            player.Initialize(this, camera);

            layerTextures[0] = content.Load<Texture2D>("textures/bgTest");
            //layerTextures[1] = content.Load<Texture2D>("textures/bgSky");
        }

        /// <summary>
        /// Saves player data.
        /// </summary>
        public void Save()
        {
            File.WriteAllText("player.json", "");   //Clear the file first.
            string output = JsonConvert.SerializeObject(player, Formatting.Indented);    //Player

            File.WriteAllText("player.json", output);
        }

        public void SaveMap()
        {
            File.WriteAllText("map1_test.json", "");   //Clear the file first.

            dynamic collectionWrapper = new
            {   //tells what all to serialize
                TileStatic = tilesStatic,
                TileDynamic = tilesDynamic,
                EntityLiving = entityLivings

            };
            string output = JsonConvert.SerializeObject(collectionWrapper, Formatting.Indented);    //serialize and turn to string

            File.AppendAllText("map1_test.json", output);
        }

        public void Load()
        {
            Player data = JsonConvert.DeserializeObject<Player>(File.ReadAllText("player.json"));

            player = data;
            player.Initialize(this, camera);
            //LoadWorldFromFile(player.location, false);
        }

        public void LoadWorldFromFile(string name, bool playerDefaultPos = true)
        {
            Console.WriteLine("a");
        }
    }
}
