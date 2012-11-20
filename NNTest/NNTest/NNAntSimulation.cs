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
    public partial class NNAntSimulation : Form, NNPopulationSimulation
    {
        private Random randNumGen = new Random();

        private Tuple<double, double>[] directionVector;
        private Vector2[] location;

        private int simAreaWidth;
        private int simAreaHeight;

        private List<Vector2> food;
        private const int foodCount = 100;

        private const double minFoodCaptureDist = 10;

        public NNAntSimulation(int populationSize)
        {
            simAreaWidth = this.Width;
            simAreaHeight = this.Height;

            InitializeComponent();

            directionVector = new Tuple<double, double>[populationSize];
            location = new Vector2[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                directionVector[i] = new Tuple<double, double>(randNumGen.NextDouble(), randNumGen.NextDouble());
                location[i] = new Vector2(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1));
            }

            food = new List<Vector2>();
            for (int i = 0; i < foodCount; i++)
                food.Add(new Vector2(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1)));
        }

        public double[] RunPopulationSimulation(List<NN> population, int numberOfIterations)
        {
            double[] score = new double[population.Count];
            Bitmap buffer = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(buffer);
            Graphics finalG = this.CreateGraphics();
            for (int i = 0; i < numberOfIterations; i++)
            {
                label.Text = "Iteration # " + i + " in progress...";
                g.Clear(System.Drawing.Color.White);
                for (int j = 0; j < food.Count; j++)
                    g.FillRectangle(Brushes.Red, food[j].X, food[j].Y, 1, 1);
                for (int j = 0; j < location.Length; j++)
                    g.FillEllipse(Brushes.Blue, location[j].X, location[j].Y, 2, 2);
                this.Update();
                int numFoodToAdd = 0;
                for (int j = 0; j < population.Count; j++)
                {
                    Tuple<int,double> nearestFoodIndex = findNearestFoodIndex(location[j]);
                    Vector2 nearestFood = food[nearestFoodIndex.Item1];
                    nearestFood.Normalize();
                    Vector2 loc = location[j];
                    //loc.Normalize();
                    double[] NNInput = new double[] { nearestFood.X, nearestFood.Y, loc.X, loc.Y };
                    double[] NNOutput = population[j].ComputeNNOutputs(NNInput);
                    
                    location[j].X += (int)Math.Round(NNOutput[0]);
                    location[j].Y += (int)Math.Round(NNOutput[1]);
                    if (location[j].X < 0) location[j].X = 0;
                    else if (location[j].X > this.Width) location[j].X = this.Width;
                    if (location[j].Y < 0) location[j].Y = 0;
                    else if (location[j].Y > this.Height) location[j].Y = this.Height;
                    
                    if (nearestFoodIndex.Item2 <= minFoodCaptureDist)
                    {
                        food.RemoveAt(nearestFoodIndex.Item1);
                        numFoodToAdd++;
                        score[j]++;
                    }
                }
                for(int j = 0; j < numFoodToAdd;j++)
                    food.Add(new Vector2(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1)));
                finalG.DrawImage(buffer, 0, 0);
            }
            int maxIndex = -1;
            double maxScore = -1;
            for(int i = 0; i < score.Length;i++)
                if (score[i] > maxScore)
                {
                    maxIndex = i;
                    maxScore = score[i];
                }
            label.Text = "Done! Max Score: "+maxScore;
            this.Update();
            Thread.Sleep(1000);
            return score;
        }

        private Tuple<int,double> findNearestFoodIndex(Vector2 input)
        {
            int output = -1;
            double outputDist = int.MaxValue;
            for (int i = 0; i < food.Count; i++)
            {
                double d = Math.Sqrt(Math.Pow(input.X - food[i].X, 2) + Math.Pow(input.Y - food[i].Y, 2));
                if (d < outputDist)
                {
                    outputDist = d;
                    output = i;
                }
            }
            return new Tuple<int,double>(output,outputDist);
        }
    }
}
