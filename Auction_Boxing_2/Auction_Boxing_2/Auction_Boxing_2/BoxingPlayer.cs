using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    

    class BoxingPlayer : IComparable<BoxingPlayer>
    {
        #region stats
        protected float maxhealth;

        public float MaxHealth
        {
            get
            {
                return maxhealth;
            }
            set
            {
                maxhealth = value;
            }
        }

        protected float maxstamina;

        public float MaxStamina
        {
            get
            {
                return maxstamina;
            }
            set
            {
                maxstamina = value;
            }
        }

        protected float maxmovement;

        public float MaxMovement
        {
            get
            {
                return maxmovement;
            }
            set
            {
                maxmovement = value;
            }
        }

        protected float maxcooldown;

        public float MaxCoolDown
        {
            get
            {
                return maxcooldown;
            }
            set
            {
                maxcooldown = value;
            }
        }

      
        #endregion

        #region some stuff

        int width;
        public int Width
        {
            get { return width; }
        }

        int height;
        public int Height
        {
            get { return height; }
        }

        public int playerindex;
        public bool isActive;
        public bool isAttacking;
        public bool isHit;

        
        #endregion

        #region important stuff
        // List of keys currently down
        List<KeyPressed> keysPressed = new List<KeyPressed>();

        public List<KeyPressed> KeysDown
        {
            get
            {
                return keysPressed;
            }


        }
        
        string sname;

        //Inputs
        public Input_Handler input;

        //Position Vector
        Vector3 position;

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

            }
        }

        //speed vector
        Vector3 speed;

        public Vector3 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        //hurtbox
        Rectangle hurtbox;

        public Rectangle Hurtbox
        {
            get
            {
                return hurtbox;
            }
            set
            {
                hurtbox = value;
            }
        }

        Rectangle hitbox;

        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }
       
        AnimationPlayer animplayer = new AnimationPlayer();
        
        public AnimationPlayer PlayerAnimationPlayer
        {
            get
            {
                return animplayer;
            }
        }

        protected State state;

        public State InternalState
        {
            set
            {
                state = value;
            }
            get
            {
                return state;
            }

        }

        protected Animation anim;
        
        public Animation PlayerAnimation
        {
            get
            {
                return anim;
            }
            set
            {
                anim = value;
            }

        }
        
        List<Texture2D> Textures = new List<Texture2D>();

        //animations attached to statenames
        public Dictionary<string, Animation> ATextures = new Dictionary<string, Animation>();

        SpriteEffects spriteEffect;

        public SpriteEffects PlayerEffect
        {
            get
            {
                return spriteEffect;
            }
            set
            {
                spriteEffect = value;
            }
        }
        #endregion

        public BoxingPlayer(float x, float y, string name, int playerindex, int width, int height, Input_Handler input)
        {
            this.width = width;
            this.height = height;
            this.input = input;
            this.isActive = input.isActive;
            
            this.playerindex = playerindex;
            this.sname = name;
            
            this.state = new StateMoving();

            maxhealth = Tools.BASE_HEALTH;
            maxcooldown = Tools.BASE_COOLDOWN;
            maxmovement = Tools.BASE_MOVEMENT;
            maxstamina = Tools.BASE_STAMINA;
  
            position = new Vector3(x, y, Tools.JUMP_HEIGHT);

            hurtbox = new Rectangle((int)position.X,(int)position.Y, width, height);
            
            spriteEffect = SpriteEffects.None;
            
       
        }

        public void update()
        {
            animplayer.PlayAnimation(state.PlayerAnimation);

            hurtbox = new Rectangle((int)position.X, (int)position.Y, width, height);
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);
            
            InternalState.Update();

            HandleState();
            
            handleMovement();
            
            handleDirection();
            
            
        }

        public void LoadContent(Dictionary<string, Animation> ATextures)
        {

            this.ATextures = ATextures;
            
            state.LoadState(this, ATextures);

            animplayer.PlayAnimation(state.PlayerAnimation);

            Hitbox = new Rectangle((int)Position.X + Width, (int)Position.Y, Width, Height);
            Debug.WriteLine("");
            input.OnKeyDown += HandleKeyDown;
            input.OnKeyRelease += HandleKeyRelease;
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(input.isActive)
                animplayer.Draw(gameTime, spriteBatch, Color.White, Tools.toVector2(position), 0, Vector2.Zero, 3f, spriteEffect);
                        

        }

        public void HandleKeyDown(int player_index, KeyPressed key)
        {
          if(!KeysDown.Contains(key))
              KeysDown.Add(key);
        }

        public void HandleKeyRelease(int player_index, KeyPressed key)
        {
            if (KeysDown.Contains(key))
                KeysDown.Remove(key);
        }

        public void HandleState()
        {
            state.HandleState();
        }

        public void handleDirection()
        {
            state.HandleDirection();
        }

        public void handleMovement()
        {
            state.HandleMovement();
        }

        public void handleCollision(List<BoxingPlayer> Players)
        {
            state.HandleCollision(Players);
        }
        public int CompareTo(BoxingPlayer player)
        {
            return this.Position.Y.CompareTo(player.Position.Y);
        }

    }
}
