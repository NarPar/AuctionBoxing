using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateRevolverReload : State
    {
        float decceleration = 500;


        public StateRevolverReload(BoxingPlayer player)
            : base(player, "RevolverReload")
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                
                player.isReloadingRevolver = false;
                ChangeState(new StateStopped(player));
            }

            /*if (player.currentHorizontalSpeed > 1 || player.currentHorizontalSpeed < -1)
            {
                //player.position.X += (float)(player.currentHorizontalSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 4);
            }
            else
                player.currentHorizontalSpeed = 0;*/

            #region KeyPress State Changes
            /*
            if (player.IsKeyDown(KeyPressed.Left))
            {
                // Change the direction
                player.ChangeDirection(-1);


                // Double tapped Left? Run mofo!
                if (player.prevKey == KeyPressed.Left && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    // Start walking!
                    ChangeState(new StateRunning(player, KeyPressed.Left));
                }
                // or walk it off
                else
                {
                    // Start walking!
                    ChangeState(new StateWalking(player, KeyPressed.Left));
                }
            }
            else if (player.IsKeyDown(KeyPressed.Right))
            {
                // Change the direction
                player.ChangeDirection(1);

                // Double tapped Left? Run mofo!
                if (player.prevKey == KeyPressed.Right && player.dbleTapTimer > 0 && player.dbleTapCounter == 2)
                {
                    // Start walking!
                    ChangeState(new StateRunning(player, KeyPressed.Right));
                }
                // or walk it off
                else
                {
                    // Start walking!
                    ChangeState(new StateWalking(player, KeyPressed.Right));
                }
            }*/

            #endregion

            base.Update(gameTime);

        }

        /*public override void OnCombo(int itemIndex)
        {
            Debug.WriteLine("COMBO " + itemIndex + " Executed!");
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

        }*/

    }
}
