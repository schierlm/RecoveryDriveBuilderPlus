using System;
using System.Windows.Forms;

namespace RecoveryDriveBuilderPlus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major < 10)
            {
                MessageBox.Show("This program requires Windows 10 or higher");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWizard());
        }
    }
}
