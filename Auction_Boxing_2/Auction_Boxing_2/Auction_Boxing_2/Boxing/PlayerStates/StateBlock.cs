using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBlock : State
    {
        float hitVelocity = 10;

        float waitTime = .5f;
        float timer = 0;

        float dodgeThreshold = .2f;

        public StateBlock(BoxingPlayer player)
            : base(player, "Block")
        {
            canCombo = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            // check for state change
            if (!player.IsKeyDown(KeyPressed.Defend))
            {
                ChangeState(new StateStopped(player));
            }

            // Track hit cooldown timer
            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Track the dodge timer
            if (dodgeThreshold > 0)
                dodgeThreshold -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // handle any horizontal movement
            player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 6);
                
        }

        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            // Nothing happens! You can't phase me bro!

            // well timed? Duck and weave!
            if (dodgeThreshold > 0)
            {
                ChangeState(new StateDodge(player));
                attackingPlayer.state.wasDodged();
            }

            // just get knocked back a little
            else if (timer <= 0)
            {
                if ((player.direction == 1 && player.position.X < attackingPlayer.position.X)
                || (player.direction == -1 && player.position.X > attackingPlayer.position.X))
                {
                    timer = waitTime;
                    player.position.X += attackingPlayer.direction * hitVelocity;

                    // and some chip damage
                    player.CurrentHealth -= 5;
                }
                else
                {
                    // You're facing the wrong way and get knocked down, homie!
                    base.isHit(attackingPlayer, expectedHitState, damage);
                }
            }
        }
    }
}
