using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /* Initializes players then
     * waits for players to join, configure their control settings, and ready 
     * 
     * NOTE: To select a menu entry, press Right key.
     */
    class LoadOut_Manager
    {
        Game_State state_man;

        //List<Input_Handler> inputs = new List<Input_Handler>();
        Input_Handler[] inputs = new Input_Handler[4];

        LoadOut_Menu[] loadouts = new LoadOut_Menu[4];

        Vector2[] readyPositions = new Vector2[4];

        List<Item>[] items;

        // a 2d array containing the players equipment for the boxing match. Order counts!
        public Item[][] equipment;

        int[] ready = { -1, -1, -1, -1 };
        protected static int totalPlayers = 0;

        KeyboardState kb;
        GamePadState gp;

        Texture2D background;
        Texture2D menuBackground;
        SpriteFont font;
        SpriteFont titlefont;
        SpriteFont loadoutfont;
        SpriteFont loadoutdisplayfont;

        Rectangle ClientBounds;

        public LoadOut_Manager(Game_State state, Rectangle ClientBounds, Input_Handler[] inputs,
            List<Item>[] items)
        {
            this.state_man = state;
            this.ClientBounds = ClientBounds;
            this.inputs = inputs;
            this.items = items;
            //this.equipment = state.equipment;
        }

        public void LoadContent(ContentManager Content)
        {
            //background = Content.Load<Texture2D>("Menu/Menu_Background");
            menuBackground = Content.Load<Texture2D>("White");
            font = Content.Load<SpriteFont>("Menu/menufont");
            titlefont = Content.Load<SpriteFont>("Menu/titlefont");
            loadoutfont = Content.Load<SpriteFont>("LoadOutFont");
            loadoutdisplayfont = Content.Load<SpriteFont>("LoadOutDisplayFont");
        }

        public void Activate(ContentManager Content)
        {

            int buffer = 4;

            readyPositions[0] = new Vector2((ClientBounds.Width / 2) / 2, (ClientBounds.Height / 2) / 2);
            readyPositions[1] = new Vector2(3 * (ClientBounds.Width / 2) / 2, (ClientBounds.Height / 2) / 2);
            readyPositions[2] = new Vector2((ClientBounds.Width / 2) / 2, 3 * (ClientBounds.Height / 2) / 2);
            readyPositions[3] = new Vector2(3 * (ClientBounds.Width / 2) / 2, 3 * (ClientBounds.Height / 2) / 2);

            //Itialize loadout rectangles
            Rectangle[] ld_positions = new Rectangle[4];
            ld_positions[0] = new Rectangle(buffer, buffer, ClientBounds.Width / 2 - (buffer * 2),
                ClientBounds.Height / 2 - (buffer * 2));
            ld_positions[1] = new Rectangle(ClientBounds.Width / 2 + buffer, buffer, ClientBounds.Width / 2 - (buffer * 2),
                        ClientBounds.Height / 2 - (buffer * 2));
            ld_positions[2] = new Rectangle(buffer, ClientBounds.Height / 2 + buffer, ClientBounds.Width / 2 - (buffer * 2),
                        ClientBounds.Height / 2 - (buffer * 2));
            ld_positions[3] = new Rectangle(ClientBounds.Width / 2 + buffer, ClientBounds.Height / 2 + buffer, ClientBounds.Width / 2 - (buffer * 2),
                        ClientBounds.Height / 2 - (buffer * 2));

            Item[] equip = new Item[4];
            for (int i = 0; i < 4; i++)
            {
                if(inputs[i].isActive)
                {
                    Debug.WriteLine("Player " + i + " is Active!");
                    loadouts[i] = new LoadOut_Menu(loadoutfont, loadoutdisplayfont, ld_positions[i], menuBackground, items[i]);
                    /*for (int j = 0; j < 4; j++)
                    {
                        equip[i] = state_man.equipment[i, j];
                        Debug.WriteLine("equip[i] = " + equip[i]);
                        if (equip[i] != null)
                            loadouts[i].EquipItem(i, true);
                    }*/
                    //loadouts[i].equipment = equip;
                    inputs[i].OnKeyRelease += HandleInput;
                    totalPlayers++;
                }
            }
        }

        /* Update returns null until all players have readied. When all players
         * are ready, it returns a list containing their custom Input_Handlers 
         */
         
        public void Update(GameTime gameTime)
        {
            // update the input states
            kb = Keyboard.GetState();
            //gp = GamePad.GetState();

            // Count how many players are ready
            int count = 0;
            foreach (int r in ready)
            {
                if (r == 1)
                    count++;
            }

            for (int i = 0; i < 4; i++)
            {
                if(inputs[i].isActive)
                    loadouts[i].Update(gameTime);
            }

            // Is everyone ready to start?
            if (totalPlayers != 0 && count == totalPlayers)
            {
                equipment = new Item[4][];//new Item[4], new Item[4], new Item[4], new Item[4] };
                //Contents of arrays are passed by value, unlike the array itself. so we must pass back the values
                for (int i = 0; i < 4; i++)
                {
                    if (inputs[i].isActive)
                    {
                        equipment[i] = new Item[4];
                        for (int j = 0; j < 4; j++)
                        {
                            
                            equipment[i][j] = loadouts[i].equipment[j];

                            // Let the manager know we're done here.

                            //if(inputs[i].isActive)
                            //state_man.equipment[i, j] = loadouts[i].equipment[j];
                        }
                    }
                }

                state_man.OnStateComplete("LoadOut");

                for (int i = 0; i < ready.Length; i++)
                    ready[i] = -1;
            }
        }

        // Clear any listeners
        public void Destruct()
        {
            for (int i = 0; i < 4; i++)
            {
                if (inputs[i].isActive)
                {
                    inputs[i].OnKeyRelease -= HandleInput;
                }
            }
        }


        public void HandleInput(int player_index, KeyPressed key)
        {
            if (key == KeyPressed.Up)
            {
                if (loadouts[player_index] != null)// && loadouts[player_index].Index != 0)
                    loadouts[player_index].Index--;
            }
            if (key == KeyPressed.Down)
            {
                if (loadouts[player_index] != null )//&& loadouts[player_index].Index != loadouts[player_index].num_entries - 1)
                    loadouts[player_index].Index++;

            }
            if (key == KeyPressed.Right)
            {
                if (loadouts[player_index] != null && loadouts[player_index].Index == loadouts[player_index].num_entries - 1)
                    ready[player_index] = 1;
                else if (loadouts[player_index] != null && loadouts[player_index].Index == 1)
                    loadouts[player_index].ChangeDisplay(false);
            }
            if (key == KeyPressed.Left)
            {
                // player is active, and currently selecting the "items" entry.
                if (loadouts[player_index] != null && loadouts[player_index].Index == 1)  
                    loadouts[player_index].ChangeDisplay(true);
            }

            // Equip the item to the button they pressed
            if (key == KeyPressed.Attack1)
            {
                if (loadouts[player_index] != null && loadouts[player_index].Index == 1)
                    loadouts[player_index].EquipItem(0);
            }
            if (key == KeyPressed.Attack2)
            {
                if (loadouts[player_index] != null && loadouts[player_index].Index == 1)
                    loadouts[player_index].EquipItem(1);
            }
            if (key == KeyPressed.Attack3)
            {
                if (loadouts[player_index] != null && loadouts[player_index].Index == 1)
                    loadouts[player_index].EquipItem(2);
            }
            if (key == KeyPressed.Attack4)
            {
                if (loadouts[player_index] != null && loadouts[player_index].Index == 1)
                    loadouts[player_index].EquipItem(3);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height), Color.White);


            for (int i = 0; i < 4; i++)
            {
                //If someone has joined, display their input menu
                if (!(ready[i] == 1))
                {
                    if (loadouts[i] != null)
                        loadouts[i].Draw(spriteBatch);
                }
                // If they're ready, display a green ready sign
                else if (loadouts[i] != null)
                {
                    spriteBatch.DrawString(font, "READY!", readyPositions[i], Color.Green);
                }
            }

            string pass = "Press 'P' to skip";
            spriteBatch.DrawString(font, pass,
                new Vector2(ClientBounds.Width - font.MeasureString(pass).X,
                    ClientBounds.Height - font.MeasureString(pass).Y), Color.Black);
        }

    }
}
