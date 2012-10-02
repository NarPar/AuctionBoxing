using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{

    // Identifies state
    enum GameState
    {
        Menu,
        Auction,
        Boxing
    }


    /* Controls state flow!
     * 
     * 
     * 
     */ 
    class State_Manager
    {
        ContentManager Content;

        Rectangle ClientBounds;

        GameState state;

        //----Inputs----//
        public Input_Handler[] inputs = new Input_Handler[4];

        //-----State Managers-----//

        Menu_Manager Menu;
        Auction_Manager Auction;
        Boxing_Manager Boxing;

        #region Properties


        #endregion



        // Constructer
        public State_Manager(ContentManager Content, Rectangle ClientBounds)
        {
            this.Content = Content;
            this.ClientBounds = ClientBounds;
            state = GameState.Menu;

            for (int i = 0; i < 4; i++)
                inputs[i] = new Input_Handler();
        }

        public void Initialize()
        {
            Menu = new Menu_Manager(this, ClientBounds, inputs);
            Auction = new Auction_Manager(this, ClientBounds, inputs);
            Boxing = new Boxing_Manager(ClientBounds, inputs);
            
        }

        public void LoadContent()
        {
            Menu.LoadContent(Content);
            Auction.LoadContent(Content);
            Boxing.LoadContent(Content);

        }

        // Updates current state
        public void Update(GameTime gameTime)
        {
            foreach (Input_Handler i in inputs)
            {
                if(i != null)
                    i.Update();
            }

            if (state == GameState.Menu)
            {
                if (Menu.Update(gameTime))
                {
                    state = GameState.Auction;
                    Debug.WriteLine("State Changed to Auction!");
                    Auction.Initialize(Content);
                }
            }
            else if (state == GameState.Auction)
            {
                if (Auction.Update(gameTime))
                {
                    state = GameState.Boxing;
                    Debug.WriteLine("State Changed to Boxing!");
                    ;
                }
            }
            else if (state == GameState.Boxing)
            {
                if (Boxing.Update(gameTime))
                {
                    state = GameState.Menu;
                    Debug.WriteLine("State Changed to Menu!");
                    Boxing.Initialize(Content);
                }
            }
        }

        // Updates current state
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(state == GameState.Menu)
                Menu.Draw(spriteBatch);
            else if (state == GameState.Auction)
                Auction.Draw(gameTime, spriteBatch);
            else if (state == GameState.Boxing)
                Boxing.Draw(gameTime, spriteBatch);
        }
    }
}
