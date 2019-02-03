using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Platformer.Core
{
    class Shot : MobileGameObject
    {
        public Shot(float x, float y, int shotDirection) :
            base(x, y, 0, false, true, 30, Constants.initialShotAcceleration, Constants.shotSpeed)
        {
            direction = shotDirection;
            textureDirection = shotDirection;
        }

        public override void Update(GameTime gameTime, List<GameObject> map, Vector2 shift)
        {
            base.Update(gameTime, map, shift);
            if((collideSides&1)!=0 || (collideSides & 4) != 0)
            {
                Die(map);
                int cote;
                if((collideSides & 1) != 0)
                {
                    cote = 0;
                }
                else
                {
                    cote = 2;
                }

                for (int i = 0; i < sideBlocks[cote].Count; i++)
                {
                    if (sideBlocks[cote][i] is MobileGameObject mobileElement)
                    {
                        mobileElement.Die(map);
                    }
                }
            }
        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side, List<GameObject> map)
        {
            base.ActionOnTouch(mobileElement, side, map);
            Print("ls;mlzm");
            mobileElement.Die(map);
        }
    }
}
