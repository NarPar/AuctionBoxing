using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Auction_Boxing_2
{
    public abstract class Game_State
    {
        protected Game game;
        protected Input_Handler[] inputs;
        protected Rectangle bounds;

        public Game_State(Game game, Input_Handler[] inputs, Rectangle bounds)
        {
            this.game = game;
            this.inputs = inputs;
            this.bounds = bounds;
        }

        public virtual void Update(GameTime gameTime)
        {

        }



        protected virtual void ChangeState(Game_State state)
        {
            game.currentState = state;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
