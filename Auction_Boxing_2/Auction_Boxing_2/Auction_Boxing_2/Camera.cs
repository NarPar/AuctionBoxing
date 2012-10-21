using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Auction_Boxing_2
{
    class Camera
    {
        Rectangle drawToRectangle;
        public Rectangle DrawToRectangle { get { return drawToRectangle; } }

        public Camera(Rectangle screenBounds)
        {
            this.drawToRectangle = screenBounds;
        }



        public void UpdateCamera(List<BoxingPlayer> players, GraphicsDevice graphicsDevice)
        {


            var positions = new List<Vector2>();

            foreach (BoxingPlayer bp in players)
            {
                if (bp != null)
                {
                    positions.Add(new Vector2(bp.position.X, bp.position.Y - (float)(.5 * bp.GetHeight)));

                }
            }

            //middle of the players position
            int xavg = 0;
            int yavg = 0;
            //setting these to -1 to show that they havent been set yet
            int xleft = -1;
            int xright = -1;
            int ytop = -1;
            int ybottom = -1;



            foreach (Vector2 v in positions)
            {

                xavg += (int)(v.X / positions.Count);
                yavg += (int)(v.Y / positions.Count);
            }

            var xlist = new List<int>();
            var ylist = new List<int>();

            positions.ForEach(delegate(Vector2 v)
            {
                xlist.Add((int)v.X);

                ylist.Add((int)v.Y);

            });
            //change these to add space at the edge of the screen
            var ytopbuffer = 2 *players[0].GetHeight;
            var xrightbuffer =2* players[0].GetWidth;
            var ybottombuffer = 2*players[0].GetHeight;
            var xleftbuffer = 2*players[0].GetWidth;

            //the respective corners of the screen
            xleft = (int)(xlist.Min() - xleftbuffer);
            xright = (int)(xlist.Max() + xrightbuffer);
            ytop = (int)(ylist.Min() - ytopbuffer);
            ybottom = (int)(ylist.Max() + ybottombuffer);

            //width and height of the screen
            var width = Math.Abs(xleft - xright);
            if (width < players[0].GetWidth) width = players[0].GetWidth;

            var height = Math.Abs(ytop - ybottom);
            if (height < players[0].GetHeight) height = players[0].GetHeight;

            //screen dimensions and aspect ratio
            Rectangle screenbounds = graphicsDevice.PresentationParameters.Bounds;
            double aspectration = (double)screenbounds.Width / (double)screenbounds.Height;
            //width = aspectratio * height
            //height = width / aspectratio

            //the raw rectangle is the rectangle that contains the player
            Rectangle raw = new Rectangle(xleft, ytop, width, height);

            //these are the calculations used to determine the proper screen dimensions
            //all the while keeping the players in the screen by inflating the raw rectangle
            int desiredheight = height;
            int desiredwidth = width;
            int xinflation = 0;
            int yinflation = 0;

            desiredheight = height;
            desiredwidth = (int)(aspectration * height);
            xinflation = (desiredwidth - width) / 2;
            raw.Inflate(xinflation, yinflation);

            //special cases for when the calculated rectangle is too large or goes over the
            // edge of the screen
            if (raw.X <= 0) raw.X = 0;
            if (raw.Y <= 0) raw.Y = 0;
            if (raw.X + raw.Width >= screenbounds.Width) raw.X = screenbounds.Width - raw.Width;
            if (raw.Y + raw.Height >= screenbounds.Height) raw.Y = screenbounds.Height - raw.Height;

            if (raw.X <= 0 && raw.Width >= screenbounds.Width)
            {
                raw.Width = screenbounds.Width;
                raw.X = 0;
            }

            if (raw.Y <= 0 && raw.Height > screenbounds.Height)
            {
                raw.Height = screenbounds.Height;
                raw.Y = 0;
            }
            //the rectanlge that the render target is drawing to
            drawToRectangle = raw;
        }

    }
}


