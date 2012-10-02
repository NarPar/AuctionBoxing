using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    enum AuctioneerState
    {
        FirstItem,
        NextItem,
        FirstBid,
        NewBid,
        GoingA,
        GoingB,
        GoingC,
        Sold
    }

    class Auctioneer
    {
        #region Fields

        AuctioneerState state;

        //----Animating
        AnimationPlayer sprite;
        Animation aIdle;
        Animation aGoing;
        Animation aSold;

        Rectangle rectangle;

        //----Speech
        SpriteFont font;
        Texture2D tSpeechBubble;
        Rectangle rSpeechBubble;

        string sCurrent;
        string sFirstItem = "Our first item up for auction is the ";
        string sNextItem = "Our next item is the ";
        string sFirstBid = " begins with an offer of ";
        string sNewBid = " raises the stakes to ";
        string[] sTimePass = { "Going once!", "Going twice!", "Going three times!" };
        string sSold = "Sold! To ";

        String parsedText = "";

        String typedText = "";

        double typedTextLength;

        int delayInMilliseconds;

        bool isDoneDrawing;


        //----Player Index
        public int iWinningPlayerIndex;
        
        //----Timers & Counters
        float timer_going;
        int counter_going;

        float timer_pause;

        //-----Events & Delegates-----//

        public delegate void ItemSoldHandler(int player_index);
        public delegate void NextItemHandler();

        public event ItemSoldHandler OnSold;
        public event NextItemHandler OnNextItem;

        #endregion



        public Auctioneer()
        {
            
        }

        public void Initialize(Rectangle podium_rectangle)
        {
            this.rectangle = new Rectangle(podium_rectangle.X + podium_rectangle.Width/2 , podium_rectangle.Y + (podium_rectangle.Height / 4), podium_rectangle.Width - 40, podium_rectangle.Height - 20);
            
            ChangeSpeech(AuctioneerState.FirstItem, -1, "");

            int speech_bubble_width = (int)font.MeasureString(sCurrent).X + 10;
            int speech_bubble_height = (int)font.MeasureString(sCurrent).Y + 10;

            rSpeechBubble = new Rectangle(rectangle.X + rectangle.Width - speech_bubble_width, rectangle.Y - rectangle.Height - speech_bubble_height,
                speech_bubble_width, speech_bubble_height);
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Menu/menufont");

            aIdle = new Animation(Content.Load<Texture2D>("Auction/Auctioneer"), 0.2f, true, 54);
            //aGoing = new Animation(...);
            //aSold = new Animation(...);

            tSpeechBubble = Content.Load<Texture2D>("Auction/Speech_Bubble");

            sprite.PlayAnimation(aIdle);

        }

        public void ChangeSpeech(AuctioneerState state, int player, string value)
        {
            this.state = state;

            switch (state)
            {
                case (AuctioneerState.FirstItem):
                    sCurrent = sFirstItem + value + ".";
                    break;
                case(AuctioneerState.NextItem):
                    sCurrent = sNextItem + value + ".";
                    break;
                case (AuctioneerState.FirstBid):
                    iWinningPlayerIndex = player;
                    sCurrent = "Player " + player + sFirstBid + value + " Million!";
                    timer_going = 3;
                    break;
                case (AuctioneerState.NewBid):
                    iWinningPlayerIndex = player;
                    sCurrent = "Player " + player + sNewBid + value + " Million!";
                    timer_going = 3;
                    break;
                case (AuctioneerState.GoingA):
                    sCurrent = sTimePass[0];
                    timer_going = 3;
                    break;
                case (AuctioneerState.GoingB):
                    sCurrent = sTimePass[1];
                    timer_going = 3;
                    break;
                case (AuctioneerState.GoingC):
                    sCurrent = sTimePass[2];
                    timer_going = 3;
                    break;
                case (AuctioneerState.Sold):
                    timer_going = 3;
                    sCurrent = sSold + "Player " + player + "!";
                    break;
                    
            }

            rSpeechBubble.Width = (int)font.MeasureString(sCurrent).X + 10;
            rSpeechBubble.Height = (int)font.MeasureString(sCurrent).Y + 10;
            rSpeechBubble.X = rectangle.X + rectangle.Width - rSpeechBubble.Width;

            int x = 0;
            typedText = "";
            typedTextLength = 0;
            parsedText = Tools.ParseText(font, sCurrent, rSpeechBubble, ref x);
            delayInMilliseconds = 50;
            isDoneDrawing = false;
        }

        public void Update(GameTime gameTime)
        {
            if (timer_going > 0)
                timer_going -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer_going <= 0)
            {
                switch (state)
                {
                    case (AuctioneerState.FirstBid):
                    case (AuctioneerState.NewBid):
                        ChangeSpeech(AuctioneerState.GoingA, -1, "");
                        //timer_going = 3;
                        break;
                    case (AuctioneerState.GoingA):
                        ChangeSpeech(AuctioneerState.GoingB, -1, "");
                        // timer_going = 0;
                        break;
                    case (AuctioneerState.GoingB):
                        ChangeSpeech(AuctioneerState.GoingC, -1, "");
                        // timer_going = 0;
                        break;
                    case (AuctioneerState.GoingC):
                        ChangeSpeech(AuctioneerState.Sold, iWinningPlayerIndex, "");
                        if (OnSold != null)
                        {
                            OnSold(iWinningPlayerIndex);
                        }//timer_going = 0;
                        break;
                    case (AuctioneerState.Sold):
                        if (OnNextItem != null)
                        {
                            OnNextItem();
                        }//timer_going = 0;
                        break;
                }
            }

            
            if (!isDoneDrawing)
            {
                
                if (delayInMilliseconds == 0)
                {
                    typedText = parsedText;
                    isDoneDrawing = true;
                }
                else if (typedTextLength < parsedText.Length)
                {
                    typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;

                    if (typedTextLength >= parsedText.Length)
                    {
                        typedTextLength = parsedText.Length;
                        isDoneDrawing = true;
                    }

                    typedText = parsedText.Substring(0, (int)typedTextLength);
                }
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //sprite.Draw(gameTime, spriteBatch, rectangle, 0, Color.White, Vector2.Zero, SpriteEffects.None);
            spriteBatch.Draw(tSpeechBubble, rSpeechBubble, Color.White);
            spriteBatch.DrawString(font, typedText, new Vector2(rSpeechBubble.X + 5, rSpeechBubble.Y), Color.Black);
        }
    }
}
