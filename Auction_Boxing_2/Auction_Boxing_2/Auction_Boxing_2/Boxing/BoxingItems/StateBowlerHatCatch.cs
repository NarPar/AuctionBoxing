using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Auction_Boxing_2.Boxing.PlayerStates;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBowlerHatCatch : State
    {
        BowlerHatInstance hat;
        bool hatRemoved = false;
        State originalState;

        public StateBowlerHatCatch(BoxingPlayer player, BowlerHatInstance hat, State originalState)
            : base(player, "BowlerCatch")
        {
            this.hat = hat;
            this.originalState = originalState;
            player.hasThrownBowlerHat = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!hatRemoved)
            {
                player.BoxingManager.removeBowlerHat(hat);
                hatRemoved = true;
                player.numBowlerReThrows = 1;
            }
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                    ChangeState(originalState);
            }

            base.Update(gameTime);
        }
    }
}
