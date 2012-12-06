using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace NNXNA
{
    public class Util
    {
        //Random number generator class which other classes can use
        public static Random randNumGen = new Random();

        //Moving average function
        public static double CalculateMovingAverage(double oldAverage, double newValue, int windowSize)
        {
            return (oldAverage * (((double)windowSize - 1.0) / (double)windowSize)) + (newValue * (1.0 / (double)windowSize));

        }

        public static float SquareRootFloat(float number)
        {
            unsafe
            {
                long i;
                float x, y;
                const float f = 1.5F;

                x = number * 0.5F;
                y = number;
                i = *(long*)&y;
                i = 0x5f3759df - (i >> 1);
                y = *(float*)&i;
                y = y * (f - (x * y * y));
                y = y * (f - (x * y * y));
                return number * y;
            }
        }

        private float SimplePower(float a, int b)
        {
            float number = 1;
            for (int i = 0; i < b; i++)
                number *= a;
            return number;
        }

        public static SpriteFont genericTextFont;
    }
}
