using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Auction_Boxing_2
{
    /// <summary>
    /// A modified Menu class that displays how many rounds the following game will be.
    /// Allows the user to modify the number of rounds.
    /// </summary>
    class Settings_Menu : Menu
    {
        int rounds = 1;
        public int Rounds
        {
            get { return rounds; }

            set
            {
                rounds = value;
                if (rounds < 1)
                    rounds = 1;
            }
        }

        string[] entries = new string[] { "Rounds: ", "Accept" };

        public Settings_Menu(SpriteFont font, Rectangle bounds)
            : base(font, new string[] { "Rounds: 1", "Accept" }, bounds)
        {

        }

        public override void ChangeIndex(int playerIndex, KeyPressed key)
        {
            // When the up or down event is fired, move the index.
            if (key == KeyPressed.Left)
                Rounds--;
            else if (key == KeyPressed.Right)
                Rounds++;

            base.ChangeIndex(playerIndex, key);
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int i = 0; i < num_entries; i++)
            {
                //Selected string is red
                Color color = Color.Black;

                if (index == i)
                    color = Color.Red;

                if (i == 0)
                {
                    string s = entries[0] + rounds;

                    spriteBatch.DrawString(font, s, entry_positions[i], color);
                }
                else
                    spriteBatch.DrawString(font, entries[i], entry_positions[i], color);
            }
        }
    }
}
