using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
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


        Color[] playerColors = new Color[4];

        float loadTimer = .5f;


        #region Events

        public delegate void PlayersReadyHandler(Color[] playerColors);

        public event PlayersReadyHandler OnReady;

        #endregion

        public Player_Select_Popup(ContentManager content, Input_Handler[] inputs, Rectangle bounds)
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
                menus[i] = new Player_Select_Menu(content, font, playerMenuBounds[i]);

                
            }
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
                        playerColors[i] = menus[i].color;
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
                    if (key == KeyPressed.Jump)
                    {
                        // Are they joining?
                        if (!playerAdded[i])
                        {
                            totalPlayers++;
                            playerAdded[i] = true;

                            // Enable input for the menu
                            inputs[i].OnKeyRelease += menus[i].ChangeIndex;

                            menus[i].color = menus[i].colors[0];
                        }
                        else
                        {
                            // Are they readying or unreadying?
                            if (!ready[i])
                                ready[i] = true;
                            else
                                ready[i] = false;
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