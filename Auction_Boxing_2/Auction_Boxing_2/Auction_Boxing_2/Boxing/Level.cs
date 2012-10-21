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
        public Texture2D Background;
        Rectangle LevelSize;

        
        public Rectangle[] platforms = new Rectangle[5];

        public Level(Boxing_Manager bm, Rectangle clientBounds, Texture2D tPlatform, Texture2D background)
        {
            BoxingManager = bm;
            this.clientBounds = clientBounds;
            this.tPlatform = tPlatform;
            this.Background = background;

            LevelSize = new Rectangle(0, 0, clientBounds.Width, clientBounds.Height);//300 * 1, 255 * 1);

            platforms[0] = new Rectangle(18 * clientBounds.Width / 25, 8 * clientBounds.Height / 30, 7 * clientBounds.Width / 25, 10);

            platforms[1] = new Rectangle(0, 16 * clientBounds.Height / 36, 10 * clientBounds.Width/ 12, 10);

            platforms[2] = new Rectangle(3 * clientBounds.Width / 27, 10 * clientBounds.Height / 16, 3* clientBounds.Width / 18, 10);

            platforms[3] = new Rectangle(clientBounds.Width / 80, 9 * clientBounds.Height / 12, 3 * clientBounds.Width / 15, 10);

            // ground
            platforms[4] = new Rectangle(0, 6 * clientBounds.Height / 7, clientBounds.Width, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, LevelSize, Color.White);

            // Draw the level
            /*for (int i = 0; i < platforms.Length; i++)
            {
                spriteBatch.Draw(tPlatform, platforms[i], Color.Brown);
            }*/
        }
    }
}
