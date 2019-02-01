﻿namespace Platformer.Core
{
    class Trampoline : Block
    {
        public Trampoline(float x, float y) :
            base(x, y+0.5f)
        {

        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side)
        {
            if (side == 1) {
                mobileElement.SpeedY = -12;
            }
        }
    }
}
