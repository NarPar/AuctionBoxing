using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

/*namespace Auction_Boxing_2
{

    // Identifies state
    enum GameState
    {
        Menu,
        Auction,
        LoadOut,
        Boxing
    }


    /* Controls state flow!
     * 
     * 
     * 
     *
    class State_Manager
    {
        ContentManager Content;

        Rectangle ClientBounds;

        GameState state;

        //----Inputs----//
        public Input_Handler[] inputs = new Input_Handler[4];

        //----Items----//
        public List<Item>[] items = new List<Item>[4];

        public Item[,] equipment = new Item[4,4];

        //-----State Managers-----//

        Menu_Manager Menu;
        Auction_Manager Auction;
        Boxing_Manager Boxing;
        LoadOut_Manager LoadOut;

        #region Properties


        #endregion



        // Constructer
        public State_Manager(ContentManager Content, Rectangle ClientBounds)
        {
            this.Content = Content;
            this.ClientBounds = ClientBounds;
            state = GameState.Menu;

            for (int i = 0; i < 4; i++)
                inputs[i] = new Input_Handler(i);
        }

        public void Initialize()
        {
            Menu = new Menu_Manager(this, ClientBounds, inputs);
            Auction = new Auction_Manager(this, ClientBounds, inputs);
            Boxing = new Boxing_Manager(this, ClientBounds, inputs);
            LoadOut = new LoadOut_Manager(this, ClientBounds, inputs);
        }

        public void LoadContent()
        {
            Menu.LoadContent(Content);
            Auction.LoadContent(Content);
            Boxing.LoadContent(Content);
            LoadOut.LoadContent(Content);

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
                    Auction.Initialize(Content);
                }
            }
            else if (state == GameState.Auction)
            {
                if (Auction.Update(gameTime))
                {
                    
                    state = GameState.LoadOut;
                    LoadOut.Activate(Content);
                    Debug.WriteLine("equipment [0,0] = " + equipment[0, 0]);
                }
            }
            else if (state == GameState.LoadOut)
            {
                if (LoadOut.Update(gameTime))
                {
                    state = GameState.Boxing;
                    Boxing.Activate(Content);
                    Debug.WriteLine("equipment [0,0] = " + equipment[0, 0]);
                }
            }
            else if (state == GameState.Boxing)
            {
                if (Boxing.Update(gameTime))
                {
                    state = GameState.Menu;
                    //Boxing.Initialize(Content);
                }
            }
        }

        // Updates current state
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (state == GameState.Menu)
                Menu.Draw(spriteBatch);
            else if (state == GameState.Auction)
                Auction.Draw(gameTime, spriteBatch);
            else if (state == GameState.Boxing)
                Boxing.Draw(gameTime, spriteBatch);
            else if (state == GameState.LoadOut)
                LoadOut.Draw(spriteBatch);
        }
    }
}
*/