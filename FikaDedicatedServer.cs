using FikaDedicatedServer.Networking;
using System.Threading;

namespace FikaDedicatedServer
{
    internal class FikaDedicatedServer
    {
        static void Main(string[] args)
        {
            FikaNatPunchServer fikaNatPunchServer = new FikaNatPunchServer();
            fikaNatPunchServer.Init();

            while(true)
            {
                fikaNatPunchServer.NetServer.PollEvents();
                fikaNatPunchServer.NetServer.NatPunchModule.PollEvents();
                Thread.Sleep(10);
            }
        }
    }
}
