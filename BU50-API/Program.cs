using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BU50_API
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Check if another instance of the application is already running
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);

            if (processes.Length > 1)
            {
                //MessageBox.Show("Another instance of the application is already running.");
                Application.ExitThread();
                Application.Exit();
                return; // Exit the application
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new FRM_Updatewebsite());
            }
            catch (Exception ex)
            {
                // Optionally log the error here

                // Close the current app
                Application.ExitThread();
                Application.Exit();

                // Start a new instance
                Process.Start(Application.ExecutablePath);
            }
        }
    }
}