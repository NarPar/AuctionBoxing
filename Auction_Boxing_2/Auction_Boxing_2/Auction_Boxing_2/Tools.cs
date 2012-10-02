using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//CHANGED! ADD ParseText()

namespace Auction_Boxing_2
{

    

    class Tools 
    {
        public const float BASE_HEALTH = 100;
        public const float BASE_STAMINA = 100;
        public const float BASE_MOVEMENT = 100;
        public const float BASE_COOLDOWN = 100;
        public const int JUMP_HEIGHT = 20;
        public const int HEIGHT = 60;
        public const int WIDTH = 50;
        public const float Y_SCALE = .5f;
        public const int HITBOX_RANGE = 10;
        protected const float SCALOR_FACTOR = .03f;

        // Reset the max's?
        public static float displayMaxHealth = 100;
        public static float displayMaxStamina = 100;
        public static float displayMaxMovement = 100;
        public static float displayMaxAttack = 50;
        public static float displayMaxDefense = 30;
        public static float displayMaxCooldown = 1000;


        public static String ParseText(SpriteFont font, String text, Rectangle bounds, ref int line_count)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > bounds.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;

                    line_count++;
                }
                line = line + word + ' ';
            }
            return returnString + line;
        }

        public static bool InRange(int x1, int x2)
        {
            int temp = Math.Abs(x1 - x2);
            if (temp < HITBOX_RANGE)
                return true;
            else
                return false;

        }

        public static Vector2 toVector2(Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector3 toVector3(Vector2 vector)
        {

            return new Vector3(vector.X, vector.Y, JUMP_HEIGHT);
        }

        public static float speedCalc(float maxmovement)
        {
            return maxmovement * SCALOR_FACTOR;
        }

        


    }
}
