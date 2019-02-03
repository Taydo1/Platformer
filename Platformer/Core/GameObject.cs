using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Platformer.Core
{
    class GameObject
    {
        protected Vector2 position;
        protected Vector2 size;
        protected Texture2D[] texture;
        protected int currentTexture;
        protected TimeSpan nextTextureTime;
        protected TimeSpan textureDuration;
        protected bool isSolid;
        protected SpriteEffects drawDirectionTexture;
        protected float rotationAngle;

        protected bool needBackground;
        protected Texture2D backgroundTexture;
        protected Vector2 backgroundPosition;

        private static GameObject screen = new GameObject(0, 0, false, 0, false, Constants.WindowHoriTileNum, Constants.WindowVertTileNum);


        public GameObject(float x, float y, bool isObjectSolid, int objectTextureDuration, bool objectNeedBackground, float width = 0, float height = 0)
        {
            position = new Vector2(x, y);
            isSolid = isObjectSolid;
            texture = null;
            size = new Vector2(width, height);
            currentTexture = 0;
            nextTextureTime = new TimeSpan(0);
            textureDuration = new TimeSpan(0, 0, 0, 0, objectTextureDuration);
            drawDirectionTexture = SpriteEffects.None;
            rotationAngle = 0;

            backgroundTexture = null;
            needBackground = objectNeedBackground;
            backgroundPosition = new Vector2((int)x, (int)y);
        }

        public virtual void Update(GameTime gameTime, List<GameObject> solidObjectList)
        {
            if(gameTime.TotalGameTime >= nextTextureTime)
            {
                nextTextureTime = gameTime.TotalGameTime + textureDuration;
                currentTexture++;
                if (currentTexture >= texture.Length)
                {
                    currentTexture -= texture.Length;
                }

                //size = new Vector2((float)texture[currentTexture].Width / Constants.TextureSize, (float)texture[currentTexture].Height / Constants.TextureSize);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 shift)
        {
            if (Colision(screen, shift)) {
                if(backgroundTexture != null)
                {
                    spriteBatch.Draw(backgroundTexture, (backgroundPosition +shift)* Constants.TileSize, null, Color.White, 0, Vector2.Zero, (float)Constants.TileSize / Constants.TextureSize, drawDirectionTexture, 0);
                }
                spriteBatch.Draw(texture[currentTexture], (Center + shift) * Constants.TileSize, null, Color.White, rotationAngle, new Vector2(texture[currentTexture].Width, texture[currentTexture].Height) / 2, (float)Constants.TileSize / Constants.TextureSize, drawDirectionTexture, 0);
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

        public virtual Texture2D[] Texture
        {
            get => texture;
            set
            {
                texture = value;
                size = new Vector2((float)texture[currentTexture].Width / Constants.TextureSize, (float)texture[currentTexture].Height / Constants.TextureSize);
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
        public bool NeedBackground { get => needBackground;}
        public Texture2D BackgroundTexture { get => backgroundTexture; set => backgroundTexture = value; }

        protected void Print(string text)
        {
            System.Console.WriteLine(text);
        }

        public static void UpdateScreen()
        {
            screen = new GameObject(0, 0, false, 0, false, Constants.WindowHoriTileNum, Constants.WindowVertTileNum);
        }
    }
}
