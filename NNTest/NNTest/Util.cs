using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    /* This utility class provide functions which are generic enough that the deserve
     * separation from the classes in which they are called. It's possible that they will be
     * used in many places in the code.
     */

    public class Util
    {
        //Random number generator class which other classes can use
        public static Random randNumGen = new Random();

        //Moving average function
        public static double CalculateMovingAverage(double oldAverage, double newValue, double windowSize)
        {
            return (oldAverage * ((windowSize - 1.0) / windowSize)) + 
                    (newValue * (1.0 / windowSize));
        }
    }
}
