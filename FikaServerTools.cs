using FikaServerTools.Config;
using FikaServerTools.Networking;

namespace FikaServerTools
{
    internal class FikaServerTools
    {
        public static FikaServerToolsConfig Config;
        private static FikaNatPunchServer _fikaNatPunchServer;

        static void Main(string[] args)
        {
            Config = ConfigManager.LoadFromArgs(args);

            if (Config == null)
            {
                Console.WriteLine("Unable to load configuration from arguments.");
                return;
            }

            if (Config.NatPunchServer.Enable)
            {
                _fikaNatPunchServer = new FikaNatPunchServer(Config.NatPunchServer);
                _fikaNatPunchServer.Start();

                while (true)
                {
                    _fikaNatPunchServer.PollEvents();
                    Thread.Sleep(10);
                }
            }
        }
    }
}
