using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NNTest
{
    [Serializable]
    public class NN
    {
        #region Member Variables

        private static Random randNumGen = new Random(); //Random Number Generator for Initialization

        //Constructor Parameters
        private int numberOfInputs;
        private int numberOfOutputs;
        private int numberOfHiddenLayers;
        private int[] numberOfNodesPerHiddenLayer;

        private int numberOfLayers; //The number of layers in the neural network (including input and output)
        private int numberOfNodes; //The total number of nodes in the network
        private int numberOfHiddenNodes; //the total number of hidden nodes in the network

        private int[] nodesPerLayer; //The nodes per layer (index 0 is input, followed by hidden layers, followed by output layers)
        private int[] inputsPerLayer; //The number of inputs for nodes in given layers
        private int[] weightsPerLayer; //The number of weights for nodes in given layers (one plus the number of inputs)
        private int[] weightStartIndexPerLayer; //The start index for each weights associated with each layer in the weight array
        private int[][] weightStartIndexPerLayerPerNode; //The (layer,node) indexed location of each weight set for any node in any layer

        private double[] weights; //The weights which comprise the neural network

        #endregion

        #region Constructors and Initialization

        public NN() : this(1, 1, 0, new int[] { }) { } //Create a default network with one input and one output node (no hidden nodes)

        public NN(int numInputs, int numOutputs, int numHiddenLayers, int[] nodesPerHiddenLayer)
        {
            //Initialize a random neural network with the appropriate structure
            initializeRandomNewNN(numInputs, numOutputs, numHiddenLayers, nodesPerHiddenLayer);
        }

        public NN(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            numberOfInputs = (int)info.GetValue("numberOfInputs", typeof(int));
            numberOfOutputs = (int)info.GetValue("numberOfOutputs", typeof(int));
            numberOfHiddenLayers = (int)info.GetValue("numberOfHiddenLayers", typeof(int));
            numberOfNodesPerHiddenLayer = (int[])info.GetValue("numberOfNodesPerHiddenLayer", typeof(int[]));

            numberOfLayers = (int)info.GetValue("numberOfLayers", typeof(int));
            numberOfNodes = (int)info.GetValue("numberOfNodes", typeof(int));
            numberOfHiddenNodes = (int)info.GetValue("numberOfHiddenNodes", typeof(int));

            nodesPerLayer = (int[])info.GetValue("nodesPerLayer", typeof(int[]));
            inputsPerLayer = (int[])info.GetValue("inputsPerLayer", typeof(int[]));
            weightsPerLayer = (int[])info.GetValue("weightsPerLayer", typeof(int[]));
            weightStartIndexPerLayer = (int[])info.GetValue("weightStartIndexPerLayer", typeof(int[]));
            weightStartIndexPerLayerPerNode = (int[][])info.GetValue("weightStartIndexPerLayerPerNode", typeof(int[][]));

            weights = (double[])info.GetValue("weights", typeof(double[]));
        }

        public void initializeRandomNewNN(int numInputs, int numOutputs, int numHiddenLayers, int[] nodesPerHiddenLayer)
        {
            numberOfInputs = numInputs;
            numberOfOutputs = numOutputs;
            numberOfHiddenLayers = numHiddenLayers;
            numberOfNodesPerHiddenLayer = nodesPerHiddenLayer;

            numberOfLayers = 2 + numHiddenLayers; //The input layer, output layer and hidden layers
            nodesPerLayer = new int[numberOfLayers];
            nodesPerLayer[0] = numInputs; //Input layer nodes
            numberOfHiddenNodes = 0;
            numberOfNodes = nodesPerLayer[0];
            for (int i = 1; i < numHiddenLayers + 1; i++)
            {
                nodesPerLayer[i] = nodesPerHiddenLayer[i - 1]; //Hidden layer nodes
                numberOfHiddenNodes += nodesPerLayer[i];
                numberOfNodes += nodesPerLayer[i];
            }
            nodesPerLayer[nodesPerLayer.Length - 1] = numOutputs; //Output layer nodes
            numberOfNodes += nodesPerLayer[nodesPerLayer.Length - 1];

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

            weightStartIndexPerLayerPerNode = new int[numberOfLayers][];

            //Populate the array with the appropriate weight indicies based on the layer number, node number and weight
            for (int i = 0; i < numberOfLayers; i++)
            {
                weightStartIndexPerLayerPerNode[i] = new int[nodesPerLayer[i]];
                for (int j = 0; j < nodesPerLayer[i]; j++)
                    weightStartIndexPerLayerPerNode[i][j] = weightStartIndexPerLayer[i] + weightsPerLayer[i] * j;
            }

            weights = new double[numWeights];

            //Populate the weights with random values between -1 and 1
            for (int i = 0; i < weights.Length; i++)
                weights[i] = randNumGen.NextDouble() - randNumGen.NextDouble();
        }

        #endregion

        #region Methods

        #region NN Computation Methods

        public double[] ComputeNNOutputs(double[] input)
        {
            double[] output = input; //Set the output equal to the input

            for (int i = 0; i < numberOfLayers; i++) //For each layer
                output = ComputerLayerOutput(i, output); //Use the result from the previous layer to compute the result for the next layer

            return output;
        }

        private double[] ComputerLayerOutput(int layerNumber, double[] input)
        {
            double[] output = new double[nodesPerLayer[layerNumber]]; //Allocate an array of the correct node size

            for (int i = 0; i < output.Length; i++) //For each node
            {
                output[i] = 0; //Set it's default output value to 0
                for (int j = 0; j < input.Length; j++) //For each input
                    output[i] += input[j] * weights[weightStartIndexPerLayerPerNode[layerNumber][i] + j]; //Add the weight value at the appropriate weight index given the layer number, node and input number
                output[i] -= weights[weightStartIndexPerLayerPerNode[layerNumber][i] + input.Length]; //Add the bias weight*(-1) (i.e. subtract the bias weight) stored in the last index given the layer number and node
            }

            return output;
        }

        #endregion

        #region Serialization Methods

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //Add all the values to the serializer
            info.AddValue("numberOfInputs", numberOfInputs);
            info.AddValue("numberOfOutputs", numberOfOutputs);
            info.AddValue("numberOfHiddenLayers", numberOfHiddenLayers);
            info.AddValue("numberOfNodesPerHiddenLayer", numberOfNodesPerHiddenLayer);

            info.AddValue("numberOfLayers", numberOfLayers);
            info.AddValue("numberOfNodes", numberOfNodes);
            info.AddValue("numberOfHiddenNodes", numberOfHiddenNodes);

            info.AddValue("nodesPerLayer", nodesPerLayer);
            info.AddValue("inputsPerLayer", inputsPerLayer);
            info.AddValue("weightsPerLayer", weightsPerLayer);
            info.AddValue("weightStartIndexPerLayer", weightStartIndexPerLayer);
            info.AddValue("weightStartIndexPerLayerPerNode", weightStartIndexPerLayerPerNode);

            info.AddValue("weights", weights);
        }

        public void SaveNNToFile(string path)
        {
            //Create a stream and binary formatter
            Stream stream = File.Open(path, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            //Serialize the object
            bformatter.Serialize(stream, this);

            //Close the stream
            stream.Close();
        }

        public static NN LoadNNFromFile(string path)
        {
            //Create a stream and binary formatter
            Stream stream = File.Open(path, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            //Create an object via deserialization
            NN output = (NN)bformatter.Deserialize(stream);

            //Close the stream
            stream.Close();

            //Return the object
            return output;
        }

        #endregion

        #region Equality Testing Methods

        public override bool Equals(object obj)
        {
            //Cast the object to a NN and compare each input. Special comparisons are used for arrays to compare by value instead of by reference.
            NN o = (NN)obj;
            return o.numberOfInputs == this.numberOfInputs &&
                   o.numberOfOutputs == this.numberOfOutputs &&
                   o.numberOfHiddenLayers == this.numberOfHiddenLayers &&
                   areArraysEqual(o.numberOfNodesPerHiddenLayer, this.numberOfNodesPerHiddenLayer) &&
                   o.numberOfLayers == this.numberOfLayers &&
                   o.numberOfNodes == this.numberOfNodes &&
                   o.numberOfHiddenNodes == this.numberOfHiddenNodes &&
                   areArraysEqual(o.nodesPerLayer, this.nodesPerLayer) &&
                   areArraysEqual(o.inputsPerLayer, this.inputsPerLayer) &&
                   areArraysEqual(o.weightsPerLayer, this.weightsPerLayer) &&
                   areArraysEqual(o.weightStartIndexPerLayer, this.weightStartIndexPerLayer) &&
                   areArraysEqual(o.weightStartIndexPerLayerPerNode, this.weightStartIndexPerLayerPerNode) &&
                   areArraysEqual(o.weights, this.weights);
        }

        private bool areArraysEqual(int[] one, int[] two)
        {
            //Special by-value comparison for int arrays
            if (one == null && two == null)
                return true;
            if (one.Length != two.Length)
                return false;

            bool returnVal = true;

            for (int i = 0; i < one.Length; i++)
                returnVal = returnVal && (one[i] == two[i]);

            return returnVal;
        }

        private bool areArraysEqual(double[] one, double[] two)
        {
            //Special by-value comparison for double arrays
            if (one == null && two == null)
                return true;
            if (one.Length != two.Length)
                return false;

            bool returnVal = true;

            for (int i = 0; i < one.Length; i++)
                returnVal = returnVal && (one[i] == two[i]);

            return returnVal;
        }

        private bool areArraysEqual(int[][] one, int[][] two)
        {
            //Special by-value comparison for int arrays of int arrays
            if (one == null && two == null)
                return true;
            if (one.Length != two.Length)
                return false;

            bool returnVal = true;

            for (int i = 0; i < one.Length; i++)
                returnVal = returnVal && areArraysEqual(one[i], two[i]);

            return returnVal;
        }

        #endregion

        #endregion

        #region Properties

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

        public int NumberOfLayers
        {
            get { return numberOfLayers; }
            set { numberOfLayers = value; }
        }

        public int NumberOfNodes
        {
            get { return numberOfNodes; }
            set { numberOfNodes = value; }
        }

        public int NumberOfHiddenNodes
        {
            get { return numberOfHiddenNodes; }
            set { numberOfHiddenNodes = value; }
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

        public int[][] WeightStartIndexPerLayerPerNode
        {
            get { return weightStartIndexPerLayerPerNode; }
            set { weightStartIndexPerLayerPerNode = value; }
        }

        public double[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        #endregion
    }
}
