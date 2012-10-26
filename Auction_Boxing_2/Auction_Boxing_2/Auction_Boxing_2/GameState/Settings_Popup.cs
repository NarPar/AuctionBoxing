using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Settings_Popup : Popup
    {

        public Settings_Menu menu;
        Rectangle menuBounds;

        public Settings_Popup(ContentManager content, Input_Handler[] inputs, Rectangle bounds)
            : base(content, bounds)
        {
            
            
            // Make a white boundary smaller than the bounds.
            menuBounds = new Rectangle(bounds.Width / 20, bounds.Height / 20,
                bounds.Width - (bounds.Width / 20) * 2, bounds.Height - (bounds.Height / 20) * 2);
            //Debug.WriteLine(menuBounds.X + " " + menuBounds.Y + " " + menuBounds.Width + " " + menuBounds.Height);

            menu = new Settings_Menu(content.Load<SpriteFont>("Menu/menufont"), menuBounds);

            inputs[0].OnKeyRelease += menu.ChangeIndex;
        }

        public void Reset(Input_Handler[] inputs)
        {
            //inputs[0].OnKeyRelease += menu.ChangeIndex;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            base.Draw(spriteBatch, font);

            spriteBatch.Draw(whiteOut, menuBounds, Color.White * .5f);

            menu.Draw(spriteBatch, font);
        }
    }
}
