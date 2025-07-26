using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using XVReborn.Properties;

namespace XVReborn
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("XVReborn.settings.json", optional: true, reloadOnChange: true)
                .Build();
            
            // Initialize settings with configuration
            Settings.Initialize(configuration);
            
            Application.Run(new Form1());
        }
    }
}
