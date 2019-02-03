using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Platformer.Core
{
    class MobileGameObject : GameObject
    {
        protected Vector2 speed;
        protected Vector2 acceleration;
        protected float direction;
        private List<Vector2> forces;
        protected bool isAlive;
        protected int textureDirection;


        protected float mass;
        protected float defaultHorizontalAcceleration;
        protected float currentHorizontalAcceleration;
        protected float maxHorizontalSpeed;

        protected int collideSides;
        protected List<GameObject>[] sideBlocks;

        protected bool applyGravity;

        public MobileGameObject(float x, float y, float objectMass, bool applyGravityState, bool isObjectSolid, int objectTextureDuration, float objectDefaultHorizontalAcceleration, float objectMaxHorizontalSpeed) :
            base(x, y, isObjectSolid, objectTextureDuration)
        {
            collideSides = 0;

            mass = objectMass;
            applyGravity = applyGravityState;
            direction = 0;
            forces = new List<Vector2>();
            isAlive = true;
            textureDirection = 1;

            defaultHorizontalAcceleration = objectDefaultHorizontalAcceleration;
            currentHorizontalAcceleration = defaultHorizontalAcceleration;
            maxHorizontalSpeed = objectMaxHorizontalSpeed;

            sideBlocks = new List<GameObject>[4];
            for(int i = 0; i < sideBlocks.Length; i++)
            {
                sideBlocks[i] = new List<GameObject>();
            }
        }

        public override void Update(GameTime gameTime, List<GameObject> map)
        {
            if(textureDirection == 1)
            {
                drawDiretionTexture = SpriteEffects.None;
            }
            else if(textureDirection == -1)
            {
                drawDiretionTexture = SpriteEffects.FlipHorizontally;
            }

            base.Update(gameTime, map);


            acceleration = Vector2.Zero;
            acceleration.X = currentHorizontalAcceleration * direction;
            if (applyGravity && (collideSides & 2)==0)
            {
                acceleration.Y = Constants.gravity;
            }

            for(int i = 0; i < forces.Count; i ++)
            {
                acceleration += forces[i] / mass;
            }
            forces.Clear();


            speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(speed.X > maxHorizontalSpeed)
            {
                speed.X = maxHorizontalSpeed;
            }else if(speed.X < -maxHorizontalSpeed)
            {
                speed.X = -maxHorizontalSpeed;
            }
            
            StopOnCollision();

            Move(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, map);

            if(position.Y > Constants.WindowVertTileNum)
            {
                isAlive = false;
            }
        }

        protected void Move(Vector2 movement, List<GameObject> solidObjectList)
        {
            float maxMultUntilCollision = 1f,
                nouveauMult,
                movementLength = movement.Length();

            for (int i = 0; i < solidObjectList.Count; i++)
            {
                if (solidObjectList[i].IsSolid && DistanceCarre(solidObjectList[i]) < Math.Pow(movementLength + DiagonalLength + solidObjectList[i].DiagonalLength, 2))
                {
                    nouveauMult = MultUntilCollision(solidObjectList[i], movement);
                    maxMultUntilCollision = Math.Min(maxMultUntilCollision, nouveauMult);
                }
            }
            position += maxMultUntilCollision * movement;

            currentHorizontalAcceleration = defaultHorizontalAcceleration;

            collideSides = 0;
            for(int i = 0; i < sideBlocks.Length; i++)
            {
                //Console.Write(" " + sideBlocks[i].Count);
                sideBlocks[i].Clear();
            }
            //Console.WriteLine();


            for (int i = 0; i < solidObjectList.Count; i++)
            {
                if (solidObjectList[i].IsSolid && DistanceCarre(solidObjectList[i]) < Math.Pow(movementLength + DiagonalLength + solidObjectList[i].DiagonalLength, 2))
                {
                    DetectCollideSide(solidObjectList[i]);
                }
            }

            for(int i = 0; i < sideBlocks.Length; i++)
            {
                for(int j = 0; j < sideBlocks[i].Count; j++)
                {
                    sideBlocks[i][j].ActionOnTouch(this, i);
                }
            }
        }

        protected void AddForce(Vector2 force)
        {
            forces.Add(force);
        }

        private void StopOnCollision()
        {
            if(speed.X < 0 && (collideSides & 4) != 0)
            {
                speed.X = 0;
            }
            if (speed.X > 0 && (collideSides & 1) != 0)
            {
                speed.X = 0;
            }
            if (speed.Y < 0 && (collideSides & 8) != 0)
            {
                speed.Y = 0;
            }
            if (speed.Y > 0 && (collideSides & 2) != 0)
            {
                speed.Y = 0;
            }
        }






        private bool CollisionLineSeg(Vector2 A, Vector2 B, Vector2 O, Vector2 P)
        {
            Vector2 AO = Vector2.Zero,
                AP = Vector2.Zero,
                AB = Vector2.Zero;
            AB.X = B.X - A.X;
            AB.Y = B.Y - A.Y;
            AP.X = P.X - A.X;
            AP.Y = P.Y - A.Y;
            AO.X = O.X - A.X;
            AO.Y = O.Y - A.Y;
            if ((AB.X * AP.Y - AB.Y * AP.X) * (AB.X * AO.Y - AB.Y * AO.X) < 0) {
                return true;
            }
            else{
                return false;
            }
        }

        private float MaxDistanceUntilColision(Vector2 A, Vector2 B, Vector2 O, Vector2 P)
        {
            Vector2 AB = Vector2.Zero,
                OP = Vector2.Zero;
            AB.X = B.X - A.X;
            AB.Y = B.Y - A.Y;
            OP.X = P.X - O.X;
            OP.Y = P.Y - O.Y;

            float l = -(-AB.X * A.Y + AB.X * O.Y + AB.Y * A.X - AB.Y * O.X) / (AB.X * OP.Y - AB.Y * OP.X);
            return l - 0.001f;
        }

        private bool CollisionSegSeg(Vector2 A, Vector2 B, Vector2 O, Vector2 P)
        {
            if (CollisionLineSeg(A, B, O, P) == false)
            {
                return false;  // inutile d'aller plus loin si le segment [OP] ne touche pas la droite (AB) 
            }

            Vector2 AB = Vector2.Zero,
                OP = Vector2.Zero;
            AB.X = B.X - A.X;
            AB.Y = B.Y - A.Y;
            OP.X = P.X - O.X;
            OP.Y = P.Y - O.Y;
            float k = -(A.X * OP.Y - O.X * OP.Y - OP.X * A.Y + OP.X * O.Y) / (AB.X * OP.Y - AB.Y * OP.X);

            if (k < 0 || k > 1)
                return false;
            else
                return true;
        }

        protected float MultUntilCollision(GameObject element, Vector2 movement)
        {
            Vector2[] points = { TopLeft, TopRight, BottomLeft, BottomRight };
            Vector2[][] coteElements = new[]
            {
                new[] {element.TopLeft, element.TopRight },
                new[] {element.TopRight, element.BottomRight },
                new[] {element.BottomRight, element.BottomLeft },
                new[] {element.BottomLeft, element.TopLeft }
            };
            float l=1f,
                nouveauL;

            for(int i = 0; i < points.Length; i++)
            {
                for(int j = 0; j < coteElements.Length; j++)
                {
                    if (CollisionSegSeg(coteElements[j][0], coteElements[j][1], points[i], points[i] + movement))
                    {
                        nouveauL = MaxDistanceUntilColision(coteElements[j][0], coteElements[j][1], points[i], points[i] + movement);
                        l = Math.Min(nouveauL, l);
                    }
                }
            }

            return l;
        }

        private void DetectCollideSide(GameObject element)
        {
            float offset = 0.01f;
            float t_collision = element.Bottom - Top;
            float b_collision = Bottom - element.Top;
            float r_collision = Right - element.Left;
            float l_collision = element.Right - Left;

            if (!((element.Left >= this.Right + offset) || (element.Right <= this.Left) || (element.Top >= this.Bottom) || (element.Bottom <= this.Top))) {
                if (r_collision < l_collision && r_collision < t_collision && r_collision < b_collision)
                {
                    //Right collision
                    collideSides |= 1;
                    sideBlocks[0].Add(element);
                }
            }
            else if (!((element.Left >= this.Right) || (element.Right <= this.Left - offset) || (element.Top >= this.Bottom) || (element.Bottom <= this.Top))) {
                if (l_collision < r_collision && l_collision < t_collision && l_collision < b_collision)
                {
                    //Left collision
                    collideSides |= 4;
                    sideBlocks[2].Add(element);
                }
            }
            else if (!((element.Left >= this.Right) || (element.Right <= this.Left) || (element.Top >= this.Bottom + offset) || (element.Bottom <= this.Top))) {
                if (b_collision < t_collision && b_collision < l_collision && b_collision < r_collision)
                {
                    //bottom collision
                    collideSides |= 2;
                    sideBlocks[1].Add(element);
                }
            }
            else if (!((element.Left >= this.Right) || (element.Right <= this.Left) || (element.Top >= this.Bottom) || (element.Bottom <= this.Top - offset))) {
                if (t_collision < b_collision && t_collision < l_collision && t_collision < r_collision)
                {
                    //Top collision
                    collideSides |= 8;
                    sideBlocks[3].Add(element);
                }
            }
        }
        

        public float SpeedX { get => speed.X; set => speed.X = value; }
        public float SpeedY { get => speed.Y; set => speed.Y = value; }
        public float DefaultHorizontalAcceleration { get => defaultHorizontalAcceleration; }
        public float CurrentHorizontalAcceleration { get => currentHorizontalAcceleration; set => currentHorizontalAcceleration = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
    }
}
