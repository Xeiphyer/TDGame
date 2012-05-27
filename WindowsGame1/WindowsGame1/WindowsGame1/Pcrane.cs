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
        public bool Visible = true;
        int cash = 0;

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
            Position = new Vector2(X, Y);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
        }

        public void hit(int dmg)
        {
            Hp = Hp - dmg;
        }

        public Rectangle getRec()
        {
            return base.Size;
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
                cash = 10;
                mCurrentState = State.Dead;
            }

        }

        public Vector2 getV()
        {
            Vector2 temp = new Vector2(Position.X, Position.Y);
            return temp;
        }

        public int leaks()
        {
            int temp = leak;
            leak = 0;
            return temp;
        }

        public int bounty()
        {
            int temp = cash;
            cash = 0;
            return temp;
        }

        private void UpdateMovement()
        {
            mSpeed = Vector2.Zero;
            mDirection = Vector2.Zero;

            if (Visible == true)
            {
                if (Position.X < 600 && Position.Y < 250)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                if (Position.X >= 600 && Position.Y < 270)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
                if (Position.X > 100 && Position.Y >= 270)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                if (Position.X <= 100 && Position.Y < 440 && Position.Y >= 270)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
                if (Position.Y >= 440 && Position.X < 770)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                if (Position.X >= 770)
                {
                    X = 0;
                    Y = 90;
                    Position = new Vector2(X, Y);
                    leak++;
                }
            }
        }

        public Vector2 getPos()
        {
            return Position;
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
