using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2.Boxing.PlayerStates
{
    class StateBowlerHatReThrow : State
    {
        int THROWN_INDEX = 6;
        bool thrownAgain = false;
        public bool caught = false;
        bool hatRemoved = false;
        BowlerHatInstance hat;
        float rethrowIncreaseFactor = .2f;

        KeyPressed itemButton;

        public StateBowlerHatReThrow(int itemIndex, BoxingPlayer player, KeyPressed key)
            : base(player, "BowlerRethrow")
        {
            canCatch = true;

            itemButton = key;
        }

        public override void Update(GameTime gameTime)
        {
            // Got the hat? Get rid of it and make a new one
            // when we're at the throw frame
            if (hat != null && !hatRemoved)
            {
                player.BoxingManager.removeBowlerHat(hat);
                hatRemoved = true;


                player.hasThrownBowlerHat = false;
                caught = true;

                player.numBowlerReThrows += rethrowIncreaseFactor;
            }

            // Waiting for the hat to return?
            if (!thrownAgain && !caught)
            {
                // Hold your hand out!
                if (player.IsKeyDown(itemButton))
                {
                    //Debug.WriteLine("WAITING TO RETHROW! " + caught);
                    player.sprite.FrameIndex = 0;
                }
                else
                    // Or stop waiting.
                    ChangeState(new StateStopped(player));
            }
            else if (!thrownAgain)
            {
                if (player.sprite.FrameIndex == THROWN_INDEX) // !hasThrown
                {
                    if (!hasPlayedSound)
                        PlaySound(player.soundEffects["BowlerThrow"]); // play the sound effect!
                    // create hat projectile
                    player.BoxingManager.addBowlerHat(player, player.numBowlerReThrows);
                    player.hasThrownBowlerHat = true;
                    thrownAgain = true;
                }
            }
            else
            {
                if (player.sprite.FrameIndex == player.animations[key].FrameCount - 1)
                {
                    ChangeState(new StateStopped(player));
                }
            }

            base.Update(gameTime);

        }

        public void CatchHat(BowlerHatInstance hat)
        {
            this.hat = hat;
        }

        
    }
}
