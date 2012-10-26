using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateLand : State
    {
        public StateLand(BoxingPlayer player)
            : base(player, "Land")
        {
            canCatch = true;
        }

        public override void Update(GameTime gameTime)
        {
            // check for state change
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                ChangeState(new StateStopped(player));

            /*if (player.currentHorizontalSpeed > 1 || player.currentHorizontalSpeed < -1)
            {
                player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 4);
            }
            else
                player.currentHorizontalSpeed = 0;*/
            base.Update(gameTime);
        }
    }
}
