using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Settings_Popup : Popup
    {
        Game_State manager;

        public Settings_Menu menu;
        Rectangle menuBounds;

        public Settings_Popup(Game_State manager, ContentManager content, Input_Handler[] inputs, Rectangle bounds)
            : base(content, bounds)
        {
            // Make a white boundary smaller than the bounds.
            menuBounds = new Rectangle(bounds.Width / 20, bounds.Height / 20,
                bounds.Width - (bounds.Width / 20) * 2, bounds.Height - (bounds.Height / 20) * 2);
            //Debug.WriteLine(menuBounds.X + " " + menuBounds.Y + " " + menuBounds.Width + " " + menuBounds.Height);

            SoundEffect menuSound = content.Load<SoundEffect>("Sounds/MenuC");

            menu = new Settings_Menu(content.Load<SpriteFont>("Menu/menufont"), menuBounds, menuSound);

            this.manager = manager;
            manager.inputs[0].OnKeyRelease += HandleInput;
        }

        // Handles input from first player.
        public void HandleInput(int playerIndex, KeyPressed key)
        {
            // If you select accept, tell the manager we're done.
            if (key == KeyPressed.Attack1 && menu.entries[menu.index] == "Accept")
            {
                manager.OnStateComplete("Settings");
            }
            else
            {
                // let the menu handle it
                menu.ChangeIndex(playerIndex, key);
            }
        }

        // Returns the number of rounds from the menu.
        public int GetRounds()
        {
            return menu.Rounds;
        }

        // Close all listeners before being dereferenced by the handling state.
        public void Destruct()
        {
            manager.inputs[0].OnKeyRelease -= HandleInput;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            base.Draw(spriteBatch, font);

            spriteBatch.Draw(whiteOut, menuBounds, Color.White * .5f);

            menu.Draw(spriteBatch, font);
        }
    }
}
