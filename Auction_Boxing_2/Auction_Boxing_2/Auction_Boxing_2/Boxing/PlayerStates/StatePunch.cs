using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StatePunch : State
    {

        float dodgedWaitPenalty = 1f;
        float dodgedWaitTimer = 0;

        public StatePunch(BoxingPlayer player)
            : base(player, "Punch")
        {
            isAttack = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1 && dodgedWaitTimer <= 0)
            {
                ChangeState(new StateStopped(player));
            }

            if (dodgedWaitTimer > 0)
                dodgedWaitTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // handle any horizontal movement
            player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 8);
        }

        /// <summary>
        /// Puts the other player into their Hit state.
        /// </summary>
        /// <param name="hitPlayer"></param>
        public override void HitOtherPlayer(BoxingPlayer hitPlayer)
        {
            // Are we at the punch frame? and is the player in front of us?
            if (player.sprite.FrameIndex == 5)
            {
                if ((player.direction == -1 &&
                     player.position.X > hitPlayer.position.X) ||
                     (player.direction == 1 &&
                     player.position.X < hitPlayer.position.X))
                {
                    hitPlayer.state.isHit(player, new StateHit(hitPlayer), 7);
                }
            }
        }

        public override void wasDodged()
        {
            dodgedWaitTimer = dodgedWaitPenalty;
        }

        public override void ChangeState(State state)
        {
            player.input.OnKeyDown -= HandleKeyDownInput;
            player.input.OnKeyRelease -= HandleKeyReleaseInput;

            base.ChangeState(state);
        }

    }
}