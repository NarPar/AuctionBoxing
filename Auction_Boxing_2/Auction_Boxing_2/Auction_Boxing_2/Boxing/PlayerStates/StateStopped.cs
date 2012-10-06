﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateStopped : State
    {
        float decceleration = 500;


        public StateStopped(BoxingPlayer player)
            : base(player, "Idle")
        {
            canCombo = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.currentHorizontalSpeed > 1 || player.currentHorizontalSpeed < -1)
            {
                player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 4);
            }
            else
                player.currentHorizontalSpeed = 0;

            #region KeyPress State Changes

            if (player.IsKeyDown(KeyPressed.Left))
            {
                // Change the direction
                player.ChangeDirection(-1);


                // Double tapped Left? Run mofo!
                if (player.prevKey == KeyPressed.Left && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    // Start walking!
                    ChangeState(new StateRunning(player, KeyPressed.Left));
                }
                // or walk it off
                else
                {
                    // Start walking!
                    ChangeState(new StateWalking(player, KeyPressed.Left));
                }
            }
            else if (player.IsKeyDown(KeyPressed.Right))
            {
                // Change the direction
                player.ChangeDirection(1);

                // Double tapped Left? Run mofo!
                if (player.prevKey == KeyPressed.Right && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    // Start walking!
                    ChangeState(new StateRunning(player, KeyPressed.Right));
                }
                // or walk it off
                else
                {
                    // Start walking!
                    ChangeState(new StateWalking(player, KeyPressed.Right));
                }
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
            else if (player.IsKeyDown(KeyPressed.Jump))
            {
                // Ollie!
                ChangeState(new StateJump(player));
            }
            else if (player.IsKeyDown(KeyPressed.Down))
            {
                ChangeState(new StateDuck(player));
                
            }

            #endregion

        }
    }
}