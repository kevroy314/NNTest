using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NNTest
{
    /* This class represents an ant in the NNAntSimulation. Not all properties need to be used, but
     * they are provided as part of the object for convenience.
     */

    class SimAnt
    {
        #region Member Variables

        private Vector2 position; //The position of the ant (pixels)
        private Vector2 lookAt; //The direction the ant is looking in vector form (unit vector)
        private double speed; //The speed of the ant (pixels/tick)
        private double orientation; //The orientation of the ant (radians)

        #endregion

        #region Constructors

        public SimAnt()
        {
            orientation = 0;
            speed = 0;
            position = Vector2.Zero;
            LookAt = new Vector2(0f, 1f); //-Sin(0), Cos(0)
        }

        //Constructor which allows for configuration of initial orientation (radians), speed (pixels/tick) and position (pixels)
        public SimAnt(double initOrientation, double initSpeed, Vector2 initPosition)
        {
            orientation = initOrientation;
            speed = initSpeed;
            position = initPosition;
            lookAt = new Vector2(-(float)Math.Sin(initOrientation), (float)Math.Cos(initOrientation));
        }

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public double Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        #endregion
    }
}
