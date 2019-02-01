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

        private static GameObject ecran = new GameObject(0, 0, false, Constants.WindowHoriTileNum, Constants.WindowVertTileNum);


        public GameObject(float x, float y, bool isObjectSolid, float width = 0, float height = 0)
        {
            position = new Vector2(x, y);
            isSolid = isObjectSolid;
            texture = null;
            size = new Vector2(width, height);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 shift)
        {
            if (Colision(ecran, shift)) {
                spriteBatch.Draw(texture, (position + shift) * Constants.TileSize, null, Color.White, 0, Vector2.Zero, (float)Constants.TileSize / Constants.TextureSize, SpriteEffects.None, 0);
            }
            /*Game1.DrawLine(spriteBatch, (TopLeft + shift) * Constants.TileSize, (TopRight + shift) * Constants.TileSize);
            Game1.DrawLine(spriteBatch, (TopRight + shift) * Constants.TileSize, (BottomRight + shift) * Constants.TileSize);
            Game1.DrawLine(spriteBatch, (BottomRight + shift) * Constants.TileSize, (BottomLeft + shift) * Constants.TileSize);
            Game1.DrawLine(spriteBatch, (BottomLeft + shift) * Constants.TileSize, (TopLeft + shift) * Constants.TileSize);*/
        }

        public virtual void ActionOnTouch(MobileGameObject mobileElement, int side)
        {
        }

        protected bool Colision(GameObject element, Vector2 shift)
        {
            if ((element.Left >= this.Right + shift.X) || (element.Right <= this.Left + shift.X) || (element.Top >= this.Bottom + shift.Y) || (element.Bottom <= this.Top + shift.Y))  // trop en haut
                return false;
            else
            {
                return true;
            }
        }

        public float DistanceCarre(GameObject element)
        {
            Vector2 pos1 = Center,
                pos2 = element.Center;
            return (pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y);
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
