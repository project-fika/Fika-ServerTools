namespace FikaServerTools.Config
{
    public static class ConfigManager
    {
        public static FikaServerToolsConfig LoadFromArgs(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No arguments provided.");
                return null;
            }

            FikaServerToolsConfig config = new();

            string serviceName = args[0];

            switch(serviceName)
            {
                case "-NatPunchServer":
                    config.NatPunchServer.Enable = true;

                    for (int i = 1; i < args.Length; i++)
                    {
                        string arg = args[i];

                        switch (arg)
                        {
                            case "-IP":
                                config.NatPunchServer.IP = args[i + 1];
                                i++;
                                break;
                            case "-Port":
                                config.NatPunchServer.Port = int.Parse(args[i + 1]);
                                i++;
                                break;
                            case "-NatIntroduceAmount":
                                config.NatPunchServer.NatIntroduceAmount = int.Parse(args[i + 1]);
                                i++;
                                break;
                            default:
                                Console.WriteLine($"Unknown argument provided: {arg}");
                                break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine($"Unknown service name provided: {serviceName}");
                    break;

            }

            return config;
        }
    }
}
