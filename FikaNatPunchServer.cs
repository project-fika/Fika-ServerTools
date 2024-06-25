using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FikaDedicatedServer.Networking
{
    class ServerPeer
    {
        public IPEndPoint InternalAddr { get; }
        public IPEndPoint ExternalAddr { get; }

        public ServerPeer(IPEndPoint internalAddr, IPEndPoint externalAddr)
        {
            InternalAddr = internalAddr;
            ExternalAddr = externalAddr;
        }
    }

    internal class FikaNatPunchServer : INatPunchListener, INetEventListener
    {
        private const int ServerPort = 6970;

        private readonly Dictionary<string, ServerPeer> _servers = new Dictionary<string, ServerPeer>();
        private NetManager _netServer;
        public NetManager NetServer
        {
            get { return _netServer; }
        }

        public void Init()
        {
            _netServer = new NetManager(this)
            {
                IPv6Enabled = false,
                NatPunchEnabled = true
            };

            _netServer.Start(ServerPort);
            _netServer.NatPunchModule.Init(this);

            Console.WriteLine($"{ColorEscapeSequence.GREEN}NatPunchServer started on port {NetServer.LocalPort}");
        }

        public void OnNatIntroductionRequest(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, string token)
        {
            string introductionType;
            string sessionId;
            ServerPeer sPeer;

            try
            {
                introductionType = token.Split(':')[0];
                sessionId = token.Split(':')[1];
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error when parsing NatIntroductionRequest: {ex.Message}");
                return;
            }

            switch (introductionType)
            {
                case "server":
                    if (!_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Console.WriteLine($"Added {sessionId} ({remoteEndPoint}) to server list.");
                    }
                    _servers[sessionId] = new ServerPeer(localEndPoint, remoteEndPoint);
                    break;

                case "client":
                    if (_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Console.WriteLine($"Introducing server {sessionId} ({sPeer.ExternalAddr}) to client ({remoteEndPoint})");

                        _netServer.NatPunchModule.NatIntroduce(
                            sPeer.InternalAddr,
                            sPeer.ExternalAddr,
                            localEndPoint,
                            remoteEndPoint,
                            token
                            );
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown request received: {introductionType}:{sessionId}");
                    break;

            }
        }

        public void OnNatIntroductionSuccess(IPEndPoint targetEndPoint, NatAddressType type, string token)
        {
            // Do nothing
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            // Do nothing
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            // Do nothing
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            // Do nothing
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            // Do nothing
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            // Do nothing
        }

        public void OnPeerConnected(NetPeer peer)
        {
            // Do nothing
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            // Do nothing
        }
    }
}
