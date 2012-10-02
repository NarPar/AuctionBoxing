using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateWalking : State
    {
        float walkSpeed = 100;

        KeyPressed key;

        public StateWalking(BoxingPlayer player, KeyPressed key)
            : base(player, "Walk")
        {
            this.key = key;
            player.currentHorizontalSpeed = player.direction * walkSpeed;
        }


        public override void Update(GameTime gameTime)
        {
            if (player.IsKeyDown(key))
                player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            else
                ChangeState(new StateStopped(player));



            // check for jump!
            if (player.IsKeyDown(KeyPressed.Jump))
            {
                // Ollie!
                ChangeState(new StateJump(player));
            }
            else if (player.IsKeyDown(KeyPressed.Attack))
            {
                // Punch it!
                ChangeState(new StatePunch(player));
            }

            else if (player.IsKeyDown(KeyPressed.Defend))
            {
                // Block it!
                ChangeState(new StateBlock(player));
            }

        }

        public override void OnCombo(int itemIndex)
        {
            Debug.WriteLine("COMBO " + itemIndex + "!");
            // COMBO MOVE!
            switch (itemIndex)
            {
                case (0):
                    ChangeState(new StateCaneBonk(itemIndex, player));
                    break;
                case (1):
                    break;
                case (2):
                    ChangeState(new StateRevolverShoot(itemIndex, player));
                    break;
                case (3):
                    break;
            }

        }
    }
}
