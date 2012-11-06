using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    enum brawlgamestate
    {
        settings,
        playerselect,
        loadout,
        brawl,
        stats
    }

    // rounds not yet implemented
    class Brawl_Game_State : Game_State
    {
        #region Fields

        SpriteFont font;

        SoundEffect soundSelect;

        brawlgamestate state;
        public brawlgamestate State { get { return state; } }


        // states
        Settings_Popup settingsManager;
        Player_Select_Popup playerSelectManager;
        LoadOut_Manager loadoutManager;
        public Boxing_Manager boxingManager;

        int rounds;
        Color[] colors; // Colors selected by the players
        List<Item>[] items; // an array of item lists containing all items, for use  in the loadout.

        #endregion

       

        public Brawl_Game_State(Game game, Input_Handler[] inputs, Rectangle bounds)
            : base(game, inputs, bounds)
        {
            // Load some assets
            font = game.Content.Load<SpriteFont>("Menu/menufont");
            SoundEffect menuSound = game.Content.Load<SoundEffect>("Sounds/MenuC");
            soundSelect = game.Content.Load<SoundEffect>("Sounds/MenuE");

            // Set initial state
            state = brawlgamestate.settings; // Start by settings the number of rounds
            settingsManager = new Settings_Popup(this, game.Content, inputs, bounds);

            // Use the boxing managers background for all the others:
            boxingManager = new Boxing_Manager(game.Content, bounds, inputs, game.GraphicsDevice, game.camera);

            // Initialize each players library of items
            items = new List<Item>[4];
            for(int i = 0; i < 4; i++)
            {
                items[i] = new List<Item>();
                // Each player gets access to the full library
                items[i].Add(new Cane(game.Content.Load<Texture2D>("LoadOut/cane_icon")));
                items[i].Add(new Revolver(game.Content.Load<Texture2D>("LoadOut/revolver_icon")));
                items[i].Add(new Bowler_Hat(game.Content.Load<Texture2D>("LoadOut/bowlerhat_icon")));
                items[i].Add(new Cape(game.Content.Load<Texture2D>("LoadOut/cape_icon")));
            }

          

            // old stuff:

            // consruct the settings popup.
            //settings// = new Settings_Popup(game.Content, inputs, bounds);
            //settings//.menu.OnEntrySelect += HandleMenuSelect;
   
            // Construct the player_select_popup
            //playerSelect = new Player_Select_Popup(game.Content, inputs, bounds, game.GraphicsDevice);

            // Construct the Loadout manager.

            // construct the boxing manager for the background
            //boxingManager = new Boxing_Manager(game.Content, bounds, inputs, game.GraphicsDevice, game.camera);



        }

        public override void Update(GameTime gameTime)
        {

            switch (state)
            {
                case(brawlgamestate.settings):
                    settingsManager.Update(gameTime);
                    break;
                case (brawlgamestate.playerselect):
                    playerSelectManager.Update(gameTime);
                    break;
                case (brawlgamestate.loadout):
                    loadoutManager.Update(gameTime);
                    break;
                case(brawlgamestate.brawl):
                    if (!boxingManager.Update(gameTime))
                        ChangeState(new Main_Game_State(this.game, inputs, bounds));
                    break;
                case (brawlgamestate.stats):
                    break;

            }

            base.Update(gameTime);
        }

        public void HandleMenuSelect(string entry)
        {
            // Are we in the settings state?
            //if (state == brawlgamestate.settings && entry == "Accept")
            //{
                
                // for testing!
                //ChangeState(new Main_Game_State(game, inputs, bounds));
            //}
            if (state == brawlgamestate.playerselect)
            {
                
            }
        }

        // Handles the transition from player select to the brawl
        public void HandlePlayersReady(Color[] colors)
        {
            
        }

        /// <summary>
        /// Called by substates to indicate they're ready. This state then 
        /// Gets the necessary information from them.
        /// </summary>
        /// <param name="state"></param>
        public override void OnStateComplete(string completedState)
        {
            switch (completedState)
            {
                case ("Settings"):
                    soundSelect.Play(); // Play the menu sound effect

                    boxingManager.NumRounds = settingsManager.GetRounds(); // Get the number of rounds

                    state = brawlgamestate.playerselect; // Move to the player select state
                    // initialize the player select state
                    playerSelectManager = new Player_Select_Popup(this, game.Content, inputs, bounds, game.GraphicsDevice);

                    // We don't need the settings manager anymore, so de-reference it.
                    settingsManager.Destruct();
                    settingsManager = null;


                    break;
                case ("PlayerSelect"):
                    soundSelect.Play(); // Play sounds

                    // Get the colors
                    colors = playerSelectManager.GetPlayerColors;

                    state = brawlgamestate.loadout;
                    loadoutManager = new LoadOut_Manager(this, bounds, inputs, items);
                    loadoutManager.LoadContent(game.Content);
                    loadoutManager.Activate(game.Content);

                    playerSelectManager.Destruct();
                    playerSelectManager = null;

                    Debug.WriteLine("Entered Loadout state");

                    break;
                case ("LoadOut"):
                    soundSelect.Play(); // Play sounds

                    state = brawlgamestate.brawl; // change the state

                    // Set up the boxer's with their colors and equipment
                    boxingManager.ApplySettings(colors, loadoutManager.equipment);
                    boxingManager.Reset();

                    // Unhook any event listenters and dereference the loadoutManager
                    loadoutManager.Destruct();
                    loadoutManager = null;

                    Debug.WriteLine("Entered Boxing state");

                    break;
                case ("Boxing"):
                    break;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case (brawlgamestate.settings):
                    boxingManager.Draw(gameTime, spriteBatch);
                    settingsManager.Draw(spriteBatch, font);
                    break;
                case (brawlgamestate.playerselect):
                    boxingManager.Draw(gameTime, spriteBatch);
                    playerSelectManager.Draw(spriteBatch, font);
                    break;
                case (brawlgamestate.loadout):
                    boxingManager.Draw(gameTime, spriteBatch);
                    loadoutManager.Draw(spriteBatch);
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
