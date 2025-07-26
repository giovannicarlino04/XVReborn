using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using XVCharaCreator.Properties;

namespace XVCharaCreator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("XVCharaCreator.settings.json", optional: true, reloadOnChange: true)
                .Build();
            
            // Initialize settings with configuration
            Settings.Initialize(configuration);
            
            Application.Run(new Form1());
        }
    }
}