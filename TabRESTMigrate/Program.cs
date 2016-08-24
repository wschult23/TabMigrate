using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineContentDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //See if we have command line arguments to run
            var commandLine = new CommandLineParser();
            if (CommandLineParser.HasUseableCommandLine(commandLine))
            {
                StartupCommandLine(commandLine);
            }
            else
            {
                //UI form we are going to show
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var form = new FormSiteExportImport();

                Application.Run(form);
            }
        }

        static void StartupCommandLine(CommandLineParser commandLine)
        {
            TaskMaster task = null;
            //Parse the details from the command line
            try
            {
                task = TaskMaster.FromCommandLine(commandLine);
            }
            catch (Exception exParseCommandLine)
            {
                Console.WriteLine( "Error parsing command line: " + exParseCommandLine);
            }

            task.ExecuteTaskBegin();
        }
    }

}
