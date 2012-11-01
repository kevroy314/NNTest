using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    interface NNPopulationSimulation
    {
        public double[] RunPopulationSimulation(List<NN> population, params object[] simParams);
    }
}
