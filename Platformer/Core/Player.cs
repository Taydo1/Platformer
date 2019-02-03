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

        private Vector2 lastCheckpoint;

        private TimeSpan nextShotTime;
        private TimeSpan rechargingDuration;

        public Player(float x, float y) :
            base(x, y, 60, true, true, 30, Constants.initialPlayerAcceleration, Constants.maxPlayerSpeed)
        {
            nextShotTime = new TimeSpan(0);
            rechargingDuration = new TimeSpan(0, 0, 0, 0, 500);
            lastCheckpoint = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime, List<GameObject> map)
        {
            base.Update(gameTime, map);
            //Print("side :" + collideSides);
            //Print("sideBlock : "+sideBlocks[0].Count + " " + sideBlocks[1].Count + " " + sideBlocks[2].Count + " " + sideBlocks[3].Count + " " + sideBlocks[4].Count);
            //Print("acceleration :" + acceleration.ToString());
            //Print("speed :" + speed.ToString());
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

        public void DetectMove(KeyboardState keyState, GamePadState padState, List<GameObject> map, GameTime gameTime, Texture2D shotTexture)
        {
            if (padState.ThumbSticks.Left.X != 0)
            {
                direction = (float)Math.Pow(padState.ThumbSticks.Left.X, 7);
                textureDirection = Math.Sign(direction);
            }
            else if (keyState.IsKeyDown(Keys.Left) == keyState.IsKeyDown(Keys.Right))
            {
                direction = -0.1f * speed.X;
                if (direction > 1) direction = 1;
                else if (direction < -1) direction = -1;
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                direction = 1;
                textureDirection = 1;
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                direction = -1;
                textureDirection = -1;
            }


            if ((keyState.IsKeyDown(Keys.Up) || padState.Buttons.A == ButtonState.Pressed)
                && ((collideSides & 2) != 0 || ((collideSides & 1) != 0 && direction < 0) || ((collideSides & 4) != 0 && direction > 0)))
            {
                speed.Y = Constants.initialPlayerJump;
            }
            if (keyState.IsKeyDown(Keys.Down) || padState.Buttons.B == ButtonState.Pressed)
            {
                if (speed.Y < 0)
                {
                    AddForce(new Vector2(0, -500f * speed.Y));
                }
            }

            if (keyState.IsKeyDown(Keys.LeftControl) || padState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (speed.Y > 0 && ((collideSides & 1) != 0 || (collideSides & 4) != 0))
                {
                    speed.Y = 0;
                }
            }


            if ((keyState.IsKeyDown(Keys.Space) || padState.Buttons.RightShoulder == ButtonState.Pressed)
                && gameTime.TotalGameTime > nextShotTime)
            {
                Shot shot = new Shot(Left, CenterVert, textureDirection);
                shot.Texture = new[] { shotTexture };
                map.Add(shot);

                nextShotTime = gameTime.TotalGameTime + rechargingDuration;
            }
        }

        public override void Die()
        {
            base.Die();
            CenterHori = lastCheckpoint.X + size.X/2;
            CenterVert = lastCheckpoint.Y + 0.99f - size.Y/2;
            speed = Vector2.Zero;
        }

        public Vector2 Checkpoint {
            set => lastCheckpoint = new Vector2(value.X, value.Y);
        }

        public static void UpdateScrollBoxes()
        {
            scrollBoxTopLeft = new Vector2(Constants.WindowHoriTileNum / 2 - 1f, Constants.WindowVertTileNum / 2);
            scrollBoxBottomRight = new Vector2(Constants.WindowHoriTileNum / 2 + 1f, Constants.WindowVertTileNum / 2 + 2.5f);
        }
        
    }
}
