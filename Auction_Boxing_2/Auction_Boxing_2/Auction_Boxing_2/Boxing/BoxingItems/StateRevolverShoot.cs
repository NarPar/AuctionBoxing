using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public class StateRevolverShoot : State
    {
        Item item;
        int itemindex;

        int shootCounter;

        public bool isShooting;

        public StateRevolverShoot(int itemIndex, BoxingPlayer player)
            : base(player, "RevolverShoot")
        {
           
            isAttack = false;

            player.input.OnKeyDown += HandleKeyDownInput;

            shootCounter = 0;
            
        }

        public override void Update(GameTime gameTime)
        {
            // Check if we're reloading
            if (player.isReloadingRevolver)
            {
                ChangeState(new StateRevolverReload(player));
            }

            if (shootCounter == 0 && player.sprite.FrameIndex == 5)
            {
                player.sprite.FrameIndex = 9;
            }
            else if(player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                if (shootCounter > 0)
                {
                    player.isReloadingRevolver = true;
                }
                ChangeState(new StateStopped(player));

            }

            /*if (player.sprite.FrameIndex == 8)
            {
                isShooting = true;
                BoxingPlayer p = player.BoxingManager.GetPlayerInFront(player);
                if (p != null)
                {
                    p.state.isHit(p, new StateRevolverHit(p), 20);
                    if(p.state is StateRevolverHit)
                    {
                        StateRevolverHit s = (StateRevolverHit)p.state
                        s.hitCounter = shootCounter;
                    }
                }
            }*/
        }

        public override void HandleKeyDownInput(int player_index, KeyPressed key)
        {

            if (shootCounter == 0 && (player.sprite.FrameIndex == 4 || player.sprite.FrameIndex == 5))
            {
                shootCounter++;
                BoxingPlayer p = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 7 * player.GetHeight / 9, player.direction);
                if (p != null)
                {
                    p.state.isHit(p, new StateRevolverHit(p), 5);
                    /*if(p.state is StateRevolverHit)
                    {
                        StateRevolverHit s = (StateRevolverHit)p.state
                        s.hitCounter = shootCounter;
                    }*/
                }
            }
            else if(shootCounter < 5 && (player.sprite.FrameIndex == 8 || player.sprite.FrameIndex == 9))
            {
                player.sprite.FrameIndex = 5;

                shootCounter++;

                BoxingPlayer p = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 2 * player.GetHeight / 3, player.direction);
                if (p != null)
                {
                    p.state.isHit(player, new StateRevolverHit(p), 5);
                    /*if(p.state is StateRevolverHit)
                    {
                        StateRevolverHit s = (StateRevolverHit)p.state
                        s.hitCounter = shootCounter;
                    }*/
                }
            }


        }

        public override void ChangeState(State state)
        {

            player.input.OnKeyDown -= HandleKeyDownInput;

            base.ChangeState(state);
        }
    }
}

