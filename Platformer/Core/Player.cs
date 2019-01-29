using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace Platformer.Core
{
    class Player : MobileGameObject
    {
        protected float direction;

        private const float playerSpeed = 100000;

        public Player(float x, float y, float objectMass, float objectMaxSpeed) :
            base(x, y, objectMass, objectMaxSpeed, true)
        {
            direction = 0;
        }

        public new void Update(GameTime gameTime, List<GameObject> solidObjectList)
        {
            base.Update(gameTime, solidObjectList);
            //ApplyForce(0, mass * gravity);

            if(direction == 1)
            {
                speed.X = playerSpeed;
            }
            else if(direction == -1)
            {
                speed.X = -playerSpeed;
            }
            else
            {
                speed.X = 0;
            }
        }

        public void DetectMove(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up))
            {
                speed.Y = -100;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                
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
    }
}
