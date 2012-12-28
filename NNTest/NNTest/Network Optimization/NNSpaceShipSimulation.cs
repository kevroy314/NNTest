using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace NNTest.Network_Optimization
{
    public partial class NNSpaceShipSimulation : Form, INNPopulationSimulation
    {
        #region Member Variables

        //The array of ants containing their associated meta-data
        private GravitationalBody[] gObjects;

        //Create a list of food locations. A list is used instead of an array so the adding and removing is handled cleanly for us.
        private List<Vector2> ships;

        private Vector2 goal;

        //Is the simulation actually shown
        private bool isShowing;

        #endregion

        public NNSpaceShipSimulation()
        {
            InitializeComponent();

            gObjects = new GravitationalBody[Params.numGBodies];

            for (int i = 0; i < gObjects.Length; i++)
            {
                gObjects[i] = new GravitationalBody(new Vector2(), 0);
                gObjects[i].randomizeParameters(0, Params.clientWidth, 0, Params.clientHeight, 0, Params.maxMass);
            }

            ships = new List<Vector2>();
        }

        #region NNPopulationSimulation Overwritten Methods

        //This function is required and allows the simulation to show it's self if it, and the calling class wishes
        public void ShowSimulation()
        {
            //Show the form
            isShowing = true;

            this.Show();
        }

        //This function is required and allows the simulation to close it's self if it, and the calling class wishes
        public void CloseSimulation()
        {
            //Close the form
            isShowing = false;

            this.Close();
        }

        public double[] RunPopulationSimulation(List<NN> population, int numberOfIterations, int highlightIndiciesUnder)
        {
            Vector2 startPos = new Vector2((float)(Util.randNumGen.NextDouble() * Params.clientWidth), (float)(Util.randNumGen.NextDouble() * Params.clientHeight));
            for (int i = 0; i < population.Count; i++)
                ships.Add(startPos);

            for(int i = 0; i < numberOfIterations;i++)
            {

            }

            double[] output = new double[population.Count];
            for (int i = 0; i < population.Count; i++)
                output[i] = Vector2.Distance(ships[i], goal);
            return output;
        }

        #endregion
    }
}
