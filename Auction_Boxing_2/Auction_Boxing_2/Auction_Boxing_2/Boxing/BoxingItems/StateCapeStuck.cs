using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    enum CapeStuckState
    {
        draw,
        stuck,
        free
    }
    class StateCapeStuck : State
    {
        CapeStuckState state;
        int count = 0; // how many tugs of the cape?

        public StateCapeStuck(int itemIndex, BoxingPlayer player)
            : base(player, "CapeStuck")
        {
          
            state = CapeStuckState.draw;
        }

        public override void Update(GameTime gameTime)
        {

            switch (state)
            {
                case (CapeStuckState.draw):
                    if (player.sprite.FrameIndex == 5)
                        state = CapeStuckState.stuck;
                    break;
                case (CapeStuckState.stuck):
                    // Hold to continue hiding
                    //if (player.IsKeyDown(KeyPressed.Attack))
                    //{
                        if (player.sprite.FrameIndex == 8)
                        {
                            player.sprite.FrameIndex = 6;
                            count++;
                        }

                        // You're free!
                        if (count >= 3)
                        {
                            state = CapeStuckState.free;
                            player.isFreeingCape = false;
                        }

                        
                    //}

                    break;
                case (CapeStuckState.free):
                    if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                    {
                        ChangeState(new StateStopped(player));
                    }
                    break;

                
            }
            base.Update(gameTime);
        }

    }
}
