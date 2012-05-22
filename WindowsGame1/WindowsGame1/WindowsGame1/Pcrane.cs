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
        public int X = 0;
        public int Y = 90;
        const int WIZARD_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        int leak = 0;
        int Hp = 1;
        bool Visible = true;

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

        public void hit(int dmg)
        {
            Hp = Hp - dmg;
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            base.Update(theGameTime, mSpeed, mDirection);
            if (Hp <= 0)
            {
                this.Visible = false;
            }

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
            mSpeed = Vector2.Zero;
            mDirection = Vector2.Zero;

            if (Visible == true)
            {
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

        public override void Draw(SpriteBatch theSpriteBatch)//Draw the sprite to the screen
        {
            if (Visible == true)
            {
                theSpriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
        }

    }
}
