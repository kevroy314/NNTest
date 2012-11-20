using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    //This class represents a population of neural networks which can breed, mutate, and evaluate their fitness
    class NNPopulation
    {
        #region Constant Values

        //The number of iterations the NNPopulationSimulation which calculates the fitness should run
        private const int numFitnessSimulationIterations = 500;

        #endregion

        #region Member Variables

        //These variables are used to manage the competition and evolution of the neural networks
        private List<NN> population; //The list of neural networks, and the population for the simulation
        private int[] networkStructure; //The structure of the neural networks
        private int[] hiddenLayerStructure; //The hidden layer structure of the neural networks
        private int startPopulationSize; //The starting population size of the genetic algorithm
        private double[] latestFitness; //The latest fitness of the genetic algorithm while processing

        #endregion

        #region Constructors

        public NNPopulation(int startingPopulationSize, int[] neuralNetworkLayerSizes)
        {
            //Populate the input variables
            networkStructure = neuralNetworkLayerSizes;
            startPopulationSize = startingPopulationSize;

            //Generate the hidden layer structure from the input network structure
            int[] hiddenLayers = new int[neuralNetworkLayerSizes.Length - 2];
            for(int i = 0; i < hiddenLayers.Length;i++)
                hiddenLayers[i] = neuralNetworkLayerSizes[i+1];
            hiddenLayerStructure = hiddenLayers;

            //Create the population list
            population = new List<NN>();

            //Generate nerual networks for the genetic algorithm to use to compete
            for (int i = 0; i < startingPopulationSize; i++)
                population.Add(new NN(neuralNetworkLayerSizes[0], neuralNetworkLayerSizes[neuralNetworkLayerSizes.Length - 1], neuralNetworkLayerSizes.Length - 2, hiddenLayers));
        }

        #endregion

        #region Genetic Functions

        //The fitness function requires the user to select a type which they can use to create a simulation.
        //This type must implement NNPopulationSimulation.
        public double[] CalculateFitness(Type simulationType)
        {
            //Create an instance of a NNPopulationSimulation of the specified type
            NNPopulationSimulation sim = (NNPopulationSimulation)Activator.CreateInstance(simulationType, new object[] { population.Count });

            //Show the simulation (the simulation may choose to not perform any operations on this call
            sim.ShowSimulation();

            //Run the simulation on the current population for a set number of iterations
            double[] output = sim.RunPopulationSimulation(population, numFitnessSimulationIterations);

            //Close the simulation if it requires that
            sim.CloseSimulation();

            //Return the simulation output
            return output;
        }

        //CONTINUE DOCUMENTING HERE!
        //Need correct selection method
        public Tuple<int, int>[] SelectCouplesToBreed(double[] fitnesses)
        {
            Tuple<int,int>[] output = new Tuple<int,int>[fitnesses.Length];

            List<KeyValuePair<int, double>> fitList = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < fitnesses.Length; i++)
                fitList.Add(new KeyValuePair<int, double>(i, fitnesses[i]));
            fitList.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair.Value); });

            output[0] = new Tuple<int,int>(fitList[fitList.Count-1].Key,0);
            for(int i = 1; i < fitList.Count;i++)
                output[i] = new Tuple<int,int>(fitList[i-1].Key,fitList[i].Key);

            return output;
        }

        public List<NN> Breed(Tuple<int, int>[] couples)
        {
            List<NN> newPopulation = new List<NN>();
            for (int i = 0; i < couples.Length; i++)
            {
                newPopulation.Add(new NN(networkStructure[0], networkStructure[networkStructure.Length - 1], hiddenLayerStructure.Length, hiddenLayerStructure));
                for (int j = 0; j < newPopulation[i].Weights.Length; j++)
                    newPopulation[i].Weights[j] = (population[couples[i].Item1].Weights[j] + population[couples[i].Item2].Weights[j]) / 2;
            }
            return newPopulation;
        }

        public void Mutate(double weightMutationProbability, int minNumberOfMutations, int maxNumberOfMutations, double weightMutationIntensityRange)
        {
            int numberOfMutations = Util.randNumGen.Next(minNumberOfMutations,maxNumberOfMutations);
            for (int i = 0; i < population.Count; i++)
                for(int j = 0; j < numberOfMutations;j++)
                    if (Util.randNumGen.NextDouble() <= weightMutationProbability)
                        population[i].Weights[Util.randNumGen.Next(0, population[i].Weights.Length - 1)] += (Util.randNumGen.NextDouble() * weightMutationIntensityRange) - (Util.randNumGen.NextDouble() * weightMutationIntensityRange);
        }

        public void RunGeneration()
        {
            double[] fitnesses = CalculateFitness();
            latestFitness = fitnesses;
            Tuple<int,int>[] breedingCouples = SelectCouplesToBreed(fitnesses);
            population = Breed(breedingCouples);
            Mutate(0.05, 0, (int)((double)population.Count * 0.5), 0.5);
        }

        #endregion

        #region Properties

        public List<NN> Population
        {
            get { return population; }
            set { population = value; }
        }

        public int[] NetworkStructure
        {
            get { return networkStructure; }
            set { networkStructure = value; }
        }

        public int[] HiddenLayerStructure
        {
            get { return hiddenLayerStructure; }
            set { hiddenLayerStructure = value; }
        }

        public int StartPopulationSize
        {
            get { return startPopulationSize; }
            set { startPopulationSize = value; }
        }

        public double[] LatestFitness
        {
            get { return latestFitness; }
            set { latestFitness = value; }
        }

        #endregion
    }
}
