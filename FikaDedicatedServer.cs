using FikaDedicatedServer.Config;
using FikaDedicatedServer.Networking;
using System.Threading;

namespace FikaDedicatedServer
{
    internal class FikaDedicatedServer
    {
        public static FikaDedicatedServerConfig Config;

        static void Main(string[] args)
        {
            Config = ConfigManager.Load();
            
            FikaNatPunchServer fikaNatPunchServer = new FikaNatPunchServer();
            fikaNatPunchServer.Init(Config.FikaNatPunchServer);

            while(true)
            {
                fikaNatPunchServer.NetServer.PollEvents();
                fikaNatPunchServer.NetServer.NatPunchModule.PollEvents();
                Thread.Sleep(10);
            }
        }
    }
}
