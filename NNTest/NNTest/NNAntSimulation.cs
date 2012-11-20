﻿using System;
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
    //This simulation class provides a way for the NN to determine its fitness each generation.
    //It is also a form, thus satisfying the requirement for a Show and Close function.
    public partial class NNAntSimulation : Form, NNPopulationSimulation
    {
        #region Constant Values

        //The amount of food which should be in each simulation at the beginning of each iteration
        private const int foodCount = 100;

        //The distance at which food may be captured (in pixels) by an ant
        private const double minFoodCaptureDist = 10;

        #endregion

        #region Member Variables

        //Create two arrays representing the locations in space each ant lives as well as the direction they are facing
        private Vector2[] directionVector;
        private Vector2[] location;

        //Create a list of food locations. A list is used instead of an array so the adding and removing is handled cleanly for us.
        private List<Vector2> food;

        //Is the simulation actually shown
        private bool isShowing;

        #endregion

        #region Constructors

        //Construct an ant simulation with a certain population
        public NNAntSimulation(int populationSize)
        {
            InitializeComponent();

            //We don't show the form by default
            isShowing = false;

            //Initialize the positions and directions of all the ants
            directionVector = new Vector2[populationSize];
            location = new Vector2[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                //Use a random direction vector and normalize it (it is a direction after all)
                directionVector[i] = new Vector2((float)Util.randNumGen.NextDouble(), (float)Util.randNumGen.NextDouble());
                directionVector[i].Normalize();

                //Initialize each location to some pixel location on the simulation draw area (in unit pixels)
                location[i] = new Vector2(Util.randNumGen.Next(0, this.Width - 1), Util.randNumGen.Next(0, this.Height - 1));
            }

            //Initialize the list of food
            food = new List<Vector2>();

            //Fill the food list with randomly generated food positions on the canvas
            for (int i = 0; i < foodCount; i++)
                food.Add(new Vector2(Util.randNumGen.Next(0, this.Width - 1), Util.randNumGen.Next(0, this.Height - 1)));
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
        public double[] RunPopulationSimulation(List<NN> population, int numberOfIterations)
        {
            //Generate an array in which the score will be output for each ant
            double[] score = new double[population.Count];

            //Initialize the variables for drawing (may not be used if isShowing stays false)
            Bitmap buffer = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(buffer);
            Graphics finalG = this.CreateGraphics();

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
                        g.FillRectangle(Brushes.Red, food[j].X, food[j].Y, 1, 1);

                    //Draw the ant circles
                    for (int j = 0; j < location.Length; j++)
                        g.FillEllipse(Brushes.Blue, location[j].X, location[j].Y, 2, 2);

                    //Draw the buffer to the form
                    finalG.DrawImage(buffer, 0, 0);

                    //Update the form
                    this.Update();
                }

                //Create a list to store the food which should be replaced at the end of the loop
                //This is done as opposed to removing as we go so that multiple ants can, in theory, "share" a piece of food
                //This was a design choice to prevent the number the ant is in the list from influencing it's success
                List<Vector2> foodToRemove = new List<Vector2>();

                //Iterate through each member of the population
                for (int j = 0; j < population.Count; j++)
                {
                    //Find the nearest food item
                    Tuple<Vector2, double> nearestFoodResult = findNearestFoodLocationAndDistance(location[j]);

                    //If the ant is near enough the food to capture it
                    if (nearestFoodResult.Item2 <= minFoodCaptureDist)
                    {
                        //Add the food to the list of food which needs to be replaced at the end of the round
                        foodToRemove.Add(nearestFoodResult.Item1);
                        //Increment the ant score
                        score[j]++;
                    }

                    //Normalize the distance so we have a direction vector to the nearest food
                    nearestFoodResult.Item1.Normalize();

                    //Locally store the location of the current ant so we can manipulate it if needed
                    Vector2 loc = location[j];
                    //loc.Normalize();

                    //NOTE: THIS SECTION SHOULD BE CHANGED TO REFLECT MORE INPUTS WHICH COULD PRODUCE A MORE ROBUST NETWORK
                    //Generate the inputs for this iteration of the neural network
                    double[] NNInput = new double[] { nearestFoodResult.Item1.X, nearestFoodResult.Item1.Y, 1, 1 };

                    //Compute the outputs from the neural network
                    double[] NNOutput = population[j].ComputeNNOutputs(NNInput);
                    
                    //Adjust the position of the ant relative to the neural network outputs
                    location[j].X += (int)Math.Round(NNOutput[0]);
                    location[j].Y += (int)Math.Round(NNOutput[1]);

                    //Confine the ant to the boundries of the area
                    //An ant can attempt to leave the boundries, but will simply walk against the wall
                    //This could, in principle, create a situation where an ant is stuck because it's inputs aren't changing
                    if (location[j].X < 0) location[j].X = 0;
                    else if (location[j].X > this.Width) location[j].X = this.Width;
                    if (location[j].Y < 0) location[j].Y = 0;
                    else if (location[j].Y > this.Height) location[j].Y = this.Height;
                }

                //For each food vector which we need to remove because it was eaten
                foreach (Vector2 currentFood in foodToRemove)
                {
                    //Remove the vector from the list
                    food.Remove(currentFood);
                    //Add a new one in its place
                    food.Add(new Vector2(Util.randNumGen.Next(0, this.Width - 1), Util.randNumGen.Next(0, this.Height - 1)));
                }
            }

            //If the simulation is showing at the end
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
                this.Update();
                Thread.Sleep(1000);
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
