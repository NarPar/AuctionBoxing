using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2
{
    enum DirectionType
    {
        Right,
        Left,
        Up

    }
    class StateJumping : State
    {
        
        int Counter;
        const int JumpInterval = 3;
        const int CounterConst = 20;
        bool hasJumped;
        DirectionType Direction;

        public StateJumping(State state)
        {
            
            StateName = "jumping";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
           
        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
            StatePlayer.isAttacking = false;
            StatePlayer.isHit = false;
            if (StatePlayer.KeysDown.Contains(KeyPressed.Left) && !StatePlayer.KeysDown.Contains(KeyPressed.Right))
            {
                Direction = DirectionType.Left;
            }
            else if (StatePlayer.KeysDown.Contains(KeyPressed.Right) && !StatePlayer.KeysDown.Contains(KeyPressed.Left))
            {
                Direction = DirectionType.Right;
            }
            else
            {
                Direction = DirectionType.Up;
            }

            Counter = -CounterConst;
            hasJumped = false; 
        }

        public override void Initialize()
        {

        }

        public override void HandleMovement()
        {
            switch(Direction)
            {
                case DirectionType.Right:
                    if (Counter < 0)
                        Translate(JumpInterval, -JumpInterval);
                    else if (Counter > 0 && Counter < CounterConst)
                        Translate(JumpInterval, JumpInterval);
                break;

                case DirectionType.Left:
                if (Counter < 0)
                    Translate(-JumpInterval, -JumpInterval);
                else if (Counter > 0 && Counter < CounterConst)
                    Translate(-JumpInterval, JumpInterval);
                break;

                case DirectionType.Up:
                    if (Counter < 0)
                        Translate(0, -JumpInterval);
                    else if (Counter > 0 && Counter < CounterConst)
                        Translate(0, JumpInterval);
                break;
            }
        }

        public override void HandleDirection()
        {
            
        }

        public override void Update()
        {
            Debug.WriteLine("Counter = " + Counter);
            if (Counter > CounterConst)
                hasJumped = true;
            else
                Counter++;

        }

        public override void HandleState()
        {
            if (hasJumped)
                StatePlayer.InternalState = new StateStopped(this);
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
            
        }
    }
}
