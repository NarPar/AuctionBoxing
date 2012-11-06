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
        KeyPressed itemButton;

        int shootCounter;

        public bool isShooting;

        float clickTimer = 0f;
        float clickTime = .01f;
        bool hasClicked1 = false;
        bool hasClicked2 = false;

        int damage = 3;

        public StateRevolverShoot(int itemIndex, BoxingPlayer player, KeyPressed key)
            : base(player, "RevolverShoot")
        {
           
            isAttack = false;

            player.input.OnKeyDown += HandleKeyDownInput;

            shootCounter = 0;

            this.itemButton = key;

            canCatch = true;
            
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

            if (clickTimer > 0)
                clickTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cock the gun!
            if (!hasClicked1 && player.sprite.FrameIndex == 3)
            {
                player.soundEffects["GunClick"].Play(.5f, 0, 0);
                hasClicked1 = true;
                clickTimer = clickTime;
            }
            else if (!hasClicked2 && clickTimer <= 0)
            {
                hasClicked2 = true;
                player.soundEffects["GunClick"].Play(.5f, 0, 0);
            }

            base.Update(gameTime);
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
            if (key == itemButton) // pressing the item button?
            {
                if (shootCounter == 0 && (player.sprite.FrameIndex == 4 || player.sprite.FrameIndex == 5))
                {

                    player.soundEffects["RevolverShoot"].Play(.5f,0,0); // play the sound effect!

                    shootCounter++;
                    BoxingPlayer p = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 7 * player.GetHeight / 9, player.direction);
                    if (p != null && !(p.state is StateKnockedDown))
                    {

                        p.beingComboedTimer = p.beingComboedCooldown;
                        p.revolverHitCounter++;
                        p.state.isHit(p, new StateRevolverHit(p), damage);
                        /*if(p.state is StateRevolverHit)
                        {
                            StateRevolverHit s = (StateRevolverHit)p.state
                            s.hitCounter = shootCounter;
                        }*/
                    }
                }
                else if (shootCounter < 5 && (player.sprite.FrameIndex == 8 || player.sprite.FrameIndex == 9))
                {
                    player.sprite.FrameIndex = 5;

                    shootCounter++;

                    BoxingPlayer p = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 2 * player.GetHeight / 3, player.direction);
                    if (p != null && !(p.state is StateKnockedDown))
                    {
                        player.soundEffects["RevolverShoot"].Play(.5f, 0, 0); // play the sound effect!
                        p.state.isHit(player, new StateRevolverHit(p), damage);
                        /*if(p.state is StateRevolverHit)
                        {
                            StateRevolverHit s = (StateRevolverHit)p.state
                            s.hitCounter = shootCounter;
                        }*/
                        // reset combo counter

                        p.beingComboedTimer = p.beingComboedCooldown;
                        p.revolverHitCounter++;
                        Debug.WriteLine("Hit " + p.revolverHitCounter + " times!");
                        if (p.beingComboedTimer > 0 && p.revolverHitCounter >= 5)
                        {
                            p.state.ChangeState(new StateKnockedDown(p, player.direction, true));
                            //player.CurrentHealth -= damage;
                        }
                    }
                }
            }
            base.HandleKeyDownInput(player_index, key);

        }

        public override void ChangeState(State state)
        {

            player.input.OnKeyDown -= HandleKeyDownInput;

            base.ChangeState(state);
        }
    }
}

