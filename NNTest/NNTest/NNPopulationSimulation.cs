using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    interface NNPopulationSimulation
    {
        double[] RunPopulationSimulation(List<NN> population, int numberOfIterations);
    }
}
