using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2.Boxing.PlayerStates
{
    public abstract class State
    {
        protected BoxingPlayer player;
        protected Color color;
        public bool isAttack;
        public bool canCatch = false;
        public bool canCombo = false;
        protected bool canAirTime = true;


        protected bool isStopping = true; // if you're stopping, apply friction and gravity.
        protected float horizontalDecelleration = 500;
        protected float gravity = 375f;

        protected string key;

        public bool IsKeyDown(KeyPressed key)
        {
            return StatePlayer.KeysDown.Contains(key);
        }

        public bool areKeysDown(KeyPressed key1, KeyPressed key2)
        {
            return StatePlayer.IsKeyDown(key1) && StatePlayer.IsKeyDown(key2);
        }

        public bool areKeysDown(KeyPressed key1, KeyPressed key2, KeyPressed key3)
        {
            return StatePlayer.IsKeyDown(key1) && StatePlayer.IsKeyDown(key2) && StatePlayer.IsKeyDown(key3);
        }

        public bool isOneOfTheseDown(KeyPressed key1, KeyPressed key2)
        {
            return StatePlayer.IsKeyDown(key1) || StatePlayer.IsKeyDown(key2);
        }

        public bool isOneOfTheseDown(KeyPressed key1, KeyPressed key2, KeyPressed key3)
        {
            return StatePlayer.IsKeyDown(key1) || StatePlayer.IsKeyDown(key2) || StatePlayer.IsKeyDown(key3);
        }

        public bool isOneOfTheseDown(KeyPressed key1, KeyPressed key2, KeyPressed key3, KeyPressed key4)
        {
            return StatePlayer.IsKeyDown(key1) || StatePlayer.IsKeyDown(key2) || StatePlayer.IsKeyDown(key3) || StatePlayer.IsKeyDown(key4);
        }

        public BoxingPlayer StatePlayer
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        public State(BoxingPlayer player, string key)
        {
            this.player = player;
            this.key = key;
            
        }

        /// <summary>
        /// Handles any effects to this player.
        /// </summary>
        /// <param name="attackingPlayer"></param>
        public virtual void isHit(BoxingPlayer attackingPlayer, State expectedHitState, int damage)
        {
            
            //Debug.WriteLine("isHit virtual!");

            if (attackingPlayer.state.isAttack)
                player.CurrentHealth -= damage;

            if (player.CurrentHealth <= 0)
                player.state.ChangeState(new StateKnockedDown(player, attackingPlayer.direction));
            else
                player.state.ChangeState(expectedHitState);
        }

        public virtual void isHitByItem(ItemInstance item, State expectedHitState)
        {
            player.CurrentHealth -= item.damage;

            if(player.CurrentHealth <= 0)
                player.state.ChangeState(new StateKnockedDown(player, item.direction));
            else
                player.state.ChangeState(expectedHitState);
        }

        /// <summary>
        /// Handles any effects and damage to the hit player
        /// </summary>
        /// <param name="hitPlayer">The player hit by this player</param>
        public virtual void HitOtherPlayer(BoxingPlayer hitPlayer)
        {
            //Debug.WriteLine("Hit Player!!!");
        }

        protected string StateName;

        public virtual void wasDodged()
        {

        }

        public virtual void Initialize()
        {

        }

        public virtual void HandleKeyDownInput(int player_index, KeyPressed key)
        {

        }

        public virtual void HandleKeyReleaseInput(int player_index, KeyPressed key)
        {

        }

        public virtual void ChangeState(State state)
        {
            player.ChangeAnimation(state.key);
            player.state = state;
        }

        public virtual void OnCombo(int itemIndex)
        {
            /*
            // canCombo is set in the child-states constructer
            if (canCombo) // If you're a state that can combo, change the state.
            {
                //Debug.WriteLine("COMBO " + itemIndex + " Executed!");
                // COMBO MOVE!
                switch (itemIndex)
                {
                    case (0):
                        ChangeState(new StateCaneBonk(itemIndex, player));
                        break;
                    case (1):
                        ChangeState(new StateBowlerHatThrow(itemIndex, player));
                        break;
                    case (2):
                        ChangeState(new StateRevolverShoot(itemIndex, player));
                        break;
                    case (3):
                        Debug.WriteLine("In cape!");
                        ChangeState(new StateCape(itemIndex, player));
                        
                        break;
                }
            }*/
        }

        public virtual void HandleMovement()
        {

        }
        public virtual void HandleDirection()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (isStopping)
            {
                // Horizontal
                if (player.currentHorizontalSpeed > 1 || player.currentHorizontalSpeed < -1)
                {
                    player.currentHorizontalSpeed -= (float)(player.currentHorizontalSpeed / 4);
                }
                else
                    player.currentHorizontalSpeed = 0;

                // If player is falling
                if (player.position.Y < player.GetGroundLevel)
                {
                    // Add gravity
                    player.currentVerticalSpeed += (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    player.position.Y = player.GetGroundLevel;
                    player.currentVerticalSpeed = 0;
                }
            }

            if (canAirTime)
            {

            }
        }

        public virtual void HandleState()
        {

        }
        public virtual void HandleCollision(List<BoxingPlayer> Players)
        {

        }

        public virtual void Translate(float x, float y)
        {

        }

        public virtual void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {

        }











    }
}
