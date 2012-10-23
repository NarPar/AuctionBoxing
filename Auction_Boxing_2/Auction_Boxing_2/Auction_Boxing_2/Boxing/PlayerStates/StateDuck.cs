using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateDuck : State
    {
        float hitVelocity = 10;

        float waitTime = .5f;
        float timer = 0;

        float dodgeThreshold = .2f;

        public StateDuck(BoxingPlayer player)
            : base(player, "Duck")
        {
            canCatch = true;
            canCombo = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (player.IsKeyDown(KeyPressed.Down))
            {
                player.sprite.FrameIndex = 5;
            }
            else if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                ChangeState(new StateStopped(player));
            }

            /*// Track hit cooldown timer
            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Track the dodge timer
            if (dodgeThreshold > 0)
                dodgeThreshold -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            */

            // handle any horizontal movement
            player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 6);


            if (player.IsKeyDown(KeyPressed.Down))
            {
                Rectangle lp = player.BoxingManager.GetLowerPlatform(player.position);

                float l = lp.Y;
                // Double tapped Down? Jump down a level!
                if (player.prevKey == KeyPressed.Down && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    if (l != player.levellevel)
                    {
                        player.levellevel = l;
                        player.position.Y += 2;
                        ChangeState(new StateFall(player, true));
                    }
                }
            }
        }

        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            //If you get jumped on, you get knocked down!
            if (attackingPlayer.state is StateFall || attackingPlayer.isFalling)
            {
                ChangeState(new StateKnockedDown(player, 0));
            }
            
        }

    }
}
