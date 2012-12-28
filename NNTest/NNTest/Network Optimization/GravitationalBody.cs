using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NNTest.Network_Optimization
{
    class GravitationalBody
    {
        #region Member Variables

        private Vector2 pos; 
        private float m;

        #endregion

        public GravitationalBody(Vector2 position, float mass)
        {
            pos = position;
            m = mass;
        }

        public void randomizeParameters(float minX, float maxX, float minY, float maxY, float minMass, float maxMass)
        {
            pos = new Vector2(((float)Util.randNumGen.NextDouble() * (maxX - minX)) - minX, ((float)Util.randNumGen.NextDouble() * (maxY - minY)) - minY);
            m = (float)Util.randNumGen.NextDouble() * (maxMass - minMass) - minMass;
        }

        public Vector2 calculateAcceleration(Vector2 position, float mass)
        {
            Vector2 r12 = Vector2.Subtract(pos,position);
            Vector2 r12n = new Vector2(r12.X,r12.Y);
            r12n.Normalize();

            return (float)(-(Params.G * m * mass / ((r12 * r12).LengthSquared()))) * r12n;
        }

        #region Properties

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public float Mass
        {
            get { return m; }
            set { m = value; }
        }

        #endregion
    }
}
