

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Platformer.Core
{
    class Spike : Block
    {
        int side;
        public Spike(float x, float y, int spikeSide) :
            base(x, y, true)
        {
            side = spikeSide;
        }

        public override Texture2D[] Texture
        {
            get => Texture;
            set
            {
                base.Texture = value;
                switch (side)
                {
                    case 0:
                        size = new Vector2((float)texture[currentTexture].Height / Constants.TextureSize, (float)texture[currentTexture].Width / Constants.TextureSize);
                        size *= .95f;
                        Right = Left + 1;
                        CenterVert = Top + 0.5f;
                        rotationAngle = (float)(-Math.PI/2);
                        break;
                    case 1:
                        size *= .95f;
                        Bottom = Top + 1;
                        CenterHori = Left + 0.5f;
                        break;
                    case 2:
                        size = new Vector2((float)texture[currentTexture].Height / Constants.TextureSize, (float)texture[currentTexture].Width / Constants.TextureSize);
                        size *= .95f;
                        CenterVert = Top + 0.5f;
                        rotationAngle = (float)(Math.PI / 2);
                        break;
                    case 3:
                        size *= .95f;
                        CenterHori = Left + 0.5f;
                        rotationAngle = (float)Math.PI;
                        break;
                }
            }
        }

        public override void ActionOnTouch(MobileGameObject mobileElement, int side)
        {
            base.ActionOnTouch(mobileElement, side);
            mobileElement.Die();
        }
    }
}
