using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Platformer.Core
{
    class GameObject
    {
        protected Vector2 position;
        protected Vector2 size;
        protected Texture2D texture;
        protected bool isSolid;


        public GameObject(float x, float y, bool isObjectSolid)
        {
            position = new Vector2(x, y);
            isSolid = isObjectSolid;
            texture = null;
            size = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position * Constants.TileSize, null, Color.White, 0, Vector2.Zero, (float)Constants.TileSize/Constants.TextureSize, SpriteEffects.None, 0);


            //Game1.DrawLine(spriteBatch, TopLeft * Constants.TileSize, TopRight * Constants.TileSize);
            //Game1.DrawLine(spriteBatch, TopRight * Constants.TileSize, BottomRight * Constants.TileSize);
            //Game1.DrawLine(spriteBatch, BottomRight * Constants.TileSize, BottomLeft * Constants.TileSize);
            //Game1.DrawLine(spriteBatch, BottomLeft * Constants.TileSize, TopLeft * Constants.TileSize);
        }


        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                size = new Vector2((float)texture.Width / Constants.TextureSize, (float)texture.Height / Constants.TextureSize);
            }
        }

        public float DistanceCarre(GameObject element)
        {
            Vector2 pos1 = Center,
                pos2 = element.Center;
            return (pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y);
        }

        public float Top
        {
            get { return position.Y; }
            protected set { position.Y = value; }
        }
        public float Bottom
        {
            get { return position.Y + size.Y; }
            protected set { position.Y = value - size.Y; }
        }
        public float Left
        {
            get { return position.X; }
            protected set { position.X = value; }
        }
        public float Right
        {
            get { return position.X + size.X; }
            protected set { position.X = value - size.X; }
        }
        public float CenterHori
        {
            get { return position.X + size.X/2; }
            protected set { position.X = value - size.X/2; }
        }
        public float CenterVert
        {
            get { return position.Y + size.Y / 2; }
            protected set { position.Y = value - size.Y / 2; }
        }
        
        public Vector2 TopLeft
        {
            get { return new Vector2(Left, Top); }
        }
        public Vector2 TopRight
        {
            get { return new Vector2(Right, Top); }
        }
        public Vector2 BottomLeft
        {
            get { return new Vector2(Left, Bottom); }
        }
        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
        }
        public Vector2 Center
        {
            get { return new Vector2(CenterHori, CenterVert); }
        }

        public float DiagonalLength
        {
            get { return (float)Math.Pow(size.X/2 + size.Y/2, 2); }
        }

        public bool IsSolid {
            get => isSolid;
        }

        protected void Print(string text)
        {
            System.Console.WriteLine(text);
        }

        
    }
}
