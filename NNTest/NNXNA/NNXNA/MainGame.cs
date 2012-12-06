using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Text;

namespace NNXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {

        #region Constant Values

        //The number of entities to be used in the population simulation
        private const int simulationPopulationSize = 30;

        //The structure of their brains
        private int[] neuralNetworkStructure = { 4, 6, 2 };

        #endregion

        #region Member Variables

        //An internal neural network which is created purely for testing without an application
        private NN testNN;

        //The neural network population for test
        private NNPopulation pop;

        //This thread will parallel code which shouldn't run in the GUI thread
        private Thread simThread;
        //This bool will be used to request the simThread be stopped
        private bool requestStop;

        #endregion

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;

        KeyboardState prevKeyboardState;
        MouseState prevMouseState;
        GamePadState prevGamePadState;

        bool doNotDraw;

        TimeSpan incrementTimeSpan;
        TimeSpan decrementTimeSpan;
        TimeSpan savedTimeSpan;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            TargetElapsedTime = new TimeSpan(166666);
            doNotDraw = false;
            incrementTimeSpan = new TimeSpan(1000);
            decrementTimeSpan = new TimeSpan(-1000);
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Initialize a test network
            testNN = new NN(4, 2, 1, new int[] { 6 });

            //Build a test population
            pop = new NNPopulation(simulationPopulationSize, neuralNetworkStructure, typeof(NNAntSimulation));

            //Output the default object test results
            ///richTextBox_simpleOut.Text = getTestDataString();

            //Fill the simThread with a default value (not running)
            simThread = new Thread(new ThreadStart(runGeneticSimulation));
            //No stop is request at the beginning of execution
            requestStop = false;

            ///label_totalIterations.Text = "0 Total Iterations, Population Size: " + pop.StartPopulationSize;

            ///comboBox_BreedingType.SelectedIndex = 0;

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
            prevGamePadState = GamePad.GetState(PlayerIndex.One);

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphics.GraphicsDevice.Viewport.Width,     // left, right
                graphics.GraphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);                                         // near, far plane

            Util.genericTextFont = Content.Load<SpriteFont>("GenericFont");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (isExitConditionMet(keyboardState, mouseState, gamePadState))
                    this.Exit();

            if (prevKeyboardState.IsKeyDown(Keys.OemTilde) && keyboardState.IsKeyUp(Keys.OemTilde))
            {
                doNotDraw = !doNotDraw;

                if (doNotDraw)
                {
                    savedTimeSpan = TargetElapsedTime;
                    TargetElapsedTime = new TimeSpan(750);
                }
                else
                {
                    TargetElapsedTime = savedTimeSpan;
                }
            }

            if (prevKeyboardState.IsKeyDown(Keys.Up))
            {
                TimeSpan span = TargetElapsedTime.Add(decrementTimeSpan);
                if (span < TimeSpan.Zero)
                    TargetElapsedTime = new TimeSpan(750);
                else
                    TargetElapsedTime = span;
            }

            if (prevKeyboardState.IsKeyDown(Keys.Down))
                TargetElapsedTime = TargetElapsedTime.Add(incrementTimeSpan);

            pop.Update(gameTime);

            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            prevGamePadState = gamePadState;

            base.Update(gameTime);
        }

        #region Keyboard, Mouse and GamePad Helper Functions

        private bool isExitConditionMet(KeyboardState keyboardState, MouseState mouseState, GamePadState gamePadState)
        {
            if ((prevKeyboardState.IsKeyDown(Keys.Escape) && keyboardState.IsKeyUp(Keys.Escape)) ||
                (gamePadState.Buttons.Back == ButtonState.Pressed))
                return true;
            return false;
        }

        #endregion

        #region Internal Test Functions

        //This function tests the serial functionality of the Neural Network object.
        //This can be useful if a particularly great neural network is consturcted and should be saved/loaded.
        private bool testSerialization(NN n)
        {
            //Save the neural network created when this object was built
            n.SaveNNToFile("Test_NN.dat");
            //Load a new neural network from that file
            NN loadedNN = NN.LoadNNFromFile("Test_NN.dat");

            //Using the custom equals function, test to see if the networks are equal then show the results.
            if (loadedNN.Equals(n))
                return true;
            return false;
        }

        //This is the default test for the neural network to confirm it is performing as expected
        public string getTestDataString(NN n)
        {
            StringBuilder outStringBuilder = new StringBuilder();
            outStringBuilder.AppendLine("Test Neural Network");
            outStringBuilder.AppendLine("-------------------");

            //Print the input data
            outStringBuilder.AppendLine("Input Data");
            outStringBuilder.AppendFormat("Number of Inputs: {0}\n", n.NumberOfInputs);
            outStringBuilder.AppendFormat("Number of Outputs: {0}\n", n.NumberOfOutputs);
            outStringBuilder.AppendFormat("Number of Hidden Layers: {0}\n", n.NumberOfHiddenLayers);
            outStringBuilder.AppendLine("Number of Nodes Per Hidden Layer: ");
            Array.ForEach(n.NumberOfNodesPerHiddenLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();

            //Print the calculated data
            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("General Calculated Metadata");
            outStringBuilder.AppendFormat("Total Number of Layers: {0}\n", n.NumberOfLayers);
            outStringBuilder.AppendFormat("Total Number of Nodes: {0}\n", n.NumberOfNodes);
            outStringBuilder.AppendFormat("Total Number of Hidden Nodes: {0}\n", n.NumberOfHiddenNodes);
            outStringBuilder.AppendLine();

            //Print the layer data
            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("Specific Calculated Metadata Per Layer");
            outStringBuilder.AppendLine("Number of Weights Per Layer: ");
            Array.ForEach(n.WeightsPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Number of Inputs Per Layer: ");
            Array.ForEach(n.InputsPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Number of Nodes Per Layer: ");
            Array.ForEach(n.NodesPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Weight Start Index Per Layer: ");
            Array.ForEach(n.WeightStartIndexPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Weight Start Index Per Node Per Layer: ");
            for (int i = 0; i < n.WeightStartIndexPerLayerPerNode.Length; i++)
            {
                Array.ForEach(n.WeightStartIndexPerLayerPerNode[i], x => outStringBuilder.AppendFormat("{0} ", x));
                outStringBuilder.AppendLine();
            }
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine();

            //Print the detailed data
            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("Detailed Data");
            outStringBuilder.AppendFormat("Number of Weights: {0}\n", n.Weights.Length);
            outStringBuilder.AppendLine("Weights: ");
            Array.ForEach(n.Weights, x => outStringBuilder.AppendFormat("{0}\n", x));
            outStringBuilder.AppendLine();

            //Return the results string
            return outStringBuilder.ToString();
        }

        private void runGeneticSimulation()
        {
            //Disable the form and change the button text
            ///SetFormEnabled(false);
            ///SetRunGenerationsButtonText("Click Escape to Abort");

            //Store the start time for benchmarking purposes
            DateTime before = DateTime.Now;

            //Create a string builder for output
            StringBuilder outputBuilder = new StringBuilder();

            //Iterate the simulation
            for (int j = 0; j < 100; j++) //MAGIC#
            {
                //Run a single generation in the neural network ant simulation
                ///pop.RunGeneration(typeof(NNAntSimulation), checkBox_showSimulation.Checked, (NNPopulation.BreedingFunction)GetSelectedBreedingIndex());

                //Get the latest fitness array from the simulation
                double[] latestFitness = pop.LatestFitness;

                //Find hte maximum fitness
                double maxFitness = -1;
                double sum = 0;
                for (int i = 0; i < latestFitness.Length; i++)
                {
                    if (latestFitness[i] > maxFitness) maxFitness = latestFitness[i];
                    sum += latestFitness[i];
                }

                //Update the chart
                ///AddChartElements(sum / latestFitness.Length, maxFitness);

                //Update the iteration counters
                ///SetIterationLabelText("Iteration: " + (j + 1) + "/" + numericUpDown_numGenerations.Value);
                ///SetTotalIterationLabelText(chart.Series[0].Points.Count + " Total Iterations, Population Size: " + pop.Population.Count);

                //Repaint the form
                ///UpdateForm();

                //Clear the output field
                ///ClearSimpleOutRichTextBoxBox();

                //Fill the output string builder with the fitness data from this generation
                for (int i = 0; i < pop.LatestFitness.Length; i++)
                    outputBuilder.AppendFormat("{0} ", pop.LatestFitness[i]);

                //Output the fitness data
                ///SetSimpleOutRichTextBoxBoxText(outputBuilder.ToString());

                //Reset the output builder
                outputBuilder.Clear();

                //If this function is running in the simulation thread and it was aborted
                if (requestStop)
                {
                    //Reset the stop request
                    requestStop = false;

                    //Break out of the loop
                    break;
                }
            }

            //Reenable the form and set the button text
            ///SetFormEnabled(true);
            ///SetRunGenerationsButtonText("Run Generations");

            //Store the end time of the simulation
            DateTime after = DateTime.Now;

            //Show a message box with the total seconds it took to run the complete program
            ///SetIterationLabelText(label_iteration.Text + ", Seconds: " + after.Subtract(before).TotalSeconds);

            //Finally update and refresh the form
            ///UpdateForm();
            ///RefreshForm();
        }

        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (!doNotDraw)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                pop.Draw(gameTime, graphics, spriteBatch);
            }

            spriteBatch.DrawString(Util.genericTextFont, "Generation #: " + pop.NumberOfGenerations, new Vector2(20, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
