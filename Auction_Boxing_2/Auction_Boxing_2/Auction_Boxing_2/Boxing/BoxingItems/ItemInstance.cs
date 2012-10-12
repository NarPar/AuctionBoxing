using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Auction_Boxing_2
{

    #region Parent Class


    public class ItemInstance
    {

        #region Fields

        //public Item item;
        //public int playerId;
        public BoxingPlayer player;

        public Vector2 position;

        public Rectangle hitbox;
        public Rectangle sprite_position;

        protected AnimationPlayer sprite;
        protected Animation animation;

        protected SpriteEffects effect;

        public float damage;

        public bool Finished;

        public bool isEffect;

        #endregion

        #region Initialize


        public ItemInstance(BoxingPlayer player, bool isEffect, Vector2 position)// Item item, SpriteEffects effect, Vector3 position)
        {
            this.player = player;
            this.isEffect = isEffect;
            //this.item = item;
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


    #region BowlerHatInstance


    public class BowlerHatInstance : ItemInstance
    {
        int width = 6, height = 4;
        int scale = 4;

        float xStart;
        float xCurrent;
        float speed = 250;
        float range = 175;
        

        bool isReturning = false;

        Texture2D texture;

        public BowlerHatInstance(BoxingPlayer player, Vector2 position, int direction) ://Item item, Texture2D texture, Vector3 position, int playerId, SpriteEffects effect) :
            base(player, false, position)
        {

            hitbox = new Rectangle((int)position.X, (int)position.Y, width * 4, height * 4);

            xStart = position.X;

            //animation = new Animation(texture, .05f, false, 30);
            //sprite.PlayAnimation(animation);

            if (direction == -1)
            {
                speed *= -1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!isReturning)
            {
                position.X += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

                // If we've gone our range, return to sender.
                if (Math.Abs(position.X - xStart) >= range)
                {
                    speed *= -1;
                    isReturning = true;
                }
            }
            else
            {
                if ((player.position - position).Length() <= speed)
                {
                    Finished = true;
                }
                else
                {
                    // Move the hat towards sender
                    Vector2 dir = (player.position - position);
                    dir.Normalize();

                    position += dir * speed;
                }


            }
            //for vertical, change to vector2.Distance(start, position);




            hitbox.X = (int)position.X - width / 2;
            hitbox.Y = (int)position.Y - height;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, Color.White);
            //sprite.Draw(gameTime, spriteBatch, sprite_position,
               // 0, Color.White, Vector2.Zero, effect);

            base.Draw(gameTime, spriteBatch);
        }

    }
    
    #endregion

    
    /*
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
