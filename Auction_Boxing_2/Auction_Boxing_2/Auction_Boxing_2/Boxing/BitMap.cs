using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace Auction_Boxing_2
{
    class BitMap
    {

        Texture2D Texture;


        /// <summary>
        /// base constructer without frame width specification
        /// </summary>
        public BitMap(Texture2D texture)
        {
            Texture = texture;
        }


        public Color[] GetData(int FrameIndex, int FrameWidth, int FrameHeight)
        {

            // Set the size of the array
            Color[] c = new Color[FrameWidth * FrameHeight];

            // Get the data and put it into the array
            Texture.GetData(0,
                new Rectangle(FrameIndex * FrameWidth, 0, FrameWidth, FrameHeight),
                c,
                0,
                FrameWidth * FrameHeight);

            return c;
        }
        
    }
}
