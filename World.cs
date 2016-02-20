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

using Lemonade.entity;
using Lemonade.tile;
using Lemonade.utility;

namespace Lemonade
{
    public class World
    {
        ContentManager content;
        public Rectangle worldRect;
        public Vector2 cameraPos;
        public Rectangle cameraRect;// { get { return new Rectangle((int)camera.Pos.X, (int)camera.Pos.Y, cameraRect.Width, cameraRect.Height); } }
        Rectangle drawRect;

        Texture2D[] layerTextures;
        RenderTarget2D tileRenderTarget;
        bool[] drawLayer;
        bool[] tileLayer;

        public Player player;
        public static List<EntityLiving> entityLivings = new List<EntityLiving>();
        public static List<ItemEntity> itemEntities = new List<ItemEntity>();
        public static List<Particle> particles = new List<Particle>();
        public static List<Tile> tiles = new List<Tile>();

        private List<Tile> collidedTiles = new List<Tile>();

        public int worldIndex;
        public bool pause, drawnTileRenderTarget = false, drawTileDEBUG = false;

        public int worldCountSecond;
        public int worldCountMinute;    //One second = one minute, IN GAME NOT IN CODE
        public int worldCountHour;
        public int worldCountDay;   //May not use

        public Color ambientColor;

        public static Camera2D camera;
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

            camera = new Camera2D(new Viewport(cameraRect), worldRect);

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
            //camera.Offset = Vector2.Zero;

            camera.MoveTo(player.center, new Vector2(0, 0), true);

            worldCountSecond++;

            if (!player.falling)
                player.Control(this);

            for (int i = entityLivings.Count - 1; i >= 0; i--)
            {
                entityLivings[i].Update();

                if (entityLivings[i].dead)
                    entityLivings.RemoveAt(i--);
            }

            for (int p = particles.Count - 1; p >= 0; p--)
            {
                particles[p].Update();

                if (particles[p].dead)
                    particles.RemoveAt(p--);
            }

            for (int ie = itemEntities.Count - 1; ie >= 0; ie--)
            {
                itemEntities[ie].Update();
                if (itemEntities[ie].dead)
                    itemEntities.RemoveAt(ie--);
            }

            cameraRect = new Rectangle((int)camera.PosUnclamped.X, (int)camera.PosUnclamped.Y, cameraRect.Width, cameraRect.Height);

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
            collidedTiles.Clear();

            //Check collisions with enemies
            //Handles damage dealing with player
            foreach (EntityLiving living in entityLivings)
            {
                //---- handle tile collisions ----//
                for (int t = tiles.Count - 1; t >= 0; t-- )
                {
                    if (living.hitbox.Intersects(tiles[t].rect))
                    {
                        if (tiles[t] is TileDynamic)
                        {
                            if (living is Player)
                            {
                                TileDynamic tileTrigger = (TileDynamic)tiles[t];

                                tileTrigger.OnCollision(this);

                                if (tileTrigger.dead)
                                    tiles.RemoveAt(t--);
                            }
                        }

                        if (tiles[t].solid)
                        {
                            Vector2 centerDistance = tiles[t].center - living.center;

                            Rectangle overlap = Rectangle.Intersect(tiles[t].rect, living.hitbox);
                            if (tiles[t].layer >= living.layer || tiles[t].wall)
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
                            collidedTiles.Add(tiles[t]);    
                        }
                    }
                }

                //Tile biggestIntersection = collidedTiles.Find(x => { return x.layer < player.layer; });   //No idea how to do this, really

                /*List<Tile> Sorted = collidedTiles.OrderByDescending(o =>
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
                }*/

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
            }
        }

        public void LoadWorld(int id)
        {
            Logger.Log("Loading world id " + id, true);

            //Clear ALL enemies/projectiles/tiles/etc
            entityLivings.Clear();
            tiles.Clear();

            if (id == 0)
            {
                player = Player.CreatePlayer(new Vector2(0, 0), 1);

                TileStatic.CreateTileStatic(new Rectangle(0, 0, 512, 512), Assets.tile_grass1, 0);
                TileStatic.CreateTileStatic(new Rectangle(512, 0, 64, 512), Assets.tile_rockwall, 4, true, Directions.West);
                TileStatic.CreateTileStatic(new Rectangle(0, 512, 512, 64), Assets.tile_rockwall, 4, true, Directions.North);
                TileStatic.CreateTileStatic(new Rectangle(512, 512, 64, 64), Assets.tile_rockwall, 4, true, Directions.NorthWest);
                //TileTrigger.CreateTileTrigger(new Rectangle(64, 0, 64, 64), true, 5, "<intro_3>");
                TileTrigger.CreateTileTriggerDamageBox(new Rectangle(64, 0, 64, 64), false, 10);
                TileTrigger.CreateTileTriggerKillBox(new Rectangle(128, 0, 64, 64), false);

                ItemEntity.CreateItemEntity(new Vector2(576 + 64, 576), Vector2.Zero, new ItemStack(game.CreateItemWeapon(0), 1), 1);
                ItemEntity.CreateItemEntity(new Vector2(576 + 128, 576 - 36), Vector2.Zero, new ItemStack(game.CreateItemWeapon(1), 58), 1);

                Enemy.CreateEnemy(new Vector2(576, 0), 2, 0);

                ambientColor = new Color(25, 25, 50, 100);

            }  

            if (id == 1)
            {
                player = Player.CreatePlayer(Vector2.Zero, 1);
                Enemy.CreateEnemy(new Vector2(500, 300), 1, 0);
                Enemy.CreateEnemy(new Vector2(300, 500), 1, 1);

                TileStatic.CreateTileStatic(new Rectangle(128, 128, 64, 64), "tile_grass1", 0);
                TileStatic.CreateTileStatic(new Rectangle(128, 360, 128, 64), "tile_grass1", 0);
            }
            drawnTileRenderTarget = false;
        }
        
        /*public void DrawTileRenderTarget(GraphicsDeviceManager graphics, SpriteBatch batch)
        {   //More efficent to do this way, as static tiles never move and so never need to be redrawn
            graphics.GraphicsDevice.SetRenderTarget(tileRenderTarget);
            graphics.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            
            foreach (Tile tile in tiles)
            {
                tile.Draw(batch);
            }

            graphics.GraphicsDevice.SetRenderTarget(null);

            drawnTileRenderTarget = true;
        }*/

        public void Draw(GraphicsDeviceManager graphics, GraphicsDevice device, SpriteBatch batch)
        {
            drawBGLayers(batch);

            //batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);

            //if (!drawnTileRenderTarget)
                //DrawTileRenderTarget(graphics, batch);

            //batch.End();

            //camera.Pos += camera.Offset;

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.GetTransformation());
            //PrimiviteDrawing.DrawRectangle(null, batch, finalRect, 1, Color.Red);

            batch.Draw((Texture2D)tileRenderTarget, Vector2.Zero, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            Rectangle finalRect = new Rectangle((int)camera.Pos.X - cameraRect.Width / 2, (int)camera.Pos.Y - cameraRect.Height / 2, 1280, 720);
            foreach (Tile tile in tiles)
            {
                if (tile.rect.Intersects(finalRect))
                    tile.Draw(batch);

                if (tile is TileStatic)
                {
                    TileStatic staticTile = (TileStatic) tile;
                    if (drawTileDEBUG)
                    {
                        staticTile.DrawDEBUG(batch, camera);
                    }
                }
            }

            foreach (EntityLiving living in entityLivings)
            {
                if (living.hitbox.Intersects(finalRect))
                    living.Draw(batch);
            }

            foreach (Particle particle in particles)
            {
                if (particle.hitbox.Intersects(finalRect))
                    particle.Draw(batch);
            }

            foreach (ItemEntity iEnt in itemEntities)
            {
                if (iEnt.hitbox.Intersects(finalRect))
                    iEnt.Draw(batch);
            }
            batch.End();

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            player.guiHUD.Draw(batch);
            player.guiInventory.Draw(batch);
            batch.End();
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
            player.Initialize();

            layerTextures[0] = Assets.GetTexture(Assets.bg_sky1);
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
                Tiles = tiles,
                EntityLiving = entityLivings
            };
            string output = JsonConvert.SerializeObject(collectionWrapper, Formatting.Indented);    //serialize and turn to string

            File.AppendAllText("map1_test.json", output);
        }

        public void Load()
        {
            Player data = JsonConvert.DeserializeObject<Player>(File.ReadAllText("player.json"));

            player = data;
            player.Initialize();
            //LoadWorldFromFile(player.location, false);
        }

        public void LoadWorldFromFile(string filePathName, bool playerDefaultPos = true)
        {
            tiles.Clear();
            entityLivings.Clear();
            itemEntities.Clear();

            player = Player.CreatePlayer(new Vector2(0, 0), 1);

            string[] data = Utilities.ReadFile(filePathName, "<data>");
            int numberTiles = Int32.Parse(data[0]), 
                numberEntities = Int32.Parse(data[1]);

            for (int i = 0; i < numberTiles; i++)
            {
                string[] tileData = Utilities.ReadFile(filePathName, "<tile" + (i + 1) + ">");

                string type = tileData[0];

                if (type == "static")
                {
                    string[] bounds = tileData[1].Split(',');
                    string texName = tileData[2];
                    int createLayer = Int32.Parse(tileData[3]);
                    string rawisWall= tileData[4];
                    bool isWall;
                    if (rawisWall == "true")
                        isWall = true;
                    else if (rawisWall == "false")
                        isWall = false;
                    else isWall = false;
                    TileStatic.CreateTileStatic(Utilities.CreateRectangleFromStrings(bounds), texName, createLayer, isWall, ParseDirections.ParseStringToDirections(tileData[5]));
                }
            }

            for (int i = 0; i < numberEntities; i++ )
            {

            }
        }
    }
}
