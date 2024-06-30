using FikaServerTools.Config;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FikaServerTools.Networking
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
        }

        public void Start()
        {
            try
            {
                _netServer.Start(_config.IP, "", _config.Port);
                _netServer.NatPunchModule.Init(this);

                Console.WriteLine($"NatPunchServer started on {_config.IP}:{NetServer.LocalPort}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NatPunchServer failed to start: {ex.Message}");
            }
        }

        public void Stop()
        {
            _netServer?.Stop();
        }

        public void PollEvents()
        {
            NetServer.PollEvents();
            NetServer.NatPunchModule.PollEvents();
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
                    if (_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Console.WriteLine($"KeepAlive {sessionId} ({remoteEndPoint})");
                    }
                    else
                    {
                        Console.WriteLine($"Added {sessionId} ({remoteEndPoint}) to server list");
                    }

                    _servers[sessionId] = new ServerPeer(localEndPoint, remoteEndPoint);
                    break;

                case "client":
                    if (_servers.TryGetValue(sessionId, out sPeer))
                    {
                        Console.WriteLine($"Introducing server {sessionId} ({sPeer.ExternalAddr}) to client ({remoteEndPoint})");

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
                        Console.WriteLine($"Unknown ServerId provided by client.");
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown request received: {token}");
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
