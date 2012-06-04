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
        string WIZARD_ASSETNAME = "tower2";
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
        bool towerStats = false;

        // grid vars
        int TileX;
        int TileY;
        int tileWidth = 34;
        int tileHeight = 34;
        int squarePositionX;
        int squarePositionY;

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
            return new Rectangle((int)range.getPosition().X,(int)range.getPosition().Y,(int)(range.getWidth() * range.Scale), (int)(range.getHeight() * range.Scale));
        }

        public bool getTowerStats()
        {
            return towerStats;
        }
        public void setTowerStats(bool input)
        {
            towerStats = input;
            return;
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
 
            if (str == "Tower")
            {
                mCurrentState = State.Tower;
                range.scale = 2.5f;
                range.setPosition(new Vector2(START_POSITION_X - 130, START_POSITION_Y - 130));
            }
            else if (str == "Button")
            {
                mCurrentState = State.Button;
                scale = 3.0f;
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
            foreach (Tower atower in towers)
            {
                atower.Update(theGameTime);
            }
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
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                towerStats = false;//tower click to turn off stats in sidebar
            }
            if (mCurrentState == State.Click && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && mousestate.X < 800 && Stats.getGold() >= 50)
            {




                // **x2** 




                //showGrid(1);

                TileX = (int)Math.Floor((float)(mousestate.X / tileWidth));
                TileY = (int)Math.Floor((float)(mousestate.Y / tileHeight));
                squarePositionX = (TileX * tileWidth) + (tileWidth / 2);
                squarePositionY = (TileY * tileHeight) + (tileHeight / 2);

                Tower aTower = new Tower("Tower", squarePositionX - 30, squarePositionY - 30);    // *X1* Center tower on mouse. Change to 1/2 texture size.
                aTower.Scale = 0.5f;
                aTower.LoadContent(mContentManager);
                towers.Add(aTower);
                mCurrentState = State.Button;
                Stats.setGold(Stats.getGold()-50);
            }
            
            if (mCurrentState == State.Tower
                && mousestate.X > START_POSITION_X
                && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
                && mousestate.Y > START_POSITION_Y
                && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    towerStats = true;//tower click to show stats in sidebar
                }
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

        public void showGrid(int status)
        {
            int x = 0;
            int y = 30;
            if (status == 0)
            {

                while (x < 800)
                {
                    while (y < 600)
                    {
                        // **x3** gonna make it draw a gridsquare on each valid tower placement spot
                        //    It'll skip the already placed tower spots, and eventually the lane
                        //    We need variables to track it though. Not sure how we are gonna do it.
                    }

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
                            aFireball.Fire(aTower.getPosition() + new Vector2(getWidth() / 2, getHeight() / 2), new Vector2(200, 200), new Vector2(1, 0));
                            break;
                        }
                    }

                    if (aCreateNew == true)
                    {
                        Fireball aFireball = new Fireball();
                        aFireball.LoadContent(mContentManager);
                        aFireball.Fire(aTower.getPosition() + new Vector2(getWidth() / 2, getHeight() / 2), new Vector2(200, 200), new Vector2(1, 0));
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

                MouseState mousestate = Mouse.GetState();

                if (mCurrentState == State.Tower
                && mousestate.X > START_POSITION_X
                && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
                && mousestate.Y > START_POSITION_Y
                && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
                {
                    range.Draw(theSpriteBatch);//hover over to see tower range
                }
                if (towerStats == true)
                {
                    theSpriteBatch.DrawString(base.font, "kills\nexperience\nsell price\nsome more shit\n...", new Vector2(820, 300), Color.Black);

                }
            }

        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 pos)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, pos, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
