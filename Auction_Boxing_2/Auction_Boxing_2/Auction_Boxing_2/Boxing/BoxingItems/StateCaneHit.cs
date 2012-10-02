using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{

    public class StateCaneHit : State
    {

        //Item item;
        int hitCounter = 0;
        float timer = .3f;

        float dodgeThreshold = .2f;
        float dodgeTimer = 0;

        float dodgeTryCooldown = .3f;
        float dodgeTryTimer = 0;

        public StateCaneHit(BoxingPlayer player)
            : base(player, "CaneHit")
        {
        }

        public override void Update(GameTime gameTime)
        {
            // check for state change
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                ChangeState(new StateStopped(player));

            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }


        /// <summary>
        /// If we're hit twice in a row, we get knocked down!
        /// </summary>
        /// <param name="attackingPlayer"></param>
        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            if (timer <= 0)
            {
                if (hitCounter >= 1)
                {
                    ChangeState(new StateKnockedDown(player, attackingPlayer.direction));
                    player.CurrentHealth -= 20;
                }
                else
                {
                    hitCounter++;
                }
            }
        }
    }
}
