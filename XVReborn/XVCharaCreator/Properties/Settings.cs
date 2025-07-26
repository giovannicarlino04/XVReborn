using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

namespace XVCharaCreator.Properties
{
    public class Settings
    {
        private static Settings? _defaultInstance;
        private static readonly object _lock = new object();
        private static IConfiguration? _configuration;
        private static string _settingsFilePath = "XVCharaCreator.settings.json";

        public static Settings Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (_lock)
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new Settings();
                        }
                    }
                }
                return _defaultInstance;
            }
        }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string data_path
        {
            get => _configuration?.GetValue<string>("Settings:data_path") ?? "";
            set
            {
                // Save to appsettings.json
                SaveSettingsToFile("Settings:data_path", value);
            }
        }

        public string language
        {
            get => _configuration?.GetValue<string>("Settings:language") ?? "uninitialized";
            set
            {
                // Save to appsettings.json
                SaveSettingsToFile("Settings:language", value);
            }
        }

        public void Save()
        {
            // This method is called by the existing code
            // The settings are already saved when the properties are set
        }

        private void SaveSettingsToFile(string key, string value)
        {
            try
            {
                var settings = new
                {
                    Settings = new
                    {
                        data_path = key == "Settings:data_path" ? value : (_configuration?.GetValue<string>("Settings:data_path") ?? ""),
                        language = key == "Settings:language" ? value : (_configuration?.GetValue<string>("Settings:language") ?? "uninitialized")
                    }
                };

                var jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, jsonString);
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }
    }
} 