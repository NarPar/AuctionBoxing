using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Camera camera;
        RenderTarget2D renderTarget;
        public Game_State currentState;
        public Input_Handler[] inputs;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";



            this.graphics.IsFullScreen = true;

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;

            camera = new Camera(new Rectangle(0,0, 800, 600));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //sm = new State_Manager(Content, Window.ClientBounds);
            //sm.Initialize();


            // Initialize the inputs.
            inputs = new Input_Handler[] { 
                new Input_Handler(0), 
                new Input_Handler(1), 
                new Input_Handler(2), 
                new Input_Handler(3) };

            // Set the current state to the main menu.
            currentState = new Main_Game_State(this, inputs, new Rectangle(0,0,Window.ClientBounds.Width, Window.ClientBounds.Height));

            //GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

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

            //sm.LoadContent();
            renderTarget = new RenderTarget2D(GraphicsDevice, 800, 600, false, SurfaceFormat.Color, DepthFormat.None);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //sm.Update(gameTime);

            foreach (Input_Handler input in inputs)
                input.Update();


            currentState.Update(gameTime);

            if (currentState is Brawl_Game_State)
            {
                Brawl_Game_State b = currentState as Brawl_Game_State;
                if (b.State == brawlgamestate.brawl)
                    camera.UpdateCamera(b.boxingManager.Players, GraphicsDevice);
            }
            else
                camera.FocusWholeScreen();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            //sm.Draw(gameTime, spriteBatch);

            currentState.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            
            int width = Window.ClientBounds.Height * (800/600);

            //Debug.WriteLine("width = " + width);
            //Debug.WriteLine("height = " + Window.ClientBounds.Height);

            Rectangle rect = new Rectangle((Window.ClientBounds.Width - width) / 2, 0, width, Window.ClientBounds.Height);

            if(!graphics.IsFullScreen)
                rect = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);

            spriteBatch.Draw(renderTarget, rect,
                camera.DrawToRectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
