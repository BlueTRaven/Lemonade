using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Lemonade;

namespace Lemonade
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /* TODO!
         * MAIN GAME:
         *#implement player
         *#Implement camera 
         *#enemy, loop through all
         * item system
         * projectiles (probably can just copy enemy code)
         *#particles
         * handle collision between several entities.
         * room system
         *#create tiles (more like "brushes")
         *#tile collision detection     TODO: fix
         *#player save
         *#player load
         * save/load system - probably only needed for player items/level/etc, and for what world player is in and player's location. Enemies probably not need to be saved. load through json?
         * player inventory system
         *-will not do Dynamic lighting
         * GUIS
         * LESS IMPORTANT
         *#safe deletion of entities
         */
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public World world;

        //public static ContentManager Content;

        public KeyboardState currentKBState, oldKBState;
        public GameMouse mouse;
        public MouseState currentMouseState;
        float Fps = 0f;
        private const int NumberSamples = 50; //Update fps timer based on this number of samples
        int[] Samples = new int[NumberSamples];
        int CurrentSample = 0;
        int TicksAggregate = 0;
        int SecondSinceStart = 0;

        public static bool paused = false;
        int timeSincePaused;

        GuiFull pauseMenu;

        Color fadeColor;

        Texture2D crosshairTex;

        public int displayWidth, displayHeight;
        double frameRate = 0.0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            displayWidth = 1280;
            displayHeight = 720;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = displayWidth;
            graphics.PreferredBackBufferHeight = displayHeight;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int DesiredFrameRate = 60;

            Logger.CreateNewLogFile();

            pauseMenu = new GuiFull(this, new Rectangle(0, 0, displayWidth, displayHeight), 0, Color.Black, false);
            world = new World();
            mouse = new GameMouse();

            TargetElapsedTime = new TimeSpan(TimeSpan.TicksPerSecond / DesiredFrameRate);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            int windowWidth = GraphicsDevice.Viewport.Width;
            int windowHeight = GraphicsDevice.Viewport.Height;

            Fonts.LoadFonts(Content);

            world.Initialize(this, Content, new Rectangle(0, 0, 2560, 1440), new Rectangle(0, 0, displayWidth, displayHeight), graphics.GraphicsDevice);
            mouse.Initialize(Content);

            world.loadContent();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            mouse.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!paused)
            {
                //pauseMenu.active = false;
                world.Update();
            }
            else
            {
                //pauseMenu.active = true;
            }

            if (keyPress(Keys.P))
            {
                if (!paused)
                    pauseMenu.Open();
                else
                    pauseMenu.Close();
            }

            pauseMenu.Update(mouse);
            oldKBState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// Fades the screen to black.
        /// </summary>
        /// <param name="timer">the timer used to calculate alpha.NOTE that this must be externally incremented.</param>
        /// <param name="maxTime">max amount of time in ticks to reach full black</param>
        /// <returns>if it's done fading.</returns>
        public bool FadeToBlack(int timer, int maxTime)
        {
            byte alphaIncrease = (byte)(255 / maxTime);

            if (timer <= maxTime)
            {
                fadeColor.A += alphaIncrease;
                Console.WriteLine(fadeColor);
                return false;
            }
            fadeColor = Color.Black;
            return true;
        }
        
        /// <summary>
        /// Used to detect single key presses.
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns>True if this is the first frame when the key is pressed.</returns>
        public bool keyPress(Keys key)
        {
            return (currentKBState.IsKeyDown(key) && oldKBState.IsKeyUp(key));
        }

        private float Sum(int[] Samples)
        {
            float RetVal = 0f;
            for (int i = 0; i < Samples.Length; i++)
            {
                RetVal += (float)Samples[i];
            }
            return RetVal;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {   //taken from some stackexchange, can't remember which
            Samples[CurrentSample++] = (int)gameTime.ElapsedGameTime.Ticks;
            TicksAggregate += (int)gameTime.ElapsedGameTime.Ticks;
            if (TicksAggregate > TimeSpan.TicksPerSecond)
            {
                TicksAggregate -= (int)TimeSpan.TicksPerSecond;
                SecondSinceStart += 1;
            }
            if (CurrentSample == NumberSamples) //We are past the end of the array since the array is 0-based and NumberSamples is 1-based
            {
                float AverageFrameTime = Sum(Samples) / NumberSamples;
                Fps = TimeSpan.TicksPerSecond / AverageFrameTime;
                CurrentSample = 0;
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //put all draw code (ex: player, enemy, etc) in between
            world.Draw(graphics, GraphicsDevice, spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            if (Fps > 0)
            {
                spriteBatch.DrawString(Fonts.munro, string.Format("Current FPS: {0}\r\nWorld time: Second: {1} Minute: {2} Hour: {3} World alpha: {4}\r\nPlayer position: X: {5} Y: {6}", Fps.ToString("000"), world.worldCountSecond, world.worldCountMinute, world.worldCountHour, world.ambientColor.A, world.player.position.X, world.player.position.Y), new Vector2(10, 10), Color.White);
            }
            
            /*
            if (paused)
            {
                PrimiviteDrawing.DrawRectangle(null, spriteBatch, spriteBatch.GraphicsDevice.Viewport.Bounds, fadeColor);
            } */
            pauseMenu.Draw(spriteBatch);

            mouse.Draw(spriteBatch);
            //PrimiviteDrawing.DrawLineSegment(null, spriteBatch, Vector2.Zero, world.cameraRect.Center.ToVector2(), Color.Orange, 1);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public ItemStack createItemStack(Item item, int stackSize)
        {
            ItemStack stack = new ItemStack(item, stackSize);
            return stack;
        }
        
        public ItemWeapon CreateItemWeapon(int id)
        {
            ItemWeapon iWep = new ItemWeapon(id);
            iWep.Initialize(this);
            return iWep;
        }
    }
}
