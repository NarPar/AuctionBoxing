using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Player_Select_Menu
    {
        Texture2D player;
        public Color color;

        public Texture2D currentTexture;

        Rectangle bounds;

        Vector2 vTitle;

        Player_Select_Popup popup;


        public Player_Select_Menu(ContentManager content, Player_Select_Popup popup, SpriteFont font, Rectangle bounds)
        {
            this.popup = popup;

            vTitle = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString("Color Select").X / 2,
                    bounds.Y + bounds.Height / 4);

            this.bounds = bounds;
            
        }

        public void Activate()
        {
           // color = colors[colorIndex];
        }

        public void ChangeIndex(int playerIndex, KeyPressed key)
        {
            if (key == KeyPressed.Left)
                currentTexture = popup.GetColor(currentTexture, true);
            else if(key == KeyPressed.Right)
                currentTexture = popup.GetColor(currentTexture, false);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Color Select", vTitle, Color.White);

            spriteBatch.Draw(currentTexture, new Rectangle(bounds.X + bounds.Width / 2 - (currentTexture.Width / 2) * 4,
                bounds.Y + bounds.Height / 2 - (currentTexture.Height / 2) * 4, 4 * currentTexture.Width, 4 * currentTexture.Height), Color.White);
        }
    }
}
