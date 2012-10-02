using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                float l = player.BoxingManager.GetLowerPlatformLevel(player.position.Y);

                // Double tapped Down? Jump down a level!
                if (player.prevKey == KeyPressed.Down && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    if (l != player.levellevel)
                    {
                        player.levellevel = l;
                        ChangeState(new StateFall(player));
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
            
            // Nothing happens! You can't phase me bro!

            // well timed? Duck and weave!
            /*if (dodgeThreshold > 0)
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
            }*/
        }

        public override void OnCombo(int itemIndex)
        {
            // COMBO MOVE!
            switch (itemIndex)
            {
                case (0):
                    ChangeState(new StateCaneBonk(itemIndex, player));
                    break;
                case (1):
                    break;
                case (2):
                    ChangeState(new StateRevolverShoot(itemIndex, player));
                    break;
                case (3):
                    break;
            }

        }
    }
}
