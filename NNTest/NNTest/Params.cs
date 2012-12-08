using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NNTest
{
    /* The Params class stores most of the configurable parameters in the application.
     */

    class Params
    {
        #region MainForm Parameters

        //The number of entities to be used in the population simulation
        public const int simulationPopulationSize = 30;

        //The structure of their brains
        public static readonly int[] neuralNetworkStructure = { 4, 6, 2 };

        #endregion

        #region NNPopulation Parameters

        //The number of iterations the NNPopulationSimulation which calculates the fitness should run (range 1-inf)
        public const int numFitnessSimulationIterations = 2000;

        //This probability will decide how likely it is a genes will mutate if it is selected to mutate (range 0-1)
        public const double probabilityOfWeightMutationIfChosenToMutate = 0.1;
        //This proportion determines the minimum number of genes that could mutate as a proprtion of the total number of genes (range 0-1)
        public const double minimumProportionOfMutationsPerPopulationMember = 0;
        //This proportion determines the maximum number of genes that could mutate as a proportion of the total number of genes (range 0-1)
        public const double maximumProportionOfMutationsPerPopulationMember = 1;
        //This range determines the maximum and minimum amount each gene will mutate if chosen to mutate (range -inf-inf)
        public const double rangeOfMutationPerturbation = 0.3;

        //This weight determines the amount of genetic material the first parent will contribute (range 0-1, firstParentBreedingWeight + secondParentBreedingWeight should equal 1 to avoid unintended mutation)
        public const double firstParentBreedingWeight = 0.5;
        //This weight determines the amount of genetic material the second parent will contribute (range 0-1, firstParentBreedingWeight + secondParentBreedingWeight should equal 1 to avoid unintended mutation)
        public const double secondParentBreedingWeight = 0.5;

        //This is used in the crossover breeding function, it determines the rate at which breeding results in a crossover (as opposed to a clone of the parents)
        public const double crossoverRate = 0.7;

        //This proportion determines the amount of entities which will breed randomly of the top fitness entities (range 0-1)
        public const double proportionToBreed = 0.5;

        //This sets the number of entities which will survive a generation because their fitness was high enough
        public const int numberOfElites = 4;
        //This sets the number of copies of each elite which should be placed back in the population
        public const int numOfCopiesOfElites = 1;

        #endregion

        #region NNAntSimulation Parameters

        //The amount of food which should be in each simulation at the beginning of each iteration
        public static int foodCount = 40;

        //The distance at which food may be captured (in pixels) by an ant
        public const double minFoodCaptureDist = 7;

        //The display size of the food
        public const int foodSize = 3;
        //The display size of the ant
        public const int antSize = 10;

        //The display Brush of the food
        public static Brush foodBrush = Brushes.Green;
        //The display Brush of the ant
        public static Brush antBrush = Brushes.Blue;
        //The display Brush of a highlighted ant
        public static Brush highlightedAntBrush = Brushes.Red;

        //Client Width is used at initialization of this class to determine the width in the designer
        public const int clientWidth = 400;
        //Client Height is used at initialization of this class to determine the height in the designer
        public const int clientHeight = 400;

        //Maximum amount of rotation the ant can have in any iteration
        public const double maxRotationRate = 0.3;

        #endregion

        #region NN Parameters

        //Used in the sigmoid function
        public const double activationResponse = 1;

        //The bias for the network
        public const double bias = -1;

        #endregion
    }
}
