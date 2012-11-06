using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateRunning : State

    {
        float runSpeed = 170;

        KeyPressed key;

        int previousIndex = -1;

        public StateRunning(BoxingPlayer player, KeyPressed key)
            : base(player, "Run")
        {
            isStopping = false;
            this.key = key;
            player.currentHorizontalSpeed = runSpeed;//player.direction* runSpeed;
            canCombo = true;
            canCatch = true;
            runSpeed *= BoxingPlayer.Scale;
        }

        public override void Update(GameTime gameTime)
        {

            if (player.sprite.FrameIndex == 4 && previousIndex != 4)
                player.soundEffects["Footstep"].Play(.5f,0,0);


            /*if (player.IsKeyDown(key))
            {
                float add = 0;
                if (player.isBumping)
                {
                    add = (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds / 2);
                }
                else
                    add = (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                //float add = (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                player.position.X += add;
            }
            else*/
            if(!player.IsKeyDown(key))
                ChangeState(new StateStopped(player));
            // handle any horizontal movement


            // check for jump!
            if (player.IsKeyDown(KeyPressed.Jump))
            {
                // Ollie!
                ChangeState(new StateJump(player, false));
            }
            /*else if (player.IsKeyDown(KeyPressed.Attack))
            {
                // Punch it!
                ChangeState(new StatePunch(player));
            }*/

            else if (player.IsKeyDown(KeyPressed.Defend))
            {
                // Block it!
                ChangeState(new StateBlock(player));
            }

            previousIndex = player.sprite.FrameIndex;

            base.Update(gameTime);
        }


    }
}
