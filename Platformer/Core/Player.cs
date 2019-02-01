using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System;

namespace Platformer.Core
{
    class Player : MobileGameObject
    {
        private static Vector2 scrollBoxTopLeft = new Vector2(Constants.WindowHoriTileNum / 2 - 1f, Constants.WindowVertTileNum / 2);
        private static Vector2 scrollBoxBottomRight = new Vector2(Constants.WindowHoriTileNum / 2 + 1f, Constants.WindowVertTileNum / 2 + 2.5f);


        public Player(float x, float y, float objectMass) :
            base(x, y, objectMass, true, true, Constants.initialPlayerAcceleration, Constants.maxPlayerSpeed)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> solidObjectList)
        {
            base.Update(gameTime, solidObjectList);
            //Print("side :" + collideSides);
            //Print("acceleration :" + acceleration.ToString());
            Print("speed :" + speed.ToString());
            //Print("position :" + position.ToString());
            //Print("direction :" + direction.ToString());
        }

        public void UpdateShift(ref Vector2 shift)
        {
            while (Left + shift.X < scrollBoxTopLeft.X)
            {
                shift.X += 0.001f;
            }
            while (Right + shift.X > scrollBoxBottomRight.X)
            {
                shift.X -= 0.001f;
            }
            while (Top + shift.Y < scrollBoxTopLeft.Y)
            {
                shift.Y += 0.001f;
            }
            while (Bottom + shift.Y > scrollBoxBottomRight.Y)
            {
                shift.Y -= 0.001f;
            }

        }

        public void DetectMove(KeyboardState state, KeyboardState previousState, List<GameObject> solidObjectList)
        {
            if (state.IsKeyDown(Keys.Left) == state.IsKeyDown(Keys.Right))
            {
                direction = -0.1f * speed.X;
                if (direction > 1) direction = 1;
                else if (direction < -1) direction = -1;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                direction = -1;
                if(speed.Y > 0 && (collideSides & 4) != 0)
                {
                    speed.Y = 0;
                }
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                direction = 1;
                if (speed.Y > 0 && (collideSides & 1) != 0)
                {
                    speed.Y = 0;
                }
            }

            if (state.IsKeyDown(Keys.Up)
                && ((collideSides & 2) != 0 || ((collideSides & 1) != 0 && direction==-1) ||( (collideSides & 4) != 0 && direction == 1)))
            {
                speed.Y = Constants.initialPlayerJump;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                if (speed.Y < 0)
                {
                    speed.Y = 0;
                }
            }
        }

        public static void UpdateScrollBoxes()
        {
            scrollBoxTopLeft = new Vector2(Constants.WindowHoriTileNum / 2 - 1f, Constants.WindowVertTileNum / 2);
            scrollBoxBottomRight = new Vector2(Constants.WindowHoriTileNum / 2 + 1f, Constants.WindowVertTileNum / 2 + 2.5f);
        }
    }
}
