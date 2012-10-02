using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;


namespace Auction_Boxing_2
{
    /// <summary>
    /// Given a position, entry height, and number of entries, will calculate the Y values to center them 
    /// in the given bounds.
    /// 
    /// 
    /// Width is fixed, height is determined by the items.
    /// </summary>
    class Display
    {
        Texture2D background;
        public Rectangle bounds;
        public Vector2[] entry_positions;

        public Display(Vector2 pos, int width, SpriteFont font, string title)// int[] entry_height_by_row, int[] entry_width_by_column)//float entry_height, float entry_width, int columns, int rows)
        {
            /*int num_rows = entry_height_by_row.Length;
            int num_columns = entry_width_by_column.Length;

            for (int i = 0; i < num_rows; i++)
            {
                height += (int)entry_height_by_row[i];
                entry_positions[i] = new Vector2(pos.X, pos.Y + height);
            }
            for (int i = 0; i < num_columns; i++)
            {
                width += (int)entry_width_by_column[i];
                entry_positions[i].X = pos.X + width;
            }*/

            bounds = new Rectangle((int)pos.X, (int)pos.Y, width, 50);

        }

        public virtual void AddText(string s)
        {

        }

        public virtual void AddTexture()
        {

        }

        public virtual void AddBar()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("White");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bounds, Color.White * .5f);
        }
    }
}
