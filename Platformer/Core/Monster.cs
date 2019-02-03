using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    class Monster : MobileGameObject
    {
        public Monster(float x, float y):
            base(x, y, 60, true, true, int.MaxValue, Constants.initialPlayerAcceleration, Constants.maxPlayerSpeed)
        {
            direction = 1;
        }

        public override void Update(GameTime gameTime, List<GameObject> map, Vector2 shift)
        {
            if (Colision(screen, shift))
            {
                base.Update(gameTime, map, shift);

                //Print("side :" + collideSides);
                //Print("acceleration :" + acceleration.ToString());
                //Print("speed :" + speed.ToString());
                //Print("position :" + position.ToString());
                //Print("direction :" + direction.ToString());

                if ((collideSides & 2) == 0 || (collideSides & 1) != 0 || (collideSides & 4) != 0)
                {
                    direction = -direction;
                    textureDirection = -textureDirection;
                    Move(-2 * speed * (float)gameTime.ElapsedGameTime.TotalSeconds, map);
                }

            }
        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side)
        {
            base.ActionOnTouch(mobileElement, side);
            mobileElement.Die();
        }

        public override Texture2D[] Texture
        {
            get => base.Texture;
            set
            {
                base.Texture = value;
                Bottom = Top + 0.99f;
            }
        }
    }
}
