using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Auction_Boxing_2.Boxing.PlayerStates;

namespace Auction_Boxing_2
{

    #region Parent Class


    public abstract class Item
    {
        public float health, stamina, movement, attack, defense, cooldown, stun, casttime;
        public bool active = false;
        public Rectangle hitbox;

        public string name;
        public string ability_description;

        public Texture2D display_texture;
        public Texture2D instanceTexture;
        public Animation animation; // the texture for the players animation.

        public Texture2D icon;

        //public State playerState; // The state the player takes when item is active.

        //public AnimationPlayer sprite;
        //public Animation itemAnimation;

        // DOn't we already know what each items stats are going to be? So can't we make them defaults in the classes?
        public Item(Texture2D display_texture, Texture2D instance_texture, Texture2D icon)//float health, float stamina, float movement, float attack, float defense, float cooldown, 
            //string name, string ability_description)
        {
            this.display_texture = display_texture;
            this.instanceTexture = instance_texture;
            this.icon = icon;
        }
        //public abstract ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect);
        public abstract State GenerateState(int itemindex);
        public virtual void updatePosition(Vector3 newposition) { }
    }

    /*public class Cape : Item
    {
        public Cape(float health, float stamina, float movement, float attack, float defense, float cooldown, string name, string ability_description) :
            base()//health, stamina, movement, attack, defense, cooldown, name, ability_description)
        {

            //itemAnimation = new Animation(blah, blah);
            //hitbox = Animation.bounds;

        }

        public void itemAbility()
        {
            if (active)
            {
                //Projectile (slow)
                /* get playerposition
                 * add projectile to updatelist?
                 * 
                //Projectile (fast/instant)
                /* get playerposition/direction
                 * check if another player hitbox intersects item holders y-position and x-position
                 * is greater than item holders if item holder is facing right, or less than if item holder
                 * is facing left
                //Weapon
                 * reposition item hitbox relative to player depending weapon animation+range.
                 */
    /*
}
else
{
 // do some passive stuff, update position etc.
}
}
}*/


    #endregion

    #region Cane

    public class Cane : Item
    {


        public Cane(Texture2D displayTexture, Texture2D instanceTexture, Texture2D icon) :
            base(displayTexture, instanceTexture, icon)
        {
            health = 20;
            stamina = 30;
            movement = 80;
            attack = 5;
            defense = 10;
            cooldown = 500;
            stun = 900;
            casttime = 100;

            name = "Cane";
            ability_description = "Clobber - bonks the ruffian on the head!";

            
            //itemAnimation = new Animation(blah, blah);
            //hitbox = Animation.bounds;

        }

        //public override ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect)
        //{
        //    return new CaneInstance(this, instanceTexture, position, id, effect);
        //}

        public override State GenerateState(int itemindex)
        {
            return null;// new StateItemBasic(state, itemindex, this);
        }
    }

    #endregion

    #region Bowler_Hat

    public class Bowler_Hat : Item
    {
        public Bowler_Hat(Texture2D displayTexture, Texture2D instanceTexture, Texture2D icon) :
            base(displayTexture, instanceTexture, icon)
        {
            health = 30;
            stamina = 20;
            movement = 70;
            attack = 1;
            defense = 10;
            cooldown = 300; // in milliseconds
            stun = 100;
            casttime = 300;

            name = "Bowler Hat";
            ability_description = "Boomerang - thows the hat and it comes right back!";

            //itemAnimation = new Animation(blah, blah);
            //hitbox = Animation.bounds;

        }

        //public override ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect)
        //{
        //    return new BowlerHatInstance(this, instanceTexture, position, id, effect);
        //}

        public override State GenerateState(int itemindex)
        {
            return null;// new StateItemBasic(state, itemindex, this);
        }
    }

    #endregion

    #region Revolver

    public class Revolver : Item
    {
        public Revolver(Texture2D displayTexture, Texture2D instanceTexture, Texture2D icon) :
            base(displayTexture, instanceTexture, icon)
        {
            health = 5;
            stamina = 20;
            movement = 80;
            attack = 30;
            defense = 15;
            cooldown = 1000;
            stun = 300;
            casttime = 1000;

            name = "Revolver";
            ability_description = "Cap - Pop a cap in their bottom!";

        }

        //public override ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect)
        //{
        //    return new RevolverInstance(this, instanceTexture, position, id, effect);
        //}

        public override State GenerateState(int itemindex)
        {
            return null;// new StateItemBasic(state, itemindex, this);
        }
    }

    #endregion

    /*#region Monocle

    public class Monocle : Item
    {
        public Monocle(Texture2D displayTexture, Texture2D instanceTexture, Texture2D icon) :
            base(displayTexture, instanceTexture, icon)
        {
            health = 50;
            stamina = 45;
            movement = 85;
            attack = 1;
            defense = 25;
            cooldown = 100;
            stun = 1000;

            name = "Monocle";
            ability_description = "Laser - Stare them down with your laser monocle!";

            //itemAnimation = new Animation(blah, blah);
            //hitbox = Animation.bounds;

        }

        public override ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect)
        {
            return new MonocleInstance(this, instanceTexture, position, id, effect);
        }

        public override State GenerateState(int itemindex, PlayerDirection direction, State state)
        {
            return null;// new StateItemBasic(state, itemindex, this);
        }
    }

#endregion

    #region Boots

    public class Boots : Item
    {
        Animation aWindup;
        Animation aCharge;

        public Boots(Texture2D displayTexture, Texture2D instanceTexture, Texture2D icon,
            Texture2D animationTextureWindup, Texture2D animationTextureCharge) :
            base(displayTexture, instanceTexture, icon)
        {
            health = 10;
            stamina = 20;
            movement = 80;
            attack = 10;
            defense = 15;
            cooldown = 1000;
            stun = 800;

            name = "Boots";
            ability_description = "Charge - Run them down!";


            aWindup = new Animation(animationTextureWindup, .2f, true, 30);
            aCharge = new Animation(animationTextureCharge, .2f, true, 30);

        }

        public override ItemInstance GenerateInstance(Vector3 position, int id, SpriteEffects effect)
        {
            return new BootsInstance(this, instanceTexture, position, id, effect);
        }

        public override State GenerateState(int itemindex, PlayerDirection direction, State state)
        {
            return null;// new StateCharging(itemindex, this, direction, state, aWindup, aCharge);
        }
    }

    #endregion*/


}
