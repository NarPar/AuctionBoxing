using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    enum CapeState
    {
        draw,
        hide,
        travel,
        reveal
    }
    class StateCape : State
    {
        CapeState state;
        bool exiting = false;

        float speed = 500;

        BoxingPlayer targetPlayer;
        float target;
        int direction; // moves backwards towards target
        int itemIndex;

        public StateCape(int itemIndex, BoxingPlayer player)
            : base(player, "Cape")
        {
            this.itemIndex = itemIndex;
            state = CapeState.draw;
            Debug.WriteLine("In cape!");
            
        }

        public override void Update(GameTime gameTime)
        {
            if (player.isFreeingCape)
                ChangeState(new StateCapeStuck(itemIndex, player));

            switch (state)
            {
                case(CapeState.draw):
                    if (player.sprite.FrameIndex == 9)
                        state = CapeState.hide;
                    break;
                case(CapeState.hide):
                     // Hold to continue hiding
                    if (player.IsKeyDown(KeyPressed.Kick))
                    {
                        player.sprite.FrameIndex = 9;

                        bool behind = false;

                        if (player.IsKeyDown(KeyPressed.Left))
                        {
                            int dir = player.direction;
                            if(player.direction == 1)
                                dir = -1;
                            targetPlayer = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 2 * player.GetHeight / 3, dir);
                            if(targetPlayer != null)
                                target = targetPlayer.position.X - 18 * BoxingPlayer.Scale;

                            direction = dir;
                        }
                        else if(player.IsKeyDown(KeyPressed.Right))
                        {
                            int dir = player.direction;
                            if(player.direction == -1)
                                dir = 1;
                            targetPlayer = player.BoxingManager.GetPlayerInFront(player, player.position.Y - 2 * player.GetHeight / 3, dir);
                            if (targetPlayer != null)
                                target = targetPlayer.position.X + 18 * BoxingPlayer.Scale;

                            direction = dir;
                        }
                        if (targetPlayer != null)
                        {
                            
                            state = CapeState.travel;
                            player.ChangeDirection(direction * -1);
                        }
                    }
                    else if (!exiting)
                    {
                        player.sprite.FrameIndex = 16;
                        state = CapeState.reveal;
                    }
                    break;
                case(CapeState.reveal):
                    if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                    {
                        ChangeState(new StateStopped(player));
                        player.isFreeingCape = true;
                    }
                    break;
                case(CapeState.travel):

                    if (player.sprite.FrameIndex == 15)
                        player.sprite.FrameIndex = 13;

                    player.position.X += (float)(direction * speed * gameTime.ElapsedGameTime.TotalSeconds);
                    float dif = Math.Abs(player.position.X - target);
                    // You're behind'm!
                    if (dif < 5 && dif > 0 
                        || player.position.X - player.GetWidth / 2 <= player.BoxingManager.bounds.X
                        || player.position.X + player.GetWidth / 2 >= player.BoxingManager.bounds.X + player.BoxingManager.bounds.Width 
                        )
                        state = CapeState.reveal;

                    break;
            }
        }

    }
}
