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

        //The number of iterations the NNPopulationSimulation which calculates the fitness should run (range 1-inf)
        private const int numFitnessSimulationIterations = 2000;

        //This probability will decide how likely it is a genes will mutate if it is selected to mutate (range 0-1)
        private const double probabilityOfWeightMutationIfChosenToMutate = 0.05;
        //This proportion determines the minimum number of genes that could mutate as a proprtion of the total number of genes (range 0-1)
        private const double minimumProportionOfMutationsPerPopulationMember = 0;
        //This proportion determines the maximum number of genes that could mutate as a proportion of the total number of genes (range 0-1)
        private const double maximumProportionOfMutationsPerPopulationMember = 1;
        //This range determines the maximum and minimum amount each gene will mutate if chosen to mutate (range -inf-inf)
        private const double rangeOfMutationIfChosenToMutate = 0.5;

        //This weight determines the amount of genetic material the first parent will contribute (range 0-1, firstParentBreedingWeight + secondParentBreedingWeight should equal 1 to avoid unintended mutation)
        private const double firstParentBreedingWeight = 0.5;
        //This weight determines the amount of genetic material the second parent will contribute (range 0-1, firstParentBreedingWeight + secondParentBreedingWeight should equal 1 to avoid unintended mutation)
        private const double secondParentBreedingWeight = 0.5;

        //This proportion determines the amount of entities which will breed randomly of the top fitness entities (range 0-1)
        private const double proportionToBreed = 0.2;

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
        public double[] CalculateFitness(Type simulationType, bool showSimulation)
        {
            //Create an instance of a NNPopulationSimulation of the specified type
            NNPopulationSimulation sim = (NNPopulationSimulation)Activator.CreateInstance(simulationType, new object[] { population.Count });

            if(showSimulation)
            //Show the simulation (the simulation may choose to not perform any operations on this call
            sim.ShowSimulation();

            //Run the simulation on the current population for a set number of iterations
            double[] output = sim.RunPopulationSimulation(population, numFitnessSimulationIterations);

            if(showSimulation)
            //Close the simulation if it requires that
            sim.CloseSimulation();

            //Return the simulation output
            return output;
        }

        //This function selects the couples which should breed.
        public Tuple<int, int>[] SelectCouplesToBreed(double[] fitnesses)
        {
            //Build an output array
            Tuple<int,int>[] output = new Tuple<int,int>[fitnesses.Length];

            //Create a list of fitness/index combinations and populate it with the fitnesses of members of the population.
            List<KeyValuePair<int, double>> fitList = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < fitnesses.Length; i++)
                fitList.Add(new KeyValuePair<int, double>(i, fitnesses[i]));

            //Sort the list (will put it in ascending order)
            fitList.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair.Value); });

            //Reverse the list so that index 0 is the most successful index.
            fitList.Reverse();

            //Get the number of top networks to breed
            int numToBreed = (int)(proportionToBreed * population.Count);

            //Create a list of potentially breedable couples
            List<Tuple<int, int>> potentialBreeders = new List<Tuple<int, int>>();

            //Loop through the triangular combination of the top numToBreed candidates
            for (int i = 0; i < numToBreed; i++)
                for (int j = i; j < numToBreed; j++)
                    //Select their indicies
                    potentialBreeders.Add(new Tuple<int, int>(fitList[i].Key, fitList[j].Key));

            //Randomly shuffle via inside out Knuth Shuffle
            for (int i = 0; i < potentialBreeders.Count; i++)
            {
                int rand = Util.randNumGen.Next(0, potentialBreeders.Count - 1);
                Tuple<int, int> tmp = potentialBreeders[i];
                potentialBreeders[i] = potentialBreeders[rand];
                potentialBreeders[rand] = tmp;
            }

            //Pull out the selected breeders, if there are not enough potential breeders to produce the appropriate number of offspring, loop back around and use the same breeders again.
            for (int i = 0; i < output.Length; i++)
                output[i] = potentialBreeders[i % potentialBreeders.Count];

            return output;
        }

        //This function will breed a set of couples defined by index tuples in the population
        public List<NN> Breed(Tuple<int, int>[] couples)
        {
            //Create a list of new neural networks for the next generation
            List<NN> newPopulation = new List<NN>();

            //For each couple in the breeding set
            for (int i = 0; i < couples.Length; i++)
            {
                //Add a new neural network to the population with the same structure as the rest of the population
                newPopulation.Add(new NN(networkStructure[0], networkStructure[networkStructure.Length - 1], hiddenLayerStructure.Length, hiddenLayerStructure));

                //For each weight in the breeding couple, average the weights of the parents to create the child weights
                for (int j = 0; j < newPopulation[i].Weights.Length; j++)
                    newPopulation[i].Weights[j] = (population[couples[i].Item1].Weights[j] * firstParentBreedingWeight + population[couples[i].Item2].Weights[j] * secondParentBreedingWeight);
            }
            
            //Return the new population
            return newPopulation;
        }

        //This function will mutate the population given some probability, a min and max number of mutations, and a range for the mutation (the weight, if mutated, will mutate within that range either positive or negative)
        public void Mutate(double weightMutationProbability, int minNumberOfMutations, int maxNumberOfMutations, double weightMutationIntensityRange)
        {
            //Decide how many mutations will happen, at most
            int numberOfMutations = Util.randNumGen.Next(minNumberOfMutations,maxNumberOfMutations);

            //For each member of the population, provide the appropriate number of opportunities for mutation
            for (int i = 0; i < population.Count; i++)
                for(int j = 0; j < numberOfMutations;j++)
                    //Decide if a given mutation chance will result in an actual mutation
                    if (Util.randNumGen.NextDouble() <= weightMutationProbability)
                        //Mutate a random weight within the mutation range (this weight could be the same weight every mutation, resulting in compounded mutations on a given weight)
                        population[i].Weights[Util.randNumGen.Next(0, population[i].Weights.Length - 1)] += (Util.randNumGen.NextDouble() * weightMutationIntensityRange) - (Util.randNumGen.NextDouble() * weightMutationIntensityRange);
        }

        //Run a generation given a particular fitness simulation, this simulation must implement NNPopulationSimulation
        public void RunGeneration(Type simulationType, bool showSimulation)
        {
            //Calculate the fitnesses of a given simiulation, this simulation must implement NNPopulationSimulation
            latestFitness = CalculateFitness(simulationType, showSimulation);

            //Select the breeding couples given their fitness
            Tuple<int,int>[] breedingCouples = SelectCouplesToBreed(latestFitness);

            //Breed the selected couples
            population = Breed(breedingCouples);

            //Mutate the new population
            Mutate(probabilityOfWeightMutationIfChosenToMutate, (int)(((double)population.Count) * minimumProportionOfMutationsPerPopulationMember), (int)(((double)population.Count) * maximumProportionOfMutationsPerPopulationMember), rangeOfMutationIfChosenToMutate);
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
