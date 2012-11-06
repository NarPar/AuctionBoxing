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
        float timer = .07f;
        float time = .07f;

        float dodgeThreshold = .2f;
        float dodgeTimer = 0;

        float dodgeTryCooldown = .3f;
        float dodgeTryTimer = 0;

        public StateCaneHit(BoxingPlayer player)
            : base(player, "CaneHit")
        {

        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            //player.soundEffects["CaneHit"].Play(); // play the sound effect!
            base.LoadState(player, ATextures);
        }

        public override void Update(GameTime gameTime)
        {
            if (!hasPlayedSound)
                PlaySound(player.soundEffects["CaneHit"]); // play the sound effect!


            // check for state change
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                ChangeState(new StateStopped(player));

            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }


        /// <summary>
        /// If we're hit twice in a row, we get knocked down!
        /// </summary>
        /// <param name="attackingPlayer"></param>
        public override void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            if (timer <= 0)
            {

                ChangeState(new StateKnockedDown(player, attackingPlayer.direction, true));
                player.CurrentHealth -= damage;
                

                if(attackingPlayer.state is StateCaneBonk)
                    player.soundEffects["CaneHit"].Play(); // play the sound effect!
                //else
                    //player.soundEffects["

                timer = time;
            }
        }
    }
}
