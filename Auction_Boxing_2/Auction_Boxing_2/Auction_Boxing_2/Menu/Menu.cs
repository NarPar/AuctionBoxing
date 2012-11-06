using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Auction_Boxing_2
{

    /// <summary>
    /// A menu is given ...
    /// </summary>
    class Menu
    {

        #region Fields

        protected SoundEffect soundMenu;

        public int index;
        
        // The index property controlls looping.
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                if (index > num_entries - 1)
                    index = 0;
                else if (index < 0)
                    index = num_entries - 1;
            }
        }

        public int num_entries;

        string[] entries;


        protected Vector2[] entry_positions;


        protected Rectangle bounds;

        #endregion

        #region Events

        public delegate void EntrySelectHandler(string entryName);

        public event EntrySelectHandler OnEntrySelect;

        #endregion

        public Menu(SpriteFont font, string[] entries, Rectangle bounds, SoundEffect sound)
        {
            soundMenu = sound;
            
            num_entries = entries.Length;
            this.entries = entries;
            this.entry_positions = new Vector2[entries.Length];

            this.bounds = bounds;
            int entryHeight = (int)font.MeasureString(entries[0]).Y;
            int buffer = 20;

            // Index for navigation
            index = 0;

            // Line up the entries in the center of the screen.
            for (int i = 0; i < entries.Length; i++)
            {
                entry_positions[i] = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(entries[i]).X / 2, 
                    bounds.Y + bounds.Height / 2 - (num_entries * (entryHeight + buffer)) / 2 + (i * (entryHeight + buffer)));
            }
        }

        public virtual void Update()
        {
            // if(playerone navigates)
            //      Move the index!
        }

        
        public virtual void ChangeIndex(int playerIndex, KeyPressed key)
        {
            //Debug.WriteLine("Key Pressed = " + key);

            

            // When the up or down event is fired, move the index.
            if (key == KeyPressed.Up)
            {
                soundMenu.Play();
                Index--;
            }
            else if (key == KeyPressed.Down)
            {
                soundMenu.Play();
                Index++;
            }

            if (key == KeyPressed.Attack1)
            {
                if (OnEntrySelect != null)
                {
                    OnEntrySelect(entries[Index]);
                }
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //spriteBatch.Draw(background, bounds, Color.White * .5f);

            for ( int i = 0; i < num_entries; i++)
            {
                //Selected string is red
                Color color = Color.Black;

                if (index == i)
                    color = Color.Red;

                spriteBatch.DrawString(font, entries[i], entry_positions[i], color);
            }

        }
    }
}
