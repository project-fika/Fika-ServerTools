using Newtonsoft.Json;
using System;
using System.IO;

namespace FikaDedicatedServer.Config
{
    public static class ConfigManager
    {
        private static string _configFilePath = $"{Directory.GetCurrentDirectory()}\\config.json";

        public static FikaDedicatedServerConfig Load()
        {
            FikaDedicatedServerConfig config = new FikaDedicatedServerConfig();

            if (!File.Exists(_configFilePath))
            {
                Save(config);
            }

            try
            {
                config = JsonConvert.DeserializeObject<FikaDedicatedServerConfig>(File.ReadAllText(_configFilePath));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }

            return config;
        }

        public static void Save(FikaDedicatedServerConfig config)
        {
            try
            {
                File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
    }
}
