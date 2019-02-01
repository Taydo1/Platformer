using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Platformer.Core
{
    class MobileGameObject : GameObject
    {
        public const float gravity = 10f;

        protected Vector2 speed;
        private Vector2 acceleration;

        protected float mass;
        protected float maxSpeed;

        protected int collideSides;

        protected bool applyGravity;


        public MobileGameObject(float x, float y, float objectMass, float objectMaxSpeed, bool applyGravityState, bool isObjectSolid) :
            base(x, y, isObjectSolid)
        {
            collideSides = 0;

            mass = objectMass;
            maxSpeed = objectMaxSpeed;
            applyGravity = applyGravityState;
        }

        public void Update(GameTime gameTime, List<GameObject> solidObjectList)
        {

            acceleration = Vector2.Zero;
            if (applyGravity && (collideSides & 2)==0)
            {
                acceleration.Y = gravity;
            }


            speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (speed.X > maxSpeed)
            {
                speed.X = maxSpeed;
            }
            else if(speed.X < -maxSpeed)
            {
                speed.X = -maxSpeed;
            }
            StopOnCollision();

            Move(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, solidObjectList);
            /*Print("side :" + collideSides);
            Print("acceleration :" + acceleration.ToString());
            Print("speed :" + speed.ToString());
            Print("position :" + position.ToString());*/
        }

        protected void Move(Vector2 movement, List<GameObject> solidObjectList)
        {
            float maxMultUntilCollision = 1f,
                nouveauMult,
                speedLength = speed.Length();

            for (int i = 0; i < solidObjectList.Count; i++)
            {
                if (solidObjectList[i].IsSolid && DistanceCarre(solidObjectList[i]) < Math.Pow(speedLength + DiagonalLength + solidObjectList[i].DiagonalLength, 2))
                {
                    nouveauMult = MultUntilCollision(solidObjectList[i], movement);
                    maxMultUntilCollision = Math.Min(maxMultUntilCollision, nouveauMult);
                }
            }
            position += maxMultUntilCollision * movement;

            collideSides = 0;
            for (int i = 0; i < solidObjectList.Count; i++)
            {
                if (solidObjectList[i].IsSolid && DistanceCarre(solidObjectList[i]) < Math.Pow(speedLength + DiagonalLength + solidObjectList[i].DiagonalLength, 2))
                {
                    DetectCollideSide(solidObjectList[i]);
                }
            }
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
            float offset = 0.001f;

            if (!((element.Left >= this.Right + offset) || (element.Right <= this.Left - offset) || (element.Top >= this.Bottom + offset) || (element.Bottom <= this.Top - offset))) {

                float t_collision = element.Bottom - Top;
                float b_collision = Bottom - element.Top;
                float r_collision = Right - element.Left;
                float l_collision = element.Right - Left;
                

                if (t_collision < b_collision && t_collision < l_collision && t_collision < r_collision)
                {
                    //Top collision
                    collideSides |= 8;
                }
                if (b_collision < t_collision && b_collision < l_collision && b_collision < r_collision)
                {
                    //bottom collision
                    collideSides |= 2;
                }
                if (l_collision < r_collision && l_collision < t_collision && l_collision < b_collision)
                {
                    //Left collision
                    collideSides |= 4;
                }
                if (r_collision < l_collision && r_collision < t_collision && r_collision < b_collision)
                {
                    //Right collision
                    collideSides |= 1; 
                }
            }
        }

    }
}
