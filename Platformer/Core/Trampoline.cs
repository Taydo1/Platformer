using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Platformer.Core
{
    class Trampoline : Block
    {
        public Trampoline(float x, float y) :
            base(x, y+0.5f, true)
        {

        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side, List<GameObject> map)
        {
            if (side == 1) {
                mobileElement.SpeedY = -12;
            }
        }
    }
}
