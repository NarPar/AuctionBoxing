using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Auction_Boxing_2
{
    /// <summary>
    /// A popup will dim whatever contents it is on, and display a menu.
    /// </summary>
    class Popup
    {
        protected Texture2D whiteOut;
        Texture2D darken;

        Rectangle bounds;

        Menu menu;

        public Popup(ContentManager content, Rectangle bounds)
        {
            this.bounds = bounds;

            whiteOut = content.Load<Texture2D>("White");
            darken = content.Load<Texture2D>("White");
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            // Dim the contents
            spriteBatch.Draw(darken, bounds, Color.Black * .5f);

        }
    }
}
