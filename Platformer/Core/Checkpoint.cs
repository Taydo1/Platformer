using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Platformer.Core
{
    class Checkpoint : GameObject
    {
        public Checkpoint(float x, float y) :
            base(x, y, false, int.MaxValue, true)
        {
            position.Y -= 1;
        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side, List<GameObject> map)
        {
            base.ActionOnTouch(mobileElement, side, map);
            if (mobileElement is Player player)
            {
                player.Checkpoint = new Vector2(Left, Bottom - 1);
            }
        }
    }
}
