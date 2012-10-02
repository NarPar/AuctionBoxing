using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Auction_Boxing_2
{
    /*enum AuctionPlayerState
    {
        idle,
        bidding
    }

    class Auction_Player
    {
        #region Fields

        //----Visuals
        AnimationPlayer sprite = new AnimationPlayer();
        Animation idle;
        Animation make_bid;

        Rectangle position;

        Texture2D tPaddle;
        Rectangle rPaddle;

        Texture2D text_back;

        AuctionPlayerState state;

        //----Accounting

        public float funds;

        public float bid;
        public Color bid_status; // Green if can bid, Red if cannot

        //----Won Items
        public List<Item> items = new List<Item>();

        #endregion


        public Auction_Player(Rectangle position)
        {
            state = AuctionPlayerState.idle;

            this.position = position;

            funds = 100;
            bid_status = Color.Green;

            bid = 10;
        }

        public void LoadContent(ContentManager Content)
        {
            idle = new Animation(Content.Load<Texture2D>("Auction/Top_Hat"), .2f, false, 667);
            make_bid = idle;

            tPaddle = Content.Load<Texture2D>("Auction/Auction_Paddle");

            text_back = Content.Load<Texture2D>("White");

            sprite.PlayAnimation(idle);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            sprite.Draw(gameTime, spriteBatch, position, 0, Color.White, Vector2.Zero, SpriteEffects.None);

            string f = "Funds: " + funds.ToString();
            string b = "Bid: " + bid.ToString();

            Vector2 p = new Vector2(position.X - (font.MeasureString(f).X / 2),
                position.Y - (position.Height / 2) - (font.MeasureString(f).Y / 2));

            Rectangle r = new Rectangle((int)(p.X - 2), (int)(p.Y - 2), (int)(font.MeasureString(f).X) + 4,
                (int)(font.MeasureString(f).Y + font.MeasureString(b).Y) + 4);

            spriteBatch.Draw(text_back, r, Color.White * .5f);

            spriteBatch.DrawString(font, "Funds: " + funds.ToString(), p, Color.Black);
            spriteBatch.DrawString(font, "Bid: " + bid.ToString(), new Vector2(p.X, p.Y + font.MeasureString("Bid").Y), bid_status);
        }
    }*/
}
