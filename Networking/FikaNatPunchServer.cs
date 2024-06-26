using FikaDedicatedServer.Config;
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

    public class FikaNatPunchServer : INatPunchListener, INetEventListener
    {
        private readonly Dictionary<string, ServerPeer> _servers = new Dictionary<string, ServerPeer>();
        private FikaNatPunchServerConfig _config;
        private NetManager _netServer;
        public NetManager NetServer
        {
            get 
            { 
                return _netServer; 
            }
        }

        public FikaNatPunchServer(FikaNatPunchServerConfig config)
        {
            _config = config;

            _netServer = new NetManager(this)
            {
                IPv6Enabled = false,
                NatPunchEnabled = true
            };

            try
            {
                _netServer.Start(_config.IP, "", _config.Port);
                _netServer.NatPunchModule.Init(this);

                Logger.LogSuccess($"NatPunchServer started on {_config.IP}:{NetServer.LocalPort}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error when starting NatPunchServer: {ex.Message}");
            }
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
                Logger.LogError($"Error when parsing NatIntroductionRequest: {ex.Message}");
                return;
            }

            switch (introductionType)
            {
                case "server":
                    if (!_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Logger.LogInfo($"Added {sessionId} ({remoteEndPoint}) to server list.");
                    }

                    _servers[sessionId] = new ServerPeer(localEndPoint, remoteEndPoint);
                    break;

                case "client":
                    if (_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Logger.LogInfo($"Introducing server {sessionId} ({sPeer.ExternalAddr}) to client ({remoteEndPoint})");

                        for(int i = 0; i < _config.NatIntroduceAmount; i++)
                        {
                            _netServer.NatPunchModule.NatIntroduce(
                                sPeer.InternalAddr,
                                sPeer.ExternalAddr,
                                localEndPoint,
                                remoteEndPoint,
                                token
                                );
                        }
                    }
                    else
                    {
                        Logger.LogError($"Unknown ServerId provided by client.");
                    }
                    break;

                default:
                    Logger.LogError($"Unknown request received: {token}");
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
