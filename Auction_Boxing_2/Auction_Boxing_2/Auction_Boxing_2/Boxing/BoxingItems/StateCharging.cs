using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /*public class StateCharging : State
    {
        const float SpeedFactor = 3f;

        const int DashTime = 100;

        const int Wait = 1; // i don't get it!

        GameTimer DashTimer;

        Animation aWindup, aCharge;

        float timer = 1000; // one second windup
        bool isWinding = true;
        bool contact;

        DirectionType Direction;

        Item item;
        int itemindex;

        List<BoxingPlayer> hitPlayers = new List<BoxingPlayer>();

        public StateCharging(int itemindex, Item item, DirectionType Direction, State state, Animation animationWindup, Animation animationCharge)
        {
            StateName = "moving";
            this.ATextures = state.StatePlayer.ATextures;
            this.Direction = Direction;
            this.itemindex = itemindex;

            this.aWindup = animationWindup;
            this.aCharge = animationCharge;

            this.item = item;

            this.PlayerAnimation = aWindup; // Animation declared by the item!!

            LoadState(state.StatePlayer, ATextures);

        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;

            this.speed = new Vector2(Tools.speedCalc(StatePlayer.MaxMovement), Tools.Y_SCALE * Tools.speedCalc(StatePlayer.MaxMovement));

            DashTimer = new GameTimer(Wait, DashTime);

            DashTimer.Start();

            StatePlayer.isAttacking = false;
            StatePlayer.isHit = false;


        }

        public override void Initialize()
        {



        }



        public override void Update(GameTime gameTime)
        {
            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer <= 0 && isWinding)
            {
                StatePlayer.CreateInstance(itemindex);
                timer = 1000; // max charge time.
                isWinding = false;
                this.PlayerAnimation = aCharge;
            }
            else if (timer <= 0 && !isWinding)
            {
                if (Direction == DirectionType.Left)
                {
                    if (contact)
                    {
                        StatePlayer.InternalState = new StateJumping(DirectionType.Right, this); // jump away
                        foreach (BoxingPlayer player in hitPlayers)
                        {
                            State state = player.InternalState;
                            player.InternalState = new StateKnockedDown(speed.X, DirectionType.Left, state);
                        }
                    }
                    else
                        StatePlayer.InternalState = new StateMoving(this);
                }
                else
                {
                    if (contact)
                    {
                        StatePlayer.InternalState = new StateJumping(DirectionType.Left, this);
                        foreach (BoxingPlayer player in hitPlayers)
                        {
                            State state = player.InternalState;
                            player.InternalState = new StateKnockedDown(speed.X, DirectionType.Left, state);
                        }
                    }
                    else
                        StatePlayer.InternalState = new StateMoving(this);
                }
            }
        }

        public override void HandleMovement()
        {
            if (!isWinding)
            {
                switch (Direction)
                {
                    case DirectionType.Up:
                        Translate(0, SpeedFactor * speed.Y);
                        break;

                    case DirectionType.Down:
                        Translate(0, SpeedFactor * -speed.Y);
                        break;

                    case DirectionType.Right:
                        Translate(SpeedFactor * speed.X, 0);
                        break;

                    case DirectionType.Left:
                        Translate(SpeedFactor * -speed.X, 0);
                        break;

                    case DirectionType.None:

                        break;

                    default:

                        break;
                }
            }

        }

        public override void HandleDirection()
        {



        }

        public override void HandleState()
        {
            /*if (!DashTimer.Enabled)
                StatePlayer.InternalState = new StateMoving(this);
            Debug.WriteLine("Dash over!");
            if (StatePlayer.KeysDown.Contains(KeyPressed.Right) && Direction == DirectionType.Left)
            {
                if (contact)
                {
                    StatePlayer.InternalState = new StateJumping(DirectionType.Right, this); // jump away
                }
                else
                    StatePlayer.InternalState = new StateMoving(this);
            }

            else if (StatePlayer.KeysDown.Contains(KeyPressed.Left) && Direction == DirectionType.Right)
            {
                
                if (contact)
                {
                    StatePlayer.InternalState = new StateJumping(DirectionType.Left,this);
                }
                else
                    StatePlayer.InternalState = new StateMoving(this);
            }
        }

        public override void Translate(float x, float y)
        {
            Vector3 Temp = StatePlayer.Position;
            Temp.X += x;
            Temp.Y += y;
            StatePlayer.Position = Temp;

        }

        public override void HandleCollision(List<BoxingPlayer> Players)
        {
            if (!isWinding)
            {
                foreach (BoxingPlayer player in Players)
                {

                    if (player.playerindex != StatePlayer.playerindex && StatePlayer.hurtbox.Intersects(player.hurtbox))
                    {
                        int offset = StatePlayer.hurtbox.Width / 2;
                        if (Direction == DirectionType.Left)
                            offset *= -1;

                        player.position.X = StatePlayer.position.X + offset;

                        State state = player.InternalState;
                        player.InternalState = new StateHit(state, item);

                        bool alreadyhit = false;
                        foreach (BoxingPlayer hitplayer in hitPlayers)
                        {
                            if (player.playerindex == hitplayer.playerindex)
                                alreadyhit = true;
                        }
                        if (!alreadyhit)
                        {
                            hitPlayers.Add(player);
                            player.CurrentHealth -= item.attack;
                        }

                        contact = true;
                    }
                }
            }
        }
    }*/
}
