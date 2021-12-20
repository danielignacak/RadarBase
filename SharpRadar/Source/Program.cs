using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SharpRadar
{
    internal static class Program
    {
        private static Mutex _mutex;
        private static Memory _memory;
        private delegate bool EventHandler(int sig);
        private static EventHandler _handler;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                _mutex = new Mutex(true, "CD953537-CB17-4735-82DC-A449D372C938", out bool singleton);
                if (singleton)
                {
                    _memory = new Memory(); // vmm.Init
                    _handler += new EventHandler(ShutdownHandler);
                    SetConsoleCtrlHandler(_handler, true); // Handle Ctrl-C exit
                    AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit; // Handle application exit
                    Console.WriteLine("Starting up GUI...");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm(_memory));
                }
                else
                {
                    throw new Exception("The Application Is Already Running!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SharpRadar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Shutdown()
        {
            _memory.Dispose(); // vmm.Close()
            Console.WriteLine("Shutting down...");
        }

        private static bool ShutdownHandler(int sig) // Handle ctrl-c
        {
            Shutdown();
            return false;
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Shutdown();
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
    }
}
