using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Auction_Boxing_2
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }
        int frameIndex;

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color, Vector2 position, float rotation, Vector2 customOrigin, float scale, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            AdvanceTimePosition(gameTime);

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.Texture.Height);

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, position, source, color, rotation, Origin, scale, spriteEffects, 0.0f);
            
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation. Alternate function allows
        /// for destRectangle and custom origin.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destPosition, float rotation, Color color,  SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            AdvanceTimePosition(gameTime);

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.Texture.Height);

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, destPosition, source, color, rotation, Vector2.Zero, spriteEffects, 0.0f);
        }

        public void AdvanceTimePosition(GameTime gameTime)
        {
            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }
        }

        /*public Color[] GetDataFromFrame()
        {
            Color[] c;

            c = new Color[Animation.FrameWidth * Animation.FrameHeight];
            Animation.Texture.GetData(0,
                new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight),
                c,
                0,
                Animation.FrameWidth * Animation.FrameHeight);

            return c;
        }*/

        public Color[] GetData()
        {
            Color[] c = Get1DColorDataArray();



            //c = new Color[Animation.Texture.Width * Animation.Texture.Height];
            //Animation.Texture.GetData(c);


            return c;
        }

        public Texture2D GetDataAsTexture(GraphicsDevice gd)
        {



            // Get data from spritesheet using the frame rectangle as a source
            Color[] data = new Color[Animation.FrameWidth * Animation.FrameHeight];
            Animation.Texture.GetData(0, 
                 new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight),
                 data, 0, Animation.FrameWidth * Animation.FrameHeight);

            // Create a new texture and assign the data to it
            Texture2D texture = new Texture2D(gd, Animation.FrameWidth, Animation.FrameHeight);
            texture.SetData(data);

            return texture;
        }

        /// <summary>
        /// Fills a 2D array with the color data from the current frame of the texture.
        /// </summary>
        /// <returns></returns>
        public Color[] Get1DColorDataArray()
        {
            // Set the size of the array
            Color[] colors1D = new Color[Animation.FrameWidth * Animation.FrameHeight];

            // Get the data and put it into the array
            Animation.Texture.GetData(0,
                new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight),
                colors1D,
                0,
                Animation.FrameWidth * Animation.FrameHeight);

            // Return the 2D array.
            return colors1D;
        }

        /// <summary>
        /// Fills a 2D array with the color data from the current frame of the texture.
        /// </summary>
        /// <returns></returns>
        public Color[,] Get2DColorDataArray()
        {
            // Set the size of the array
            Color[] colors1D = new Color[Animation.FrameWidth * Animation.FrameHeight];

            // Get the data and put it into the array
            Animation.Texture.GetData(0,
                new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight),
                colors1D,
                0,
                Animation.FrameWidth * Animation.FrameHeight);

            // Convert the 1D array into a 2D array
            Color[,] colors2D = new Color[Animation.Texture.Width, Animation.Texture.Height];
            for (int x = 0; x < Animation.Texture.Width; x++)
                for (int y = 0; y < Animation.Texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * Animation.Texture.Width];

            // Return the 2D array.
            return colors2D;
        }
    }
}
