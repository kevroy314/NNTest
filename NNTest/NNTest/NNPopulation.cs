using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    class NNPopulation
    {
        private static Random randNumGen = new Random();

        private List<NN> population;
        private int[] networkStructure;
        private int[] hiddenLayerStructure;
        private int startPopulationSize;

        public NNPopulation(int startingPopulationSize, int[] neuralNetworkLayerSizes)
        {
            networkStructure = neuralNetworkLayerSizes;
            startPopulationSize = startingPopulationSize;

            int[] hiddenLayers = new int[neuralNetworkLayerSizes.Length - 2];
            for(int i = 1; i < hiddenLayers.Length;i++)
                hiddenLayers[i-1] = neuralNetworkLayerSizes[i];
            hiddenLayerStructure = hiddenLayers;

            for (int i = 0; i < startingPopulationSize; i++)
                population.Add(new NN(neuralNetworkLayerSizes[0], neuralNetworkLayerSizes[neuralNetworkLayerSizes.Length - 1], neuralNetworkLayerSizes.Length - 2, hiddenLayers));
        }

        #region Genetic Functions

        public double[] CalculateFitness()
        {
            NNPopulationSimulation sim = new NNAntSimulation(population.Count);
            return sim.RunPopulationSimulation(population, 100);
        }

        //Need correct selection method
        public Tuple<int, int>[] SelectCouplesToBreed(double[] fitnesses)
        {
            List<Tuple<int, int>> output = new List<Tuple<int, int>>();
            for (int i = 1; i < fitnesses.Length * 0; i+=2)
                output.Add(new Tuple<int, int>(i, i - 1));
            return output.ToArray<Tuple<int, int>>();
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
            int numberOfMutations = randNumGen.Next(minNumberOfMutations,maxNumberOfMutations);
            for (int i = 0; i < population.Count; i++)
                for(int j = 0; j < numberOfMutations;j++)
                    if(randNumGen.NextDouble() <= weightMutationProbability)
                        population[i].Weights[randNumGen.Next(0, population.Count - 1)] += (randNumGen.NextDouble() * weightMutationIntensityRange) - (randNumGen.NextDouble() * weightMutationIntensityRange);
        }

        public void RunGeneration()
        {
            double[] fitnesses = CalculateFitness();
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

        #endregion
    }
}
