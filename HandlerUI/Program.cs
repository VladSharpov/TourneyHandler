using HandlerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandlerUI
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

            // Initialize the database connections
            GlobalConfig.InitializeConnections(DatabaseType.TextFile);

            //Application.Run(new TournamentDashboardForm());
            Application.Run(new CreateTournamentForm());
        }
    }
}
