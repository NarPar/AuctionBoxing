using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateJump : State
    {
        float virticalSpeed = -3500;
        float horizontalAcceleration = 400;
        float maxHorizontalSpeed = 12000;

        float gravity = 1200f;

        public float startPosition;


        public StateJump(BoxingPlayer player)
            : base(player, "Jump")
        {
            startPosition = player.levellevel;

            player.currentVerticalSpeed = -400;
            canCatch = true;
        }


        public override void Update(GameTime gameTime)
        {
            // handle any horizontal movement
            player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (player.IsKeyDown(KeyPressed.Left))
            {
                player.currentHorizontalSpeed -= (float)(horizontalAcceleration * gameTime.ElapsedGameTime.TotalSeconds);

            }
            else if (player.IsKeyDown(KeyPressed.Right))
            {
                player.currentHorizontalSpeed += (float)(horizontalAcceleration * gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Note: player.currentHorizontalSpeed will always be positive.
            if (Math.Abs(player.currentHorizontalSpeed) >= maxHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds)
                player.currentHorizontalSpeed = (float)(player.direction * maxHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);




            // handle virtical stuff after the launch frame
            if (player.sprite.FrameIndex >= 1)
            {
                player.position.Y += (float)(player.currentVerticalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                
                // if the player is holding jump, reduce the pull of gravity
                if (player.IsKeyDown(KeyPressed.Jump) && player.currentVerticalSpeed < 0)
                {
                    player.currentVerticalSpeed += (float)((gravity * gameTime.ElapsedGameTime.TotalSeconds) / 2);
                }
                else
                    player.currentVerticalSpeed += (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds);

                if (player.position.Y >= player.levellevel)
                {
                    player.currentVerticalSpeed = 0;
                    player.position.Y = player.levellevel;
                    player.isFalling = false;
                    ChangeState(new StateLand(player));
                }

                // You're falling!
                if (player.currentVerticalSpeed > 0)
                {
                    player.platform = player.BoxingManager.GetLowerPlatform(player.position);
                    player.levellevel = player.platform.Y;
                    player.isFalling = true;
                }
            }

            /*if (player.IsKeyDown(KeyPressed.Attack))
            {
                // Punch it!
                ChangeState(new StatePunch(player));
            }*/

        }

        public override void isHit(Auction_Boxing_2.BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            ChangeState(new StateKnockedDown(player,attackingPlayer.direction));
        }
    }
}

