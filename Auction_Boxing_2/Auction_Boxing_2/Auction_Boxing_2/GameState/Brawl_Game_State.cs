using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    enum brawlgamestate
    {
        settings,
        playerselect,
        brawl,
        stats
    }

    // rounds not yet implemented
    class Brawl_Game_State : Game_State
    {
        #region Fields

        SpriteFont font;

        brawlgamestate state;

        Boxing_Manager boxingManager;

        Settings_Popup settings;

        Player_Select_Popup playerSelect;

        int rounds;



        #endregion

        public Brawl_Game_State(Game game, Input_Handler[] inputs, Rectangle bounds)
            : base(game, inputs, bounds)
        {

            font = game.Content.Load<SpriteFont>("Menu/menufont");
            state = brawlgamestate.settings; // Start by settings the number of rounds

            // construct the boxing manager for the background
            boxingManager = new Boxing_Manager(game.Content, bounds, inputs, game.GraphicsDevice); 

            // consruct the settings popup.
            settings = new Settings_Popup(game.Content, inputs, bounds);
            settings.menu.OnEntrySelect += HandleMenuSelect;
   
            // Construct the player_select_popup
            playerSelect = new Player_Select_Popup(game.Content, inputs, bounds, game.GraphicsDevice);
            //playerSelect.OnReady += HandlePlayersReady;

        }

        public override void Update(GameTime gameTime)
        {

            switch (state)
            {
                case(brawlgamestate.settings):
                    settings.Update(gameTime);
                    break;
                case (brawlgamestate.playerselect):
                    playerSelect.Update(gameTime);
                    break;
                case(brawlgamestate.brawl):
                    boxingManager.Update(gameTime);
                    break;
                case (brawlgamestate.stats):
                    break;

            }
           

            base.Update(gameTime);
        }

        public void HandleMenuSelect(string entry)
        {
            // Are we in the settings state?
            if (state == brawlgamestate.settings && entry == "Accept")
            {
                // Get the number of rounds
                rounds = settings.menu.Rounds;
                // stop listening
                settings.menu.OnEntrySelect -= HandleMenuSelect;

                // Move to the next state
                state = brawlgamestate.playerselect;

                // Change the listeners
                settings.menu.OnEntrySelect -= HandleMenuSelect;
                inputs[0].OnKeyRelease -= settings.menu.ChangeIndex;

                playerSelect.OnReady += HandlePlayersReady;

                // for testing!
                //ChangeState(new Main_Game_State(game, inputs, bounds));
            }
            if (state == brawlgamestate.playerselect)
            {
                
            }
        }

        // Handles the transition from player select to the brawl
        public void HandlePlayersReady(Color[] colors)
        {
            boxingManager.ApplySettings(colors);
            state = brawlgamestate.brawl;
            playerSelect.OnReady -= HandlePlayersReady;

            boxingManager.Reset();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case (brawlgamestate.settings):
                    boxingManager.Draw(gameTime, spriteBatch);
                    settings.Draw(spriteBatch, font);
                    break;
                case (brawlgamestate.playerselect):
                    boxingManager.Draw(gameTime, spriteBatch);
                    playerSelect.Draw(spriteBatch, font);
                    break;
                case (brawlgamestate.brawl):
                    boxingManager.Draw(gameTime, spriteBatch);
                    break;
                case (brawlgamestate.stats):
                    boxingManager.Draw(gameTime, spriteBatch);
                    break;

            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}
