using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /* An input handler is generated for each player.
     * The input handler:
     *      - checks for button releases
     *      - fires an event corresponding to the button release
     *      
     * Each player has their keyboard keys stored here.
     * This class also checks the gampad corresponding to the player.
     */

    public enum KeyPressed
    {
        Up,
        Down,
        Left,
        Right,
        Defend,
        Jump,
        Attack,
        Kick
    }

    public class Input_Handler
    {
        public bool isActive;

        //-----Keyboard-----//

        KeyboardState kbPrevious;
        KeyboardState kbCurrent;

        public Keys kbUp;
        public Keys kbDown;
        public Keys kbLeft;
        public Keys kbRight;

        public Keys kbDefend;
        public Keys kbJump;
        public Keys kbAttack;

        //-----Gamepad-----//

        GamePadState gpPrevious;
        GamePadState gpCurrent;

        int player_index;
        PlayerIndex gamePadPlayerIndex;

        bool isGamePad;


        //-----Events & Delegates-----//

        public delegate void InputPressHandler(int player_index, KeyPressed key);
        public delegate void KeyRealease(KeyPressed key);
        public delegate void KeyPress(KeyPressed key);

        public event InputPressHandler OnKeyRelease;
        public event InputPressHandler OnKeyDown;
        public event InputPressHandler OnKeyHold;

        /// <summary>
        /// An input handler is initialized with the player number.
        /// </summary>
        /// <param name="playerNumber"></param>
        public Input_Handler(int playerNumber)
        {
            this.isActive = false;

            Initialize(playerNumber);
        }

        public void Initialize(int playerNumber)
        {
            this.player_index = playerNumber;

            // Init keyboard controls
            switch (playerNumber)
            {
                case 0:
                    kbUp = Keys.W;
                    kbDown = Keys.S;
                    kbLeft = Keys.A;
                    kbRight = Keys.D;

                    kbDefend = Keys.Z;
                    kbJump = Keys.X;
                    kbAttack = Keys.C;

                    gamePadPlayerIndex = PlayerIndex.One;
                    break;

                case 1:
                    kbUp = Keys.I;
                    kbDown = Keys.K;
                    kbLeft = Keys.J;
                    kbRight = Keys.L;

                    kbDefend = Keys.N;
                    kbJump = Keys.M;
                    kbAttack = Keys.OemComma;

                    gamePadPlayerIndex = PlayerIndex.Two;
                    break;

                case 2:
                    kbUp = Keys.Up;
                    kbDown = Keys.Down;
                    kbLeft = Keys.Left;
                    kbRight = Keys.Right;

                    kbDefend = Keys.OemBackslash;
                    kbJump = Keys.Delete;
                    kbAttack = Keys.End;

                    gamePadPlayerIndex = PlayerIndex.Three;
                    break;

                case 3:
                    kbUp = Keys.NumPad8;
                    kbDown = Keys.NumPad5;
                    kbLeft = Keys.NumPad4;
                    kbRight = Keys.NumPad6;

                    kbDefend= Keys.NumPad1;
                    kbJump = Keys.NumPad2;
                    kbAttack = Keys.NumPad3;

                    gamePadPlayerIndex = PlayerIndex.Four;
                    break;
            }
        }

        public void Update()
        {

            //Update States
            kbPrevious = kbCurrent;
            kbCurrent = Keyboard.GetState();

            gpPrevious = gpCurrent;
            gpCurrent = GamePad.GetState(gamePadPlayerIndex);

            //------Press-------//

            //Up Key Pressed?
            if ((kbPrevious.IsKeyUp(kbUp) && kbCurrent.IsKeyDown(kbUp)) ||
                (gpPrevious.IsButtonUp(Buttons.DPadUp) && gpCurrent.IsButtonDown(Buttons.DPadUp)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Up);

            }

            //Down Key Pressed?
            if ((kbPrevious.IsKeyUp(kbDown) && kbCurrent.IsKeyDown(kbDown)) ||
                (gpPrevious.IsButtonUp(Buttons.DPadDown) && gpCurrent.IsButtonDown(Buttons.DPadDown)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Down);

            }
            //Left Key Pressed?
            if ((kbPrevious.IsKeyUp(kbLeft) && kbCurrent.IsKeyDown(kbLeft)) ||
                (gpPrevious.IsButtonUp(Buttons.DPadLeft) && gpCurrent.IsButtonDown(Buttons.DPadLeft)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Left);
            }
            //Right Key Pressed?
            if ((kbPrevious.IsKeyUp(kbRight) && kbCurrent.IsKeyDown(kbRight)) ||
                (gpPrevious.IsButtonUp(Buttons.DPadRight) && gpCurrent.IsButtonDown(Buttons.DPadRight)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Right);
            }
            //Defend Key Pressed?
            if ((kbPrevious.IsKeyUp(kbDefend) && kbCurrent.IsKeyDown(kbDefend)) ||
                (gpPrevious.IsButtonUp(Buttons.X) && gpCurrent.IsButtonDown(Buttons.X)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Defend);
            }
            //Jump Key Pressed?
            if ((kbPrevious.IsKeyUp(kbJump) && kbCurrent.IsKeyDown(kbJump)) ||
                (gpPrevious.IsButtonUp(Buttons.A) && gpCurrent.IsButtonDown(Buttons.A)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Jump);
            }
            //Attack Key Pressed?
            if ((kbPrevious.IsKeyUp(kbAttack) && kbCurrent.IsKeyDown(kbAttack)) ||
                (gpPrevious.IsButtonUp(Buttons.B) && gpCurrent.IsButtonDown(Buttons.B)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Attack);
            }

            if (
               (gpPrevious.IsButtonUp(Buttons.Y) && gpCurrent.IsButtonDown(Buttons.Y)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Kick);
            }

            // Keys held?

            //Up Key held down?
            if (kbCurrent.IsKeyDown(kbUp) || gpCurrent.IsButtonDown(Buttons.DPadUp))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Up);

            }
            //Down Key held down?
            if (kbCurrent.IsKeyDown(kbDown) || gpCurrent.IsButtonDown(Buttons.DPadDown))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Down);

            }
            //Left Key held down?
            if (kbCurrent.IsKeyDown(kbLeft) || gpCurrent.IsButtonDown(Buttons.DPadLeft))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Left);
            }
            //Right Key held down?
            if (kbCurrent.IsKeyDown(kbRight) || gpCurrent.IsButtonDown(Buttons.DPadRight))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Right);
            }

            if (kbCurrent.IsKeyDown(kbDefend) || gpCurrent.IsButtonDown(Buttons.X))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Defend);
            }
            if (kbCurrent.IsKeyDown(kbJump) || gpCurrent.IsButtonDown(Buttons.A))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Jump);
            }
            if (kbCurrent.IsKeyDown(kbAttack) || gpCurrent.IsButtonDown(Buttons.B))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Attack);
            }
            if (gpCurrent.IsButtonDown(Buttons.Y))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Kick);
            }


            //------RELEASE-------//

            //Up Key Released?
            if ((kbPrevious.IsKeyDown(kbUp) && kbCurrent.IsKeyUp(kbUp)) || 
                (gpPrevious.IsButtonDown(Buttons.DPadUp) && gpCurrent.IsButtonUp(Buttons.DPadUp)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Up);

            }

            //Down Key Released?
            if ((kbPrevious.IsKeyDown(kbDown) && kbCurrent.IsKeyUp(kbDown)) || 
                (gpPrevious.IsButtonDown(Buttons.DPadDown) && gpCurrent.IsButtonUp(Buttons.DPadDown)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Down);

            }
            //Left Key Released?
            if ((kbPrevious.IsKeyDown(kbLeft) && kbCurrent.IsKeyUp(kbLeft)) || 
                (gpPrevious.IsButtonDown(Buttons.DPadLeft) && gpCurrent.IsButtonUp(Buttons.DPadLeft)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Left);
            }
            //Right Key Released?
            if ((kbPrevious.IsKeyDown(kbRight) && kbCurrent.IsKeyUp(kbRight)) || 
                (gpPrevious.IsButtonDown(Buttons.DPadRight) && gpCurrent.IsButtonUp(Buttons.DPadRight)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Right);
            }
            //Defend Key Released?
            if ((kbPrevious.IsKeyDown(kbDefend) && kbCurrent.IsKeyUp(kbDefend)) || 
                (gpPrevious.IsButtonDown(Buttons.X) && gpCurrent.IsButtonUp(Buttons.X)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Defend);
            }

            if ((kbPrevious.IsKeyDown(kbJump) && kbCurrent.IsKeyUp(kbJump)) || 
                (gpPrevious.IsButtonDown(Buttons.A) && gpCurrent.IsButtonUp(Buttons.A)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Jump);
            }
            if ((kbPrevious.IsKeyDown(kbAttack) && kbCurrent.IsKeyUp(kbAttack)) || 
                (gpPrevious.IsButtonDown(Buttons.B) && gpCurrent.IsButtonUp(Buttons.B)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Attack);
            }
            if (//(kbPrevious.IsKeyDown(kbAttack) && kbCurrent.IsKeyUp(kbAttack)) ||
               (gpPrevious.IsButtonDown(Buttons.Y) && gpCurrent.IsButtonUp(Buttons.Y)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Kick);
            }
        }

    }
}
