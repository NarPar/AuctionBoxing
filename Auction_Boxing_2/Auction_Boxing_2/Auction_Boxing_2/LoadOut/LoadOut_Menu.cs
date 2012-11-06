using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class LoadOut_Menu
    {
        public int num_entries;

        string[] entries = { "Player ", "Items: ", "Equipped: ", "<READY>" };

        int index = 1;
        public int Index
        {
            get { return index; }
            set
            {
                if (index == 1)
                    index = 3;
                else if (index == 3)
                    index = 1;
            }
        }

        int displayindex = 0;
        public int DisplayIndex
        {
            get { return displayindex; }
            set
            {
                displayindex = value;
                if (displayindex > items.Count - 1)
                    displayindex = 0;
                if (displayindex < 0)
                    displayindex = items.Count - 1;
            }
        }

        Vector2[] entry_positions;
        Rectangle[] equipmentIcon_positions = new Rectangle[4];
        string[] inputnames = new string[4];
        Vector2[] inputname_positions = new Vector2[4];

        List<Item> items;
        ItemStatDisplay item_preview;//LoadOut_Display item_preview;
        public Item[] equipment = new Item[4];

        Texture2D background;
        Rectangle bounds;
        SpriteFont font;
        SpriteFont displayFont;

        public LoadOut_Menu(SpriteFont font, SpriteFont displayFont, Rectangle rbounds, Texture2D background, List<Item> items)
        {
            this.font = font;
            this.displayFont = displayFont;
            this.items = items;
            num_entries = entries.Length;
            this.entry_positions = new Vector2[entries.Length];
            this.background = background;
            //Make background slightly smaller than bounds
            this.bounds = new Rectangle(rbounds.X + 10, rbounds.Y + 10, rbounds.Width - 20, rbounds.Height - 20);
            //this.equipment = equip;
            for (int i = 0; i < 4; i++)
            {
                if (equipment[i] != null)
                    EquipItem(i, true);
                else
                    Debug.WriteLine("equipment is null! " + i);
                //if (equip[i] != null)
                    //;//EquipItem(i, true);
                //else
                  //  Debug.WriteLine("equip is null! " + i);
            }

            //player name
            entry_positions[0] = new Vector2(bounds.X + bounds.Width / 2, bounds.Y);
            //item array
            entry_positions[1] = new Vector2(bounds.X, bounds.Y + font.MeasureString("A").Y);
            //display
            item_preview = new ItemStatDisplay(displayFont,//new LoadOut_Display(displayFont, 
                new Rectangle(bounds.X + bounds.Width / 4, bounds.Y + (int)(font.MeasureString("A").Y * 2),
                    bounds.Width / 2, bounds.Height / 2), background, background);
            item_preview.SetItem(items[0]);
            //equipped
            entry_positions[2] = new Vector2(bounds.X, bounds.Y + (int)(font.MeasureString("A").Y * 2) + bounds.Height / 2 + bounds.Height / 16);
            //ready
            entry_positions[3] = new Vector2(bounds.X, entry_positions[2].Y + (int)(font.MeasureString("A").Y * 3));

            inputnames[0] = " 0: ";
            inputnames[1] = " 1: ";
            inputnames[2] = " 2: ";
            inputnames[3] = " 3: ";

            int buffer = 2;
            int iconstartX = (int)entry_positions[2].X + (int)font.MeasureString("Equipment").X + buffer;
            int iconwidth =  items[0].icon.Bounds.Width;
            int iconheight = items[0].icon.Bounds.Height;
            for (int i = 0; i < 4; i++)
            {
                inputname_positions[i] = new Vector2(iconstartX + i * ((buffer) + (int)font.MeasureString(inputnames[i]).X + iconwidth), 
                    (int)entry_positions[2].Y);
                equipmentIcon_positions[i] = new Rectangle((int)inputname_positions[i].X + (int)font.MeasureString(inputnames[i]).X, 
                    (int)entry_positions[2].Y, iconwidth, iconheight);
            }

            //Debug
            //equipment[0] = items[0];
            //equipment[1] = items[1];
            //equipment[2] = items[2];
            //equipment[3] = items[3];
        }

        public void ChangeDisplay(bool isLeft)
        {
            if (items.Count != 0)
            {
                if (isLeft)
                    DisplayIndex--;
                else
                    DisplayIndex++;
                item_preview.SetItem(items[DisplayIndex]);
            }
        }

        public void EquipItem(int index)
        {
            if(equipment[index] != null)
            {
                items.Add(equipment[index]);
                equipment[index] = null;
                displayindex = items.Count - 1;
            }
            else if (items.Count != 0)
            {
                equipment[index] = items[displayindex];
                items.Remove(items[displayindex]);
                DisplayIndex = 0;

                if (items.Count == 0)
                    item_preview.Clear();
            }
        }

        public void EquipItem(int index, bool b)
        {
            if (items.Count != 0)
            {
                Debug.WriteLine(items.IndexOf(equipment[index]));
                equipment[index] = items[items.IndexOf(equipment[index])];
                items.Remove(equipment[index]);
                DisplayIndex = 0;

                if (items.Count == 0)
                    item_preview.Clear();
            }
        }

        public void Update(GameTime gameTime)
        {
            item_preview.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bounds, Color.White * .5f);

            for ( int i = 0; i < entries.Length; i++)
            {
                //Selected string is red
                Color color = Color.Black;

                if (index == i)
                    color = Color.Red;

                spriteBatch.DrawString(font, entries[i], entry_positions[i], color);
            }

            if (items.Count != 0)
            {
                spriteBatch.Draw(items[displayindex].icon,
                    new Rectangle((int)entry_positions[1].X + 50, (int)entry_positions[1].Y,
                        items[displayindex].icon.Bounds.Width,
                        items[displayindex].icon.Bounds.Height), Color.White);
            }
            

            item_preview.Draw(spriteBatch, font, true);

            for (int i = 0; i < 4; i++)
            {
                spriteBatch.DrawString(font, inputnames[i], inputname_positions[i], Color.Black);
                if (equipment[i] != null)
                {
                    spriteBatch.Draw(equipment[i].icon, equipmentIcon_positions[i], Color.White);
                }
            }

        }
    }
}
