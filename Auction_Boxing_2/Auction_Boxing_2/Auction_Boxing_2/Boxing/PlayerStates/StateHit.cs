using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateHit : State
    {
        //Item item;
        int hitCounter = 0;
        float timer = .3f;

        float dodgeThreshold = .2f;
        float dodgeTimer = 0;

        float dodgeTryCooldown = .3f;
        float dodgeTryTimer = 0;

        public StateHit(BoxingPlayer player)
            : base(player, "PunchHit")
        {
            player.input.OnKeyDown += HandleKeyDownInput;
        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {

            base.LoadState(player, ATextures);
        }

        public override void Update(GameTime gameTime)
        {
            // check for state change
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                ChangeState(new StateStopped(player));

            if(timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (timer <= 0)
             //   StatePlayer.InternalState = new StateMoving(this);


            // Track the dodge timer
            if (dodgeTimer > 0)
                dodgeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (dodgeTryTimer > 0)
                dodgeTryTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (player.currentVerticalSpeed < 0)
                player.currentVerticalSpeed = player.currentVerticalSpeed / 2;

            base.Update(gameTime);
        }

        public override void HandleKeyDownInput(int player_index, KeyPressed key)
        {
            if (key == KeyPressed.Defend && dodgeTryTimer <= 0)
            {

                dodgeTimer = dodgeThreshold;

                dodgeTryTimer = dodgeTryCooldown;
            }
        }

        /// <summary>
        /// If we're hit twice in a row, we get knocked down!
        /// </summary>
        /// <param name="attackingPlayer"></param>
        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            if (timer <= 0)
            {
                // well timed? Duck and weave!
                if (dodgeTimer > 0)
                {
                    ChangeState(new StateDodge(player));
                    attackingPlayer.state.wasDodged();
                }
                else
                    hitCounter++;
            }

            if (hitCounter >= 1)
            {
                ChangeState(new StateKnockedDown(player, attackingPlayer.direction));
                player.CurrentHealth -= 20;
            }
        }

        public override void ChangeState(State state)
        {
            // unlisten
            player.input.OnKeyDown -= HandleKeyDownInput;

            base.ChangeState(state);
        }


    }
}
