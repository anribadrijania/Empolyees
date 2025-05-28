using System;
using System.Windows.Forms;
using YourProjectNamespace;

namespace Empolyees
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DatabaseHelper.InitializeDatabase();
            DatabaseHelper.SeedTestData();
            Application.Run(new Form1());
        }

    }
}
