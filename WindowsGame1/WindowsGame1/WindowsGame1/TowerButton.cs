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
    class TowerButton : sprite
    {
        State mCurrentState;
        int START_POSITION_X;
        int START_POSITION_Y;
        string WIZARD_ASSETNAME = "tower2";
        ContentManager mContentManager;
        Vector2 mStartingPosition = Vector2.Zero;
        MouseState mouseState;
        MouseState lastMouseState;
        List<Tower> towers;

        // grid vars
        int TileX;
        int TileY;
        int tileWidth = 34;
        int tileHeight = 34;
        int squarePositionX;
        int squarePositionY;

        enum State//2 button states
        {
            Button,
            Click
        }

        public TowerButton(int x, int y)//constructor
        {
            towers = new List<Tower>();
            START_POSITION_X = x;
            START_POSITION_Y = y;
            setPosition(new Vector2(x, y));
            mCurrentState = State.Button;
            scale = 3.0f;
        }

        public void LoadContent(ContentManager theContentManager)//loads image
        {
            mContentManager = theContentManager;
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
        }

        public void setImage(ContentManager theContentManager, String str)//changes the image for the tower
        {
            base.LoadContent(theContentManager, str);
        }

        public List<Tower> getTowers()//get the list of all the towers
        {
            return towers;
        }

        public List<Fireball> getFire()//gets all the fireball lists from each tower combines them and returns a list
        {
            List<Fireball> temp = new List<Fireball>();

            foreach (Tower atower in towers)
            {
                temp.AddRange(atower.getList());
            }

            return temp;
        }

        public void reset()//clear all the towers and fireballs to reset the level
        {
            towers.Clear();
            foreach(Tower atower in towers)
            {
                atower.reset();
            }
        }

        public void Update(GameTime theGameTime)//checks to see if it was clicked
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            UpdateClick(mouseState, lastMouseState);
            foreach (Tower atower in towers)
            {
                atower.Update(theGameTime);
            }
        }

        private void UpdateClick(MouseState mousestate, MouseState lastmousestate)
        {
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
                Stats.setGold(Stats.getGold() - 50);
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

        public override void Draw(SpriteBatch theSpriteBatch)//draws all the fireballs and towers
        {
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
                bool flag = false;
                foreach (Tower aTower in towers)
                {
                    aTower.Draw(theSpriteBatch);
                    if (aTower.getTowerStats() == true)
                    {
                        flag = true;
                    }
                }

                if (flag == false)
                {
                    base.Draw(theSpriteBatch);
                }

                MouseState mousestate = Mouse.GetState();
            }

        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 pos)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, pos, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }


    }
}
