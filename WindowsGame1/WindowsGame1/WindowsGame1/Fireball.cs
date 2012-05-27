using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace WindowsGame1
{
    class Fireball : sprite
    {
        const int MAX_DISTANCE = 500;//range
        public bool Visible = false;
        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        public Fireball()
        {

        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "Fireball");
            Scale = 0.3f;
        }

        public void Update(GameTime theGameTime, Vector2 XY)//XY is from Pcrane
        {
            if (Vector2.Distance(mStartPosition, Position) > MAX_DISTANCE)
            {
                Visible = false;
                //Position = mStartPosition;
            }

            if (Visible == true)
            {
                if (Position.X == XY.X)
                {
                    mDirection.X = 0;
                }
                
                else if (Position.X > XY.X)
                {
                    mDirection.X = MOVE_LEFT;
                }

                else if (Position.X < XY.X)
                {
                    mDirection.X = MOVE_RIGHT;
                }

                if (Position.Y == XY.Y)
                {
                    mDirection.Y = 0;
                }

                else if (Position.Y > XY.Y)
                {
                    mDirection.Y = MOVE_UP;
                }

                else if (Position.Y < XY.Y)
                {
                    mDirection.Y = MOVE_DOWN;
                }

            }
            base.Update(theGameTime, mSpeed, mDirection);
        }


        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
               // base.Draw(theSpriteBatch);
                base.Draw(theSpriteBatch);
            }
        }

        public Vector2 getPos()
        {
            return Position;
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
            Visible = true;
            // int X = (int)mStartPosition.X;
            // int Y = (int)mStartPosition.Y;
        }



    }
}