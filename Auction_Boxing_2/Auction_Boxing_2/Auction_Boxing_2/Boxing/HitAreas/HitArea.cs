using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Auction_Boxing_2
{
    public abstract class HitArea
    {
        protected BoxingPlayer Player;

        protected Vector2 position;

        public bool Enabled
        {
            get { return Enabled; }
            set { Enabled = value; }
        }


        protected Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
    }
}
