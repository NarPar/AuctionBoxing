using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateKnockedDown : State
    {
        //const int DownTime = 1500;

        //const int Wait = 1;

        //GameTimer DownTimer;

        float gravity = 375f;
        int dir;

        //float speedReductionValue = .5f;

        float knockbackVelocity = 400;

        public StateKnockedDown(BoxingPlayer player, int dir)
            : base(player, "Down")
        {
            isStopping = false;
            this.dir = dir; // The direction which they are knocked down.
            //Debug.WriteLine("Fall direction = " + dir);
        }

        public override void ChangeState(State state)
        {
            player.currentHorizontalSpeed = -knockbackVelocity;
            base.ChangeState(state);
        }

        public override void Update(GameTime gameTime)
        {
            if (player.currentVerticalSpeed < 0)
                player.currentVerticalSpeed = player.currentVerticalSpeed / 2;

            // If player is falling
            if (player.position.Y < player.GetGroundLevel)
            {
                player.position.Y += (float)(player.currentVerticalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                // add acceleration
                player.currentVerticalSpeed += (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds);

                if (player.sprite.FrameIndex >= 5)
                    player.sprite.FrameIndex = 5;
            }


            else if (player.isDead)
                player.sprite.FrameIndex = 5;
            else if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1 && !player.isDead)
                ChangeState(new StateStopped(player));



            if (player.position.Y >= player.levellevel)
            {
                //Debug.WriteLine("Landed! ");
                player.currentVerticalSpeed = 0;
                player.position.Y = player.levellevel;
            }

            /*player.position.X += player.currentHorizontalSpeed;

            // Horizontal
            if (player.currentHorizontalSpeed > 1 || player.currentHorizontalSpeed < -1)
            {
                player.currentHorizontalSpeed += (float)(player.currentHorizontalSpeed / 6);
            }
            else
                player.currentHorizontalSpeed = 0;*/

            // Knock them back
            player.position.X += dir * knockbackVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            knockbackVelocity -= knockbackVelocity / 12;
            if (knockbackVelocity < 1 && knockbackVelocity > -1)
                knockbackVelocity = 0;

            
        }

        // if you're knocked down, nothing can hit you
        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            
        }
        // Not even items!
        public override void  isHitByItem(ItemInstance item, State expectedHitState)
        {
 	        // base.isHitByItem(item, expectedHitState);
        }






    }
}