using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Text.Json;

namespace XVReborn.Properties
{
    public class Settings
    {
        private static Settings? _defaultInstance;
        private static readonly object _lock = new object();
        private static IConfiguration? _configuration;
        private static string _settingsFilePath = "XVReborn.settings.json";

        // Custom settings storage for modlist and addonmodlist
        private static StringCollection? _cachedModlist;
        private static StringCollection? _cachedAddonModlist;

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
            // Load cached modlists on initialization
            LoadModlistsFromFile();
        }

        public string datafolder
        {
            get => _configuration?.GetValue<string>("Settings:datafolder") ?? "";
            set
            {
                SaveSettingsToFile("Settings:datafolder", value);
            }
        }

        public StringCollection modlist
        {
            get
            {
                if (_cachedModlist == null)
                {
                    LoadModlistsFromFile();
                }
                return _cachedModlist ?? new StringCollection();
            }
            set
            {
                _cachedModlist = value;
                SaveModlistsToFile();
            }
        }

        public StringCollection addonmodlist
        {
            get
            {
                if (_cachedAddonModlist == null)
                {
                    LoadModlistsFromFile();
                }
                return _cachedAddonModlist ?? new StringCollection();
            }
            set
            {
                _cachedAddonModlist = value;
                SaveModlistsToFile();
            }
        }

        public string language
        {
            get => _configuration?.GetValue<string>("Settings:language") ?? "";
            set
            {
                SaveSettingsToFile("Settings:language", value);
            }
        }

        public void Save()
        {
            // Save modlists when Save() is called
            SaveModlistsToFile();
        }

        public void Reset()
        {
            // Reset settings to default values
            try
            {
                var settings = new
                {
                    Settings = new
                    {
                        datafolder = "",
                        modlist = new string[0],
                        addonmodlist = new string[0],
                        language = ""
                    }
                };

                var jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, jsonString);
                
                // Reset cached modlists
                _cachedModlist = new StringCollection();
                _cachedAddonModlist = new StringCollection();
                
                // Reload configuration after reset
                ReloadConfiguration();
            }
            catch
            {
                // Ignore errors when resetting settings
            }
        }

        private static void LoadModlistsFromFile()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var jsonContent = File.ReadAllText(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<JsonElement>(jsonContent);
                    
                    _cachedModlist = new StringCollection();
                    _cachedAddonModlist = new StringCollection();
                    
                    if (settings.TryGetProperty("Settings", out var settingsElement))
                    {
                        // Load modlist
                        if (settingsElement.TryGetProperty("modlist", out var modlistElement) && 
                            modlistElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in modlistElement.EnumerateArray())
                            {
                                if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                {
                                    _cachedModlist.Add(item.GetString()!);
                                }
                            }
                        }
                        
                        // Load addonmodlist
                        if (settingsElement.TryGetProperty("addonmodlist", out var addonmodlistElement) && 
                            addonmodlistElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in addonmodlistElement.EnumerateArray())
                            {
                                if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                {
                                    _cachedAddonModlist.Add(item.GetString()!);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _cachedModlist = new StringCollection();
                    _cachedAddonModlist = new StringCollection();
                }
            }
            catch
            {
                _cachedModlist = new StringCollection();
                _cachedAddonModlist = new StringCollection();
            }
        }

        private void SaveModlistsToFile()
        {
            try
            {
                var settings = new
                {
                    Settings = new
                    {
                        datafolder = _configuration?.GetValue<string>("Settings:datafolder") ?? "",
                        modlist = _cachedModlist?.Cast<string>().ToArray() ?? new string[0],
                        addonmodlist = _cachedAddonModlist?.Cast<string>().ToArray() ?? new string[0],
                        language = _configuration?.GetValue<string>("Settings:language") ?? ""
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

        private void ReloadConfiguration()
        {
            try
            {
                var newConfiguration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(_settingsFilePath, optional: true, reloadOnChange: true)
                    .Build();
                
                _configuration = newConfiguration;
            }
            catch
            {
                // Ignore errors when reloading configuration
            }
        }

        private void SaveSettingsToFile(string key, string value)
        {
            try
            {
                var settings = new
                {
                    Settings = new
                    {
                        datafolder = key == "Settings:datafolder" ? value : (_configuration?.GetValue<string>("Settings:datafolder") ?? ""),
                        modlist = _cachedModlist?.Cast<string>().ToArray() ?? new string[0],
                        addonmodlist = _cachedAddonModlist?.Cast<string>().ToArray() ?? new string[0],
                        language = key == "Settings:language" ? value : (_configuration?.GetValue<string>("Settings:language") ?? "")
                    }
                };

                var jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, jsonString);
                
                // Reload configuration after saving
                ReloadConfiguration();
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }

        private string[] GetModlistArray()
        {
            return _cachedModlist?.Cast<string>().ToArray() ?? new string[0];
        }

        private string[] GetAddonModlistArray()
        {
            return _cachedAddonModlist?.Cast<string>().ToArray() ?? new string[0];
        }
    }
} 
 