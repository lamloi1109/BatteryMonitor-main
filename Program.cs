using BatteryMonitor.SQLlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryMonitor
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string userId = Properties.Settings.Default.userId;
            sqlitedbofff sqlLite = new sqlitedbofff();
            ////SQL LITE
            sqlLite.CreateDatabaseFile();
            if (string.IsNullOrEmpty(userId))
            {
                Application.Run(new Login());
                return;
            }
            Application.Run(new MenuForm());
            //Application.Run(new test());


        }
    }
}
