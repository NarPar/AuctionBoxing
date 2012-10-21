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
        public SelectionContent selection; // contains the color and texture being displayed
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

        public void ChangeIndex(int playerIndex, KeyPressed key)
        {
            if (key == KeyPressed.Left)
                selection = popup.GetColor(selection, true);
            else if (key == KeyPressed.Right)
                selection = popup.GetColor(selection, false);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Color Select", vTitle, Color.White);

            spriteBatch.Draw(selection.texture, new Rectangle(bounds.X + bounds.Width / 2 - (selection.texture.Width / 2) * 4,
                bounds.Y + bounds.Height / 2 - (selection.texture.Height / 2) * 4, 4 * selection.texture.Width, 4 * selection.texture.Height), Color.White);
        }
    }
}