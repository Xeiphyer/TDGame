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
        const string WIZARD_ASSETNAME = "Pcrane";
        int X = 0;
        int Y = 90;
        const int WIZARD_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        int leak = 0;

        enum State
        {
            Walking
        }

        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(X, Y);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            base.Update(theGameTime, mSpeed, mDirection);

        }

        public Vector2 getV()
        {
            Vector2 temp = new Vector2(X, Y);
            return temp;
        }

        public int leaks()
        {
            int temp = leak;
            leak = 0;
            return temp;
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (X < 180 && Y < 100)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                    X = X + MOVE_RIGHT;
                }
                if (X == 180 && Y < 145)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                    Y = Y + MOVE_DOWN;
                }
                if (X > 30 && Y == 145)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                    X = X + MOVE_LEFT;
                }
                if (X == 30 && Y < 190 && Y > 90)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                    Y = Y + MOVE_DOWN;
                }
                if (Y == 190 && X < 230)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                    X = X + MOVE_RIGHT;
                }
                if (X == 230)
                {
                    X = 0;
                    Y = 90;
                    Position = new Vector2(X, Y);
                    leak++;
                }

              /*  if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                    X = X + MOVE_LEFT;
                }

                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                    X = X + MOVE_RIGHT;
                }
                
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_UP;
                    Y = Y + MOVE_UP;
                }

                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                    Y = Y + MOVE_DOWN;
                }*/

            }
        }

    }
}
