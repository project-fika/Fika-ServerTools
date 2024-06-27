using System;

namespace FikaServerTools.Config
{
    public static class ConfigManager
    {

        public static FikaServerToolsConfig Load(string[] args)
        {
            FikaServerToolsConfig config = new FikaServerToolsConfig();

            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string argName = args[i];
                    string argValue = args[i + 1];

                    switch (argName)
                    {
                        case "-NatPunchServer":
                            config.NatPunchServer.Enable = true;
                            break;
                        case "-IP":
                            config.NatPunchServer.IP = argValue;
                            i++;
                            break;
                        case "-Port":
                            config.NatPunchServer.Port = int.Parse(argValue);
                            i++;
                            break;
                        case "-NatIntroduceAmount":
                            config.NatPunchServer.NatIntroduceAmount = int.Parse(argValue);
                            i++;
                            break;
                        default:
                            Logger.LogError($"Invalid argument provided: {argName}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }

            return config;
        }
    }
}
