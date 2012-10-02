using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Auction_Boxing_2
{
    /*public class HitBox : HitArea
    {
        protected Rectangle hitbox;

        public int X { get { return hitbox.X; } set { hitbox.X = value; } }
        public int Y { get { return hitbox.Y; } set { hitbox.Y = value; } }
        public int Width { get { return hitbox.Width; } set { hitbox.Width = value; } }
        public int Height { get { return hitbox.Height; } set { hitbox.Height = value; } }

        public bool Moving
        {
            get { return (Velocity.X == 0) && (Velocity.Y == 0); }
        }

        public bool isDebug { get; set; }
        public bool Enabled { get; set; }

        public Vector2 PlayerPosition;

        DirectionType Direction;

        Texture2D Debug;



        public HitBox(float x, float y, float width, float height, BoxingPlayer Player, bool Enabled)
        {
            this.hitbox = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.Velocity = new Vector2();
            this.PlayerPosition = new Vector2(x, y);
            this.Enabled = Enabled;
            Velocity = Vector2.Zero;
            this.Player = Player;
        }




        public void LoadContent(ContentManager Content)
        {
            Debug = Content.Load<Texture2D>("White");
            Direction = DirectionType.None;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Enabled)
                spriteBatch.Draw(Debug, hitbox, Debug.Bounds, Color.White, 0, new Vector2(Debug.Width / 2.0f, Debug.Height), SpriteEffects.None, 1);//Debug, hitbox, Color.White *.3f);
        }

        public void HandleDirection(SpriteEffects Effect)
        {
            if (Effect == SpriteEffects.None)
                Direction = DirectionType.Right;
            if (Effect == SpriteEffects.FlipHorizontally)
                Direction = DirectionType.Left;
        }

        public void Update()
        {
            if (Direction == DirectionType.Right)
                hitbox = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, Width, Height);
            if (Direction == DirectionType.Left)
                hitbox = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, Width, Height);

        }

        public bool Intersects(Rectangle hurtbox)
        {
            if (hitbox.Intersects(hurtbox))
                return true;
            else
                return false;
        }

        public void Translate(float x, float y)
        {
            hitbox.X += (int)x;
            hitbox.Y += (int)y;

        }

        public void Translate(int x, int y)
        {
            hitbox.X += x;
            hitbox.Y += y;

        }



    }*/
}
