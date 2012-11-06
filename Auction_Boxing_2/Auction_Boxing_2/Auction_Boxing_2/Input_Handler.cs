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
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Select
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
        public Keys kbAttack1;
        public Keys kbAttack2;
        public Keys kbAttack3;
        public Keys kbAttack4;

        public Keys kbSelect;


        //-----Gamepad-----//

        GamePadState gpPrevious;
        GamePadState gpCurrent;

        int player_index;
        PlayerIndex gamePadPlayerIndex;

        bool isGamePad;

        float gamepadTriggerThreshold = 0.5f;


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

            // Gamepad:
            /*
             * up = jump
             * trigger = block
             * a = attack 1
             * b = attack 2
             * x = attack 3
             * y = attack 4
            */

            // KEyboard sucks mind you!
            // Init keyboard controls
            switch (playerNumber)
            {
                case 0:
                    kbUp = Keys.W;
                    kbDown = Keys.S;
                    kbLeft = Keys.A;
                    kbRight = Keys.D;

                    kbDefend = Keys.Q;
                    kbJump = Keys.W;
                    kbAttack1 = Keys.Z;
                    kbAttack2 = Keys.X;
                    kbAttack3 = Keys.C;
                    kbAttack4 = Keys.V;

                    kbSelect = Keys.F;

                    gamePadPlayerIndex = PlayerIndex.One;
                    break;

                case 1:
                    kbUp = Keys.I;
                    kbDown = Keys.K;
                    kbLeft = Keys.J;
                    kbRight = Keys.L;

                    kbDefend = Keys.U;
                    kbJump = Keys.I;
                    kbAttack1 = Keys.N;
                    kbAttack2 = Keys.M;
                    kbAttack3 = Keys.OemComma;
                    kbAttack4 = Keys.OemPeriod;

                    kbSelect = Keys.OemSemicolon;

                    gamePadPlayerIndex = PlayerIndex.Two;
                    break;

                case 2:
                    kbUp = Keys.Up;
                    kbDown = Keys.Down;
                    kbLeft = Keys.Left;
                    kbRight = Keys.Right;

                    kbDefend = Keys.OemBackslash;
                    kbJump = Keys.Delete;
                    //kbAttack = Keys.End;

                    gamePadPlayerIndex = PlayerIndex.Three;
                    break;

                case 3:
                    kbUp = Keys.NumPad8;
                    kbDown = Keys.NumPad5;
                    kbLeft = Keys.NumPad4;
                    kbRight = Keys.NumPad6;

                    kbDefend= Keys.NumPad1;
                    kbJump = Keys.NumPad2;
                    //kbAttack = Keys.NumPad3;

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
            //Defend Key Pressed? LEft or right triggers past a certain threshold (currently .5)
            if ((kbPrevious.IsKeyUp(kbDefend) && kbCurrent.IsKeyDown(kbDefend)) ||
                (gpPrevious.Triggers.Left < gamepadTriggerThreshold && gpCurrent.Triggers.Left >= gamepadTriggerThreshold)  ||
                (gpPrevious.Triggers.Right < gamepadTriggerThreshold && gpCurrent.Triggers.Right >= gamepadTriggerThreshold))
            {
                
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Defend);
            }
            //Jump Key Pressed?
            if ((kbPrevious.IsKeyUp(kbJump) && kbCurrent.IsKeyDown(kbJump)) ||
                (gpPrevious.IsButtonUp(Buttons.DPadUp) && gpCurrent.IsButtonDown(Buttons.DPadUp)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Jump);
            }
            //Attack 1
            if ((kbPrevious.IsKeyUp(kbAttack1) && kbCurrent.IsKeyDown(kbAttack1)) ||
                (gpPrevious.IsButtonUp(Buttons.A) && gpCurrent.IsButtonDown(Buttons.A)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Attack1);
            }

            // Attack 2
            if ((kbPrevious.IsKeyUp(kbAttack2) && kbCurrent.IsKeyDown(kbAttack2)) || 
                (gpPrevious.IsButtonUp(Buttons.B) && gpCurrent.IsButtonDown(Buttons.B)))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Attack2);
            }
             
            // Attack 3
            if ((kbPrevious.IsKeyUp(kbAttack3) && kbCurrent.IsKeyDown(kbAttack3)) || 
                gpPrevious.IsButtonUp(Buttons.X) && gpCurrent.IsButtonDown(Buttons.X))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Attack3);
            }

            // Attack 4
            if ((kbPrevious.IsKeyUp(kbAttack4) && kbCurrent.IsKeyDown(kbAttack4)) || 
                gpPrevious.IsButtonUp(Buttons.Y) && gpCurrent.IsButtonDown(Buttons.Y))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Attack4);
            }

            // Select
            if ((kbPrevious.IsKeyUp(kbSelect) && kbCurrent.IsKeyDown(kbSelect)) ||
                gpPrevious.IsButtonUp(Buttons.Back) && gpCurrent.IsButtonDown(Buttons.Back))
            {
                if (OnKeyDown != null)
                    OnKeyDown(player_index, KeyPressed.Select);
            }

            // ================= Keys held? ==========================

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

            // Defend if left or right trigger past trigger threshold (currently .5)
            if (kbCurrent.IsKeyDown(kbDefend) || 
                (gpCurrent.Triggers.Left >= gamepadTriggerThreshold) || // left trigger
                (gpCurrent.Triggers.Right >= gamepadTriggerThreshold)) // right trigger
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Defend);
            }
            // Up is jump
            if (kbCurrent.IsKeyDown(kbJump) || gpCurrent.IsButtonDown(Buttons.DPadUp))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Jump);
            }
            // attack 1 = A
            if (kbCurrent.IsKeyDown(kbAttack1) || gpCurrent.IsButtonDown(Buttons.A))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Attack1);
            }
            // Attack 2 = B
            if (kbCurrent.IsKeyDown(kbAttack2) || gpCurrent.IsButtonDown(Buttons.B))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Attack2);
            }
            // Attack 3 = X
            if (kbCurrent.IsKeyDown(kbAttack3) || gpCurrent.IsButtonDown(Buttons.X))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Attack3);
            }
            // Attack 4 = Y
            if (kbCurrent.IsKeyDown(kbAttack4) || gpCurrent.IsButtonDown(Buttons.Y))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Attack4);
            }

            // Attack 4 = Y
            if (kbCurrent.IsKeyDown(kbSelect) || gpCurrent.IsButtonDown(Buttons.Back))
            {
                if (OnKeyHold != null)
                    OnKeyHold(player_index, KeyPressed.Select);
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
            //Defend Key Released? Either left or right trigger past threshold (currently .5)
            if ((kbPrevious.IsKeyDown(kbDefend) && kbCurrent.IsKeyUp(kbDefend)) || 
                ((gpPrevious.Triggers.Left >= gamepadTriggerThreshold && gpCurrent.Triggers.Left < gamepadTriggerThreshold)  ||
                (gpPrevious.Triggers.Right >= gamepadTriggerThreshold && gpCurrent.Triggers.Right < gamepadTriggerThreshold)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Defend);
            }

            if ((kbPrevious.IsKeyDown(kbJump) && kbCurrent.IsKeyUp(kbJump)) || 
                (gpPrevious.IsButtonDown(Buttons.DPadUp) && gpCurrent.IsButtonUp(Buttons.DPadUp)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Jump);
            }

            //Attack 1
            if ((kbPrevious.IsKeyDown(kbAttack1) && kbCurrent.IsKeyUp(kbAttack1)) ||
                (gpPrevious.IsButtonDown(Buttons.A) && gpCurrent.IsButtonUp(Buttons.A)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Attack1);
            }

            // Attack 2
            if ((kbPrevious.IsKeyDown(kbAttack2) && kbCurrent.IsKeyUp(kbAttack2)) || 
                (gpPrevious.IsButtonDown(Buttons.B) && gpCurrent.IsButtonUp(Buttons.B)))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Attack2);
            }
             
            // Attack 3
            if ((kbPrevious.IsKeyDown(kbAttack3) && kbCurrent.IsKeyUp(kbAttack3)) || 
                gpPrevious.IsButtonDown(Buttons.X) && gpCurrent.IsButtonUp(Buttons.X))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Attack3);
            }

            // Attack 4
            if ((kbPrevious.IsKeyDown(kbAttack4) && kbCurrent.IsKeyUp(kbAttack4)) || 
                gpPrevious.IsButtonDown(Buttons.Y) && gpCurrent.IsButtonUp(Buttons.Y))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Attack4);
            }

            // Attack 4
            if ((kbPrevious.IsKeyDown(kbSelect) && kbCurrent.IsKeyUp(kbSelect)) ||
                gpPrevious.IsButtonDown(Buttons.Back) && gpCurrent.IsButtonUp(Buttons.Back))
            {
                if (OnKeyRelease != null)
                    OnKeyRelease(player_index, KeyPressed.Select);
            }
        }

    }
}
