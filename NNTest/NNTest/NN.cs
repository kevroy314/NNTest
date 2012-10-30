using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNTest
{
    class NN
    {

        #region Member Variables

        private static Random randNumGen = new Random(); //Random Number Generator for Initialization

        //Constructor Parameters
        private int numberOfInputs;
        private int numberOfOutputs;
        private int numberOfHiddenLayers;
        private int[] numberOfNodesPerHiddenLayer;

        private int numberOfLayers; //The number of layers in the neural network (including input and output)

        private double[] weights; //The weights which comprise the neural network

        private int[] nodesPerLayer; //The nodes per layer (index 0 is input, followed by hidden layers, followed by output layers)
        private int[] inputsPerLayer; //The number of inputs for nodes in given layers
        private int[] weightsPerLayer; //The number of weights for nodes in given layers (one plus the number of inputs)
        private int[] weightStartIndexPerLayer; //The start index for each weights associated with each layer in the weight array

        #endregion

        public NN(int numInputs, int numOutputs, int numHiddenLayers, int[] nodesPerHiddenLayer)
        {
            numberOfInputs = numInputs;
            numberOfOutputs = numOutputs;
            numberOfHiddenLayers = numHiddenLayers;
            numberOfNodesPerHiddenLayer = nodesPerHiddenLayer;

            numberOfLayers = 2 + numHiddenLayers; //The input layer, output layer and hidden layers
            nodesPerLayer = new int[numberOfLayers];
            nodesPerLayer[0] = numInputs; //Input layer nodes
            for (int i = 1; i < numHiddenLayers + 1; i++)
                nodesPerLayer[i] = nodesPerHiddenLayer[i - 1]; //Hidden layer nodes
            nodesPerLayer[nodesPerLayer.Length - 1] = numOutputs; //Output layer nodes

            inputsPerLayer = new int[numberOfLayers];
            weightsPerLayer = new int[numberOfLayers];
            weightStartIndexPerLayer = new int[numberOfLayers];

            int numWeights = nodesPerLayer[0] * 2; //Add two weights (normal weight and bias for each input) for each input node
            inputsPerLayer[0] = 1; //Input layer
            weightsPerLayer[0] = 2; //Input layer, each node has inputs and bias
            weightStartIndexPerLayer[0] = 0; //Input layer weights start at index 0

            for (int i = 1; i < numberOfLayers - 1; i++) //For every hidden layer
            {
                inputsPerLayer[i] = nodesPerLayer[i - 1]; //Set the number of inputs in the layer to the number of nodes in the previous layer
                weightsPerLayer[i] = nodesPerLayer[i - 1] + 1; //Set the number of weights in the layer to the number of nodes in the previous layer plus one for the bias
                weightStartIndexPerLayer[i] = numWeights; //Set the start index for the layer equal to the current number of weights calculated minus 1
                numWeights += weightsPerLayer[i] * nodesPerLayer[i]; //Add number of weights for each node from previous hidden layer
            }

            inputsPerLayer[numberOfLayers - 1] = nodesPerLayer[numberOfLayers - 2]; //Output layer has the same number of inputs as there are nodes in the previous layer (could be input layer if no hidden layers exist)
            weightsPerLayer[numberOfLayers - 1] = nodesPerLayer[numberOfLayers - 2] + 1; //Output layer has the same number of weights as there are nodes in the previous layer plus one (could be input layer if no hidden layers exist)
            weightStartIndexPerLayer[numberOfLayers - 1] = numWeights; //Set the start index for the layer equal to the current number of weights calculated minus 1
            numWeights += (nodesPerLayer[numberOfLayers - 2] + 1) * nodesPerLayer[numberOfLayers - 1]; //Add number of weights for each node for output layer

            weights = new double[numWeights];

            //Populate the weights with random values between -1 and 1
            for (int i = 0; i < weights.Length; i++)
                weights[i] = randNumGen.NextDouble() - randNumGen.NextDouble();
        }

        #region Properties

        public int NumberOfLayers
        {
            get { return numberOfLayers; }
            set { numberOfLayers = value; }
        }

        public double[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        public int[] NodesPerLayer
        {
            get { return nodesPerLayer; }
            set { nodesPerLayer = value; }
        }

        public int[] InputsPerLayer
        {
            get { return inputsPerLayer; }
            set { inputsPerLayer = value; }
        }

        public int[] WeightsPerLayer
        {
            get { return weightsPerLayer; }
            set { weightsPerLayer = value; }
        }

        public int[] WeightStartIndexPerLayer
        {
            get { return weightStartIndexPerLayer; }
            set { weightStartIndexPerLayer = value; }
        }

        public int NumberOfInputs
        {
            get { return numberOfInputs; }
            set { numberOfInputs = value; }
        }

        public int NumberOfOutputs
        {
            get { return numberOfOutputs; }
            set { numberOfOutputs = value; }
        }

        public int NumberOfHiddenLayers
        {
            get { return numberOfHiddenLayers; }
            set { numberOfHiddenLayers = value; }
        }

        public int[] NumberOfNodesPerHiddenLayer
        {
            get { return numberOfNodesPerHiddenLayer; }
            set { numberOfNodesPerHiddenLayer = value; }
        }

        #endregion
    }
}
