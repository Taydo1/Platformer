using Microsoft.Xna.Framework;

namespace Platformer.Core
{
    class Checkpoint : GameObject
    {
        public Checkpoint(float x, float y) :
            base(x, y, false, int.MaxValue, true)
        {
            position.Y -= 1;
        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side)
        {
            base.ActionOnTouch(mobileElement, side);
            if (mobileElement is Player player)
            {
                player.Checkpoint = new Vector2(Left, Bottom - 1);
            }
        }
    }
}
