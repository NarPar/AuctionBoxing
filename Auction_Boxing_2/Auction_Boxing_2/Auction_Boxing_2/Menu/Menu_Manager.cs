using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

//namespace Auction_Boxing_2
//{
    /* Initializes players then
     * waits for players to join, configure their control settings, and ready 
     * 
     * NOTE: To select a menu entry, press Right key.
     */
    /*class Menu_Manager
    {
        State_Manager state_man;

        //List<Input_Handler> inputs = new List<Input_Handler>();
        Input_Handler[] inputs = new Input_Handler[4];

        Menu[] menus = new Menu[4];
        string[] entries = { "Player ", "Up: ", "Down: ", "Left: ", "Right: ", 
                               "Attack 1: ", "Attack 2: ", "Attack 3: ", "Attack 4: ", "<READY>" };

        int[] ready = { -1, -1, -1, -1 };
        protected static int totalPlayers = 0;

        bool playerOneAdded;
        bool playerTwoAdded;
        bool playerThreeAdded;
        bool playerFourAdded;

        KeyboardState kb;
        GamePadState gp;

        Texture2D background;
        Texture2D menuBackground;
        SpriteFont font;
        SpriteFont titlefont;

        Rectangle ClientBounds;

        public Menu_Manager(State_Manager state, Rectangle ClientBounds, Input_Handler[] inputs)
        {
            this.state_man = state;
            this.ClientBounds = ClientBounds;
            this.inputs = inputs;
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Menu/Menu_Background");
            menuBackground = Content.Load<Texture2D>("White");
            font = Content.Load<SpriteFont>("Menu/menufont");
            titlefont = Content.Load<SpriteFont>("Menu/titlefont");

        }

        /* Update returns null until all players have readied. When all players
         * are ready, it returns a list containing their custom Input_Handlers
         
        public bool Update(GameTime gameTime)
        {
            // update the input states
            kb = Keyboard.GetState();
            //gp = GamePad.GetState();

            CheckForJoin();

            // Count how many players are ready
            int count = 0;
            foreach (int r in ready)
            {
                if (r == 1)
                    count++;
            }

            // Is everyone ready to start?
            if (totalPlayers != 0 && count == totalPlayers)
            {
                //Contents of arrays are passed by value, unlike the array itself. so we must pass back the values
                for(int i = 0; i < 4; i++)
                    state_man.inputs[i] = this.inputs[i];
                
                for (int i = 0; i < ready.Length; i++)
                    ready[i] = -1;
                return true;
            }
            else
                return false;
        }

        /* Checks the number keys 1-4 to see if they're pressed.
         * If they are, then indicate they've been added by setting
         * player[X]Added to true to ensure no repeats.
         * Then create new input_handler for that player.
         
        private void CheckForJoin()
        {
            // 1 key down?
            if (!playerOneAdded && kb.IsKeyDown(Keys.D1))
            {
                // Add player 0
                playerOneAdded = true;

                //Init other properties and get menu values
                InitPlayerValues(0);
                

            }
            // 2 key down?
            if (!playerTwoAdded && kb.IsKeyDown(Keys.D2))
            {
                // Add player 1
                playerTwoAdded = true;
                //Init other properties and get menu values
                InitPlayerValues(1);

            }
            // 3 key down?
            if (!playerThreeAdded && kb.IsKeyDown(Keys.D3))
            {
                // Add player 2
                playerThreeAdded = true;
                //Init other properties and get menu values
                InitPlayerValues(2);
            }
            // 4 key down?
            if (!playerFourAdded && kb.IsKeyDown(Keys.D4))
            {
                // Add player 3
                playerFourAdded = true;
                //Init other properties and get menu values
                InitPlayerValues(3);
            }
        }

        //Inits input player values
        private void InitPlayerValues(int index)
        {
            totalPlayers++;
            //Create customizable input handler
            inputs[index].Initialize(index, false);

            //Hook up event to listener
            inputs[index].OnKeyRelease += HandleInput;

            //Create new menu for player
            string[] values = { (index + 1).ToString(), inputs[index].kbUp.ToString(), inputs[index].kbDown.ToString(), 
                                 inputs[index].kbLeft.ToString(), inputs[index].kbRight.ToString(), 
                                 inputs[index].kbAttack_0.ToString(), inputs[index].kbAttack_1.ToString(),
                                 inputs[index].kbAttack_2.ToString(), inputs[index].kbAttack_3.ToString(), ""  };

            //menus[index] = new Menu(entries, values, new Rectangle(index * ClientBounds.Width / 4, 0, ClientBounds.Width / 4, ClientBounds.Height),
                //menuBackground);

        }

        

        public void HandleInput(int player_index, KeyPressed key)
        {
            if (key == KeyPressed.Up)
            {
                if (menus[player_index] != null)
                    if (menus[player_index].index == 0)
                        menus[player_index].index = menus[player_index].num_entries - 1;
                    else
                        menus[player_index].index--;
            }
            if (key == KeyPressed.Down)
            {
                if (menus[player_index] != null)
                    menus[player_index].index = (menus[player_index].index + 1) % menus[player_index].num_entries;
                
            }
            if (key == KeyPressed.Right)
            {
                if (menus[player_index] != null && menus[player_index].index == menus[player_index].num_entries - 1)
                    ready[player_index] = 1;
            }
            if (key == KeyPressed.Left)
            {
                if (menus[player_index] != null)
                    ready[player_index] = 0;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height), Color.White);

            spriteBatch.DrawString(titlefont, "Auction Boxing!", new Vector2( ClientBounds.Width / 2 - titlefont.MeasureString("Auction Boxing!").X / 2,
                    (ClientBounds.Height / 5) - titlefont.MeasureString("Auction Boxing!").Y / 2), Color.Black);

            //If a player hasn't join yet, invite them to do so!
            if(!playerOneAdded)
                spriteBatch.DrawString(font, "<Press 1 to Join>", new Vector2(((1) * ClientBounds.Width / 5) - font.MeasureString("<Press 1 to Join>").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("<Press 1 to Join>").Y / 2), Color.Black);
            if (!playerTwoAdded)
                spriteBatch.DrawString(font, "<Press 2 to Join>", new Vector2(((2) * ClientBounds.Width / 5) - font.MeasureString("<Press 2 to Join>").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("<Press 2 to Join>").Y / 2), Color.Black);
            if (!playerThreeAdded)
                spriteBatch.DrawString(font, "<Press 3 to Join>", new Vector2(((3) * ClientBounds.Width / 5) - font.MeasureString("<Press 3 to Join>").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("<Press 3 to Join>").Y / 2), Color.Black);
            if (!playerFourAdded)
                spriteBatch.DrawString(font, "<Press 4 to Join>", new Vector2(((4) * ClientBounds.Width / 5) - font.MeasureString("<Press 4 to Join>").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("<Press 4 to Join>").Y / 2), Color.Black);

            
            for (int i = 0; i < 4; i++)
            {
                //If someone has joined, display their input menu
                if (!(ready[i] == 1))
                {
                    if (menus[i] != null)
                        menus[i].Draw(spriteBatch, font);
                }
                // If they're ready, display a green ready sign
                else
                    spriteBatch.DrawString(font, "READY!", new Vector2(((i + 1) * ClientBounds.Width / 5) - font.MeasureString("READY!").X / 2,
                    (ClientBounds.Height / 2) - font.MeasureString("READY!").Y / 2), Color.Green);
            }
        }

    }
}
    */