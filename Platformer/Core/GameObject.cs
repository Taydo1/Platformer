using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    class GameObject
    {
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
            System.Console.WriteLine(position.X + " " + position.Y);
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

        public float Bottom
        {
            get { return position.Y + size.Y; }
            protected set { position.Y = value - size.Y; }
        }
    }
}
