using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auction_Boxing_2
{

    class StateHit : State
    {
        const int State_Time = 20;
        int Counter;
        public StateHit(State state)
        {
            
            StateName = "hit";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
            
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
            Counter--;
        }

        public override void HandleState()
        {
            if (Counter <= 0)
                StatePlayer.InternalState = new StateStopped(this);
        }

        public override void Translate(float x, float y)
        {


        }

        public override void HandleCollision(List<BoxingPlayer> Players)
        {

        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
            StatePlayer.isAttacking = false;
            StatePlayer.isHit = false;
            Counter = State_Time;
             
        }

    }
}
