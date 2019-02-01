using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Platformer.Core
{
    class Player : MobileGameObject
    {
        private static Vector2 scrollBoxTopLeft = new Vector2(Constants.WindowHoriTileNum / 2 - 1f, Constants.WindowVertTileNum / 2);
        private static Vector2 scrollBoxBottomRight = new Vector2(Constants.WindowHoriTileNum / 2 + 1f, Constants.WindowVertTileNum / 2 + 2.5f);

        protected float direction;



        public Player(float x, float y, float objectMass) :
            base(x, y, objectMass, true, true)
        {
            direction = 0;
        }

        public override void Update(GameTime gameTime, List<GameObject> solidObjectList)
        {
            base.Update(gameTime, solidObjectList);

            speed.X = Constants.playerSpeed * direction;


            //Print("side :" + collideSides);
            //Print("acceleration :" + acceleration.ToString());
            //Print("speed :" + speed.ToString());
            //Print("position :" + position.ToString());
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

        public void DetectMove(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up) && ( (collideSides&1) !=0 || (collideSides & 2) != 0 || (collideSides & 4) != 0))
            {
                speed.Y = Constants.playerJump;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                if (speed.Y < 0)
                {
                    speed.Y = 0;
                }
            }

            if (state.IsKeyDown(Keys.Left) == state.IsKeyDown(Keys.Right))
            {
                direction = 0;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                direction = -1;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                direction = 1;
            }
        }

        public static void UpdateScrollBoxes()
        {
            scrollBoxTopLeft = new Vector2(Constants.WindowHoriTileNum / 2 - 1f, Constants.WindowVertTileNum / 2);
            scrollBoxBottomRight = new Vector2(Constants.WindowHoriTileNum / 2 + 1f, Constants.WindowVertTileNum / 2 + 2.5f);
        }
    }
}
