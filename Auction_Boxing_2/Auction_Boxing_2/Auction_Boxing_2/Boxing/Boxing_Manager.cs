using System;
using System.Collections.Generic;
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

        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        Dictionary<string, BitMap> bitmaps = new Dictionary<string, BitMap>();

        SpriteFont font;

        Texture2D blank;

        //When players use items, the item attacks manifest as objects.
        //List<ItemInstance> itemInstances = new List<ItemInstance>();

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

        float roundStartTimer;
        float roundStartTime = 3f;

        boxingstate state;

        bool drawCollisionBoxes = true;

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


        public Boxing_Manager(ContentManager content, Rectangle ClientBounds, Input_Handler[] inputs)
        {
            this.bounds = new Rectangle(0, 0, ClientBounds.Width, ClientBounds.Height);
            this.inputs = inputs;

            background = content.Load<Texture2D>("Boxing/LevelBackground");
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

            for (int i = 0; i < 4; i++)
                playerStartPositions[i] = new Vector2(bounds.X + bounds.Width / 5 * (i + 1), 4 * bounds.Height / 5);

            level = new Level(this, ClientBounds, blank);
            //level.platforms[level.platforms.Length - 1].Y = (int)playerStartPositions[0].Y;

            healthBarDimensions = new Rectangle(0, 0, ClientBounds.Width / 16, ClientBounds.Height / 80);

            LoadContent(content);
        }

        public void LoadContent(ContentManager Content)
        {
            // Create animation library to pass to players.

            #region set frame width

            // frame widths
            int wIdle = 15;
            int wWalk = 12;
            int wRun = 27;
            int wJump = 20;
            int wLand = 18;
            int wPunch = 37;
            int wPunchHit = 34;
            int wDodge = 17;
            int wBlock = 15;
            int wDown = 43;
            int wDuck = 16;

            // item frame widths
            int wCaneBonk = 54;
            int wCaneHit = 19;
            int wCanePull = 76;
            int wCaneBalance = 28;

           

            int wRevolverShoot = 56;
            int wRevolverHit = 15;
            int wRevolverReload = 56;

            int wBowlerThrow = 34;
            int wBowlerCatch = 37;
            int wBowlerReThrow = 34;

            #endregion

            #region set frame times

            // frame times
            float fIdle = 0.1f;
            float fWalk = 0.1f;
            float fRun = 0.05f;
            float fJump = 0.1f;
            float fLand = 0.1f;
            float fPunch = 0.09f;
            float fPunchHit = 0.1f;
            float fDodge = 0.05f;
            float fBlock = 0.1f;
            float fDown = 0.08f;
            float fDuck = 0.08f;

            // item frame times
            float fCaneBonk = 0.1f;
            float fCaneHit = 0.1f;
            float fCanePull = 0.1f;
            float fCaneBalance = 0.1f;

            float fRevolverShoot = 0.1f;
            float fRevolverHit = 0.05f;
            float fRevolverReload = 0.1f;

            float fBowlerThrow = 0.1f;
            float fBowlerCatch = 0.1f;
            float fBowlerReThrow = 0.1f;

            #endregion

            #region load textures

            // Load textures
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

            // item textures
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

            #endregion

            #region initialize animations

            // Initialize animations;
            animations.Add("Idle", new Animation(idle, fIdle, true, wIdle));
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

            // item animations
            animations.Add("CaneBonk", new Animation(caneBonk, fCaneBonk, false, wCaneBonk));
            animations.Add("CaneHit", new Animation(caneHit, fCaneHit, false, wCaneHit));
            animations.Add("CanePull", new Animation(canePull, fCanePull, false, wCanePull));
            animations.Add("CaneBalance", new Animation(caneBalance, fCaneBalance, false, wCaneBalance));

            animations.Add("RevolverShoot", new Animation(revolverShoot, fRevolverShoot, false, wRevolverShoot));
            animations.Add("RevolverHit", new Animation(revolverHit, fRevolverHit, false, wRevolverHit));
            animations.Add("RevolverReload", new Animation(revolverReload, fRevolverReload, false, wRevolverReload));

            animations.Add("bowlerThrow", new Animation(bowlerThrow, fBowlerThrow, false, wBowlerThrow));
            animations.Add("bowlerCatch", new Animation(bowlerCatch, fBowlerCatch, false, wBowlerCatch));
            animations.Add("bowlerReThrow", new Animation(bowlerReThrow, fBowlerReThrow, false, wBowlerReThrow));

            #endregion

        }

        // Apply's settings gathered before the boxing begins.
        public void ApplySettings(Color[] colors)
        {
            for(int i = 0; i < 4; i++)
            {
                if(colors[i] != Color.Transparent)
                    players[i] = new BoxingPlayer(this, i, playerStartPositions[i], animations, inputs[i], colors[i], blank,
                        healthBarDimensions); // Figure out the boxing players.
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

            isRoundOver = false;
            
        }

        public void Activate(ContentManager Content)
        {
            /*List<Item>[] equippedItems = { new List<Item>(), new List<Item>(), new List<Item>(), new List<Item>() };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    equippedItems[i].Add(new Cane(Content.Load<Texture2D>("BoxingItems/cane"),
                    Content.Load<Texture2D>("BoxingItems/Cane_Attack"),
                    Content.Load<Texture2D>("LoadOut/cane_icon")));
                    equippedItems[i].Add(new Bowler_Hat(Content.Load<Texture2D>("Items/bowlerhat_image"),
                        Content.Load<Texture2D>("BoxingItems/Bowler_Attack"),
                        Content.Load<Texture2D>("LoadOut/bowlerhat_icon")));
                    equippedItems[i].Add(new Revolver(Content.Load<Texture2D>("Items/revolver_image"),
                        Content.Load<Texture2D>("BoxingItems/Revolver_Attack"),
                        Content.Load<Texture2D>("LoadOut/revolver_icon")));
                    equippedItems[i].Add(new Boots(Content.Load<Texture2D>("Items/Boots_Image"),
                        Content.Load<Texture2D>("BoxingItems/gust_attack"),
                        Content.Load<Texture2D>("LoadOut/boots_icon"),
                        Content.Load<Texture2D>("Boxing/ffsp1charge"),
                        Content.Load<Texture2D>("Boxing/ffsp1jumping")));
                }

            }
            this.Items = equippedItems;

            Battlefield = new Rectangle(0, 140, bounds.Width, 55);

            List<BoxingPlayer> activePlayers = new List<BoxingPlayer>();

            for (int i = 0; i < 4; i++)
            {
                activePlayers.Add(new BoxingPlayer(bounds.Width * (1 - .9f), bounds.Height * (1 - i * .1f) - 100, "player" + i, i, Tools.WIDTH, Tools.HEIGHT, inputs[i]));
                activePlayers[i].OnUseItem += CreateInstance;
            }

            Players = activePlayers;
            Item[] equipment = new Item[4];
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                     equipment[j] = state_manager.equipment[i,j];
                
                Players[i].LoadContent(Content, ATextures, equipment);
            }

            for (int i = 0; i < 4; i++)
            {

                displays[i] = new PlayerStatDisplay(font, i + 1, Players[i], inputs[i], new Rectangle((i * bounds.Width / 4) + 1, 1, (bounds.Width / 4) - 2, bounds.Height / 4),
                    Content.Load<Texture2D>("White"), Content.Load<Texture2D>("White"));
            }*/
        }

        /*public void CreateInstance(ItemInstance instance)
        {
            /*itemInstances.Add(instance);
            Debug.WriteLine("Item instance = " + (instance is BowlerHatInstance).ToString());
        }*/

        // Find the first player in front of player
        public BoxingPlayer GetPlayerInFront(BoxingPlayer p, float y)
        {
            BoxingPlayer f = null;

            float min = 0;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerIndex != p.playerIndex)
                {
                    if (p.direction == 1
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
                    else if (p.direction == -1
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

        public void Update(GameTime gameTime)
        {
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

                    int deadCount = 0;

                    for(int i = 0; i < 4; i++)
                    {
                        BoxingPlayer player = players[i];
                        if(player != null)
                        {
                            player.Update(gameTime);

                            player.position.X = MathHelper.Clamp(player.position.X,
                                bounds.Left + player.GetWidth / 2, bounds.Right - player.GetWidth / 2);

                            
                            HandleCollisions(i);

                            if (player.isDead)
                                deadCount++;
                        }

                    }

                    // Handle the winning
                    if (winTimer > 0)
                        winTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (isRoundOver && winTimer <= 0)
                    {
                        state = boxingstate.stats;
                        restartTimer = restartTime;
                    }

                    if (!isRoundOver && deadCount >= numberOfPlayers - 1)
                    {
                        isRoundOver = true;
                        winTimer = winTime;

                        Debug.WriteLine("GameOver!");

                        // Who's the winner?
                        for (int i = 0; i < 4; i++)
                        {
                            if (players[i] != null && !players[i].isDead)
                            {
                                winner = i+1;
                            }
                        }
                    }

                    break;
                case(boxingstate.stats):
                    if (restartTimer > 0)
                        restartTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Reset();

                    break;
            }


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

        public void CheckForCollision()
        {
            /*for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    TexturesCollide(players[i].sprite.Get2DColorDataArray(), players[i].Transform,
                        players[j].sprite.Get2DColorDataArray(), players[j].Transform);


                    /*
                    if (players[i] != null && players[j] != null)
                    {
                        if (j != i && IntersectPixels(players[i].CollisionRect, players[i].sprite.GetDataFromFrame(),
                            players[j].CollisionRect, players[j].sprite.GetDataFromFrame()))
                        {
                            Debug.WriteLine("HIT!");
                        }
                    }*/
                //}
            //}
        }

        public float GetLowerPlatformLevel(float platformlevel)
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
        }

        public bool TexturesCollide(Color[,] image1, Matrix matrix1, Color[,] image2, Matrix matrix2)
        {
            Matrix mat1to2 = matrix1 * Matrix.Invert(matrix2);
            int width1 = image1.GetLength(0);
            int height1 = image1.GetLength(1);
            int width2 = image2.GetLength(0);
            int height2 = image2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (image1[x1, y1].A != 0)
                            {
                                if (image2[x2, y2].A != 0)
                                {
                                    
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        //IntersectPixels method taken directly from the XNA 2D per pixel collision check. Doesn't need to be changed as far as I can see. 
        private bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        } 

        public void HandleCollisions(int player)
        {
            // level collision
            if (players[player].isFalling)
            {
                Vector2 bottomLeft = new Vector2(players[player].position.X - players[player].GetWidth / 2,
                    players[player].position.Y);// + players[player].GetHeight - players[player].GetHeight / 2);
                Vector2 bottomRight = new Vector2(players[player].position.X + players[player].GetWidth - players[player].GetWidth / 2,
                    players[player].position.Y);// + players[player].GetHeight - players[player].GetHeight / 2);

                for (int j = 0; j < level.platforms.Length; j++)
                {

                    if ((bottomLeft.X > level.platforms[j].X
                        && bottomLeft.X < level.platforms[j].X + level.platforms[j].Width
                        && bottomLeft.Y > level.platforms[j].Y
                        && bottomLeft.Y < level.platforms[j].Y + level.platforms[j].Height) ||
                        (bottomRight.X > level.platforms[j].X
                        && bottomRight.X < level.platforms[j].X + level.platforms[j].Width
                        && bottomRight.Y > level.platforms[j].Y
                        && bottomRight.Y < level.platforms[j].Y + level.platforms[j].Height))
                    {
                        players[player].levellevel = level.platforms[j].Y;//position.Y = level.platforms[j].Y;
                    }
                }
            }


            for(int i = 0; i < 4; i++)
            {
                // pvp
                if(i != player && players[i] != null)
                {

                    BoxingPlayer thisPlayer = players[player];
                    BoxingPlayer otherPlayer = players[i];

                    Rectangle playerRectangle = thisPlayer.CalculateCollisionRectangle();
                        //new Rectangle(0,0, (int)(thisPlayer.GetWidth / thisPlayer.scale), 
                            //(int)(thisPlayer.GetHeight / thisPlayer.scale)), thisPlayer.TransformMatrix);

                    Rectangle otherPlayerRectangle = otherPlayer.CalculateCollisionRectangle();
                        //new Rectangle(0,0, (int)(otherPlayer.GetWidth / otherPlayer.scale),
                            //(int)(otherPlayer.GetHeight / otherPlayer.scale)), otherPlayer.TransformMatrix);


                    if(playerRectangle.Intersects(otherPlayerRectangle))
                    {
                        //Debug.WriteLine("Rectangle Collision!");
                        if (thisPlayer.isAttacking)
                            thisPlayer.HitOtherPlayer(otherPlayer);
                        else if ((thisPlayer.isFalling || thisPlayer.state is StateFall) && otherPlayer.state is StateDuck)
                            otherPlayer.state.isHit(thisPlayer, new StateKnockedDown(otherPlayer, 0), 10);

                        /*
                         * Pixel collision currently doesn't work. For now, we'll just use Rectangle Collision.
                         */
                        if(IntersectPixels(thisPlayer.TransformMatrix, (int)(thisPlayer.GetWidth / thisPlayer.scale), 
                            (int)(thisPlayer.GetHeight / thisPlayer.scale), thisPlayer.sprite.GetData(),//thisPlayer.Get1DColorData, 
                            otherPlayer.TransformMatrix, (int)(otherPlayer.GetWidth / otherPlayer.scale),
                            (int)(otherPlayer.GetHeight / otherPlayer.scale), otherPlayer.sprite.GetData()))//otherPlayer.Get1DColorData))
                        {
                            Debug.WriteLine("Pixel Collision!");
                            if(thisPlayer.isAttacking)
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
                                player.Hit(instance.item);
                            Debug.WriteLine(player.Position.Y - instance.position.Y);
                            //if(item is BowlerHatInstance && Math.Abs(player.Position.Y - item.position.Y 
                        }
                    }
                }
            }
             * */
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case(boxingstate.idle):
                    spriteBatch.Draw(background, bounds, Color.White);
                    break;
                case(boxingstate.roundstart):
                    spriteBatch.Draw(background, bounds, Color.White);

                    level.Draw(spriteBatch);

                    // Draw the round start count down
                    string s = "Round Starting in\n" + (int)roundStartTimer;
                    spriteBatch.DrawString(font, s, new Vector2(bounds.Width / 2 - font.MeasureString(s).X,
                        bounds.Height / 2 - font.MeasureString(s).Y), Color.Yellow);

                    // Draw the players
                    foreach (BoxingPlayer player in players)
                    {
                        if (player != null)
                            player.Draw(gameTime, spriteBatch);
                    }

                    break;
                case(boxingstate.box):
                    spriteBatch.Draw(background, bounds, Color.White);

                    level.Draw(spriteBatch);

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
                                Debug.WriteLine("DRAWING BOUND");
                                spriteBatch.Draw(blank, playerRectangle, Color.Blue);
                            }

                            player.Draw(gameTime, spriteBatch);
                        }
                    }
                    break;
                case(boxingstate.stats):
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
                    string w = "Player " + winner + "Wins!";
                    spriteBatch.DrawString(font, w,
                        new Vector2(bounds.X + bounds.Width / 2 - font.MeasureString(w).X / 2,
                            bounds.Y + bounds.Height / 2 - font.MeasureString(w).Y / 2), Color.Goldenrod);
                    break;
                    
            }


            



            /*for (int i = 0; i < 4; i++)
            {
                displays[i].Draw(spriteBatch, font);

            }
            foreach (BoxingPlayer player in Players)
            {
                player.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(background, player.Hurtbox, Color.White * .5f);
            }

            foreach (ItemInstance item in itemInstances)
            {
                item.Draw(gameTime, spriteBatch);
                //spriteBatch.Draw(background, item.hitbox, Color.White * .5f);
            }

            string pass = "Press 'R' to skip";
            spriteBatch.DrawString(font, pass,
                new Vector2(bounds.Width - font.MeasureString(pass).X,
                    bounds.Height - font.MeasureString(pass).Y), Color.Black);
            */
        }

        #region Pixel Collision Helpers

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
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
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

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
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


        

        #endregion
    }
}
