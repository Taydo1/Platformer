using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            spriteBatch.Draw(texture, position, Color.White);
            //System.Console.WriteLine(position.X + " " + position.Y);
        }


        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                size = new Vector2(texture.Width, texture.Height);
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

        public bool IsSolid {
            get => isSolid;
        }

        protected void Print(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}
