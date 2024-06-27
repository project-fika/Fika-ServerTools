using FikaServerTools.Config;
using FikaServerTools.Networking;
using System.Threading;

namespace FikaServerTools
{
    internal class FikaServerTools
    {
        public static FikaServerToolsConfig Config { get; set; }
        private static FikaNatPunchServer _fikaNatPunchServer;

        static void Main(string[] args)
        {
            Config = ConfigManager.Load(args);

            Logger.LogInfo("FikaServerTools started!");

            if(Config.NatPunchServer.Enable)
            {
                FikaNatPunchServer fikaNatPunchServer = new FikaNatPunchServer(Config.NatPunchServer);
                fikaNatPunchServer.Start();
            }

            // Main thread loop
            while(true)
            {
                _fikaNatPunchServer?.PollEvents();
                Thread.Sleep(10);
            }
        }
    }
}