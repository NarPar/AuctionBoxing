using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2
{
    class StateStopped : State
    {
        public StateStopped(State state)
        {
            StateName = "stopped";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
            
        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
            StatePlayer.isAttacking = false;
            StatePlayer.isHit = false;
        }

        public override void Initialize()
        {

        }

        public override void HandleMovement()
        {


        }

        public override void HandleDirection()
        {


        }

        public override void Update()
        {
            
        }

        public override void HandleState()
        {
            if (StatePlayer.isHit)
                StatePlayer.InternalState = new StateHit(this);
            else if (StatePlayer.KeysDown.Contains(KeyPressed.Up) || StatePlayer.KeysDown.Contains(KeyPressed.Down) || StatePlayer.KeysDown.Contains(KeyPressed.Right) || StatePlayer.KeysDown.Contains(KeyPressed.Left))
                StatePlayer.InternalState = new StateMoving(this);
            else if (StatePlayer.KeysDown.Contains(KeyPressed.Attack3))
                StatePlayer.InternalState = new StateJumping(this);
            else if (StatePlayer.KeysDown.Contains(KeyPressed.Attack0))
                StatePlayer.InternalState = new StateCasting(this);
        }

        public override void Translate(float x, float y)
        {


        }

        public override void HandleCollision(List<BoxingPlayer> Players)
        {
            foreach (BoxingPlayer p in Players)
            {
                if (StatePlayer.Hurtbox.Intersects(p.Hitbox) && p.isAttacking)
                {
                    StatePlayer.isHit = true;
                }
            }
        }

        
        
    }
}
