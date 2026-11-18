using System;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new numEdad());
        }
    }
}