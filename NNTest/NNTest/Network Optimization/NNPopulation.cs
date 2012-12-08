using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    /* This class represents a population of neural networks which can breed, mutate, and evaluate their fitness
     */

    class NNPopulation
    {
        //The possible breeding functions the population can use when transitioning generations.
        public enum BreedingFunction { Average = 0, Crossover = 1, PickEach = 2 };

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
            for (int i = 0; i < hiddenLayers.Length; i++)
                hiddenLayers[i] = neuralNetworkLayerSizes[i + 1];
            hiddenLayerStructure = hiddenLayers;

            //Create the population list
            population = new List<NN>();

            //Generate nerual networks for the genetic algorithm to use to compete
            for (int i = 0; i < startingPopulationSize; i++)
                population.Add(new NN(neuralNetworkLayerSizes[0], 
                                      neuralNetworkLayerSizes[neuralNetworkLayerSizes.Length - 1], 
                                      neuralNetworkLayerSizes.Length - 2, 
                                      hiddenLayers));
        }

        #endregion

        #region Genetic Functions

        //The fitness function requires the user to select a type which they can use to create a simulation.
        //This type must implement NNPopulationSimulation.
        public double[] CalculateFitness(Type simulationType, bool showSimulation)
        {
            //Create an instance of a NNPopulationSimulation of the specified type
            INNPopulationSimulation sim = (INNPopulationSimulation)Activator.CreateInstance(simulationType, 
                                                                                          new object[] { population.Count });

            if (showSimulation)
                //Show the simulation (the simulation may choose to not perform any operations on this call
                sim.ShowSimulation();

            //Run the simulation on the current population for a set number of iterations
            double[] output = sim.RunPopulationSimulation(population, 
                                                          Params.numFitnessSimulationIterations, 
                                                          Params.numberOfElites * Params.numOfCopiesOfElites);

            if (showSimulation)
                //Close the simulation if it requires that
                sim.CloseSimulation();

            //Return the simulation output
            return output;
        }

        public KeyValuePair<int, double>[] GetSortedFitnessList(double[] fitnesses)
        {
            //Create a list of fitness/index combinations and populate it with the fitnesses of members of the population.
            List<KeyValuePair<int, double>> fitList = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < fitnesses.Length; i++)
                fitList.Add(new KeyValuePair<int, double>(i, fitnesses[i]));

            //Sort the list (will put it in ascending order)
            fitList.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair.Value); });

            //Reverse the list so that index 0 is the most successful index.
            fitList.Reverse();

            return fitList.ToArray();
        }

        //This function selects the couples which should breed.
        public Tuple<int, int>[] SelectCouplesToBreed(KeyValuePair<int, double>[] sortedFitnessList)
        {
            //Build an output array
            Tuple<int, int>[] output = new Tuple<int, int>[sortedFitnessList.Length];

            //Get the number of top networks to breed
            int numToBreed = (int)(Params.proportionToBreed * population.Count);

            //Create a list of potentially breedable couples
            List<Tuple<int, int>> potentialBreeders = new List<Tuple<int, int>>();

            //Loop through the triangular combination of the top numToBreed candidates
            for (int i = 0; i < numToBreed; i++)
                for (int j = i + 1; j < numToBreed; j++)
                    //Select their indicies
                    potentialBreeders.Add(new Tuple<int, int>(sortedFitnessList[i].Key, 
                                                              sortedFitnessList[j].Key));

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

        public List<NN> BreedPickEach(Tuple<int, int>[] couples, KeyValuePair<int, double>[] sortedFitnessList)
        {
            //Create a list of new neural networks for the next generation
            List<NN> newPopulation = new List<NN>();

            //Place the elites in the population
            for (int i = 0; i < Params.numberOfElites; i++)
                for (int j = 0; j < Params.numOfCopiesOfElites; j++)
                    newPopulation.Add(population[sortedFitnessList[i].Key]);

            //Filling the remainder of the population
            int k = 0;
            while (newPopulation.Count < population.Count)
            {
                //If we randomly are above the crossover rate
                if (Util.randNumGen.NextDouble() > Params.crossoverRate)
                {
                    //Add the first parent
                    newPopulation.Add(population[couples[k].Item1]);
                    //If we have room for another add the second parent
                    if (!(newPopulation.Count <= population.Count))
                        newPopulation.Add(population[couples[k].Item2]);
                }
                //If we are below the crossover rate
                else
                {
                    //Create a new neural network with the same structure as the rest of the population
                    NN child1 = new NN(networkStructure[0], 
                                       networkStructure[networkStructure.Length - 1], 
                                       hiddenLayerStructure.Length, 
                                       hiddenLayerStructure);

                    //Create a new neural network with the same structure as the rest of the population
                    NN child2 = new NN(networkStructure[0], 
                                       networkStructure[networkStructure.Length - 1], 
                                       hiddenLayerStructure.Length, 
                                       hiddenLayerStructure);

                    //Add all genes from each parent up to the crossover point to the two different children
                    int i;
                    for (i = 0; i < population[k].Weights.Length; i++)
                    {
                        //Randomly stitch one of the parents genes onto each child
                        if (Util.randNumGen.NextDouble() > 0.5)
                        {
                            child1.Weights[i] = population[couples[k].Item1].Weights[i];
                            child2.Weights[i] = population[couples[k].Item2].Weights[i];
                        }
                        else
                        {
                            child1.Weights[i] = population[couples[k].Item2].Weights[i];
                            child2.Weights[i] = population[couples[k].Item1].Weights[i];
                        }
                    }

                    //Add the first child to the new population
                    newPopulation.Add(child1);
                    //If we have room for more, add the second child to the population
                    if (!(newPopulation.Count <= population.Count))
                        newPopulation.Add(child2);
                }

                //Confirm that the iteration variable does not exceed the array size from which it is pulling values
                k = (k + 1) % couples.Length;
            }

            //Return the new population
            return newPopulation;
        }

        //This breeding function will pick a point in the genome and cut it, creating two children from two parents with genes in some proportion contiguously from each parent
        public List<NN> BreedCrossover(Tuple<int, int>[] couples, KeyValuePair<int, double>[] sortedFitnessList)
        {
            //Create a list of new neural networks for the next generation
            List<NN> newPopulation = new List<NN>();

            //Place the elites in the population
            for (int i = 0; i < Params.numberOfElites; i++)
                for (int j = 0; j < Params.numOfCopiesOfElites; j++)
                    newPopulation.Add(population[sortedFitnessList[i].Key]);

            //Filling the remainder of the population
            int k = 0;
            while (newPopulation.Count < population.Count)
            {
                //If we randomly are above the crossover rate
                if (Util.randNumGen.NextDouble() > Params.crossoverRate)
                {
                    //Add the first parent
                    newPopulation.Add(population[couples[k].Item1]);
                    //If we have room for another add the second parent
                    if (!(newPopulation.Count <= population.Count))
                        newPopulation.Add(population[couples[k].Item2]);
                }
                //If we are below the crossover rate
                else
                {
                    //Create a new neural network with the same structure as the rest of the population
                    NN child1 = new NN(networkStructure[0], 
                                       networkStructure[networkStructure.Length - 1], 
                                       hiddenLayerStructure.Length, 
                                       hiddenLayerStructure);

                    //Create a new neural network with the same structure as the rest of the population
                    NN child2 = new NN(networkStructure[0], 
                                       networkStructure[networkStructure.Length - 1], 
                                       hiddenLayerStructure.Length, 
                                       hiddenLayerStructure);

                    //Determine the crossover point in the genes
                    int crossoverPoint = Util.randNumGen.Next(0, population[k].Weights.Length);

                    //Add all genes from each parent up to the crossover point to the two different children
                    int i;
                    for (i = 0; i < crossoverPoint; i++)
                    {
                        child1.Weights[i] = population[couples[k].Item1].Weights[i];
                        child2.Weights[i] = population[couples[k].Item2].Weights[i];
                    }

                    //Add all genes from each parent past the crossover point to the opposite children from the before the crossover point
                    for (; i < population[k].Weights.Length; i++)
                    {
                        child1.Weights[i] = population[couples[k].Item2].Weights[i];
                        child2.Weights[i] = population[couples[k].Item1].Weights[i];
                    }

                    //Add the first child to the new population
                    newPopulation.Add(child1);
                    //If we have room for more, add the second child to the population
                    if (!(newPopulation.Count <= population.Count))
                        newPopulation.Add(child2);
                }

                //Confirm that the iteration variable does not exceed the array size from which it is pulling values
                k = (k + 1) % couples.Length;
            }

            //Return the new population
            return newPopulation;
        }

        //This function will breed a set of couples defined by index tuples in the population
        public List<NN> BreedAverage(Tuple<int, int>[] couples, KeyValuePair<int, double>[] sortedFitnessList)
        {
            //Create a list of new neural networks for the next generation
            List<NN> newPopulation = new List<NN>();

            //Place the elites in the population
            for (int i = 0; i < Params.numberOfElites; i++)
                for (int j = 0; j < Params.numOfCopiesOfElites; j++)
                    newPopulation.Add(population[sortedFitnessList[i].Key]);

            //Filling the remainder of the population
            int k = 0;
            while (newPopulation.Count < population.Count)
            {
                //Add a new neural network to the population with the same structure as the rest of the population
                newPopulation.Add(new NN(networkStructure[0], 
                                         networkStructure[networkStructure.Length - 1], 
                                         hiddenLayerStructure.Length, 
                                         hiddenLayerStructure));

                //For each weight in the breeding couple, average the weights of the parents to create the child weights
                for (int j = 0; j < newPopulation[k].Weights.Length; j++)
                    newPopulation[k].Weights[j] = (population[couples[k].Item1].Weights[j] * Params.firstParentBreedingWeight + population[couples[k].Item2].Weights[j] * Params.secondParentBreedingWeight);

                //Confirm that the iteration variable does not exceed the array size from which it is pulling values
                k = (k + 1) % couples.Length;
            }

            //Return the new population
            return newPopulation;
        }

        //This function will mutate the population given some probability, a min and max number of mutations, and a range for the mutation (the weight, if mutated, will mutate within that range either positive or negative)
        public int Mutate(double weightMutationProbability, int minNumberOfMutations, int maxNumberOfMutations, double weightMutationIntensityRange)
        {
            //A counter that accumulates the actual number of times a mutation occurs
            int mutationCount = 0;

            //Decide how many mutations will happen, at most
            int numberOfMutations = Util.randNumGen.Next(minNumberOfMutations, maxNumberOfMutations);

            //For each member of the population, provide the appropriate number of opportunities for mutation
            for (int i = 0; i < population.Count; i++)
                for (int j = 0; j < numberOfMutations; j++)
                    //Decide if a given mutation chance will result in an actual mutation
                    if (Util.randNumGen.NextDouble() <= weightMutationProbability)
                    {
                        //Mutate a random weight within the mutation range (this weight could be the same weight every mutation, resulting in compounded mutations on a given weight)
                        int mutatedIndex = Util.randNumGen.Next(0, population[i].Weights.Length - 1);
                        double mutatedDegree = (Util.randNumGen.NextDouble() * weightMutationIntensityRange) - 
                                               (Util.randNumGen.NextDouble() * weightMutationIntensityRange);
                        population[i].Weights[mutatedIndex] += mutatedDegree;
                        mutationCount++;
                    }
            return mutationCount;
        }

        //Run a generation given a particular fitness simulation, this simulation must implement NNPopulationSimulation
        public void RunGeneration(Type simulationType, bool showSimulation, BreedingFunction function)
        {
            //Calculate the fitnesses of a given simiulation, this simulation must implement NNPopulationSimulation
            latestFitness = CalculateFitness(simulationType, showSimulation);

            KeyValuePair<int, double>[] sortedFitnessList = GetSortedFitnessList(latestFitness);

            //Select the breeding couples given their fitness
            Tuple<int, int>[] breedingCouples = SelectCouplesToBreed(sortedFitnessList);

            //Breed the selected couples
            if(function == BreedingFunction.Average)
                population = BreedAverage(breedingCouples, sortedFitnessList);
            else if(function == BreedingFunction.Crossover)
                population = BreedCrossover(breedingCouples, sortedFitnessList);
            else if(function == BreedingFunction.PickEach)
                population = BreedPickEach(breedingCouples, sortedFitnessList);

            //Mutate the new population
            Mutate(Params.probabilityOfWeightMutationIfChosenToMutate, 
                  (int)(((double)population.Count) * Params.minimumProportionOfMutationsPerPopulationMember), 
                  (int)(((double)population.Count) * Params.maximumProportionOfMutationsPerPopulationMember), 
                  Params.rangeOfMutationPerturbation);
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
