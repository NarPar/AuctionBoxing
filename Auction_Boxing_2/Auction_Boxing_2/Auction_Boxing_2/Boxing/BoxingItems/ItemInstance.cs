using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Auction_Boxing_2.Boxing.PlayerStates;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    #region Parent Class

    abstract public class ItemInstance
    {
        #region Fields

        //public Item item;
        //public int playerId;
        public int id;
        public BoxingPlayer player;

        public Vector2 position;
        public int direction;

        public Rectangle hitbox;
        public Rectangle sprite_position;

        // Pixel collision fields
        float rotation = 0;

        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }
        public Vector2 Origin
        {
            get { return sprite.Origin; } // i believe the origin is at the bottom center.
        }



        public AnimationPlayer sprite;
        protected Animation animation;

        protected SpriteEffects effect;


        public float speed = 500;
        public float damage;

        public int moveDirection;
        public bool Finished;
        public int noCatch = 0;
        public bool isEffect;

        #endregion

        #region Initialize

        public ItemInstance(BoxingPlayer player, bool isEffect, int id)
        {
            this.player = player;
            this.isEffect = isEffect;
            this.id = id;
        }

        #endregion

        #region Update


        public virtual void Update(GameTime gameTime)
        {
            // Update the matrix transform for pixel collision
            if (direction == 1)
            {
                transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                            Matrix.CreateScale(player.scales) *
                            Matrix.CreateRotationZ(rotation) *
                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
            else
            {
                transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                            Matrix.CreateScale(player.scales) *
                            Matrix.CreateRotationZ(rotation) *
                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
        
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

        
        double speedFactor = 1;
        float scale;

        Vector2 hatOffset = new Vector2(16f, -30.5f);
        Vector2 leftHatOffset = new Vector2(-16f, -30.5f);

        bool isReturning = false;

        Animation anim;

        public BowlerHatInstance(BoxingPlayer player, Dictionary<string, Animation> animations, int id, float speedFactor) :
            base(player, false, id)
        {
            direction = player.direction;
            moveDirection = player.direction;
            damage = 1;

            speed *= speedFactor;
            Debug.WriteLine("Speed = " + speed);

            scale = BoxingPlayer.Scale;
            hatOffset *= scale;
            leftHatOffset *= scale;
            if (direction == -1)
            {
                speed *= -1;
                position = Vector2.Add(player.position, leftHatOffset);
            }
            else
            {
                position = Vector2.Add(player.position, hatOffset);
            }

            hitbox = new Rectangle((int)position.X, (int)position.Y, (int) (width * scale), (int) (height * scale));

            anim = animations["bowlerHat"];
            sprite.PlayAnimation(anim);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 playerpos = player.position;
            Vector2 hatpos = position;
            if (player.direction == -1)
            {
                hatpos -= leftHatOffset;
            }
            else
            {
                hatpos -= hatOffset;
            }
            Vector2 diff;
            if (direction == 1)
            {
                diff = playerpos - hatpos;
            }
            else
            {
                diff = hatpos - playerpos;
            }
            float distance = diff.Length();
            if (distance > 3 || noCatch++ < 2)
            {
                if (!isReturning)
                { // go go go
                    position.X += (float)(speed * speedFactor * gameTime.ElapsedGameTime.TotalSeconds);
                    speedFactor -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (speedFactor < 0)
                    {
                        speedFactor = 0;
                        isReturning = true;
                        moveDirection *= -1;
                    }
                }
                else // return to sender
                {
                    
                    speedFactor += gameTime.ElapsedGameTime.TotalSeconds;
                    if (speedFactor > 1)
                    {
                        speedFactor = 1;
                    }
                    diff.Normalize();
                    position += diff * (float)(speed * speedFactor * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            else // catch dat hat
            {
                if (player.state.canCatch)
                {
                    // Is the player waiting to rethrow?
                    if (player.state is StateBowlerHatReThrow)
                    {
                        StateBowlerHatReThrow s = player.state as StateBowlerHatReThrow;
                        s.CatchHat(this);
                        Finished = true;
                    }
                    else
                    {
                        Finished = true;
                        player.state.ChangeState(new StateBowlerHatCatch(player, this, player.state));
                    }
                }
            }
            //for vertical, change to vector2.Distance(start, position);

            hitbox.X = (int)position.X - width / 2;
            hitbox.Y = (int)position.Y - height;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.Texture, hitbox, Color.White);
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
