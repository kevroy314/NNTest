using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
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
    }
}
