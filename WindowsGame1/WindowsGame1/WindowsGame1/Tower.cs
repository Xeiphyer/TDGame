using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame1
{
    class Tower : sprite
    {
        int START_POSITION_X;
        int START_POSITION_Y;
        const string WIZARD_ASSETNAME = "tower1";
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        List<Fireball> mFireballs = new List<Fireball>();
        ContentManager mContentManager;
        Rectangle mSource;
        int cooldown = 100;
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect pew;

        State mCurrentState;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;
        MouseState mouseState;
        MouseState lastMouseState;

        enum State
        {
            Tower,
            Button,
            Click
        }
        
        public Tower(String str,int X, int Y)
        {
            START_POSITION_X = X;
            START_POSITION_Y = Y;
            if(str == "Tower")
            {
                mCurrentState = State.Tower;
            }
            else if(str == "Button")
            {
                mCurrentState = State.Button;
            }
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
            soundEngine = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
            soundEngineInstance = soundEngine.CreateInstance();
            pew = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
        }

        public void Update(GameTime theGameTime,Vector2 XY)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            UpdateClick(mouseState, lastMouseState);
            UpdateFireball(theGameTime, XY);
            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateClick(MouseState mousestate, MouseState lastmousestate)
        {
            if (mCurrentState == State.Button
                && mousestate.X > START_POSITION_X
                && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
                && mousestate.Y > START_POSITION_Y
                && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
            {
                //soundEngine.Play();
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                 {
                     mCurrentState = State.Click;
                 }

            }
        }

        private void UpdateFireball(GameTime theGameTime, Vector2 XY)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime, XY);
            }

            cooldown = --cooldown;

            if (cooldown <= 0)
            {
                ShootFireball();
                cooldown = 100;
                soundEngine.Play();
            }
        }

        private void ShootFireball()
        {
            if (mCurrentState == State.Tower)
            {
                bool aCreateNew = true;
            /*    foreach (Fireball aFireball in mFireballs)
                {
                    if (aFireball.Visible == false)
                    {
                        aCreateNew = false;
                        aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(-1, -1));
                        break;
                    }
                }*/

                if (aCreateNew == true)
                {
                    Fireball aFireball = new Fireball();
                    aFireball.LoadContent(mContentManager);
                    aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(1, 0));
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
            if (mCurrentState == State.Click)
            {
                base.Draw(theSpriteBatch, Color.Red);
                mCurrentState = State.Button;
            }
            else
            {
                base.Draw(theSpriteBatch);
            }
        }

    }
}
