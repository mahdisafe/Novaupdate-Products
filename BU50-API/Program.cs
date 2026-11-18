using System;
using System.Diagnostics;
using System.Linq;
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

            var autoMode = Environment.GetCommandLineArgs()
                .Any(a => string.Equals(a, "--auto", StringComparison.OrdinalIgnoreCase)
                       || string.Equals(a, "--silent", StringComparison.OrdinalIgnoreCase));

            try
            {
                Application.Run(new FRM_Updatewebsite());
            }
            catch (Exception)
            {
                Environment.ExitCode = 1;
                Application.ExitThread();
                Application.Exit();

                // Interactive runs may self-restart; scheduled/auto runs must not loop.
                if (!autoMode)
                    Process.Start(Application.ExecutablePath);
            }
        }
    }
}