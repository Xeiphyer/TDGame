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
        public int START_POSITION_X;
        public int START_POSITION_Y;
        string WIZARD_ASSETNAME = "tower1";
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        List<Fireball> mFireballs = new List<Fireball>();
        ContentManager mContentManager;
        Rectangle mSource;
        int cooldown = 10;
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect pew;

        State mCurrentState;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;
        MouseState mouseState;
        MouseState lastMouseState;
        List<Tower> towers;

        Pcrane target;

        sprite range = new sprite();

        enum State
        {
            Tower,
            Button,
            Click
        }

        public bool changeMouse()
        {
            if (mCurrentState == State.Click && Stats.getGold() >= 50)
            {
                return true;
            }
            else
            {
                mCurrentState = State.Button;
                return false;
            }
        }

        public void setImage(ContentManager theContentManager, String str)
        {
            base.LoadContent(theContentManager, str);
        }

        public Rectangle getRange()
        {
            return base.Size;
        }

        public Pcrane getTarget()
        {
            return target;
        }

        public void setTarget(Pcrane newTarget)
        {
            target = newTarget;
        }

        public Tower(String str, int X, int Y)
        {
            START_POSITION_X = X;
            START_POSITION_Y = Y;
            setPosition(new Vector2(START_POSITION_X, START_POSITION_Y));
            towers = new List<Tower>();
            range.setPosition(new Vector2(START_POSITION_X - 100, START_POSITION_Y - 100));

            if (str == "Tower")
            {
                mCurrentState = State.Tower;
                range.scale = 2.5f;
            }
            else if (str == "Button")
            {
                mCurrentState = State.Button;
                range.scale = 0.01f;
            }
        }

        public List<Tower> getTowers()
        {
            return towers;
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

        public void reset()
        {
            towers.Clear();
            mFireballs.Clear();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.LoadContent(theContentManager);
            }

            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
            range.LoadContent(theContentManager, "circle");
            Source = new Rectangle(0, 0, 200, Source.Height);
            soundEngine = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
            soundEngineInstance = soundEngine.CreateInstance();
            //pew = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
        }

        public void Update(GameTime theGameTime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            UpdateFireball(theGameTime);
            if (target != null && target.dead() == true)
            {
                target = null;
            }
            range.Update(theGameTime, mSpeed, mDirection);
            base.Update(theGameTime, mSpeed, mDirection);
            UpdateClick(mouseState, lastMouseState);
        }

       /* public void Update(GameTime theGameTime, Vector2 XY)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            UpdateFireball(theGameTime, XY);
            range.Update(theGameTime, mSpeed, mDirection);
            base.Update(theGameTime, mSpeed, mDirection);
            UpdateClick(mouseState, lastMouseState);
        }*/

        private void UpdateClick(MouseState mousestate, MouseState lastmousestate)
        {
            if (mCurrentState == State.Click && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && Stats.getGold() >= 50)
            {
                Tower aTower = new Tower("Tower", mousestate.X - 35, mousestate.Y - 35);    // *X1* Center tower on mouse. Change to 1/2 texture size.
                aTower.Scale = 0.5f;
                aTower.LoadContent(mContentManager);
                towers.Add(aTower);
                mCurrentState = State.Button;
                Stats.setGold(Stats.getGold()-50);
            }
            if (mCurrentState == State.Button
                && mousestate.X > START_POSITION_X
                && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
                && mousestate.Y > START_POSITION_Y
                && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
            {
                //hover over button here
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    mCurrentState = State.Click;//button click here
                }
            }
        }

        private void UpdateFireball(GameTime theGameTime)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                if (target != null)
                {
                    aFireball.Update(theGameTime, target.getPosition());
                }
                else
                {
                    aFireball.setVisible(false);
                }
            }

            cooldown = --cooldown;

            if (cooldown <= 0 && target != null)
            {
                ShootFireball(target.getPosition());
                cooldown = 10;
                //soundEngine.Play();
            }
        }

        /* private void ShootFireball()
         {
                 if (mCurrentState == State.Tower)
                 {
                     bool aCreateNew = true;
                         foreach (Fireball aFireball in mFireballs)
                         {
                             if (aFireball.Visible == false)
                             {
                                 aCreateNew = false;
                                 aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(1, 0));
                                 break;
                             }
                         }

                     if (aCreateNew == true)
                     {
                         Fireball aFireball = new Fireball();
                         aFireball.LoadContent(mContentManager);
                         aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(1, 0));
                         mFireballs.Add(aFireball);
                     }
                 }
         }*/

        private void ShootFireball(Vector2 enemyPos)
        {
            foreach (Tower aTower in towers)
            {
                if (aTower.mCurrentState == State.Tower)
                {
                    bool aCreateNew = true;
                    foreach (Fireball aFireball in mFireballs)
                    {
                        if (aFireball.Visible == false)
                        {
                            aCreateNew = false;
                            aFireball.Fire(aTower.getPosition() + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(1, 0));
                            break;
                        }
                    }

                    if (aCreateNew == true)
                    {
                        Fireball aFireball = new Fireball();
                        aFireball.LoadContent(mContentManager);
                        aFireball.Fire(aTower.getPosition() + new Vector2(Size.Width / 2, Size.Height / 2), new Vector2(200, 200), new Vector2(1, 0));
                        mFireballs.Add(aFireball);
                    }
                }
            }
        }

        public List<Fireball> getList()
        {
            return mFireballs;
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
                foreach (Tower aTower in towers)
                {
                    aTower.Draw(theSpriteBatch);
                }
            }

            else
            {
                foreach (Tower aTower in towers)
                {
                    aTower.Draw(theSpriteBatch);
                }
                base.Draw(theSpriteBatch);
                range.Draw(theSpriteBatch);
            }

        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 pos)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, pos, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
