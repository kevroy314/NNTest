using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NNTest
{
    class SimAnt
    {
        public SimAnt()
        {
            orientation = 0;
            speed = 0;
            position = Vector2.Zero;
            LookAt = new Vector2(0f, 1f);
        }

        public SimAnt(double initOrientation, double initSpeed, Vector2 initPosition)
        {
            orientation = initOrientation;
            speed = initSpeed;
            position = initPosition;
            lookAt = new Vector2(-(float)Math.Sin(initOrientation), (float)Math.Cos(initOrientation));
        }

        private double orientation;

        public double Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        private double speed;

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector2 lookAt;

        public Vector2 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }
    }
}
