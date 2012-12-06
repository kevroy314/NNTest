using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNXNA
{
    //This interface provides functionality which must be present in each simulation.
    //Although this functionality does not always need to be used, it will be called.
    interface NNPopulationSimulation
    {
        //This function returns the array of fitness doubles that resulted from the simulation of the population over a given number of iterations.
        double[] GetSimulationResults();
        //This function provides an interface to show the simulation window.
        void ShowSimulation();
        //This function provides an interface to close the simulation window.
        void CloseSimulation();
        //This function provides the update method for the simulation.
        void Update(GameTime gameTime);
        //This function provides the draw method for the simulation.
        void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch);
        //This function returns true if the simulation is complete, false if it is incomplete.
        bool IsSimulationComplete();
    }
}
