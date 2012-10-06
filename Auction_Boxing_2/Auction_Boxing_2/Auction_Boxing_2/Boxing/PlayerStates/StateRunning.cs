﻿using System;
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
        float runSpeed = 260;

        KeyPressed key;

        public StateRunning(BoxingPlayer player, KeyPressed key)
            : base(player, "Run")
        {
            this.key = key;
            player.currentHorizontalSpeed = player.direction* runSpeed;
            canCombo = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.IsKeyDown(key))
                player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            else
                ChangeState(new StateStopped(player));
            // handle any horizontal movement


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


    }
}