using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auction_Boxing_2 
{
    class CastingState : State
    {
        public CastingState(State state)
        {
            StateName = "casting";
            this.ATextures = state.StatePlayer.ATextures;
            LoadState(state.StatePlayer, ATextures);
        }

        public override void LoadState(Player player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
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
            HandleState();
        }

        public override void HandleState()
        {
            if (!StatePlayer.KeysDown.Contains(KeyPressed.Attack0))
                StatePlayer.InternalState = new StateMoving(this);

        }

        public override void Translate(float x, float y)
        {


        }

        
    }
}
