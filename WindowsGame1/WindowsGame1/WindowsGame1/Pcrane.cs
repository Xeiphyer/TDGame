using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace WindowsGame1
{
    class Pcrane : sprite
    {
        const string ASSETNAME = "Pcrane";
        const int WIZARD_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        int MAX_HP;//changes the Hp of cranes
        int Hp;//keeps track of hp
        public bool Visible = true;
        Texture2D HealthBar;
        //sprite HealthBar;

        enum State
        {
            Walking,
            Dead
        }

        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        public void LoadContent(ContentManager theContentManager)
        {
            setPosition(new Vector2(0, 90));
            base.LoadContent(theContentManager, ASSETNAME);
            HealthBar = theContentManager.Load<Texture2D>("HealthBar_thumb") as Texture2D;
            Hp = MAX_HP;
        }

        public void hit(int dmg)
        {
            Hp = Hp - dmg;
            Hp = (int)MathHelper.Clamp(Hp, 0, MAX_HP);//keeps Hp between 0 and 100 for health bar
        }

        public void setHp(int newHp)
        {
            MAX_HP = newHp;
            Hp = MAX_HP;
        }

        public bool dead()
        {
            if (mCurrentState == State.Dead)
            {
                return true;
            }
            else
                return false;
        }

        public void Update(GameTime theGameTime)
        {
            this.UpdateMovement();

            base.Update(theGameTime, mSpeed, mDirection);
            if (Hp <= 0 && mCurrentState == State.Walking)
            {
                this.Visible = false;
                Stats.setGold(Stats.getGold() + 10);
                mCurrentState = State.Dead;
            }

        }

        private void UpdateMovement()
        {
            mSpeed = Vector2.Zero;
            mDirection = Vector2.Zero;

            if (Visible == true)
            {
                if (getPosition().X < 600 && getPosition().Y < 250)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                if (getPosition().X >= 600 && getPosition().Y < 270)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
                if (getPosition().X > 100 && getPosition().Y >= 270)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                if (getPosition().X <= 100 && getPosition().Y < 440 && getPosition().Y >= 270)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
                if (getPosition().Y >= 440 && getPosition().X < 770)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                if (getPosition().X >= 770)
                {
                    setPosition(new Vector2(0, 90));
                    Stats.setLives(Stats.getLives() - 1);
                }
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch, Color clr)//Draw the sprite to the screen
        {
            if (Visible == true)
            {
                
                theSpriteBatch.Draw(mSpriteTexture, getPosition(), new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), clr, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);

                //backround of health bar
                theSpriteBatch.Draw(HealthBar, new Vector2 (base.getPosition().X, base.getPosition().Y + (180*scale)), new Rectangle(0, 45, HealthBar.Width, 13), Color.Gray);
                
                //Draw the current health level based on the current Health
                theSpriteBatch.Draw(HealthBar, new Rectangle((int)base.getPosition().X, (int)base.getPosition().Y + (int)(180 * scale), (int)(HealthBar.Width * ((double)Hp / MAX_HP)), 13), new Rectangle(0, 45, HealthBar.Width, 44), Color.Red);

                //Draw the box around the health bar
                theSpriteBatch.Draw(HealthBar, new Rectangle((int)base.getPosition().X, (int)base.getPosition().Y + (int)(180 * scale), HealthBar.Width, 14), new Rectangle(0, 0, HealthBar.Width, 14), Color.White);
            }
        }

    }
}
