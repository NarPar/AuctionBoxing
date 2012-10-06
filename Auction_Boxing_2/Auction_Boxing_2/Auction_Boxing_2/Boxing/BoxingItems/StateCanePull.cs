using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateCanePull : State
    {
        Item item;
        int itemindex;

        float dodgedWaitPenalty = 1f;
        float dodgedWaitTimer = 0;

        float holdTimer = 0;
        float holdTime = .2f;

        public StateCanePull(int itemIndex, BoxingPlayer player)
            : base(player, "CanePull")
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
        }

        /// <summary>
        /// Puts the other player into their Hit state.
        /// </summary>
        /// <param name="hitPlayer"></param>
        public override void HitOtherPlayer(BoxingPlayer hitPlayer)
        {
            // Are we at the pull frame? and is the player in front of us?
            if (player.sprite.FrameIndex > 5  && player.sprite.FrameIndex < 9)  //6 || player.sprite.FrameIndex == 7
                //|| player.sprite.FrameIndex == 8)
            {
                if ((player.direction == -1 &&
                     player.position.X > hitPlayer.position.X) )
                {
                    hitPlayer.position.X += 10;
                    if (hitPlayer.position.X > player.position.X - 30)
                        hitPlayer.position.X = player.position.X - 30;
                }
                if((player.direction == 1 &&
                     player.position.X < hitPlayer.position.X))
                {
                    hitPlayer.position.X -= 10;
                    if (hitPlayer.position.X < player.position.X + 30)
                        hitPlayer.position.X = player.position.X + 30;
                }

                // Stun player!

                //hitPlayer.state.isHit(hitPlayer, new StateCaneHit(hitPlayer), 0);
            }
        }

        public override void wasDodged()
        {
            dodgedWaitTimer = dodgedWaitPenalty;
        }

        public override void ChangeState(State state)
        {
            //player.input.OnKeyDown -= HandleKeyDownInput;
            //player.input.OnKeyRelease -= HandleKeyReleaseInput;

            base.ChangeState(state);
        }
    }
}

