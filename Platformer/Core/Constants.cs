using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Core
{
    public static class Constants
    {
        public const int TileSize = 64;
        public const int TextureSize = 128;

        public const int WindowHoriTileNum = 5;
        public const int WindowVertTileNum = 8;

        public const int WindowWidth = WindowHoriTileNum * TileSize;
        public const int WindowHeight = WindowVertTileNum * TileSize;
    }
}
