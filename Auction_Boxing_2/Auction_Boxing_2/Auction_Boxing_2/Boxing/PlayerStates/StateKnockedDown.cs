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

        float knockbackVelocity = 10;

        public StateKnockedDown(BoxingPlayer player, int dir)
            : base(player, "Down")
        {
            this.dir = dir; // The direction which they are knocked down.
        }


        public override void Update(GameTime gameTime)
        {
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

            // Knock them back
            player.position.X += dir * knockbackVelocity;

            knockbackVelocity -= .2f;
            if (knockbackVelocity < 0)
                knockbackVelocity = 0;

            
        }

        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            
        }





    }
}