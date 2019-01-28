using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer.Core
{
    class Player : MobileGameObject
    {
        public Player(float x, float y, float objectMass) :
            base(x, y, objectMass)
        {

        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ApplyForce(0, mass * gravity);

            if(Bottom > Platformer.Game1.WINDOW_HEIGHT)
            {
                ApplyForce(0, -mass * gravity);
                speed.Y = 0;
            }
        }

        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up))
            {
                speed.Y = -100;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 1;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                position.Y += 1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 1;
            }
        }
    }
}
