﻿using System.Collections.Generic;

namespace Platformer.Core
{
    class Ice : Block
    {
        public Ice(float x, float y) :
            base(x, y, false)
        {

        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side, List<GameObject> map)
        {
            if (side == 1)
            {
                mobileElement.CurrentHorizontalAcceleration = mobileElement.DefaultHorizontalAcceleration / 10;
            }
        }
    }
}
