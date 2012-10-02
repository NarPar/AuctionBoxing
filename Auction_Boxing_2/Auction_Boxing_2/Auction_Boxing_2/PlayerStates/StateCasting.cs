using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2 
{
    class StateCasting : State
    {
        DirectionType Direction;

        public StateCasting(State state)
        {
            StateName = "casting";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
            

        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
            StatePlayer.isAttacking = true;

           // if (StatePlayer.PlayerEffect == SpriteEffects.None)
             //   StatePlayer.Hitbox = new Rectangle((int)StatePlayer.Position.X,(int)StatePlayer.Position.Y,Tools.WIDTH, Tools.HEIGHT);
            
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
            else if (!StatePlayer.KeysDown.Contains(KeyPressed.Attack0))
                StatePlayer.InternalState = new StateMoving(this);
            else {}
        }

        public override void Translate(float x, float y)
        {


        }

        public override void HandleCollision(List<BoxingPlayer> Players)
        {
            
        }
        
    }
}
