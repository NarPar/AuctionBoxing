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

        //Dictionary<int, Color[]> frameData = new Dictionary<int, Color[]>();

        /// <summary>
        /// 
        /// </summary>
        Color[,] colorData;

        Color[] spriteImageData;

        /// <summary>
        /// All frames in the animation arranged horizontally.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { Texture = value; }
        }
        Texture2D texture;

        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            get { return frameWidth; }
        }
        int frameWidth;

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// base constructer without frame width specification
        /// </summary>
        public BitMap(Texture2D texture, int width)
        {
            Color[] data;
            Vector2 area;
            for(int i = 0; i < texture.Width / width; i++)
            {
                area = new Vector2();
                // Extract color data
                data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                // Convert it into an easily indexable 2d array
                Color[,] colors2D = new Color[texture.Width, texture.Height];
                for (int x = 0; x < texture.Width; x++)
                    for (int y = 0; y < texture.Height; y++)
                        colors2D[x, y] = data[x + y * texture.Width];

                colorData = colors2D;
            }
        }

        /*public static void CheckPixelCollision(Rectangle rectangleA, Color hit, BitMap hitMap, int frameIndexA, 
            Rectangle rectangleB, Color recieve, BitMap recieveMap, int frameIndexB)
        {
            // grab the Color[]
            Color[] dataA = hitMap.GetDataFromFrame(frameIndexA);
            Color[] dataB = recieveMap.GetDataFromFrame(frameIndexB);

            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);


            // Look at each pixel in the hit map
            for (int i = 0; i < hitMap.frameWidth; i++)
            {
                // and compare it against every pixel in the recieve map
                for (int j = 0; j < recieveMap.frameWidth; j++)
                {

                }
            }
        }*/

        /*public Color[] GetDataFromFrame(int index)
        {
            Color[] c;

            spriteImageData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(0,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                spriteImageData,
                currentFrame.X * currentFrame.Y,
                frameSize.X * frameSize.Y); 

            return c;
        }*/


        /*/// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(Texture2D texture, float frameTime, bool isLooping, int width)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            this.frameWidth = width;
        }*/
    }
}
