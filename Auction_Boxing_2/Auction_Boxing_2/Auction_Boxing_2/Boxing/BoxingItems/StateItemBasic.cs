using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Auction_Boxing_2
{
    /*public class StateItemBasic : State
    {
        const float SpeedFactor = 3f;

        const int DashTime = 100;

        const int Wait = 1; // i don't get it!

        GameTimer DashTimer;

        float timer = 1000; // one second windup
        bool isCasting = true;

        DirectionType Direction;

        Item item;
        int itemindex;

        public StateItemBasic(State state, int itemindex, Item item)
        {
            StateName = "casting";
            this.ATextures = state.StatePlayer.ATextures;
            this.Direction = Direction;
            this.itemindex = itemindex;

            this.item = item;

            timer = item.casttime;

            LoadState(state.StatePlayer, ATextures);

        }

        public override void LoadState(BoxingPlayer player, Dictionary<string, Animation> ATextures)
        {
            this.StatePlayer = player;
            this.PlayerAnimation = ATextures[StateName];
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
            if (timer <= 0 && isCasting)
            {
                StatePlayer.CreateInstance(itemindex);
                timer = item.cooldown;
                isCasting = false;
            }
            else if (timer <= 0 && !isCasting)
            {

                StatePlayer.InternalState = new StateMoving(this);

            }
        }

        public override void HandleMovement()
        {

        }

        public override void HandleDirection()
        {



        }

        public override void HandleState()
        {

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
           
        }
    }*/
}
