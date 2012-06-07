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

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;
        MouseState mouseState;
        MouseState lastMouseState;

        Pcrane target;
        sprite range = new sprite();
        bool towerStats = false;


        public void setImage(ContentManager theContentManager, String str)//changes the image for the tower
        {
            base.LoadContent(theContentManager, str);
        }

        public Rectangle getRange()//returns a rectange the size and position of the range
        {
            return new Rectangle((int)range.getPosition().X,(int)range.getPosition().Y,(int)(range.getWidth() * range.Scale), (int)(range.getHeight() * range.Scale));
        }

        public bool getTowerStats()//should the tower stats be displayed?
        {
            return towerStats;
        }
        public void setTowerStats(bool input)//sets if the tower stats should be displayed
        {
            towerStats = input;
            return;
        }

        public Pcrane getTarget()//get the target that the towers are shooting at
        {
            return target;
        }

        public void setTarget(Pcrane newTarget)//sets the target that the towers shoot at
        {
            target = newTarget;
        }

        public Tower(String str, int X, int Y)//constructor
        {
            START_POSITION_X = X;
            START_POSITION_Y = Y;
            setPosition(new Vector2(START_POSITION_X, START_POSITION_Y));
            range.scale = 2.5f;
            range.setPosition(new Vector2(START_POSITION_X - 130, START_POSITION_Y - 130));
        }

        public void reset()//clear all fireballs to reset the level
        {
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
            //Source = new Rectangle(0, 0, 200, Source.Height);
            soundEngine = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
            soundEngineInstance = soundEngine.CreateInstance();
            //pew = theContentManager.Load<SoundEffect>("Pew_Pew-DKnight556-1379997159");
        }

        public void Update(GameTime theGameTime)//updates fireballs, towers, mouse clicks
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

        private void UpdateClick(MouseState mousestate, MouseState lastmousestate)//checks to see if the tower has been clicked to display the stats on the sidebar
        {
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                towerStats = false;//tower click to turn off stats in sidebar
            }
             
            if (mousestate.X > START_POSITION_X
                && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
                && mousestate.Y > START_POSITION_Y
                && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    towerStats = true;//tower click to show stats in sidebar
                }
            }
        }

        private void UpdateFireball(GameTime theGameTime)//updates the fireballs
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

        private void ShootFireball(Vector2 enemyPos)//creates fireballs to shoot/repositions invisible fireballs to be reused
        {
                bool aCreateNew = true;
                foreach (Fireball aFireball in mFireballs)
                {
                    if (aFireball.Visible == false)
                    {
                        aCreateNew = false;
                        aFireball.Fire(getPosition() + new Vector2(getWidth() / 2, getHeight() / 2), new Vector2(200, 200), new Vector2(1, 0));
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Fireball aFireball = new Fireball();
                    aFireball.LoadContent(mContentManager);
                    aFireball.Fire(getPosition() + new Vector2(getWidth() / 2, getHeight() / 2), new Vector2(200, 200), new Vector2(1, 0));
                    mFireballs.Add(aFireball);
                }
        }

        public List<Fireball> getList()//gets the list of fireballs
        {
            return mFireballs;
        }

        public override void Draw(SpriteBatch theSpriteBatch)//draws all the fireballs and towers
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Draw(theSpriteBatch);
            }

            base.Draw(theSpriteBatch);

            MouseState mousestate = Mouse.GetState();

            if (mousestate.X > START_POSITION_X
            && mousestate.X < (START_POSITION_X + (int)(mSpriteTexture.Width * Scale))
            && mousestate.Y > START_POSITION_Y
            && mousestate.Y < (START_POSITION_Y + (int)(mSpriteTexture.Height * Scale)))
            {
                //hover over to see tower range
            }

            if (towerStats == true)
            {
                range.Draw(theSpriteBatch);
                theSpriteBatch.DrawString(base.font, "kills\nexperience\nsell price\nsome more shit\n...", new Vector2(820, 200), Color.Black);//text display on the sidebar
                scale = 1.5f;//scale up for the side window
                Draw(theSpriteBatch, new Vector2(850,50));
                scale = 0.5f;//scale back to the original size
            }
            
        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 pos)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, pos, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
