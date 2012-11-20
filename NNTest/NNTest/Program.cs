using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NNTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //The network testing application is the main form
            Application.Run(new NetworkTester());
        }
    }
}
