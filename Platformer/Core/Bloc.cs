using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    class Block : GameObject
    {
        public Block(float x, float y, bool objectNeedBackground) :
            base(x, y, true, int.MaxValue, objectNeedBackground)
        {
            
        }
    }
}
