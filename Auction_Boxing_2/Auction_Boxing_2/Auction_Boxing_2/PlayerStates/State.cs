using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Auction_Boxing_2
{
    abstract class State
    {

        protected Texture2D Texture;
        protected BoxingPlayer player;
        protected bool isActive;
        protected Animation playeranimation;
        protected Dictionary<string, Animation> ATextures;
        protected Vector2 speed;
        protected Color color;
        public Animation PlayerAnimation
        {
            get
            {
                return playeranimation;
            }
            set
            {
                 playeranimation = value;
            }
        }
        
        public BoxingPlayer StatePlayer
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        protected string StateName;

        public abstract void Initialize();
        public abstract void HandleMovement();
        public abstract void HandleDirection();
        public abstract void Update();
        public abstract void HandleState();
        public abstract void HandleCollision(List<BoxingPlayer> Players);
        public abstract void Translate(float x, float y);
        public abstract void LoadState(BoxingPlayer player, Dictionary<string,Animation> ATextures);
        

        








    }
}
