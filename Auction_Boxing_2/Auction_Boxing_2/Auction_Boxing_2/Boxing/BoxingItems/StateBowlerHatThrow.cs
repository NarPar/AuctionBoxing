using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBowlerHatThrow : State
    {

        float holdTimer = 0;
        float holdTime = .2f;

        bool buttonHeld = true;

        int itemIndex;

        public StateBowlerHatThrow(int itemIndex, BoxingPlayer player)
            : base(player, "bowlerThrow")
        {
            this.itemIndex = itemIndex;
            holdTimer = holdTime;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                    ChangeState(new StateStopped(player));
            }

            if (holdTimer > 0)
                holdTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!player.IsKeyDown(KeyPressed.Attack))
                buttonHeld = false;

            if (buttonHeld && holdTimer <= 0)
                ChangeState(new StateBowlerHatReThrow(itemIndex, player));
        }
    }
}
