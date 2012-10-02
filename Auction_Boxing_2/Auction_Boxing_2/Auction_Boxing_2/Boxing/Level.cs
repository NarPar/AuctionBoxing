using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Level
    {
        Boxing_Manager BoxingManager;
        Rectangle clientBounds;
        Texture2D tPlatform;

        
        public Rectangle[] platforms = new Rectangle[3];

        public Level(Boxing_Manager bm, Rectangle clientBounds, Texture2D tPlatform)
        {
            BoxingManager = bm;
            this.clientBounds = clientBounds;
            this.tPlatform = tPlatform;

            platforms[0] = new Rectangle(0, 2 * clientBounds.Height / 5, clientBounds.Width, 20);

            platforms[1] = new Rectangle(0, 3 * clientBounds.Height / 5, clientBounds.Width, 20);

            // ground
            platforms[2] = new Rectangle(0, 4 * clientBounds.Height / 5, clientBounds.Width, 20);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the level
            for (int i = 0; i < platforms.Length; i++)
            {
                spriteBatch.Draw(tPlatform, platforms[i], Color.Brown);
            }
        }
    }
}
