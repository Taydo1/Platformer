namespace Platformer.Core
{
    class Shot : MobileGameObject
    {
        public Shot(float x, float y, int shotDirection) :
            base(x, y, 0, false, false, 30, Constants.initialShotAcceleration, Constants.shotSpeed)
        {
            direction = shotDirection;
            textureDirection = shotDirection;
        }
    }
}
