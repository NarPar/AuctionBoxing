using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{

    public class StateRevolverHit : State
    {

        //Item item;
        public int hitCounter = 0;
        float timer = .5f;
        float time = .5f;

        float dodgeThreshold = .2f;
        float dodgeTimer = 0;

        float dodgeTryCooldown = .3f;
        float dodgeTryTimer = 0;

        float pHealthstart;

        float moveSpeed = 45;


        public StateRevolverHit(BoxingPlayer player)
            : base(player, "PunchHit")
        {
            pHealthstart = player.CurrentHealth;

        }

        public override void Update(GameTime gameTime)
        {
            // check for state change
            if (!(timer > 0) && player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                ChangeState(new StateStopped(player));
            }

            // reduce counters
            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (player.IsKeyDown(KeyPressed.Left))
            {
                player.direction = -1;
                player.position.X += (float)(player.direction * moveSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (player.IsKeyDown(KeyPressed.Right))
            {
                player.direction = 1;
                player.position.X += (float)(player.direction * moveSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// If we're hit 5 times by the revolver, we get knocked down!
        /// </summary>
        /// <param name="attackingPlayer"></param>
        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            if (timer > 0)
            {
                hitCounter++;
                timer = time;
                player.sprite.FrameIndex = 0;
                player.CurrentHealth -= damage / 2;
            }
            if (hitCounter >= 4)
            {
                ChangeState(new StateKnockedDown(player, attackingPlayer.direction));
                player.CurrentHealth -= damage;
            }
        }
    }
}
