using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NNTest
{
    public partial class NNAntSimulation : Form, NNPopulationSimulation
    {
        private Random randNumGen = new Random();

        private Tuple<double, double>[] lookAt;
        private Point[] location;

        private int simAreaWidth;
        private int simAreaHeight;

        private List<Point> food;
        private const int foodCount = 100;

        private const double minFoodCaptureDist = 1;

        public NNAntSimulation(int populationSize)
        {
            simAreaWidth = this.Width;
            simAreaHeight = this.Height;

            InitializeComponent();

            lookAt = new Tuple<double, double>[populationSize];
            location = new Point[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                lookAt[i] = new Tuple<double, double>(randNumGen.NextDouble(), randNumGen.NextDouble());
                location[i] = new Point(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1));
            }

            food = new List<Point>();
            for (int i = 0; i < foodCount; i++)
                food.Add(new Point(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1)));
        }

        public double[] RunPopulationSimulation(List<NN> population, int numberOfIterations)
        {
            double[] score = new double[population.Count];

            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int j = 0; j < population.Count; j++)
                {
                    Tuple<int,double> nearestFoodIndex = findNearestFoodIndex(location[j]);
                    double[] NNInput = new double[] { food[nearestFoodIndex.Item1].X,food[nearestFoodIndex.Item1].Y,lookAt[j].Item1,lookAt[j].Item2};
                    double[] NNOutput = population[j].ComputeNNOutputs(NNInput);
                    location[j].X += (int)Math.Round(lookAt[j].Item1 * NNOutput[0]);
                    location[j].Y += (int)Math.Round(lookAt[j].Item2 * NNOutput[1]);
                    if (nearestFoodIndex.Item2 <= minFoodCaptureDist)
                    {
                        food.RemoveAt(nearestFoodIndex.Item1);
                        food.Add(new Point(randNumGen.Next(0, simAreaWidth - 1), randNumGen.Next(0, simAreaHeight - 1)));
                        score[j]++;
                    }
                }
            }

            return score;
        }

        private Tuple<int,double> findNearestFoodIndex(Point input)
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
