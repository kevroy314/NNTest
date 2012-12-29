using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace NNTest
{
    public partial class NNSpaceShipSimulation : Form, INNPopulationSimulation
    {
        #region Member Variables

        //The array of ants containing their associated meta-data
        private GravitationalBody[] gObjects;

        //Create a list of food locations. A list is used instead of an array so the adding and removing is handled cleanly for us.
        private List<Vector2> ships;
        private Vector2 startPos;
        private Vector2 goal;

        //Is the simulation actually shown
        private bool isShowing;

        #endregion

        public NNSpaceShipSimulation(int populationSize)
        {
            InitializeComponent();

            isShowing = false;
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
            Bitmap buffer = new Bitmap(Params.clientWidth, Params.clientHeight);
            Graphics g = Graphics.FromImage(buffer);
            Graphics finalG = g;
            if (isShowing) finalG = this.CreateGraphics();

            goal = new Vector2((float)Util.randNumGen.NextDouble() * Params.clientWidth, (float)Util.randNumGen.NextDouble() * Params.clientHeight);
            startPos = new Vector2((float)(Util.randNumGen.NextDouble() * Params.clientWidth), (float)(Util.randNumGen.NextDouble() * Params.clientHeight));

            gObjects = new GravitationalBody[Params.numGBodies];

            for (int i = 0; i < gObjects.Length; i++)
            {
                gObjects[i] = new GravitationalBody(new Vector2(), 0);
                gObjects[i].randomizeParameters(0, Params.clientWidth, 0, Params.clientHeight, 0, Params.maxMass);
            }

            ships = new List<Vector2>();

            for (int i = 0; i < population.Count; i++)
                ships.Add(startPos);

            for(int i = 0; i < numberOfIterations;i++)
            {
                if (isShowing)
                {
                    render(g);
                    finalG.DrawImage(buffer, 0, 0);
                    this.Update();
                }

                for (int j = 0; j < ships.Count; j++)
                {
                    Vector2 acc = Vector2.Zero;
                    for (int k = 0; k < gObjects.Length; k++)
                        acc += gObjects[k].calculateAcceleration(ships[j], Params.shipMass, Params.minGravityDistance);
                    ships[j] -= acc;
                }
            }

            double[] output = new double[population.Count];
            for (int i = 0; i < population.Count; i++)
                output[i] = Vector2.Distance(ships[i], goal);
            return output;
        }

        public void render(Graphics g)
        {
            for (int i = 0; i < gObjects.Length; i++)
                g.FillEllipse(Brushes.Blue, 
                    gObjects[i].Position.X - (gObjects[i].Mass * Params.gBodyDrawScale) / 2, 
                    gObjects[i].Position.Y - (gObjects[i].Mass * Params.gBodyDrawScale) / 2, 
                    gObjects[i].Mass * Params.gBodyDrawScale, 
                    gObjects[i].Mass * Params.gBodyDrawScale);
            g.FillEllipse(Brushes.Green, startPos.X - 5, startPos.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Red, goal.X - 5, goal.Y - 5, 10, 10);
            for (int i = 0; i < ships.Count; i++)
                g.FillRectangle(Brushes.Black, ships[i].X - 2, ships[i].Y - 2, 4, 4);
        }

        #endregion
    }
}
