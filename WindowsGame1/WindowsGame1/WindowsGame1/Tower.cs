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
    class Tower : sprite
    {
        const int START_POSITION_X = 100;
        const int START_POSITION_Y = 100;
        const string WIZARD_ASSETNAME = "tower1";
        List<Fireball> mFireballs = new List<Fireball>();
        ContentManager mContentManager;
        Rectangle mSource;

        enum State
        {
            Walking
        }

        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;

        public Tower()
        {
        }

        //The Rectangular area from the original image that 
        //defines the Sprite. 
         public Rectangle Source
        {
            get { return mSource; }

            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }



        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
            Source = new Rectangle(0, 0, 200, Source.Height);
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            //UpdateMovement(aCurrentKeyboardState);
            //UpdateJump(aCurrentKeyboardState);
            //UpdateDuck(aCurrentKeyboardState);
            UpdateFireball(theGameTime, aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateFireball(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.RightControl) == true && mPreviousKeyboardState.IsKeyDown(Keys.RightControl) == false)
            {
                ShootFireball();
            }
        }

        private void ShootFireball()
        {
            if (mCurrentState == State.Walking)
            {
                bool aCreateNew = true;
                foreach (Fireball aFireball in mFireballs)
                {
                    if (aFireball.Visible == false)
                    {
                        aCreateNew = false;
                        aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                        new Vector2(200, 0), new Vector2(1, 0));
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Fireball aFireball = new Fireball();
                    aFireball.LoadContent(mContentManager);
                    aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                    new Vector2(200, 200), new Vector2(1, 0));
                    mFireballs.Add(aFireball);
                }
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Draw(theSpriteBatch);
            }

            base.Draw(theSpriteBatch);
        }

    }
}
