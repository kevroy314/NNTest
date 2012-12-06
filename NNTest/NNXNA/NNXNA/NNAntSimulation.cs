using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNXNA
{
    //This simulation class provides a way for the NN to determine its fitness each generation.
    //It is also a form, thus satisfying the requirement for a Show and Close function.
    public partial class NNAntSimulation : NNPopulationSimulation
    {
        #region Constant Values

        //The amount of food which should be in each simulation at the beginning of each iteration
        private const int foodCount = 40;

        //The distance at which food may be captured (in pixels) by an ant
        private const double minFoodCaptureDist = 7;

        //The display size of the food
        private const int foodSize = 3;
        //The display size of the ant
        private const int antSize = 10;

        //The display Brush of the food
        private Color foodBrush = Color.Green;
        //The display Brush of the ant
        private Color antBrush = Color.Blue;
        //The display Brush of a highlighted ant
        private Color highlightedAntBrush = Color.Red;

        //Client Width is used at initialization of this class to determine the width in the designer
        private const int clientWidth = 400;
        //Client Height is used at initialization of this class to determine the height in the designer
        private const int clientHeight = 400;

        //Maximum amount of rotation the ant can have in any iteration
        private const double maxRotationRate = 0.3;

        #endregion

        #region Member Variables

        //The array of ants containing their associated meta-data
        private SimAnt[] ants;

        //Create a list of food locations. A list is used instead of an array so the adding and removing is handled cleanly for us.
        private List<Vector2> food;

        //Is the simulation actually shown
        private bool isShowing;

        //The iteration counter for the simulation
        private int currentIteration;

        //The population for the simulation
        private List<NN> population;

        //The number of iterations the simulation is to run before completion
        private int numberOfIterations;

        //The number of indicies to highlight under a certain index (used for highlighting elites)
        private int highlightIndiciesUnder;

        //This value shows if the simulation is complete.
        private bool isSimulationComplete;

        //This array contains the score which will be output for each ant.
        private double[] score;

        #endregion

        #region Constructors

        //Construct an ant simulation with a certain population
        public NNAntSimulation(List<NN> population, int numberOfIterations, int highlightIndiciesUnder)
        {
            //Set the member variables
            this.population = population;
            this.numberOfIterations = numberOfIterations;
            this.highlightIndiciesUnder = highlightIndiciesUnder;

            //We don't show the form by default
            isShowing = true;

            //Initialize the positions and directions of all the ants
            ants = new SimAnt[population.Count];

            for (int i = 0; i < ants.Length; i++)
            {
                //Create a random ant
                ants[i] = new SimAnt(Util.randNumGen.NextDouble() * Math.PI * 2, 0, new Vector2(Util.randNumGen.Next(0, clientWidth), Util.randNumGen.Next(0, clientHeight)));
            }

            //Initialize the list of food
            food = new List<Vector2>();

            //Fill the food list with randomly generated food positions on the canvas
            for (int i = 0; i < foodCount; i++)
                food.Add(new Vector2(Util.randNumGen.Next(0, clientWidth - 1), Util.randNumGen.Next(0, clientHeight - 1)));

            currentIteration = 0;

            isSimulationComplete = false;

            score = new double[population.Count];

            for (int i = 0; i < score.Length; i++)
                score[i] = 0f;
        }

        #endregion

        #region NNPopulationSimulation Overwritten Methods

        //This function is required and allows the simulation to show it's self if it, and the calling class wishes
        public void ShowSimulation()
        {
            //Show the form
            isShowing = true;
        }

        //This function is required and allows the simulation to close it's self if it, and the calling class wishes
        public void CloseSimulation()
        {
            //Close the form
            isShowing = false;
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            //If the graphics variables are not null
            if (isShowing)
            {
                
                //Draw the simulation

                spriteBatch.DrawString(Util.genericTextFont, "Iteration: " + currentIteration + " of " + numberOfIterations, new Vector2(20, 45), Color.White);

                //Draw the iteration number
                ///label.Text = "Iteration # " + i + " in progress...";

                //Clear the graphics
                ///g.Clear(System.Drawing.Color.White);

                //Draw the food rectangles
                for (int j = 0; j < food.Count; j++)
                {
                    VertexPositionColor[] verts = new VertexPositionColor[5];

                    float offset = foodSize / 2;

                    verts[0] = new VertexPositionColor(new Vector3(food[j].X - offset, food[j].Y - offset, 0), foodBrush);
                    verts[1] = new VertexPositionColor(new Vector3(food[j].X + offset, food[j].Y - offset, 0), foodBrush);
                    verts[2] = new VertexPositionColor(new Vector3(food[j].X + offset, food[j].Y + offset, 0), foodBrush);
                    verts[3] = new VertexPositionColor(new Vector3(food[j].X - offset, food[j].Y + offset, 0), foodBrush);
                    verts[4] = new VertexPositionColor(new Vector3(food[j].X - offset, food[j].Y - offset, 0), foodBrush);

                    graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, verts, 0, verts.Length - 1);
                }

                //Draw the ant circles
                for (int j = 0; j < ants.Length; j++)
                    if (j < highlightIndiciesUnder)
                        graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, ants[j].getVertexPositionColor(highlightedAntBrush, antSize), 0, ants[j].NumVertices);
                    else
                        graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, ants[j].getVertexPositionColor(antBrush, antSize), 0, ants[j].NumVertices);

                //Draw the buffer to the form
                ///finalG.DrawImage(buffer, 0, 0);

                //Find the maximum score this round
                ///int maxIndex = -1;
                ///double maxScore = -1;
                ///for (int i = 0; i < score.Length; i++)
                ///    if (score[i] > maxScore)
                ///    {
                ///        maxIndex = i;
                ///        maxScore = score[i];
                ///    }

                //Display the best score for one second
                ///label.Text = "Done! Max Score: " + maxScore;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (currentIteration < numberOfIterations)
            {

                //Create a list to store the food which should be replaced at the end of the loop
                //This is done as opposed to removing as we go so that multiple ants can, in theory, "share" a piece of food
                //This was a design choice to prevent the number the ant is in the list from influencing it's success
                List<Vector2> foodToRemove = new List<Vector2>();

                //Iterate through each member of the population
                for (int j = 0; j < population.Count; j++)
                {
                    //Find the nearest food item
                    Tuple<Vector2, double> nearestFoodResult = findNearestFoodLocationAndDistance(ants[j].Position);

                    //If the ant is near enough the food to capture it
                    if (nearestFoodResult.Item2 < minFoodCaptureDist)
                    {
                        //Add the food to the list of food which needs to be replaced at the end of the round
                        foodToRemove.Add(nearestFoodResult.Item1);
                        //Increment the ant score
                        score[j]++;
                    }

                    //Normalize the distance so we have a direction vector to the nearest food
                    Vector2 foodDirection = ants[j].Position - nearestFoodResult.Item1;
                    foodDirection.Normalize();

                    //NOTE: THIS SECTION SHOULD BE CHANGED TO REFLECT MORE INPUTS WHICH COULD PRODUCE A MORE ROBUST NETWORK
                    //Generate the inputs for this iteration of the neural network
                    double[] NNInput = new double[] { foodDirection.X, foodDirection.Y, ants[j].LookAt.X, ants[j].LookAt.Y };

                    //Compute the outputs from the neural network
                    double[] NNOutput = population[j].ComputeNNOutputs(NNInput);

                    double rotationForce = NNOutput[0] - NNOutput[1];

                    if (rotationForce < -maxRotationRate)
                        rotationForce = -maxRotationRate;
                    else if (rotationForce > maxRotationRate)
                        rotationForce = maxRotationRate;

                    ants[j].Orientation += rotationForce;

                    ants[j].Speed = (NNOutput[0] + NNOutput[1]);

                    ants[j].LookAt = new Vector2(-(float)Math.Sin(ants[j].Orientation), (float)Math.Cos(ants[j].Orientation));

                    //Adjust the position of the ant relative to the neural network outputs
                    float x = ants[j].LookAt.X * (float)ants[j].Speed + ants[j].Position.X;
                    float y = ants[j].LookAt.Y * (float)ants[j].Speed + ants[j].Position.Y;

                    //Wrap the ant location around the map so it represents a torus
                    x = (x + clientWidth) % clientWidth;
                    y = (y + clientHeight) % clientHeight;

                    ants[j].Position = new Vector2(x, y);
                }

                //For each food vector which we need to remove because it was eaten
                foreach (Vector2 currentFood in foodToRemove)
                {
                    //Remove the vector from the list
                    food.Remove(currentFood);
                    //Add a new one in its place
                    food.Add(new Vector2(Util.randNumGen.Next(0, clientWidth - 1), Util.randNumGen.Next(0, clientHeight - 1)));
                }

                currentIteration++;
            }
            else
            {
                isSimulationComplete = true;
            }
        }

        public bool IsSimulationComplete()
        {
            return isSimulationComplete;
        }

        public double[] GetSimulationResults()
        {
            return score;
        }

        #endregion

        #region Private Helper Functions

        //This helper function locates the nearest food index and distance from the input vector
        private Tuple<Vector2, double> findNearestFoodLocationAndDistance(Vector2 input)
        {
            //The index we will output
            Vector2 minFoodItem = input;
            //The distance we will output
            double minDist = int.MaxValue;

            //For each food
            foreach(Vector2 foodItem in food)
            {
                //Calculate the distance
                double d = Vector2.Distance(input, foodItem);
                
                //If this is a new biggest
                if (d < minDist)
                {
                    //Set the distance and food item to the current iteration results
                    minDist = d;
                    minFoodItem = foodItem;
                }
            }

            //Return the results
            return new Tuple<Vector2, double>(minFoodItem, minDist);
        }

        #endregion
    }
}
