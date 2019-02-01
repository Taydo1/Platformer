using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    public static class Constants
    {
        public const int TileSize = 64;
        public const int TextureSize = 128;

        public static int WindowWidth = 960;
        public static int WindowHeight = 540;

        public static float WindowHoriTileNum = (float)WindowWidth / TileSize;
        public static float WindowVertTileNum = (float)WindowHeight / TileSize;



        public const float playerSpeed = 3f;
        public const float playerJump = -7.9f;

    }
}
