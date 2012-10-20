using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Player_Select_Menu
    {
        Texture2D player;
        public Color color;
        public Color[] colors = new Color[] { Color.White, Color.Red, Color.Blue, Color.Green };
        public Texture2D[] coloredTextures;
        Texture2D currentTexture;
        int colorIndex = 0;

        Rectangle bounds;

        Vector2 vTitle;

        // The index property controlls looping.
        public int ColorIndex
        {
            get { return colorIndex; }
            set
            {
                colorIndex = value;
                if (colorIndex > colors.Length - 1)
                    colorIndex = 0;
                else if (colorIndex < 0)
                    colorIndex = colors.Length - 1;
            }
        }

        public Player_Select_Menu(ContentManager content, SpriteFont font, Rectangle bounds)
        {
            player = content.Load<Texture2D>("Boxing/Player_Idle_Side");

            coloredTextures = new Texture2D[colors.Length];

            Texture2D t = player;
            Color[] c = new Color[player.Width * player.Height];
            for(int i = 0; i < colors.Length; i++)
            {

                t.GetData(c);
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j] == Color.Magenta)
                    {
                        c[j] = colors[i];
                        //Debug.WriteLine("Magenta replaced with " + colors[i]);
                    }
                    //Debug.WriteLine("NOT MAGENTA");

                }
                //Debug.WriteLine("Textured done " + i);
                t.SetData(c);
                coloredTextures[i] = t;
            }

            currentTexture = coloredTextures[3];
            // make a new texture for each color, rewriting magenta with the color.

            vTitle = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString("Color Select").X / 2,
                    bounds.Y + bounds.Height / 4);

            this.bounds = bounds;
            
        }

        public void Activate()
        {
            color = colors[colorIndex];
        }

        public void ChangeIndex(int playerIndex, KeyPressed key)
        {
            if (key == KeyPressed.Left)
            {
                ColorIndex--;
                //color = colors[ColorIndex];
                currentTexture = coloredTextures[ColorIndex];
                Debug.WriteLine("Color changed!");
            }
            if (key == KeyPressed.Right)
            {
                ColorIndex++;
                //color = colors[ColorIndex];
                currentTexture = coloredTextures[ColorIndex];
                Debug.WriteLine("Color changed!");
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Color Select", vTitle, Color.White);

            spriteBatch.Draw(currentTexture, new Rectangle(bounds.X + bounds.Width / 2 - (player.Width / 2) * 4,
                bounds.Y + bounds.Height / 2 - (player.Height / 2) * 4, 4 * player.Width, 4 * player.Height), Color.White);
        }
    }
}
