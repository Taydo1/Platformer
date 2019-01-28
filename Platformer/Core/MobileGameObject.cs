using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Core
{
    class MobileGameObject : GameObject
    {
        public const float gravity = 100f;

        protected Vector2 speed;
        private Vector2 acceleration;

        protected float mass;
        private List<Vector2> forces;




        public MobileGameObject(float x, float y, float objectMass) :
            base(x, y)
        {
            forces = new List<Vector2>();
            mass = objectMass;
        }

        public void Update(GameTime gameTime)
        {
            acceleration = Vector2.Zero;
            for (int i = 0; i < forces.Count; i++)
            {
                acceleration += (forces[i]/mass);
            }
            forces.Clear();

            speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void ApplyForce(float f_x, float f_y)
        {
            forces.Add(new Vector2(f_x, f_y));
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
            if ((AB.X * AP.Y - AB.Y * AP.X) * (AB.X * AO.Y - AB.Y * AO.X) < 0)
                return true;
            else
                return false;
        }

        protected bool CollisionSegSeg(Vector2 A, Vector2 B, Vector2 O, Vector2 P)
        {
            if (CollisionLineSeg(A, B, O, P) == false)
                return false;  // inutile d'aller plus loin si le segment [OP] ne touche pas la droite (AB) 
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
    }
}
