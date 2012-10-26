using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBowlerHatThrow : State
    {
        float holdTimer = 0;
        float MAX_HOLD_TIME = .16f;

        bool buttonHeld = true;
        int THROWN_INDEX = 9;

        //bool hatThrown = false;
        bool noHat = false;

        int itemIndex;
        KeyPressed itemButton;

        public StateBowlerHatThrow(int itemIndex, BoxingPlayer player, KeyPressed key)
            : base(player, "BowlerThrow")
        {
            if (player.hasThrownBowlerHat)
                noHat = true;
                //ChangeState(new StateBowlerHatReThrow(itemIndex, player)); // Wait to recieve!
            holdTimer = MAX_HOLD_TIME;

            this.itemButton = key;
        }

        public override void Update(GameTime gameTime)
        {
            if (!player.hasThrownBowlerHat && player.sprite.FrameIndex == THROWN_INDEX) // !hasThrown
            {
                // create hat projectile
                player.BoxingManager.addBowlerHat(player, player.numBowlerReThrows);
                player.hasThrownBowlerHat = true;
            }
            else if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
            {
                ChangeState(new StateStopped(player));
            }

            

            if (holdTimer > 0)
                holdTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!player.IsKeyDown(itemButton))
            {
                buttonHeld = false;
                //Debug.WriteLine("Button OFf!");
            }

            if (buttonHeld && holdTimer <= 0)
                ChangeState(new StateBowlerHatReThrow(itemIndex, player, itemButton));
            else if (!buttonHeld && holdTimer <=0 && noHat)
                ChangeState(new StateStopped(player));

            base.Update(gameTime);
        }
    }
}
