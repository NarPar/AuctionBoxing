using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    class Main_Game_State : Game_State
    {
        Texture2D background;

        SpriteFont font;

        Menu menu;

        bool isStateConfig; // Are we showing the button config?

        public Main_Game_State(Game game, Input_Handler[] inputs, Rectangle bounds) 
            : base(game, inputs, bounds)
        {
            font = game.Content.Load<SpriteFont>("Menu/menufont");
            menu = new Menu(font, new string[] { "Brawl", "Auction", "Config", "Exit" }, bounds);
            inputs[0].OnKeyRelease += menu.ChangeIndex;
            menu.OnEntrySelect += MenuEntrySelect;

            //Debug.WriteLine(bounds.X + " " + bounds.Y + " " + bounds.Width + " " + bounds.Height);

            background = game.Content.Load<Texture2D>("Menu/Menu_Background");
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void MenuEntrySelect(string entry)
        {
            // If player 1 selects the brawl button, switch currentState to the Brawl_State.
            if (entry == "Brawl")
            {
                ChangeState(new Brawl_Game_State(game, inputs, bounds));
                inputs[0].OnKeyRelease -= menu.ChangeIndex;
                menu.OnEntrySelect -= MenuEntrySelect;
            }
            // Exit
            else if (entry == "Exit")
                game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // Background is not drawing with this rect :(?!
            spriteBatch.Draw(background, new Rectangle(0,0, background.Width, background.Height), Color.White);

            menu.Draw(spriteBatch, font);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
