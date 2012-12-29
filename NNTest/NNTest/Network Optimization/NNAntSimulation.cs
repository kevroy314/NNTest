using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework;

namespace NNTest
{
    /* This simulation class provides a way for the NN to determine its fitness each generation.
     * It is also a form, thus satisfying the requirement for a Show and Close function.
     */

    public partial class NNAntSimulation : Form, INNPopulationSimulation
    {
        #region Member Variables

        //The array of ants containing their associated meta-data
        private SimAnt[] ants;

        //Create a list of food locations. A list is used instead of an array so the adding and removing is handled cleanly for us.
        private List<Vector2> food;

        //Is the simulation actually shown
        private bool isShowing;

        #endregion

        #region Constructors

        //Construct an ant simulation with a certain population
        public NNAntSimulation(int populationSize)
        {
            //Initialize the GUI
            InitializeComponent();

            //We don't show the form by default
            isShowing = false;

            //Initialize the positions and directions of all the ants
            ants = new SimAnt[populationSize];

            for (int i = 0; i < ants.Length; i++)
            {
                //Create a random ant
                ants[i] = new SimAnt(Util.randNumGen.NextDouble() * Math.PI * 2, 
                                     0, 
                                     new Vector2(Util.randNumGen.Next(0, Params.clientWidth), 
                                                 Util.randNumGen.Next(0, Params.clientHeight)));
            }

            //Initialize the list of food
            food = new List<Vector2>();

            //Fill the food list with randomly generated food positions on the canvas
            for (int i = 0; i < Params.foodCount; i++)
                food.Add(new Vector2(Util.randNumGen.Next(0, Params.clientWidth - 1), 
                                     Util.randNumGen.Next(0, Params.clientHeight - 1)));
        }

        #endregion

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

        //This is the primary overwritten function for the NNPopulationSimulation interface
        //It will run a simulation which will show the ants finding food, or simulate without display.
        public double[] RunPopulationSimulation(List<NN> population, int numberOfIterations, int highlightIndiciesUnder)
        {
            //Generate an array in which the score will be output for each ant
            double[] score = new double[population.Count];

            //Initialize the variables for drawing (may not be used if isShowing stays false)
            Bitmap buffer = new Bitmap(Params.clientWidth, Params.clientHeight);
            Graphics g = Graphics.FromImage(buffer);
            Graphics finalG = g;
            if(isShowing) finalG = this.CreateGraphics();

            //Iterate through the number of request iterations
            for (int i = 0; i < numberOfIterations; i++)
            {
                //If the graphics variables are not null
                if (isShowing)
                {
                    //Draw the simulation

                    //Draw the iteration number
                    label.Text = "Iteration # " + i + " in progress...";

                    //Clear the graphics
                    g.Clear(System.Drawing.Color.White);

                    //Draw the food rectangles
                    for (int j = 0; j < food.Count; j++)
                        g.FillRectangle(Params.foodBrush, 
                                        food[j].X - Params.foodSize / 2, 
                                        food[j].Y - Params.foodSize / 2, 
                                        Params.foodSize, 
                                        Params.foodSize);

                    //Draw the ant circles
                    for (int j = 0; j < ants.Length; j++)
                        if(j<highlightIndiciesUnder)
                            g.FillEllipse(Params.highlightedAntBrush, 
                                          ants[j].Position.X - Params.antSize / 2, 
                                          ants[j].Position.Y - Params.antSize / 2, 
                                          Params.antSize, 
                                          Params.antSize);
                        else
                            g.FillEllipse(Params.antBrush, 
                                          ants[j].Position.X - Params.antSize / 2, 
                                          ants[j].Position.Y - Params.antSize / 2, 
                                          Params.antSize, 
                                          Params.antSize);
                }

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
                    if (nearestFoodResult.Item2 < Params.minFoodCaptureDist)
                    {
                        //Add the food to the list of food which needs to be replaced at the end of the round
                        foodToRemove.Add(nearestFoodResult.Item1);
                        //Increment the ant score
                        score[j]++;
                    }
                    
                    //Calculate the wrapping change in x and y
                    //float dx = Math.Abs(ants[j].Position.X - nearestFoodResult.Item1.X);
                    //float dy = Math.Abs(ants[j].Position.Y - nearestFoodResult.Item1.Y);
                    //if (dx > Params.clientWidth / 2)
                    //    dx = (Params.clientWidth / 2) - dx;
                    //if (dy > Params.clientHeight / 2)
                    //    dy = (Params.clientHeight / 2) - dy;
                    //This is our direction
                    //Vector2 foodDirection = new Vector2(dx, dy);
                    //Normalize the direction to represent the unit vector of the direction
                    //foodDirection.Normalize();

                    //Normalize the distance so we have a direction vector to the nearest food
                    Vector2 foodDirection = nearestFoodResult.Item1;
                    foodDirection = nearestFoodResult.Item1 - ants[j].Position;
                    foodDirection.Normalize();

                    if (isShowing)
                    {
                        //If we're showing the form, draw a line pointing to the food and a line pointing in the ant look at direction
                        g.DrawLine(Pens.Green, ants[j].Position.X, ants[j].Position.Y, ants[j].Position.X + foodDirection.X * 10, ants[j].Position.Y + foodDirection.Y * 10);
                        g.DrawLine(Pens.Yellow, ants[j].Position.X, ants[j].Position.Y, ants[j].Position.X + ants[j].LookAt.X * 10, ants[j].Position.Y + ants[j].LookAt.Y * 10);
                        //Draw the buffer to the form
                        finalG.DrawImage(buffer, 0, 0);

                        //Update the form
                        this.Update();
                    }

                    //Generate the inputs for this iteration of the neural network
                    double[] NNInput = new double[] { foodDirection.X,foodDirection.Y, ants[j].LookAt.X, ants[j].LookAt.Y};

                    //Compute the outputs from the neural network
                    double[] NNOutput = population[j].ComputeNNOutputs(NNInput);

                    //Compute the force of rotation
                    double rotationForce = NNOutput[0] - NNOutput[1];

                    //Limit the degree of rotation
                    if (rotationForce < -Params.maxRotationRate)
                        rotationForce = -Params.maxRotationRate;
                    else if (rotationForce > Params.maxRotationRate)
                        rotationForce = Params.maxRotationRate;

                    //Add the degree of rotation to the orientation
                    ants[j].Orientation += rotationForce;

                    //Calculate the speed as the sum of the network outputs
                    ants[j].Speed = (NNOutput[0] + NNOutput[1]);

                    //Calculate the look at vector
                    ants[j].LookAt = new Vector2(-(float)Math.Sin(ants[j].Orientation), 
                                                  (float)Math.Cos(ants[j].Orientation));

                    //Adjust the position of the ant relative to the neural network outputs
                    float x = ants[j].LookAt.X * (float)ants[j].Speed + ants[j].Position.X;
                    float y = ants[j].LookAt.Y * (float)ants[j].Speed + ants[j].Position.Y;

                    //Wrap the ant location around the map so it represents a torus
                    x = (x + Params.clientWidth) % Params.clientWidth;
                    y = (y + Params.clientHeight) % Params.clientHeight;

                    //Set the new position
                    ants[j].Position = new Vector2(x, y);
                }

                //For each food vector which we need to remove because it was eaten
                foreach (Vector2 currentFood in foodToRemove)
                {
                    //Remove the vector from the list
                    food.Remove(currentFood);
                }

                for(int j = 0; j < Params.foodCount - food.Count;j++)
                    //Add a new food for every missing food
                    food.Add(new Vector2(Util.randNumGen.Next(0, Params.clientWidth - 1), 
                                         Util.randNumGen.Next(0, Params.clientHeight - 1)));
            }

            //If the simulation is showing at the end of the iteration count
            if (isShowing)
            {
                //Find the maximum score this round
                int maxIndex = -1;
                double maxScore = -1;
                for (int i = 0; i < score.Length; i++)
                    if (score[i] > maxScore)
                    {
                        maxIndex = i;
                        maxScore = score[i];
                    }

                //Display the best score for one second
                label.Text = "Done! Max Score: " + maxScore;

                //Update the form
                this.Update();
            }

            //Return the scores from this round
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
                //Calculate the distance in a wrapped space
                //double dx = Math.Abs(input.X - foodItem.X);
                //double dy = Math.Abs(input.Y - foodItem.Y);
                //if (dx > Params.clientWidth / 2)
                //    dx = (Params.clientWidth / 2) - dx;
                //if (dy > Params.clientHeight / 2)
                //    dy = (Params.clientHeight / 2) - dy;

                //We don't need to square root it because we're just looking at the relative distance
                //double d = (dx * dx) + (dy * dy);
                

                //Calculate the distance
                double d = Vector2.Distance(input, foodItem);
                //double d = Math.Sqrt((input.X - foodItem.X) * (input.X - foodItem.X) + 
                //                     (input.Y - foodItem.Y) * (input.Y - foodItem.Y));

                //If this is a new biggest
                if (d < minDist)
                {
                    //Set the distance and food item to the current iteration results
                    minDist = d;
                    minFoodItem = foodItem;
                }
            }

            //Confirm the minimum distance is the euclidean distance
            //minDist = Math.Sqrt((input.X - minFoodItem.X) * (input.X - minFoodItem.X) +
            //                    (input.Y - minFoodItem.Y) * (input.Y - minFoodItem.Y));

            //Return the results
            return new Tuple<Vector2, double>(minFoodItem, minDist);
        }

        #endregion
    }
}
