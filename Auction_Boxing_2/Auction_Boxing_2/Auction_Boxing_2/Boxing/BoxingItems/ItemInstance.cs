using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Auction_Boxing_2
{

    /*#region Parent Class


    public class ItemInstance
    {

        #region Fields

        public Item item;
        public int playerId;

        public Vector3 position;

        public Rectangle hitbox;
        public Rectangle sprite_position;

        protected AnimationPlayer sprite;
        protected Animation animation;

        protected SpriteEffects effect;

        public float damage;

        public bool end;

        public bool isEffect;

        #endregion

        #region Initialize


        public ItemInstance(bool isEffect, Item item, int playerId, SpriteEffects effect, Vector3 position)
        {
            this.isEffect = isEffect;
            this.item = item;
            this.playerId = playerId;
            this.effect = effect;
            this.position = position;
            this.damage = damage;
        }


        #endregion

        #region Update


        public virtual void Update(GameTime gameTime)
        {
            
        }


        #endregion

        #region Draw


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }


        #endregion 
    }


    #endregion

    #region CaneInstance


    public class CaneInstance : ItemInstance
    {

        public CaneInstance(Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(false, item, playerId, effect, position)
        {
            int width = 44;
            int height = 70;

            int position_offset = -40;
            if (effect == SpriteEffects.FlipHorizontally)
            {
                position_offset *= -1;
            }

            sprite_position = new Rectangle((int)position.X + position_offset, (int)position.Y, width, height);
            hitbox = new Rectangle((int)position.X + position_offset - width/2, (int)position.Y - height, width, height);

            animation = new Animation(texture, .05f, false, 44);
            sprite.PlayAnimation(animation);
        }

        public override void Update(GameTime gameTime)
        {
            if (sprite.FrameIndex >= sprite.Animation.FrameCount - 1)
                end = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, sprite_position,
                0, Color.White, Vector2.Zero, effect);
            

            base.Draw(gameTime, spriteBatch);
        }

    }


    #endregion

    #region BowlerHatInstance


    public class BowlerHatInstance : ItemInstance
    {
        int width;

        float xStart;
        float xCurrent;
        float speed = 250;
        float range = 175;
        bool isLeft;

        public BowlerHatInstance(Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(false,item, playerId, effect, position)
        {
            width = 30;
            int height = 22;
            int vertical_offset = 30;

            

            sprite_position = new Rectangle((int)position.X, (int)position.Y - vertical_offset, width, height);
            hitbox = new Rectangle((int)position.X - width / 2, (int)position.Y - vertical_offset - height, width, height);

            position.Y += 10;

            animation = new Animation(texture, .05f, false, 30);
            sprite.PlayAnimation(animation);

            xStart = position.X;
            xCurrent = xStart;

            if (effect == SpriteEffects.None)
            {
                isLeft = true;
                speed *= -1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            xCurrent += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            if (Math.Abs(xCurrent - xStart) >= range)
            {
                speed *= -1;
            }

            if ((isLeft && xCurrent > xStart) || (!isLeft && xCurrent < xStart))
                end = true;

            hitbox.X = (int)xCurrent - width / 2;
            sprite_position.X = (int)xCurrent;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, sprite_position,
                0, Color.White, Vector2.Zero, effect);

            base.Draw(gameTime, spriteBatch);
        }

    }
    
    #endregion

    #region RevolverInstance


    public class RevolverInstance : ItemInstance
    {

        int width;

        float xStart;
        float xCurrent;
        float speed = 1600;
        float range = 800;
        bool isLeft;

        public RevolverInstance(Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(false, item, playerId, effect, position)
        {
            width = 30;
            int height = 22;
            int vertical_offset = 30;

            sprite_position = new Rectangle((int)position.X, (int)position.Y - vertical_offset, width, height);
            hitbox = new Rectangle((int)position.X - width / 2, (int)position.Y - vertical_offset - height, width, height);

            position.Y += 20;

            animation = new Animation(texture, .05f, false, 7);
            sprite.PlayAnimation(animation);

            xStart = position.X;
            xCurrent = xStart;

            if (effect == SpriteEffects.None)
            {
                isLeft = true;
                speed *= -1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            xCurrent += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            if (Math.Abs(xCurrent - xStart) >= range)
            {
                end = true;
            }

            hitbox.X = (int)xCurrent - width / 2;
            sprite_position.X = (int)xCurrent;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, sprite_position,
                0, Color.White, Vector2.Zero, effect);

            base.Draw(gameTime, spriteBatch);
        }

    }

    #endregion

    #region MonocleInstance


    public class MonocleInstance : ItemInstance
    {

        public MonocleInstance(Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(false, item, playerId, effect, position)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, 44, 70);

            animation = new Animation(texture, .05f, false, 44);
            sprite.PlayAnimation(animation);
        }

        public override void Update(GameTime gameTime)
        {
            if (sprite.FrameIndex >= sprite.Animation.FrameCount)
                end = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, hitbox, 0, Color.White, Vector2.Zero, effect);

            base.Draw(gameTime, spriteBatch);
        }

    }

    #endregion

    #region BootsInstance


    public class BootsInstance : ItemInstance
    {

        public BootsInstance(Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(true, item, playerId, effect, position)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, 44, 70);

            animation = new Animation(texture, .05f, false, 25);
            sprite.PlayAnimation(animation);
        }

        public override void Update(GameTime gameTime)
        {
            if (sprite.FrameIndex >= sprite.Animation.FrameCount - 1)
                end = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, hitbox, 0, Color.White, Vector2.Zero, effect);

            base.Draw(gameTime, spriteBatch);
        }

    }

    #endregion*/
}
