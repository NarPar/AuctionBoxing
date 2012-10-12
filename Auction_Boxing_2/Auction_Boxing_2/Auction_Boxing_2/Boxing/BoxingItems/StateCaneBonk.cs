using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateCaneBonk : State
    {
        Item item;
        int itemindex;

        float dodgedWaitPenalty = 1f;
        float dodgedWaitTimer = 0;

        float holdTimer = 0;
        float holdTime = .2f;

        bool buttonHeld = true;

        public StateCaneBonk(int itemIndex, BoxingPlayer player)
            : base(player, "CaneBonk")
        {
            isAttack = true;
            holdTimer = holdTime;
        }

        public override void Update(GameTime gameTime)
        {
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1 && dodgedWaitTimer <= 0)
            {
                ChangeState(new StateStopped(player));
            }

            if (holdTimer > 0)
                holdTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!player.IsKeyDown(KeyPressed.Attack))
                buttonHeld = false;

            if(buttonHeld && holdTimer <= 0)
                ChangeState(new StateCanePull(itemindex, player));
            
            if (dodgedWaitTimer > 0)
                dodgedWaitTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Puts the other player into their Hit state.
        /// </summary>
        /// <param name="hitPlayer"></param>
        public override void HitOtherPlayer(BoxingPlayer hitPlayer)
        {
            // Are we at the punch frame? and is the player in front of us?
            if (player.sprite.FrameIndex == 7)
            {
                if ((player.direction == -1 &&
                     player.position.X > hitPlayer.position.X) ||
                     (player.direction == 1 &&
                     player.position.X < hitPlayer.position.X))
                {
                    hitPlayer.state.isHit(player, new StateCaneHit(hitPlayer), 10);
                    //Debug.WriteLine("Other player hit!");
                }
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

