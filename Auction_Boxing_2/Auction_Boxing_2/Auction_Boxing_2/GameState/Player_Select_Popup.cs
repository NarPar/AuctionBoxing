using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /// <summary>
    /// Holds a color and a texture with that color.
    /// </summary>
    class SelectionContent
    {
        public Color color;
        public Texture2D texture;
        public SelectionContent(Color color, Texture2D texture)
        {
            this.color = color;
            this.texture = texture;
        }
    }


    /* Initializes players then
     * waits for players to join, configure their control settings, and ready 
     * 
     * NOTE: To select a menu entry, press Right key.
     */
    class Player_Select_Popup : Popup
    {

        //List<Input_Handler> inputs = new List<Input_Handler>();
        Input_Handler[] inputs = new Input_Handler[4];

        Player_Select_Menu[] menus = new Player_Select_Menu[4];

        bool[] ready = new bool[4];
        protected static int totalPlayers = 0;

        bool[] playerAdded = new bool[4];

        Rectangle[] playerMenuBounds = new Rectangle[4];

        Texture2D menuBackground;
        SpriteFont font;
        SpriteFont titlefont;

        Rectangle ClientBounds;

        // The total selection of colors for the players to choose from
        Color[] colors = new Color[] { Color.Navy, Color.DarkGoldenrod, Color.Sienna, Color.RoyalBlue, Color.DarkOrange, Color.YellowGreen,
            Color.Lavender, Color.SlateGray, Color.Maroon, Color.MediumSeaGreen };

        // The available textures.
        List<SelectionContent> availableSelections = new List<SelectionContent>();


        Color[] playerColors = new Color[4];

        float loadTimer = .5f;


        #region Events

        public delegate void PlayersReadyHandler(Color[] playerColors);

        public event PlayersReadyHandler OnReady;

        #endregion

        public Player_Select_Popup(ContentManager content, Input_Handler[] inputs, Rectangle bounds, GraphicsDevice graphicsDevice)
            : base(content, bounds)
        {
            this.inputs = inputs;
            this.ClientBounds = bounds;

            font = content.Load<SpriteFont>("Menu/menufont");
            for (int i = 0; i < 4; i++)
            {
                // Hook up input listener
                inputs[i].OnKeyRelease += HandleInput;

                // Set positions
                playerMenuBounds[i] = new Rectangle(((i) * ClientBounds.Width / 4), 0, ClientBounds.Width / 4, ClientBounds.Height);

                // Initialize the meus
                menus[i] = new Player_Select_Menu(content, this, font, playerMenuBounds[i]);

                
            }

            populateColors(content, graphicsDevice);
        }


        /// <summary>
        ///  make a new texture for each color, rewriting magenta with the color.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        void populateColors(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // load the texture to be recolored
            Texture2D template = content.Load<Texture2D>("Boxing/Player_Idle_Side");

            // make a new texture that is a recoloring of the template.
            for (int i = 0; i < colors.Length; i++)
            {
                Texture2D t = new Texture2D(graphicsDevice, template.Width, template.Height);
                Color[] c = new Color[template.Width * template.Height];
                template.GetData(c);

                // Replace magenta pixels with the color
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j] == Color.Magenta)
                    {
                        c[j] = colors[i];
                    }

                }
                
                t.SetData(c); // set the data
                // add it to the available list
                availableSelections.Add(new SelectionContent(colors[i], t));
            }
        }

        /// <summary>
        /// A player looking for a new color will call this; they'll swap
        /// their current texture for a different available texture.
        /// </summary>
        /// <param name="texturetobeswitched">The texture the player currently has</param>
        /// <returns></returns>
        public SelectionContent GetColor(SelectionContent texturetobeswitched, bool l)
        {
            if (availableSelections.Count > 0)
            {

                int pop = 0;
                int push = availableSelections.Count - 1;

                if (l)
                {
                    pop = push;
                    push = 0;
                }

                SelectionContent t = availableSelections[pop];
                availableSelections.RemoveAt(pop);

                availableSelections.Insert(push, texturetobeswitched);

                return t;
            }
            else
                return null;
        }

        public void LoadContent(ContentManager Content)
        {
            menuBackground = Content.Load<Texture2D>("White");
            font = Content.Load<SpriteFont>("Menu/menufont");
            titlefont = Content.Load<SpriteFont>("Menu/titlefont");

        }

        /* Update returns null until all players have readied. When all players
         * are ready, it returns a list containing their custom Input_Handlers
         */
        public override void Update(GameTime gameTime)
        {
            if(loadTimer > 0)
                loadTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Count how many players are ready
            int count = 0;
            foreach (bool r in ready)
            {
                if (r)
                    count++;
            }
            
            // Is everyone ready to start?
            if (totalPlayers != 0 && count == totalPlayers)
            {
                // Get the colors
                for (int i = 0; i < 4; i++)
                {
                    if (playerAdded[i])
                        playerColors[i] = menus[i].selection.color;
                    else
                        playerColors[i] = Color.Transparent;
                }


                if (OnReady != null)
                {
                    OnReady(playerColors);
                }
            }
        }


        public void HandleInput(int player_index, KeyPressed key)
        {
            if(loadTimer <= 0)
            {
            for (int i = 0; i < 4; i++)
            {
                // Find the player
                if (player_index == i)
                {
                    if (key == KeyPressed.Attack1)
                    {
                        // Are they joining?
                        if (!playerAdded[i])
                        {
                            totalPlayers++;
                            playerAdded[i] = true;

                            // Enable input for the menu
                            inputs[i].OnKeyRelease += menus[i].ChangeIndex;

                            if (availableSelections.Count > 0)
                            {
                                menus[i].selection = availableSelections[0];
                                availableSelections.RemoveAt(0);
                            }
                        }
                        else
                        {
                            // Are they readying or unreadying?
                            if (!ready[i])
                            {
                                ready[i] = true;
                                menus[i].ready = true;
                            }
                            else
                            {
                                menus[i].ready = false;
                                ready[i] = false;
                            }
                        }
                    }

                }
            }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            base.Draw(spriteBatch, font);

            //If a player hasn't join yet, invite them to do so!
            for (int i = 0; i < 4; i++)
            {
                if (!playerAdded[i])
                {
                    spriteBatch.DrawString(font, "<Jump to Join>", new Vector2(playerMenuBounds[i].X + playerMenuBounds[i].Width/ 2 - font.MeasureString("<Jump to Join>").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("<Jump to Join>").Y / 2), Color.White);
                }
                else if (!ready[i])
                {
                    if (menus[i] != null)
                        menus[i].Draw(spriteBatch, font);
                }
                else
                {
                    spriteBatch.DrawString(font, "READY!", new Vector2((playerMenuBounds[i].X + playerMenuBounds[i].Width/ 2 - font.MeasureString("READY!").X / 2),
                        (ClientBounds.Height / 2) - font.MeasureString("READY!").Y / 2), Color.Green);
                }
            }
        }
    }
}