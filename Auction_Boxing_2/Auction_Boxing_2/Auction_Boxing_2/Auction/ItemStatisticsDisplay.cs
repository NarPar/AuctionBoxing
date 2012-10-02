using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class ItemStatisticsDisplay : Display
    {
        string itemName;

        public ItemStatisticsDisplay(Vector2 pos, int width, SpriteFont font) :
            base(pos, width, font, "")
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
