using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateFall : State
    {
        float verticalSpeed = -3500;
        float horizontalAcceleration = 200;
        float maxHorizontalSpeed = 12000;

        float gravity = 1000f;
        bool drop; // drop if going between platforms, else falling after animation.
        public float startPosition;

        public StateFall(BoxingPlayer player, bool drop)
            : base(player, "Jump")
        {
            if (drop)
            {
                //player.position.Y += 5;
                player.currentVerticalSpeed = 20;
            }
            else
            {
                
                player.sprite.FrameIndex = 5;
            }
            canCatch = true;
        }


        public override void Update(GameTime gameTime)
        {
            if(!drop)
                player.sprite.FrameIndex = 5;

            // handle any horizontal movement
            //player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
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
                if (player.IsKeyDown(KeyPressed.Jump))
                    player.currentVerticalSpeed += (float)((gravity * gameTime.ElapsedGameTime.TotalSeconds) / 2);
                else
                    player.currentVerticalSpeed += (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds);

                if (player.position.Y >= player.levellevel)//startPosition)
                {
                    player.position.Y = player.levellevel;

                    ChangeState(new StateLand(player));
                }

                if (player.currentVerticalSpeed > 0)
                {
                    player.isFalling = true;
                }
            }

        }

        public override void isHit(Auction_Boxing_2.BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            ChangeState(new StateKnockedDown(player, attackingPlayer.direction));
        }
    }
}

