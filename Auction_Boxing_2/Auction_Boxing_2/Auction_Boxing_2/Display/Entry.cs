using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Entry
    {
        public Rectangle rectangle;

        public Entry(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }
    }

    class StringEntry : Entry
    {
        string s;

        public StringEntry(Vector2 position, string s, SpriteFont font) : 
            base(new Rectangle((int)position.X, (int)position.Y, (int)font.MeasureString(s).X, (int)font.MeasureString(s).Y))
        {
            this.s = s;
        }
    }

    class TextureEntry : Entry
    {
        Texture2D texture;

        public TextureEntry(Vector2 position, Texture2D texture, int width, int height ) :
            base(new Rectangle((int)position.X, (int)position.Y, width, height))
        {
            this.texture = texture;
        }
    }

    class BarEntry : Entry
    {
        Texture2D texture;
        int maxWidth;

        public BarEntry(Vector2 position, Texture2D texture, int maxWidth, int height) :
            base(new Rectangle((int)position.X, (int)position.Y, maxWidth, height))
        {
            this.texture = texture;
            this.maxWidth = maxWidth;
        }
    }
}
