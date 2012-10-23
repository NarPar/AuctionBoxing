using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Auction_Boxing_2.Boxing.PlayerStates;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    enum PlayerDirection
    {
        Right,
        Left
    }

    public class BoxingPlayer : IComparable<BoxingPlayer>
    {
        public static int Scale = 2;
        

        public int GetWidth
        {
            get { return (int)(15 * Scale); }
        }

        public int GetHeight
        {
            get { return (int)(38 * Scale); }
        }

        #region stats

        // the health everyone starts with without item mods.
        public float baseHealth = 100;

        // the dimensions of the health bar displayed below the player
        public Rectangle rHealthBar;
        public int healthBarMaxWidth;

        protected float maxhealth;

        public float MaxHealth
        {
            get
            {
                return baseHealth; //+ any mods!;
            }
            set
            {
                baseHealth = value;
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
            get { return maxcooldown; }
            set { maxcooldown = value; }
        }

        protected float currenthealth;

        public float CurrentHealth
        {
            get { return currenthealth; }
            set
            {
                currenthealth = value >= 0 ? value : 0;

                // check if dead
                if (!isDead && currenthealth <= 0)
                {
                    isDead = true;
                    BoxingManager.NotifyPlayerDeath(playerIndex);
                }

                // update health bar
                rHealthBar.Width = (int)(healthBarMaxWidth * (currenthealth / MaxHealth));
            }
        }

        protected float currentstamina;

        public float CurrentStamina
        {
            get { return currentstamina; }
            set { currentstamina = value; }
        }

        public float CurrentCooldown
        {
            get { return CurrentCooldown; }
            set { CurrentCooldown = value; }
        }

        #endregion

        #region some stuff

        public int playerIndex;
        public bool isHit;

        const int TimerInterval = 190;
        const int WaitTimerInterval = 8;


        #endregion

        #region Input/Position/move Stuff
        // List of keys currently down
        List<KeyPressed> keysPressed = new List<KeyPressed>();

        public List<KeyPressed> KeysDown
        {
            get
            {
                return keysPressed;
            }
        }

        //Inputs
        public Input_Handler input;

        //Position Vector
        public Vector2 position;

        //speed vector
        Vector2 speed;

        public Vector2 Speed
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

        // if its right, its +1, left is -1
        public int direction;

        //hurtbox
        public Rectangle hurtbox;

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

        // for flipping the player 
        SpriteEffects spriteEffect;

        public float dbleTapTimer;
        public float dbleTapTime = .4f;
        public KeyPressed prevKey;
        public int dbleTapCounter = 0;

        public GameTimer DblJumpTimer;

        public GameTimer DashTimerRight;

        public GameTimer DashTimerLeft;

        //public List<BoxingItem> Items = new List<BoxingItem>();

        //public Item[] equippedItems = new Item[4];



        //public delegate void UseItemEventHandler(ItemInstance item);

        //public event UseItemEventHandler OnUseItem;

        
        Texture2D blank;

        Color color;

        #endregion

        public float GetGroundLevel
        {
            get { return levellevel; }
        }
        public Rectangle platform;

        public float currentVerticalSpeed = 0;
        public float currentHorizontalSpeed = 0;

        Vector2 startPosition;

        public Boxing_Manager BoxingManager;

        public bool isFalling = false;
        public float levellevel;
        public bool isCaught = false; // can't do anything (getting pulled by cane)
        public float numBowlerReThrows = 1;
        public bool isFreeingCape = false;


        #region Items

        //public Item[] items = new Item[4];
        public bool isReloadingRevolver = false;
        public bool hasThrownBowlerHat = false;

        bool[] items = new bool[4];

        // an array of size 4 containing the animations of the items.
        //public Dictionary<string, Animation>[] itemAnimations = new Dictionary<string, Animation>[4];

        #endregion

        #region Animation

        public string currentAnimationKey;

        public AnimationPlayer sprite = new AnimationPlayer();

        //animations attached to statenames
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        // Haven't implemented. If player is attacking, they get draw last so they appear ontop of other person.
        public short drawPriority = 0;

        #endregion

        #region State Control

        // current state decides what the player is doing at any given time
        // probably the most important part of the player
        public State state;

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

        #endregion

        #region PerPixel

        // To handle per pixel collision while scaling sprites, we'll need to use a matrix to hold the position

        public bool isAttacking
        {
            get { return state.isAttack; }//return (state is StatePunch || state is StateCaneBonk); }
        }

        public Vector3 scales = new Vector3(Scale, Scale, 1); // maybe z should be 0?
        float rotation = 0;
        private Matrix transform;

        public Matrix Transform
        {
            get { return transform; }
        }


        //--------------old variables-----------

        public Vector2 Origin
        {
            get { return sprite.Origin; } // i believe the origin is at the bottom center.
        }

        // How to do flipping?!

        public Matrix TransformMatrix
        {
            get
            {
                return 
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateScale(scales) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
        }

        // This rectangle is for preliminary checking. If two players collision rectangles collide,
        // then we check the pixels.
        public Rectangle BoundingRectangle
        {
            get
            {
                int width = (int)(animations[currentAnimationKey].FrameWidth * Scale);
                int height = (int)(animations[currentAnimationKey].FrameHeight *Scale);

                return new Rectangle((int)position.X - width / 2, (int)position.Y - height,
                    width, height);
            }
        }

        public Rectangle NonScaledBoundingRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    animations[currentAnimationKey].FrameWidth, animations[currentAnimationKey].FrameHeight);

            }

        }

        public Rectangle CollisionRectangle;

        // Get the color data of the current frame of the current animation
        public Color[] Get1DColorData
        {
            get { return sprite.Get1DColorDataArray(); }
        }

        #endregion

        #region ComboControl

        List<KeyPressed> comboKeys = new List<KeyPressed>();

        float comboTimer = 0.0f;
        float comboTime = .5f; // the time between keys

        KeyPressed[,] combinations = new KeyPressed[4,3];

        #endregion

        public bool isDead;

        public bool isBumping = false;

        public BoxingPlayer(Boxing_Manager bm, int playerIndex, Vector2 startPosition, Dictionary<string, Animation> animations, Input_Handler input, Color color,
            Texture2D blank, Rectangle healthBar, Rectangle platform)//items)
        {
            this.BoxingManager = bm;

            // To test the accuracy of the collision rect
            this.blank = blank;

            this.playerIndex = playerIndex;

            this.animations = animations;

            this.input = input;

            this.color = color; 

            this.startPosition = startPosition;
            this.platform = platform;
            levellevel = platform.Y;

            this.rHealthBar = healthBar;
            healthBarMaxWidth = healthBar.Width;

            // Listen for input
            input.OnKeyDown += HandleKeyDown;
            input.OnKeyRelease += HandleKeyRelease;

            // 
            items[0] = true;
            items[1] = true;
            items[2] = true;
            items[3] = true;

            combinations[0, 0] = KeyPressed.Defend;
            combinations[0, 1] = KeyPressed.Up;
            combinations[0, 2] = KeyPressed.Attack;

            combinations[1, 0] = KeyPressed.Defend;
            combinations[1, 1] = KeyPressed.Up;
            combinations[1, 2] = KeyPressed.Kick;

            combinations[2, 0] = KeyPressed.Defend;
            combinations[2, 1] = KeyPressed.Down;
            combinations[2, 2] = KeyPressed.Attack;

            combinations[3, 0] = KeyPressed.Defend;
            combinations[3, 1] = KeyPressed.Down;
            combinations[3, 2] = KeyPressed.Kick;

            // Set players for first round.
            Reset(startPosition);
        }

        public void LoadContent(ContentManager Content, Dictionary<string, Animation> animations)//, Item[] Items)
        {
        }

        // Reset stats for next round
        public void Reset(Vector2 startPosition)
        {
            currentAnimationKey = "Idle";

            position = startPosition;
            levellevel = startPosition.Y;
            sprite.PlayAnimation(animations[currentAnimationKey]);
            
            // Set our collision rect. The position represents the bottom center of the sprite.
            ChangeAnimation(currentAnimationKey);
            CollisionRectangle = BoundingRectangle;

            // initial state
            this.state = new StateStopped(this);
            
            // Set the direction of the player based on which player they are.
            switch (playerIndex)
            {
                case (0):
                case (2):
                    direction = 1;
                    spriteEffect = SpriteEffects.None;
                    break;
                case (1):
                case (3):
                    direction = -1;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    break;
            }

            spriteEffect = SpriteEffects.None;

            // Set base stats
            CurrentHealth = MaxHealth;

            isDead = false;
            isReloadingRevolver = false;
            isHit = false;
            //CurrentStamina = MaxStamina;

            //maxcooldown = Tools.BASE_COOLDOWN;
            //maxmovement = Tools.BASE_MOVEMENT;
        }

        public void Update(GameTime gameTime)
        {
            if (dbleTapTimer > 0)
                dbleTapTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                dbleTapCounter = 0;

            if (comboTimer > 0)
                comboTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                comboKeys.Clear();

            sprite.Update(gameTime);

            state.Update(gameTime);

            /*if (!(state is StateJump ))//|| state is StateFall))
            {
                isFalling = false;
            }*/

            // If the player walks off a platform, they fall!
            if (!(state is StateJump))
            {
                if (((position.X + (15 * Scale) / 2) < platform.X) || ((position.X - (15 * Scale) / 2) > platform.X + platform.Width))
                {
                    platform = BoxingManager.GetLowerPlatform(position);
                    levellevel = platform.Y;
                    position.Y += 2;
                    state.ChangeState(new StateFall(this, true));
                    Debug.WriteLine("Falling after off ledge!");
                }
            }
            // else, if your falling after jumping
            else if (currentVerticalSpeed > 0 && position.Y < platform.Y)
            {
                
                // see if you jumped onto another platform
                platform = BoxingManager.GetLowerPlatform(position);
                levellevel = platform.Y;
            }



            UpdateTransform();
        }

        public void ChangeAnimation(string index)
        {
            currentAnimationKey = index;
            sprite.PlayAnimation(animations[currentAnimationKey]);
        }

        /// <summary>
        /// Change the direction sets the spriteEffect and inverts the horizontal scale (flips the matrix)
        /// </summary>
        /// <param name="i">either 1 (right) or -1 (left)</param>
        public void ChangeDirection(int i)
        {
            direction = i;
            if (i == -1)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
                scales.X = -1 * Scale;
            }
            else if (i == 1)
            {
                spriteEffect = SpriteEffects.None;
                scales.X = Scale;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(blank, CollisionRect, Color.Red);
            //spriteBatch.Draw(blank, BoundingRectangle, Color.Red);
            // Draw sprite
            //sprite.Draw(gameTime, spriteBatch, color, position, 0, Vector2.Zero, 0f, spriteEffect);
            rHealthBar.X = (int)position.X - healthBarMaxWidth / 2;
            rHealthBar.Y = (int)position.Y;// +GetHeight;
            spriteBatch.Draw(blank, rHealthBar, Color.Red);
            //sprite.Draw(gameTime, spriteBatch, BoundingRectangle, 0, color, spriteEffect);
            sprite.Draw(gameTime, spriteBatch, Color.White, position, rotation, Origin, Scale, spriteEffect);
        }

        // Add a key to the list
        public void HandleKeyDown(int player_index, KeyPressed key)
        {
            //Debug.WriteLine("KeyPressed = " + key);
            if (!KeysDown.Contains(key))
                KeysDown.Add(key);

            //-----dbleTap-----
            if (dbleTapCounter == 2)
            {
                dbleTapCounter = 0;
            }
            if(dbleTapCounter == 0)
                dbleTapTimer = dbleTapTime;
            dbleTapCounter++;

            //-----Combo detection-----

            // Add the new key, there will always be 3 keys in comboKeys.
            comboKeys.Add(key);
            if (comboKeys.Count > 3)
                comboKeys.RemoveAt(0);
            // This is cleared in the update after the timer is 0

            // Are we in a combo sequence? (Subequent keys pressed within a time limit)
            if (comboTimer > 0 && comboKeys.Count == 3) { CheckForCombo(); }

            comboTimer = comboTime; // reset combo timer

            // (Debug) Print the key list
            /*string s = "";
            for(int i = 0; i < comboKeys.Count; i++)
            {
                s +=  comboKeys[i].ToString() + " ";
            }*/
            //Debug.WriteLine("Keys: " + s);
        }

        // Remove the key from the list + set prev key
        public void HandleKeyRelease(int player_index, KeyPressed key)
        {
            if (KeysDown.Contains(key))
                KeysDown.Remove(key);

            prevKey = key;
        }

        public void CheckForCombo()
        {
            // Loop through the combinations list (set in the constructor) and look for 
            // a match in the comboKeys.
            for (int i = 0; i < 4; i++)
            {
                int c = 0;
                for (int j = 0; j < 3; j++)
                {//C:\Users\Nicholas\Documents\GitHub\AuctionBoxing\Auction_Boxing_2\Auction_Boxing_2\Auction_Boxing_2\Boxing\BoxingPlayer.cs
                    if (items[i] && comboKeys[j] == combinations[i, j]) { c++; }  
                    // We have a combo if all keys match!
                    if (c == 3) { state.OnCombo(i); Debug.WriteLine("Combo request! " + i);  }
                }
            }
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

        /// <summary>
        /// Calls the state to handle any effects to a hit player and this player
        /// </summary>
        /// <param name="hitPlayer">The player hit by this player</param>
        public void HitOtherPlayer(BoxingPlayer hitPlayer)
        {
            state.HitOtherPlayer(hitPlayer);
        }

        public bool IsKeyDown(KeyPressed key)
        {
            return this.KeysDown.Contains(key);
        }

        public void UpdateTransform()
        {
            if (direction == 1)
            {
                transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                            Matrix.CreateScale(scales) *
                            Matrix.CreateRotationZ(rotation) *
                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
            else
            {
                transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                            Matrix.CreateScale(scales) *
                            Matrix.CreateRotationZ(rotation) *
                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
        }

        public int CompareTo(BoxingPlayer player)
        {
            return this.position.Y.CompareTo(player.position.Y);
        }

        // Wrappa!
        public bool IntersectAttackPixels(BoxingPlayer b)
        {
            return IntersectPixels(Transform, sprite.Animation.FrameWidth, sprite.Animation.FrameHeight,
                BoxingManager.GetBitmapData(currentAnimationKey, sprite.FrameIndex, sprite.Animation.FrameWidth, sprite.Animation.FrameHeight),//sprite.GetData(),
                           b.Transform, b.sprite.Animation.FrameWidth, b.sprite.Animation.FrameHeight, 
                          b.sprite.GetData(), true);//b.sprite.GetData());
        }

        public bool IntersectPixels(BoxingPlayer b)
        {
            return IntersectPixels(Transform, sprite.Animation.FrameWidth, sprite.Animation.FrameHeight, sprite.GetData(),
                           b.Transform, b.sprite.Animation.FrameWidth, b.sprite.Animation.FrameHeight,
                          b.sprite.GetData(), true);//b.sprite.GetData());
        }

        // Item wrappa!
        public bool IntersectPixels(ItemInstance item)
        {
            return IntersectPixels(Transform, sprite.Animation.FrameWidth, sprite.Animation.FrameHeight, sprite.GetData(),
                           item.Transform, item.sprite.Animation.FrameWidth, item.sprite.Animation.FrameHeight,
                          item.sprite.GetData(), false);//b.sprite.GetData());
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
            Matrix transformA, int widthA, int heightA, Color[] dataA,
            Matrix transformB, int widthB, int heightB, Color[] dataB,
            bool attack)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // if attack is false, we're looking at item vs. player.
                        // so we don't need to check red, any pixel collision will do.
                        if (!attack)
                        {
                            // If both pixels are not completely transparent,
                            if (colorA.A != 0 && colorB.A != 0)
                            {
                                // then an intersection has been found
                                return true;
                            }
                            //Debug.WriteLine(colorA);
                        }else if (colorA == Color.Red && colorB.A != 0)
                        {
                            //Debug.WriteLine(colorA);
                            return true;
                        }
                        /*
                        // Using the bitmasks, if there is red on blue then we have
                        // a collision
                        if((colorA == Color.Red && colorB == Color.Blue)
                            || (colorA == Color.Blue && colorB == Color.Blue))
                        {
                            return true;
                        }
                        */
                    }
                    // Move to the next pixel in the row
                    posInB += stepX;
                }
                // Move to the next row
                yPosInB += stepY;
            }
            // No intersection found
            return false;
        }

        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public Rectangle CalculateCollisionRectangle()
        {
            // whoops! Just using htis for now.
            return BoundingRectangle;

            Rectangle rectangle = NonScaledBoundingRectangle;
            Matrix transform = TransformMatrix;

            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
    }
}
