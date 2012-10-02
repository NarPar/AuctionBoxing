using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

/*namespace Auction_Boxing_2
{
    class Auction_Manager
    {
        #region fields

        //State_Manager state_manager;

        SpriteFont font;

        Texture2D effect;
        Texture2D background;

        //AnimationPlayer AuctioneerSprite = new AnimationPlayer();
        //Animation aAuctioneer_Sold;

        Texture2D tPodium;
        Texture2D tItemStand;
        //Texture2D tSpeechBubble;

        Texture2D tCurrentItem;
        Rectangle rCurrentItem;


        Rectangle rPodium;
        Rectangle rItemStand;
        //Rectangle rSpeechBubble;
        //Rectangle rAuctioneer;

        Auctioneer auctioneer;

        //string auctioneerNext = "Our next item is ";
        //string auctioneerHighestBid = "Highest bid is ";
        //string[] auctioneerTimePass = { "Going once!", "Going twice!", "Going three times!" };
        //string auctioneerSold = "SOLD! To ";
        //string auctioneer_Current_String;
        //float auctioneer_timer = 3.0f;
        //int timer_counter;

        bool bAuctionComplete = false;
        

        ItemStatDisplay display;
        //ItemStatisticsDisplay display;

        KeyboardState kb;

        Input_Handler[] inputs = new Input_Handler[4];

        Auction_Player[] players = new Auction_Player[4];

        //Item[] boughtItems; //Items bought by players at their correspoding indexes. jagged or 2D array?

        List<Item> items_for_auction = new List<Item>(); // a list of the items up for auction;

        Item current_item; // the current item up for auction, will be passed to display. MAY NOT NEED VAR FOR THIS
        int items_sold = 0;

        int prev_bid = 0;
        int bid_to_beat = 0; // The current highest bid. Any bids over this will replace it.

        //float timer; // Start timer after first bid on item, reset it every bid after that. When it terminates, move onto next item.

        /* Perhaps every players bidding thingy starts at the bid_to_beat, then they can increase it using Up and Down.
         * Each players bidding value is displayed in green, when they lack sufficient funds to make a bid, 
         * the cannot bid on the current item (maybe a bool?) and their bidding value turns red.
         *

        int winning_player; // Represents the winning players index (0-3). 
        
        /* When the item timer runs out, the current item is placed in this players index in items[].
         *

        Rectangle bounds;
        int stageHeight;

        #endregion

        public Auction_Manager(State_Manager sm, Rectangle ClientBounds, Input_Handler[] inputs)
        {
            this.state_manager = sm;

            this.bounds = new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height);
            this.inputs = inputs;

            auctioneer = new Auctioneer();
            auctioneer.OnSold += SellItem;
            auctioneer.OnNextItem += NextItem;

            //auctioneer_Current_String = auctioneerNext;

            //Fill items_for_auction list here!
        }

        public void Initialize(ContentManager Content)
        {
            //Initialize how an Auction_Player for how many inputs have been activated.
            for (int i = 0; i < 4; i++)
            {
                if (inputs[i].isActive)
                {
                    int width = bounds.Width / 7;
                    int height = bounds.Height / 7;

                    players[i] = new Auction_Player(new Rectangle((i + 1) * (bounds.Width / 5), bounds.Height,
                        width, height));

                    inputs[i].OnKeyRelease += HandleInput;
                }

            }

            //Load their content.
            for (int i = 0; i < 4; i++)
            {
                if (players[i] != null)
                {
                    players[i].LoadContent(Content);
                }
            }

            GenerateItems(Content);
            NextItem();
        }

        private void GenerateItems(ContentManager Content)
        {
            items_for_auction.Add(new Cane(Content.Load<Texture2D>("BoxingItems/cane"),
                    Content.Load<Texture2D>("BoxingItems/Cane_Attack"),
                    Content.Load<Texture2D>("LoadOut/cane_icon")));
            items_for_auction.Add(new Bowler_Hat(Content.Load<Texture2D>("Items/bowlerhat_image"),
                Content.Load<Texture2D>("BoxingItems/Bowler_Attack"),
                Content.Load<Texture2D>("LoadOut/bowlerhat_icon")));
            items_for_auction.Add(new Revolver(Content.Load<Texture2D>("Items/revolver_image"),
                Content.Load<Texture2D>("BoxingItems/Revolver_Attack"),
                Content.Load<Texture2D>("LoadOut/revolver_icon")));
            items_for_auction.Add(new Boots(Content.Load<Texture2D>("Items/Boots_Image"),
                Content.Load<Texture2D>("BoxingItems/gust_attack"),
                Content.Load<Texture2D>("LoadOut/boots_icon"),
                Content.Load<Texture2D>("Boxing/ffsp1charge"),
                Content.Load<Texture2D>("Boxing/ffsp1jumping")));
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Auction/auction_display_font");

            effect = Content.Load<Texture2D>("White");
            background = Content.Load<Texture2D>("Auction/Stage_Background");
            tPodium = Content.Load<Texture2D>("Auction/Podium");
            tItemStand = Content.Load<Texture2D>("Auction/Item_Stand");

            auctioneer.LoadContent(Content);

            display = new ItemStatDisplay(font, new Rectangle(bounds.Width / 8, bounds.Width / 6, bounds.Width / 4, bounds.Height / 4),
                Content.Load<Texture2D>("White"), Content.Load<Texture2D>("White"));

            
            stageHeight = 8 * (bounds.Height / 9);

            rPodium = new Rectangle(5 * (bounds.Width / 6) - 2 * tPodium.Width / 3, stageHeight - 2 * tPodium.Height / 3, 2 * tPodium.Width / 3, 2 * tPodium.Height / 3);
            rItemStand = new Rectangle(bounds.Width / 2 - tItemStand.Width, stageHeight - 2 * tItemStand.Height, 2 * tItemStand.Width, 2 * tItemStand.Height);

            auctioneer.Initialize(rPodium);
        }

        private void SellItem(int player)
        {
            players[player - 1].funds -= (int)players[player - 1].bid;
            players[player - 1].items.Add(current_item);
            items_for_auction.Remove(current_item);
            items_sold++;

            prev_bid = 0;
            bid_to_beat = 0;

            //Check if last item
            if (items_for_auction.Count <= 0)
            {
                bAuctionComplete = true;
            }
        }

        /* Pick random item from the list, set the display to that item, make auctioneer introduce item, and place
         * item texture on stand
         *
        private void NextItem()
        {
            // Select random item from the list
            Random rnd = new Random();

            current_item = items_for_auction[rnd.Next(items_for_auction.Count - 1)];


            display.SetItem(current_item);

            //Set Auctioneer text
            if(items_sold == 0)
                auctioneer.ChangeSpeech(AuctioneerState.FirstItem, -1, current_item.name);
            else
                auctioneer.ChangeSpeech(AuctioneerState.NextItem, -1, current_item.name);

            tCurrentItem = current_item.display_texture;
            rCurrentItem = new Rectangle(rItemStand.X, rItemStand.Y - rItemStand.Height, rItemStand.Width, rItemStand.Height);

            items_sold++;
        }

        private void MakeBid(int player, int bid)
        {
            if(bid_to_beat == 0)
                auctioneer.ChangeSpeech(AuctioneerState.FirstBid, player, bid.ToString());
            else
                auctioneer.ChangeSpeech(AuctioneerState.NewBid, player, bid.ToString());

            prev_bid = bid_to_beat;
            bid_to_beat = bid;

        }

        public void HandleInput(int player_index, KeyPressed key)
        {
            if (key == KeyPressed.Defend)
            {
                int bid = (int)players[player_index].bid;
                int funds = (int)players[player_index].funds;

                if (bid <= funds && bid > bid_to_beat)
                {
                    //players[player_index].funds -= bid;
                    MakeBid(player_index + 1, bid);

                    if (players[player_index].funds < bid)
                        players[player_index].bid = players[player_index].funds;
                }
            }
            if (key == KeyPressed.Up)
            {
                if (players[player_index].bid < players[player_index].funds)
                {
                    players[player_index].bid += 1;
                }
            }
            if (key == KeyPressed.Down)
            {
                if (players[player_index].bid > 0)
                {
                    players[player_index].bid -= 1;
                }
            }
        }

        public bool Update(GameTime gameTime)
        {
            auctioneer.Update(gameTime);

            kb = Keyboard.GetState();

            display.Update(gameTime);

            for (int i = 0; i < 4; i++)
            {
                if (players[i] != null)
                {
                    players[i].Update(gameTime);
                    if (players[i].bid <= bid_to_beat)
                        players[i].bid_status = Color.Red;
                    else
                        players[i].bid_status = Color.Green;
                }
            }

            //Switch to Boxing by pressing T
            if (kb.IsKeyDown(Keys.T) || bAuctionComplete)
                return true;
            else
                return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bounds, Color.White);

            //draw sprite behind podium
            auctioneer.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(tPodium, rPodium, Color.White);
            spriteBatch.Draw(tItemStand, rItemStand, Color.White);
            spriteBatch.Draw(tCurrentItem, rCurrentItem, Color.White);

            display.Draw(spriteBatch, font, true);

            for (int i = 0; i < 4; i++)
            {
                if (players[i] != null)
                {
                    players[i].Draw(gameTime, spriteBatch, font);
                }
            }

            //A dark effect to make the light over the item look more pronounced.
            spriteBatch.Draw(effect, bounds, Color.Black * .3f);
            //spriteBatch.Draw(tSpeechBubble, rSpeechBubble, Color.White);


            //spriteBatch.DrawString(font, auctioneer_Current_String, new Vector2(rSpeechBubble.X + 5, rSpeechBubble.Y), Color.Black);
            spriteBatch.Draw(effect, new Rectangle(4 * bounds.Width / 9, 0, bounds.Width / 9, stageHeight - rItemStand.Height + 20), Color.White * .3f);

            string pass = "Press 'T' to skip";
            spriteBatch.DrawString(font, pass,
                new Vector2(bounds.Width - font.MeasureString(pass).X,
                    bounds.Height - font.MeasureString(pass).Y), Color.White);
        }
    }
}*/
