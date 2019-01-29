using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    class GameObject
    {
        public const int WINDOW_WIDTH = Platformer.Game1.WINDOW_WIDTH;
        public const int WINDOW_HEIGHT = Platformer.Game1.WINDOW_HEIGHT;

        protected Vector2 position;
        protected Vector2 size;
        protected Texture2D texture;

        public GameObject(float x, float y)
        {
            position = new Vector2(x, y);
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

        protected void Print(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}
