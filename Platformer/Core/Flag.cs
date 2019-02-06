using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Platformer.Core
{
    class Flag : Checkpoint
    {
        public Flag(float x, float y):
            base(x, y)
        {

        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side, List<GameObject> map)
        {
            base.ActionOnTouch(mobileElement, side, map);
            if (mobileElement is Player player)
            {
                player.Win = true;
            }
        }
    }
}
