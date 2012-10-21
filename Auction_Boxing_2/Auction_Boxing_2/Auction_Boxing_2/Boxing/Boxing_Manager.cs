using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Auction_Boxing_2.Boxing.PlayerStates; // Fix this!
using System.Diagnostics;

namespace Auction_Boxing_2
{
    enum boxingstate
    {
        idle,
        roundstart,
        roundend,
        box,
        stats
    }

    // Rounds not yet implemented
    public class Boxing_Manager
    {
        #region fields

        Texture2D background;

        Level level;

        KeyboardState kb;

        Input_Handler[] inputs = new Input_Handler[4];

        BoxingPlayer[] players = new BoxingPlayer[4];
        public List<BoxingPlayer> Players
        {
            get { return players.ToList<BoxingPlayer>(); }
        }

        bool collide = false;

        Camera camera;
        RenderTarget2D renderTarget;
        GraphicsDevice graphicsDevice;

        //List<BoxingPlayer> Players = new List<BoxingPlayer>();

        Rectangle bounds;

        public static Rectangle Battlefield;

        protected PlayerStatDisplay[] displays = new PlayerStatDisplay[4];

        string shit = "hit";
        string smoving = "moving";
        string sstopped = "stopped";
        string sattack1 = "Jump";
        string sattack2 = "Attack";
        string sattack3 = "attack3";
        string sjump = "jumping";
        string spickup = "puckup";
        string sdead = "dead";
        string scharging = "charging";
        string name = "ffsp1";
        string scasting = "casting";

        List<string> StateNames = new List<string>();

        Dictionary<string, Animation> playerAnims = new Dictionary<string, Animation>();
        Dictionary<string, Animation> itemAnims = new Dictionary<string, Animation>();
        Dictionary<string, BitMap> bitmaps = new Dictionary<string, BitMap>();
        string[] animKeys;
        Texture2D[] animTextureTemplates;
        float[] animFrameTimes;
        int[] animFrameWidths;
        bool[] animLooping;

        SpriteFont font;

        Texture2D blank;

        // When players use items, the item attacks manifest as objects.
        public List<ItemInstance> itemInstances;
        int instanceID = 0;

        public void addBowlerHat(BoxingPlayer p, float multiplier)
        {
            itemInstances.Add(new BowlerHatInstance(p, itemAnims, instanceID++, multiplier));
        }

        public void removeBowlerHat(BowlerHatInstance hat)
        {
            itemInstances.Remove(hat);
        }

        Vector2[] playerStartPositions = new Vector2[4];


        #endregion

        #region ItemNames

        string cane = "cane";

        # endregion

        #region ItemField



        List<string> ItemNames = new List<string>();


        //Item[,] Items;
        //List<BoxingItem>[] BoxingItems = { new List<BoxingItem>(), new List<BoxingItem>(), new List<BoxingItem>(), new List<BoxingItem>() };

        Dictionary<string, Texture2D> ItemTextures = new Dictionary<string, Texture2D>();

        #endregion

        Rectangle healthBarDimensions;

        public int NumRounds { get; set; }
        int currentRound = 1;
        float roundStartTimer;
        float roundStartTime = 3f;

        boxingstate state;

        bool drawCollisionBoxes = false;

        int deadCount = 0;
        int winner;
        float winTime = 2f;
        float winTimer = 0;
        bool isRoundOver;

        float restartTime = 3;
        float restartTimer = 0;

        int numberOfPlayers;

        public float GetGroundLevel
        {
            get
            {
                return playerStartPositions[0].Y;
            }
        }

        public Boxing_Manager(ContentManager content, Rectangle ClientBounds, Input_Handler[] inputs,
            GraphicsDevice gd, Camera camera)
        {
            this.graphicsDevice = gd;
            this.camera = camera;

            //camera = new Camera(ClientBounds);

            this.bounds = new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height);
            this.inputs = inputs;

            background = content.Load<Texture2D>("Boxing/AB Background");
            font = content.Load<SpriteFont>("Menu/menufont");
            blank = content.Load<Texture2D>("White");

            #region Add StateNames
            StateNames.Add(smoving);
            StateNames.Add(sstopped);
            StateNames.Add(sjump);
            StateNames.Add(sattack1);
            StateNames.Add(sattack3);
            StateNames.Add(scasting);
            StateNames.Add(shit);
            StateNames.Add(sdead);
            #endregion

            #region ItemNames
            ItemNames.Add(cane);

            #endregion

            state = boxingstate.idle;

            level = new Level(this, ClientBounds, blank, background);

            for (int i = 0; i < 4; i++)
                playerStartPositions[i] = new Vector2(bounds.X + bounds.Width / 5 * (i + 1), level.platforms[level.platforms.Length - 1].Y);

           
            //level.platforms[level.platforms.Length - 1].Y = (int)playerStartPositions[0].Y;

            NumRounds = 1;
            itemInstances = new List<ItemInstance>();

            healthBarDimensions = new Rectangle(0, 0, ClientBounds.Width / 16, ClientBounds.Height / 80);

            LoadContent(content);
        }

        public void LoadContent(ContentManager Content)
        {
            

            //Change to using a text file!!

            // Create animation library to pass to players.

            animKeys = new string[] {
                "Idle",
                "Walk",
                "Run",
                "Jump",
                "Land",
                "Punch",
                "PunchHit",
                "Dodge",
                "Block",
                "Down",
                "Duck",
                "CaneBonk",
                "CaneHit",
                "CanePull",
                "CaneBalance",
                "RevolverShoot",
                "RevolverHit",
                "RevolverReload",
                "BowlerThrow",
                "BowlerCatch",
                "BowlerRethrow",
                "Cape",
                "CapeStuck"};
                

            #region set frame width

            animFrameWidths = new int[] {
                15,
                12,
                27,
                20,
                18,
                37,
                34,
                17,
                15,
                43,
                16,

                54,
                19,
                76,
                28,
            
                56,
                15,
                56,
            
                34,
                37,
                34,

                54,
                54
                };

            

            // frame widths

            int wBowlerHat = 6;
            
            #endregion

            #region set frame times

            animFrameTimes = new float[] { 

                0.1f,
                0.1f,
                0.05f,
                0.1f,
                0.1f,
                0.09f,
                0.1f,
                0.05f,
                0.1f,
                0.08f,
                0.08f,

                0.1f,
                0.1f,
                0.1f,
                0.1f,

                0.1f,
                0.05f,
                0.1f,

                0.1f,
                0.05f,
                0.1f,

                0.1f,
                0.1f
                };

            #endregion

            #region load textures

            /*// Load textures
            Texture2D idle = Content.Load<Texture2D>("Boxing/Player_Idle_Side");
            Texture2D walk = Content.Load<Texture2D>("Boxing/Player_Walking_Side");
            Texture2D run = Content.Load<Texture2D>("Boxing/Player_Running_Side");
            Texture2D jump = Content.Load<Texture2D>("Boxing/Player_Jump");
            Texture2D land = Content.Load<Texture2D>("Boxing/Player_Land");
            Texture2D punch = Content.Load<Texture2D>("Boxing/Player_Punch");
            Texture2D punchHit = Content.Load<Texture2D>("Boxing/Player_Punch_Hit");
            Texture2D dodge = Content.Load<Texture2D>("Boxing/Player_Dodge");
            Texture2D block = Content.Load<Texture2D>("Boxing/Player_Block");
            Texture2D down = Content.Load<Texture2D>("Boxing/Player_Knocked_Down");
            Texture2D duck = Content.Load<Texture2D>("Boxing/Player_Duck");

            // player item use textures
            Texture2D caneBonk = Content.Load<Texture2D>("BoxingItems/Player_Cane");
            Texture2D caneHit = Content.Load<Texture2D>("BoxingItems/Player_Cane_Hit");
            Texture2D canePull = Content.Load<Texture2D>("BoxingItems/Player_Cane_Pull");
            Texture2D caneBalance = Content.Load<Texture2D>("BoxingItems/Player_Cane_Balance");

            Texture2D revolverShoot = Content.Load<Texture2D>("BoxingItems/Player_Revolver");
            Texture2D revolverHit = Content.Load<Texture2D>("BoxingItems/Player_Revolver_Hit");
            Texture2D revolverReload = Content.Load<Texture2D>("BoxingItems/Player_Revolver_Reload");

            Texture2D bowlerThrow = Content.Load<Texture2D>("BoxingItems/Player_BowlerHat");
            Texture2D bowlerCatch = Content.Load<Texture2D>("BoxingItems/Player_BowlerHat_Catch");
            Texture2D bowlerReThrow = Content.Load<Texture2D>("BoxingItems/Player_BowlerHat_ReThrow");

        */
            #endregion

            #region initialize animations

            animLooping = new bool[] {
                true,
                true,
                true,
                false,
                true,
                false,
                true,
                true,
                true,
                false,
                false,

                false,
                false,
                false,
                false,

                false,
                false,
                false,

                false,
                false,
                false,

                false,
                false
                };

            Texture2D bowlerHat = Content.Load<Texture2D>("BoxingItems/BowlerHat_Instance");

            // Initialize animations;
            /*animations.Add("Idle", new Animation(idle, fIdle, true, wIdle));
            animations.Add("Walk", new Animation(walk, fWalk, true, wWalk));
            animations.Add("Run", new Animation(run, fRun, true, wRun));
            animations.Add("Jump", new Animation(jump, fJump, false, wJump));
            animations.Add("Land", new Animation(land, fLand, true, wLand));
            animations.Add("Punch", new Animation(punch, fPunch, false, wPunch));
            animations.Add("PunchHit", new Animation(punchHit, fPunchHit, true, wPunchHit));
            animations.Add("Dodge", new Animation(dodge, fDodge, true, wDodge));
            animations.Add("Block", new Animation(block, fBlock, true, wBlock));
            animations.Add("Down", new Animation(down, fDown, false, wDown));
            animations.Add("Duck", new Animation(duck, fDuck, false, wDuck));
			
            animations.Add("RevolverShoot", new Animation(revolverShoot, fRevolverShoot, false, wRevolverShoot));
            animations.Add("RevolverHit", new Animation(revolverHit, fRevolverHit, false, wRevolverHit));
            animations.Add("RevolverReload", new Animation(revolverReload, fRevolverReload, false, wRevolverReload));

            animations.Add("bowlerThrow", new Animation(bowlerThrow, fBowlerThrow, false, wBowlerThrow));
            animations.Add("bowlerCatch", new Animation(bowlerCatch, fBowlerCatch, false, wBowlerCatch));
            animations.Add("bowlerReThrow", new Animation(bowlerReThrow, fBowlerReThrow, false, wBowlerReThrow));
            */

            // item animations

            itemAnims.Add("bowlerHat", new Animation(bowlerHat, 1f, true, wBowlerHat));
			
            // Template textures to be used when re-coloring them
            animTextureTemplates = new Texture2D[] {
                Content.Load<Texture2D>("Boxing/Player_Idle_Side"),
                Content.Load<Texture2D>("Boxing/Player_Walking_Side"),
                Content.Load<Texture2D>("Boxing/Player_Running_Side"),
                Content.Load<Texture2D>("Boxing/Player_Jump"),
                Content.Load<Texture2D>("Boxing/Player_Land"),
                Content.Load<Texture2D>("Boxing/Player_Punch"),
                Content.Load<Texture2D>("Boxing/Player_Punch_Hit"),
                Content.Load<Texture2D>("Boxing/Player_Dodge"),
                Content.Load<Texture2D>("Boxing/Player_Block"),
                Content.Load<Texture2D>("Boxing/Player_Knocked_Down"),
                Content.Load<Texture2D>("Boxing/Player_Duck"),
                Content.Load<Texture2D>("BoxingItems/Player_Cane"),
                Content.Load<Texture2D>("BoxingItems/Player_Cane_Hit"),
                Content.Load<Texture2D>("BoxingItems/Player_Cane_Pull"),
                Content.Load<Texture2D>("BoxingItems/Player_Cane_Balance"),
                Content.Load<Texture2D>("BoxingItems/Player_Revolver"),
                Content.Load<Texture2D>("BoxingItems/Player_Revolver_Hit"),
                Content.Load<Texture2D>("BoxingItems/Player_Revolver_Reload"),
                Content.Load<Texture2D>("BoxingItems/Player_BowlerHat"),
                Content.Load<Texture2D>("BoxingItems/Player_BowlerHat_Catch"),
                Content.Load<Texture2D>("BoxingItems/Player_BowlerHat_ReThrow"),
                Content.Load<Texture2D>("BoxingItems/Player_Cape"),
                Content.Load<Texture2D>("BoxingItems/Player_Cape_Stuck")
                };

            #endregion

            // Initialize bitmaps
            bitmaps.Add("Punch", new BitMap(Content.Load<Texture2D>("Boxing/Bitmaps/Player_Punch_Bitmap")));
            
            // item animations
            bitmaps.Add("CaneBonk", new BitMap(Content.Load<Texture2D>("Boxing/Bitmaps/Player_Cane_Bitmap")));
            bitmaps.Add("CanePull", new BitMap(Content.Load<Texture2D>("Boxing/Bitmaps/Player_Cane_Pull_Bitmap")));
        }

        // Apply's settings gathered before the boxing begins.
        public void ApplySettings(Color[] colors)
        {
            
            //Need to make copies of the textures recolored with the players selected color
            for(int i = 0; i < 4; i++)
            {
                // If the color is transparent, the player isn't playing.
                if (colors[i] != Color.Transparent)
                {
                    // The players dictionary of animations
                    Dictionary<string, Animation> coloredAnims = new Dictionary<string, Animation>();

                    Debug.WriteLine("Number of textures to recolor: " + animTextureTemplates.Length);
                    // loop through each template texture and recolor it
                    for(int j = 0; j < animTextureTemplates.Length; j++)
                    {
                        Debug.WriteLine("index = " + j);
                        Texture2D template = animTextureTemplates[j];// template
                        Texture2D t = new Texture2D(graphicsDevice, template.Width, template.Height); //colored
                        Color[] c = new Color[template.Width * template.Height];
                        template.GetData(c);

                        int count = 0;

                        // Replace magenta pixels with the color
                        for (int k = 0; k < c.Length; k++)
                        {
                            // This is awesome!
                            if (c[k] == Color.Magenta || (c[k].R == 254 && c[k].G == 0 && c[k].B == 254)
                                || (c[k].R == 253 && c[k].G == 0 && c[k].B == 253)
                                || (c[k].R == 252 && c[k].G == 0 && c[k].B == 252)
                                || (c[k].R == 251 && c[k].G == 0 && c[k].B == 251)
                                || (c[k].R == 250 && c[k].G == 0 && c[k].B == 250))
                            {
                                count++;
                                c[k] = colors[i];
                            }
                        }
                        t.SetData(c); // set the data
                        // add it to the available list
                        coloredAnims.Add(animKeys[j], new Animation(t,animFrameTimes[j],animLooping[j],animFrameWidths[j]));
                    }

                    players[i] = new BoxingPlayer(this, i, playerStartPositions[i], coloredAnims, inputs[i], colors[i], blank,
                        healthBarDimensions, level.platforms[level.platforms.Length - 1]); // Figure out the boxing players.
                }
            }
        }

        // Resets the game for another round.
        public void Reset()
        {
            // Reset the state
            state = boxingstate.roundstart;
            roundStartTimer = roundStartTime;

            numberOfPlayers = 0;

            // Reset the players
            for (int i = 0; i < 4; i++)
            {
                if (players[i] != null)
                {
                    players[i].Reset(playerStartPositions[i]);
                    numberOfPlayers++;
                }
            }

            Debug.WriteLine("num of players = " + numberOfPlayers);

            // reset item instances
            itemInstances.Clear();

            deadCount = 0;
            isRoundOver = false;
        }

        // Find the first player in front of player
        public BoxingPlayer GetPlayerInFront(BoxingPlayer p, float y, int direction)
        {
            BoxingPlayer f = null;

            float min = 0;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerIndex != p.playerIndex && !players[i].isDead)
                {
                    if (direction == 1
                        && players[i].position.X > p.position.X 
                        && y > players[i].position.Y - players[i].GetHeight
                        && y < players[i].position.Y)
                    {
                        float d = players[i].position.X - p.position.X;
                        if (min == 0 || d < min)
                        {
                            min = d;
                            f = players[i];
                        }
                    }
                    else if (direction == -1
                        && players[i].position.X < p.position.X
                        && y > players[i].position.Y - players[i].GetHeight
                        && y < players[i].position.Y)
                    {
                        float d = p.position.X - players[i].position.X;
                        if (min == 0 || d < min)
                        {
                            min = d;
                            f = players[i];
                        }
                    }
                }
            }
            return f;
        }

        public bool Update(GameTime gameTime)
        {

            camera.UpdateCamera(players.ToList<BoxingPlayer>(), graphicsDevice);

            switch (state)
            {
                // The idle state is just to display the background while the settings are configured.
                case(boxingstate.idle):
                    // if animated background, update it.
                    break;
                // Will display the "Round X, Begin!" animation.
                case(boxingstate.roundstart):
                    if (roundStartTimer > 0)
                        roundStartTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        state = boxingstate.box;
                    break;
                // Will handle all the logic for the boxing; player updates, collision, etc.
                case(boxingstate.box):
                    foreach (ItemInstance item in itemInstances)
                    {
                        item.Update(gameTime);
                        HandleCollisions(item);
                    }
                    for(int i = 0; i < 4; i++)
                    {
                        BoxingPlayer player = players[i];
                        if(player != null)
                        {
                            player.Update(gameTime);

                            // keep them from going off the screen
                            player.position.X = MathHelper.Clamp(player.position.X,
                                bounds.Left + player.GetWidth / 2, bounds.Right - player.GetWidth / 2);//- (player.GetWidth / 2 - 15 * player.scale), bounds.Right - player.GetWidth / 2 - (player.GetWidth / 2 - 15 * player.scale));

                            HandleCollisions(i);
                        }

                    }

                    // Handle the winning
                    if (winTimer > 0)
                        winTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (deadCount >= numberOfPlayers - 1)
                    {
                        isRoundOver = true;
                        winTimer = winTime;


                        // Who's the winner?
                        for (int i = 0; i < 4; i++)
                        {
                            if (players[i] != null && !players[i].isDead)
                            {
                                winner = i+1;
                            }
                        }

                        state = boxingstate.roundend;
                        restartTimer = restartTime;
                    }

                    break;
                case (boxingstate.roundend):
                    if (currentRound == NumRounds) // this was the last round, show stats
                    {
                        Debug.WriteLine("Round {0} Complete!", currentRound);
                        state = boxingstate.stats;
                    }
                    else // reset for next round
                    {
                        if (restartTimer > 0)
                        {
                            restartTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            Debug.WriteLine("Round {0} Complete!", currentRound);
                            currentRound++;
                            Reset();
                        }
                    }
                    break;
                case (boxingstate.stats):
                    Debug.WriteLine("A winner is you!");
                    return false;
                    break;
            }

            return true;

            /*
            kb = Keyboard.GetState();

            Players.Sort();

            HandleCollisions();

            foreach (BoxingPlayer player in Players)
            {
                //player.handleCollision(Players);
                if (player.InternalState is StateCharging)
                {
                    player.handleCollision(Players);
                }
                player.update(gameTime);
            }

            List<ItemInstance> instancesToRemove = new List<ItemInstance>();

            foreach (ItemInstance item in itemInstances)
            {
                item.Update(gameTime);
                if (item.end)
                    instancesToRemove.Add(item);
            }

            for (int i = 0; i < instancesToRemove.Count; i++)
                itemInstances.Remove(instancesToRemove[i]);

            foreach (PlayerStatDisplay display in displays)
            {
                display.Update();
            }

            //Switch to Menu by pressing R
            if (kb.IsKeyDown(Keys.R))
                return true;
            else
                return false;*/
        }

        /*public float GetLowerPlatformLevel(float platformlevel)
        {
            float l = platformlevel;
            for (int i = 0; i < level.platforms.Length; i++)
            {
                if (level.platforms[i].Y > l)
                {
                    l = level.platforms[i].Y;
                    return l;// 
                }

            }
            return l;
        }*/

        public Rectangle GetLowerPlatform(Vector2 pos)
        {
            //float l = platformlevel;
            for (int i = level.platforms.Length - 1; i > 0; i--)
            {
                // the left and right of the player corners
                int left = (int)(pos.X - (15 * BoxingPlayer.Scale) / 2);
                int right = (int)(pos.X + (15 * BoxingPlayer.Scale) / 2);

                if (right > level.platforms[i].X && left < level.platforms[i].X + level.platforms[i].Width && level.platforms[i].Y > pos.Y)
                {
                   // l = level.platforms[i].Y;
                    //Debug.WriteLine("Found new platform!");
                    return level.platforms[i];// 
                }

            }
            return level.platforms[level.platforms.Length - 1];
        }

        /// <summary>
        /// Collision of item instances against players!
        /// </summary>
        /// <param name="item">The item checking collision</param>
        public void HandleCollisions(ItemInstance item)
        {
            // For attacking player-on-player collision (Uses per pixel)
            for (int i = 0; i < 4; i++)
            {
                if (players[i] != null && !players[i].isDead)
                {
                    if (players[i] != item.player) // collision with unfriendly player
                    {
                        // Check for a collision!
                        if(players[i].IntersectPixels(item))
                            players[i].state.isHitByItem(item, new StateHit(players[i]));// TODO : check for collision with player
                    }
                }
            }
        }

        public void HandleCollisions(int player)
        {
            
            // level collision
            if (players[player].currentVerticalSpeed > 0)// && players[player].state is StateFall))
            {
                Vector2 bottomLeft = new Vector2(players[player].position.X - players[player].GetWidth / 2,
                    players[player].position.Y);// + players[player].GetHeight - players[player].GetHeight / 2);
                Vector2 bottomRight = new Vector2(players[player].position.X + players[player].GetWidth - players[player].GetWidth / 2,
                    players[player].position.Y);// + players[player].GetHeight - players[player].GetHeight / 2);

                for (int j = level.platforms.Length - 1; j >= 0; j--)//(int j = 0; j < level.platforms.Length; j++)
                {
                    if (j != 0)
                    {
                        if ((bottomLeft.X > level.platforms[j].X
                            && bottomLeft.X < level.platforms[j].X + level.platforms[j].Width
                            && bottomLeft.Y < level.platforms[j].Y
                            && bottomLeft.Y > level.platforms[j - 1].Y + level.platforms[j - 1].Height) ||
                            //&& bottomLeft.Y > level.platforms[j].Y
                            //&& bottomLeft.Y < level.platforms[j].Y + level.platforms[j].Height) ||
                            (bottomRight.X > level.platforms[j].X
                            && bottomRight.X < level.platforms[j].X + level.platforms[j].Width
                             && bottomLeft.Y < level.platforms[j].Y
                             && bottomLeft.Y > level.platforms[j - 1].Y + level.platforms[j - 1].Height))
                        //&& bottomRight.Y > level.platforms[j].Y
                        //&& bottomRight.Y < level.platforms[j].Y + level.platforms[j].Height))
                        {

                            players[player].platform = level.platforms[j];
                            players[player].levellevel = level.platforms[j].Y;//position.Y = level.platforms[j].Y;
                        }
                    }
                    else // top level
                    {
                        if ((bottomLeft.X > level.platforms[j].X
                            && bottomLeft.X < level.platforms[j].X + level.platforms[j].Width
                            && bottomLeft.Y < level.platforms[j].Y) ||
                            (bottomRight.X > level.platforms[j].X
                            && bottomRight.X < level.platforms[j].X + level.platforms[j].Width
                             && bottomLeft.Y < level.platforms[j].Y))
                        {
                            players[player].platform = level.platforms[j];
                            players[player].levellevel = level.platforms[j].Y;
                        }
                    }
                }
            }

            // For attacking player-on-player collision (Uses per pixel)
            for(int i = 0; i < 4; i++)
            {
                // pvp
                if(i != player && players[i] != null && !players[i].isDead)
                {

                    BoxingPlayer thisPlayer = players[player];
                    BoxingPlayer otherPlayer = players[i];

                    if (thisPlayer.BoundingRectangle.Intersects(otherPlayer.BoundingRectangle))
                    {
                        if (thisPlayer.isAttacking && (thisPlayer.GetGroundLevel == otherPlayer.GetGroundLevel))
                        {
                            collide = thisPlayer.IntersectPixels(otherPlayer);
                            if (collide)
                                thisPlayer.HitOtherPlayer(otherPlayer);
                        }
                    }
                }
            }
            
            /*
            foreach (ItemInstance instance in itemInstances)
            {
                if (!instance.isEffect)
                {
                    foreach (BoxingPlayer player in Players)
                    {
                        if (player.Hurtbox.Intersects(instance.hitbox) && player.playerindex != instance.playerId)
                        {
                            if (Math.Abs(player.Position.Y - instance.position.Y) <= 20 && !(player.InternalState is StateHit))
                    /            player.Hit(instance.item);
                            Debug.WriteLine(player.Position.Y - instance.position.Y);
                            //if(item is BowlerHatInstance && Math.Abs(player.Position.Y - item.position.Y 
                        }
                    }
                }
            }
             * */
        }

        public Color[] GetBitmapData(string key, int index, int framew, int frameh)
        {
            return bitmaps[key].GetData(index, framew, frameh);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            

            switch (state)
            {
                case (boxingstate.idle):
                    spriteBatch.Draw(background, bounds, Color.White);
                    break;
                case (boxingstate.roundstart):
                    spriteBatch.Draw(background, bounds, Color.White);

                    level.Draw(spriteBatch);

                    Rectangle cameraBounds = camera.DrawToRectangle;

                    // Draw the round start count down
                    string s = "Round Starting in\n" + (int)roundStartTimer;
                    spriteBatch.DrawString(font, s, new Vector2(cameraBounds.X + cameraBounds.Width / 2 - font.MeasureString(s).X,
                        cameraBounds.Height + cameraBounds.Height / 2 - font.MeasureString(s).Y), Color.Yellow);

                    // Draw the players
                    foreach (BoxingPlayer player in players)
                    {
                        if (player != null)
                            player.Draw(gameTime, spriteBatch);
                    }

                    break;
                case (boxingstate.box):
                    //if (!collide)
                    //    spriteBatch.Draw(background, bounds, Color.White);
                    //else
                    //    spriteBatch.Draw(background, bounds, Color.White);

                    level.Draw(spriteBatch);

                    foreach (ItemInstance item in itemInstances)
                    {
                        item.Draw(gameTime, spriteBatch);
                    }

                    // Draw the players
                    foreach (BoxingPlayer player in players)
                    {
                        if (player != null)
                        {
                            Vector2 bottomLeft = new Vector2(player.position.X - player.GetWidth / 2,
                                player.position.Y);// + player.GetHeight - player.GetHeight / 2);
                            Vector2 bottomRight = new Vector2(player.position.X + player.GetWidth - player.GetWidth / 2,
                                player.position.Y);// + player.GetHeight - player.GetHeight / 2);

                            spriteBatch.Draw(blank, new Rectangle((int)bottomLeft.X, (int)bottomLeft.Y, 3, 3), Color.Red);
                            spriteBatch.Draw(blank, new Rectangle((int)bottomRight.X, (int)bottomRight.Y, 3, 3), Color.Red);

                            if (drawCollisionBoxes)
                            {
                                Rectangle playerRectangle = player.CalculateCollisionRectangle();
                                //new Rectangle(0, 0, player.GetWidth / 4, player.GetHeight / 4), player.TransformMatrix);
                                //Debug.WriteLine("DRAWING BOUND");
                                spriteBatch.Draw(blank, playerRectangle, Color.Blue);

                                // Draw the current color data in the top corner

                                // Create a new texture and assign the data to it
                                //Texture2D texture = 

                                //spriteBatch.Draw(player.sprite.GetDataAsTexture(gd), 
                                //new Rectangle(0,0,(int)(player.GetWidth / player.scale), (int)(player.GetHeight / player.scale)), Color.White);
                            }

                            player.Draw(gameTime, spriteBatch);
                        }
                    }
                    break;
                case (boxingstate.roundend):
                    spriteBatch.Draw(background, bounds, Color.White);
                    // Draw the players
                    foreach (BoxingPlayer player in players)
                    {
                        if (player != null)
                        {
                            if (drawCollisionBoxes)
                            {
                                Rectangle playerRectangle = player.CalculateCollisionRectangle();
                                //new Rectangle(0, 0, player.GetWidth / 4, player.GetHeight / 4), player.TransformMatrix);

                                spriteBatch.Draw(blank, playerRectangle, Color.Blue);
                            }

                            player.Draw(gameTime, spriteBatch);


                        }
                    }
                    Rectangle cameraBounds2 = camera.DrawToRectangle;

                    // Draw the winner!
                    string w = "Player " + winner + " Takes the Round!";
                    spriteBatch.DrawString(font, w,
                        new Vector2(cameraBounds2.X + cameraBounds2.Width / 2 - font.MeasureString(w).X / 2,
                            cameraBounds2.Y + cameraBounds2.Height / 2 - font.MeasureString(w).Y / 2), Color.Goldenrod);
                    break;
                case (boxingstate.stats):
                    spriteBatch.Draw(background, bounds, Color.White);
                    // Draw the players
                    foreach (BoxingPlayer player in players)
                    {
                        if (player != null)
                        {
                            if (drawCollisionBoxes)
                            {
                                Rectangle playerRectangle = player.CalculateCollisionRectangle();
                                //new Rectangle(0, 0, player.GetWidth / 4, player.GetHeight / 4), player.TransformMatrix);

                                spriteBatch.Draw(blank, playerRectangle, Color.Blue);
                            }

                            player.Draw(gameTime, spriteBatch);
                        }
                    }
                    // Draw the winner!
                    string win = "Player " + winner + "Wins!";
                    spriteBatch.DrawString(font, win,
                        new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(win).X / 2,
                            bounds.Y + bounds.Height / 2 - font.MeasureString(win).Y / 2), Color.Goldenrod);
                    break;
            }
        }

        public void NotifyPlayerDeath(int playerIndex)
        {
            deadCount++;
        }
    }
}
