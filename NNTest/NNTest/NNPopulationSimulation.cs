using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    //This interface provides functionality which must be present in each simulation.
    //Although this functionality does not always need to be used, it will be called.
    interface NNPopulationSimulation
    {
        double[] RunPopulationSimulation(List<NN> population, int numberOfIterations);
        void ShowSimulation();
        void CloseSimulation();
    }
}
