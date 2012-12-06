using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNXNA
{
    class SimAnt
    {
        private VertexPositionColor[] vertices;

        private int numVertices;

        public int NumVertices
        {
            get { return numVertices; }
            set { numVertices = value; }
        }

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

            vertices = new VertexPositionColor[5];

            numVertices = vertices.Length - 1;
        }

        public VertexPositionColor[] getVertexPositionColor(Color c, float size)
        {
            vertices[0].Position = new Vector3(position.X - size, position.Y - size, 0);
            vertices[0].Color = c;

            vertices[1].Position = new Vector3(position.X - size, position.Y + size, 0);
            vertices[1].Color = c;

            vertices[2].Position = new Vector3(position.X + size, position.Y + size, 0);
            vertices[2].Color = c;

            vertices[3].Position = new Vector3(position.X + size, position.Y - size, 0);
            vertices[3].Color = c;

            vertices[4].Position = new Vector3(position.X - size, position.Y - size, 0);
            vertices[4].Color = c;

            return vertices;
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
