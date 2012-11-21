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
        //This function returns the array of fitness doubles that resulted from the simulation of the population over a given number of iterations.
        double[] RunPopulationSimulation(List<NN> population, int numberOfIterations);
        //This function provides an interface to show the simulation window.
        void ShowSimulation();
        //This function provides an interface to close the simulation window.
        void CloseSimulation();
    }
}
