using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2
{
    
    class StateMoving : State
    {

        public StateMoving()
        {
            StateName = "moving";
            
        }

        public StateMoving(State state)
        {
            StateName = "moving";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
            Debug.WriteLine("State = " + StateName);
        }

        public override void LoadState(BoxingPlayer player, Dictionary<string,Animation> ATextures)
        {
            this.StatePlayer = player;
            this.ATextures = ATextures;
            this.PlayerAnimation = ATextures[StateName];
            this.speed = new Vector2(Tools.speedCalc(StatePlayer.MaxMovement), Tools.Y_SCALE * Tools.speedCalc(StatePlayer.MaxMovement));
            StatePlayer.isAttacking = false;
            StatePlayer.isHit = false;

        }
        public override void Initialize()
        {
            
            
            
        }

        public override void Update()
        {
            StatePlayer.Hurtbox = new Rectangle((int)StatePlayer.Position.X, (int)StatePlayer.Position.Y, Tools.WIDTH, Tools.HEIGHT);

        }

        public override void HandleMovement()
        {
            
            if (StatePlayer.KeysDown.Contains(KeyPressed.Up) && !StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Left))
                Translate(0, -speed.Y);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Down) && !StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Left))
                Translate(0, speed.Y);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Up) && !StatePlayer.KeysDown.Contains(KeyPressed.Down))
                Translate(speed.X, 0);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Left) && !StatePlayer.KeysDown.Contains(KeyPressed.Up) && !StatePlayer.KeysDown.Contains(KeyPressed.Down))
                Translate(-speed.X, 0);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Up) && StatePlayer.KeysDown.Contains(KeyPressed.Right))
                Translate(speed.X, -speed.Y);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Up) && StatePlayer.KeysDown.Contains(KeyPressed.Left))   
                Translate(-speed.X, -speed.Y);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Down) && StatePlayer.KeysDown.Contains(KeyPressed.Right))
                Translate(speed.X, speed.Y);

            if (StatePlayer.KeysDown.Contains(KeyPressed.Down) && StatePlayer.KeysDown.Contains(KeyPressed.Left))
                Translate(-speed.X, speed.Y);
            
        }

        public override void HandleDirection()
        {
            
            if (StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Left))
                StatePlayer.PlayerEffect = SpriteEffects.None;
            if (StatePlayer.KeysDown.Contains(KeyPressed.Left) && !StatePlayer.KeysDown.Contains(KeyPressed.Right))
                StatePlayer.PlayerEffect = SpriteEffects.FlipHorizontally;
        
             
        }

        public override void HandleState()
        {
            if (StatePlayer.isHit)
                StatePlayer.InternalState = new StateHit(this);

            else if (StatePlayer.KeysDown.Contains(KeyPressed.Attack3))
                StatePlayer.InternalState = new StateJumping(this);

            else if (!StatePlayer.KeysDown.Contains(KeyPressed.Up) && !StatePlayer.KeysDown.Contains(KeyPressed.Down) && !StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Left))
                StatePlayer.InternalState = new StateStopped(this);
            
            else if (StatePlayer.KeysDown.Contains(KeyPressed.Attack0))
                StatePlayer.InternalState = new StateCasting(this);
        }
        public override void Translate(float x, float y)
        {
            Vector3 Temp = StatePlayer.Position;
            Temp.X += x;
            Temp.Y += y;
            StatePlayer.Position = Temp;

        }
        public override void HandleCollision(List<BoxingPlayer> Players)
        {
            foreach (BoxingPlayer p in Players)
            {
                if (p.Hitbox.Intersects(StatePlayer.Hurtbox) && p.isAttacking)
                {
                    StatePlayer.isHit = true;
                }
            }
        }
    }
}
