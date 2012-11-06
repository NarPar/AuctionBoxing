using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

//CHANGED: Uses tools.ParseText()

namespace Auction_Boxing_2
{
    class ItemStatDisplay
    {
        SpriteFont font;

        Rectangle bounds;
        Rectangle ability_background;

        Texture2D background;
        Texture2D tBar;

        bool clear = false;

        // Title
        string title = "Item_Name";
        Vector2 vTitle;

        // Stats
        string[] strings = { "Hp", "Sp", "Mve", "Atk", "Def", "Ref" };
        Vector2[] textPositions;
        Rectangle[] barPositions;
        Color[] barColors = { Color.Red, Color.Yellow, Color.Green, Color.Purple, Color.Blue, Color.Orange };
        int rHealth, rStamina, rMovement, rAttack, rDefense, rCooldown;

        // Ability Description
        string ability_description = "kicks ass";
        Vector2 vDescription;
        int description_wrap_count = 0; //How many lines to we need
       
        // Text Animation and wrapping (fitting it into the bounds)
        String parsedText = "";
        String typedText = "";
        double typedTextLength;
        int delayInMilliseconds;
        bool isDoneDrawing;

        float health, stamina, movement, attack, defense, cooldown;
        float fullBar;
        

        public ItemStatDisplay(SpriteFont font, Rectangle bounds, Texture2D background, Texture2D bar)
        {
            this.font = font;
            this.bounds = bounds;
            this.background = background;
            this.tBar = bar;

            // Initialize positions----

            int margin = 1; // the whitespace around the entry

            // Dimensions of the strings
            int stringHeight = (int)font.MeasureString("S").Y;
            int stringWidth = (int)font.MeasureString("MMM").X;

            // Dimensions of the bars
            int textX = bounds.X + stringWidth / 2;
            int barHeight = 2 * stringHeight / 3;

            int barX = bounds.X + 2 * stringWidth + 2;
            
            // Portions?
            int vertical_Portion = bounds.Height / 14;
            int textWidth = bounds.Width / 4;
            int barWidth = bounds.X + bounds.Width - (textX + textWidth);
            Vector2 start = new Vector2(bounds.X, bounds.Y);

            // Set position of the Title
            vTitle = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(title).X, bounds.Y);

            //Put the positions into convinient arrays.
            textPositions = new Vector2[6];
            barPositions = new Rectangle[6];
            for (int i = 1; i <= textPositions.Length; i++)
            {
                textPositions[i - 1] = new Vector2(textX + margin, start.Y + margin + (i * (stringHeight) + (i * 2 * margin)));
                barPositions[i - 1] = new Rectangle(barX + 3 * margin + stringWidth, (int)start.Y + margin + (i * (stringHeight) + (i * 2 * margin)), barWidth, stringHeight);
            }
            fullBar = barWidth;

            // Set positions of the description and description background
            vDescription = new Vector2(textX + margin, start.Y + margin + (7 * (stringHeight) + (7 * 2 * margin)));
            ability_background = new Rectangle(bounds.X, bounds.Y + bounds.Height, bounds.Width, 0);
        }

        public void SetItem(Item item)
        {
            clear = false;

            // Set title
            title = item.name;
            vTitle.X = bounds.X + bounds.Width / 2 - font.MeasureString(title).X;

            // Set the stat values
            ability_description = item.ability_description;
            health = item.health;
            stamina = item.stamina;
            movement = item.movement;
            attack = item.attack;
            defense = item.defense;
            cooldown = item.cooldown;

            // Set the bar lengths
            barPositions[0].Width = (int)(fullBar * (health / Tools.displayMaxHealth));
            Debug.WriteLine("healthwidth = " + barPositions[0].Width);
            barPositions[1].Width = (int)(fullBar * (stamina / Tools.displayMaxStamina));
            barPositions[2].Width = (int)(fullBar * (movement / Tools.displayMaxMovement));
            barPositions[3].Width = (int)(fullBar * (attack / Tools.displayMaxAttack));
            barPositions[4].Width = (int)(fullBar * (defense / Tools.displayMaxDefense));
            barPositions[5].Width = (int)(fullBar * (cooldown / Tools.displayMaxCooldown));

            //Set description rect
            Vector2 v = font.MeasureString(ability_description);

            description_wrap_count = (int)(Math.Ceiling(v.X / bounds.Width));
            ability_background.Height = (int)v.Y * description_wrap_count;

            typedText = "";
            typedTextLength = 0;
            parsedText = Tools.ParseText(font, ability_description, new Rectangle(ability_background.X + 4, ability_background.Y,
                ability_background.Width - 8, ability_background.Height), ref description_wrap_count);
            delayInMilliseconds = 25;
            isDoneDrawing = false;

            if(description_wrap_count != 0)
                ability_background.Height = (int)v.Y * (description_wrap_count - 1) + 5;
        }

        public void Clear()
        {
            clear = true;
        }


        public void Update(GameTime gameTime)
        {
            if (!isDoneDrawing)
            {
                if (delayInMilliseconds == 0)
                {
                    typedText = parsedText;
                    isDoneDrawing = true;
                }
                else if (typedTextLength < parsedText.Length)
                {
                    typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;

                    if (typedTextLength >= parsedText.Length)
                    {
                        typedTextLength = parsedText.Length;
                        isDoneDrawing = true;
                    }
                    typedText = parsedText.Substring(0, (int)typedTextLength);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, bool drawDescription)
        {
            // Draw backgrounds
            spriteBatch.Draw(background, bounds, Color.White * .5f);
            spriteBatch.Draw(background, ability_background, Color.White * .5f);

            if (!clear)
            {
                // Draw title
                spriteBatch.DrawString(font, title, vTitle, Color.Black);

                // Draw stat names and bars
                for (int i = 0; i < 6; i++)
                {
                    spriteBatch.DrawString(font, strings[i], textPositions[i], Color.Black);
                    spriteBatch.Draw(tBar, barPositions[i], barColors[i]);
                }

                // Draw the animating description
                if (drawDescription)
                    spriteBatch.DrawString(font, typedText, vDescription, Color.Black);
            }
            else
            {
                //just draw stat names
                for (int i = 0; i < 6; i++)
                {
                    spriteBatch.DrawString(font, strings[i], textPositions[i], Color.Black);
                }
            }
        }
    }
}
