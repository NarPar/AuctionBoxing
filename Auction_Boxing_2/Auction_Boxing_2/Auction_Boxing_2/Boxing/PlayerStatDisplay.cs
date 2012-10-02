using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Auction_Boxing_2
{


    public class PlayerStatDisplay
    {
        #region Fields

        Rectangle bounds;

        Texture2D background;
        Texture2D tBar;

        int barWidth;

        float ViewScaleFactor = .1f;

        int textX;

        bool isActive;

        Input_Handler input;

        // Title
        string title = "";

        float health, stamina;
        float fullBar = 100;

        Vector2 vTitle, vHealth, vStamina;
        Rectangle rHealth, rStamina;

        Rectangle[] rAbilities = new Rectangle[4];

        BoxingPlayer player;
        #endregion

        public PlayerStatDisplay(SpriteFont font, int player_index, BoxingPlayer player,Input_Handler input, Rectangle bounds, Texture2D background, Texture2D bar)
        {
            this.player = player;
            this.bounds = bounds;
            this.background = background;
            this.tBar = bar;
            this.health = player.MaxHealth;
            this.stamina = player.MaxStamina;
            this.input = input;
            this.isActive = input.isActive;

            title = "Player " + player_index;

            //===================Initialize positions====================
            int margin = 1; // the whitespace around the entry

            // Dimensions of the strings
            int stringHeight = (int)font.MeasureString("S").Y;
            int stringWidth = (int)font.MeasureString("MMM").X;

            // Dimensions of the bars
            int textX = bounds.X + stringWidth / 2;
            int barHeight = (int)( 1.5f * stringHeight / 3);

            int barX = bounds.X + (int)(1.25f * stringWidth + 2);

            // Portions?
            int vertical_Portion = bounds.Height / 14;
            int textWidth = bounds.Width / 3;
            barWidth = bounds.X + bounds.Width - barX - 4;
            Vector2 start = new Vector2(bounds.X, bounds.Y);

            // Set position of the Title
            vTitle = new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(title).X, bounds.Y);

            vHealth = new Vector2(textX + margin, start.Y + margin + (1 * (stringHeight) + (1 * 2 * margin)));
            vStamina = new Vector2(textX + margin, start.Y + margin + (2 * (stringHeight) + (2 * 2 * margin)));

            rHealth = new Rectangle(barX, (int)start.Y + margin + (1 * (stringHeight) + (1 * 2 * margin)), barWidth, stringHeight);
            rStamina = new Rectangle(barX, (int)start.Y + margin + (2 * (stringHeight) + (2 * 2 * margin)), barWidth, stringHeight);

            Debug.WriteLine("rHealth.Y = " + rHealth.Y);

            rHealth.Width = (int)(barWidth * (player.CurrentHealth / player.MaxHealth));
            rStamina.Width = (int)(barWidth * (player.CurrentStamina / player.MaxStamina));

            int squarewidth = bounds.Width / 8;

            // Set position of ability cooldown squares
            for (int i = 0; i < 4; i++)
            {
                rAbilities[i] = new Rectangle(((i + 1) * (bounds.X + bounds.Width) / 5) - squarewidth/2,
                    (int)start.Y + margin + (3 * (stringHeight) + (3 * 2 * margin)), squarewidth, squarewidth);
                //Debug.WriteLine("Player " + player_index + " square " + (i + 1) + " initialized");
                Debug.WriteLine("Player " + player_index + " square. X = " + rAbilities[i].X + "square.Y = " + rAbilities[i].Y);
            }
        }

        public void Update()
        {
            //rHealth.Width = (int)(player.CurrentHealth * ViewScaleFactor);
            // rStamina.Width = (int)(player.CurrentStamina);
            rHealth.Width = (int)(barWidth * (player.CurrentHealth / player.MaxHealth));
            rStamina.Width = (int)(barWidth * (player.CurrentStamina / player.MaxStamina));
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (input.isActive)
            {
                spriteBatch.Draw(background, bounds, Color.White * .5f);

                // Draw player name
                spriteBatch.DrawString(font, title, vTitle, Color.Black);

                // Draw bar names
                spriteBatch.DrawString(font, "Hp", vHealth, Color.Black);
                spriteBatch.DrawString(font, "Sp", vStamina, Color.Black);

                // Draw bars
                spriteBatch.Draw(tBar, rHealth, Color.Red);
                spriteBatch.Draw(tBar, rStamina, Color.Yellow);

                // Draw cooldown thingers
                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(tBar, rAbilities[i], Color.Blue);
                    //Debug.WriteLine("Player square " + (i + 1) + " initialized");
                }
            }

        }

        public void DeInitialize()
        {
            input.isActive = false;

        }


    }
}
