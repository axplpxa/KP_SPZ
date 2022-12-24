using SLB_Zakorko_KP;
using System;
using System.Windows.Forms;

namespace SLB_Zakorko_KP
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
            Application.Run(new LSB_());
        }
    }
}
