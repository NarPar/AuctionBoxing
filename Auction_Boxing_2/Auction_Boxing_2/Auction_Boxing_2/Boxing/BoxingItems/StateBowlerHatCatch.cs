using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBowlerHatCatch : State
    {
        public StateBowlerHatCatch(int itemIndex, BoxingPlayer player)
            : base(player, "bowlerCatch")
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                    ChangeState(new StateStopped(player));
            }
        }
    }
}
