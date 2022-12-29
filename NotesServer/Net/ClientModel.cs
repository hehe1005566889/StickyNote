using FlyBirdCommon.Logger;
using NotesServer.Protocol.Net;
using NotesServer.Protocol.Net.Handler;
using NotesServer.Protocol.Net.Packets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StickyNotes.Protocol.Net
{
    class ClientModel : IDisposable
    {
        private readonly Thread ClientThread;
        private readonly Socket ClientSocket;
        private static readonly Logger logger = SharedLoggers.GetNetLogger();

        private bool IsAlive = false;
        public bool IsLogin = false;
        private readonly ServerAdapter Adapter;
        public readonly PacketHandler Handler = PacketHandler.GetHandler();

        #region DEBUG
        public static bool isDebug = true;
        public static void Debug(object msg)
        {
            if (!isDebug)
                return;
            logger.Info("[DEBUG] {0}", msg.ToString());
        }
        
        public static void Debug(object msg, params string[] replaces)
        {
            if (!isDebug)
                return;
            logger.Info("[DEBUG] {0}", string.Format(msg.ToString(), replaces));
        }
        #endregion

        public ClientModel(Socket socket, ServerAdapter adapter)
        {
            logger.Success("Client Connect (Address : {0})", ((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
            
            Adapter = adapter;
            ClientSocket = socket;

            ClientThread = new Thread(OnRecieve)
            {
                IsBackground = true,
                Name = "ClientThread"
            };
            StartListener();
            GC.Collect();
        }

        public void StartListener()
        {
            IsAlive = true;
            ClientThread.Start();
            GC.Collect();
        }

        private void OnRecieve()
        {
            while (IsAlive && ClientSocket.Connected)
            {
                try
                {
                    byte[] buffer = new byte[10240];
                    var length = ClientSocket.Receive(buffer);
                    byte[] data = new byte[length];
                    Array.Copy(buffer, data, length);
                    GC.SuppressFinalize(buffer);

                    using (PacketPackage package = new PacketPackage(data))
                    {
                        package.ReadPacket();
                        PacketStream read = package.DecodeStream();
                        int packetID = package.GetPacketID();
                        Debug("[DEBUG] Recieve Packet ID {0}", packetID.ToString());

                        if (packetID < 0)
                            continue;

                        if (packetID != 0 && !IsLogin)
                            Disconnect();

                        Send(Handler.HandlePacket(packetID, read, this));

                        read.Dispose();
                        package.Dispose();
                        GC.Collect();
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.ToString());
                    Disconnect();
                }
            }
        }

        public void Send(byte[] send)
        {
            try
            {
                var size = ClientSocket.Send(send);
                Debug("[DEBUG] Send Size {0}", size.ToString());
                if (size is 0)
                    logger.Error("Send Error");
            } 
            catch(Exception e)
            {
                logger.Warn(e.ToString());
            }
        }

        public void SendPacket(IPacket packet)
        {
            using (PacketStream stream = new PacketStream()) {
                if (!packet.WriteReturn(stream))
                    logger.Info("Write Error");

                using (PacketPackage package = new PacketPackage(stream))
                {
                    Send(package.EncodePacket(packet.GetReturnPacketID()));
                }
            }
        }

        public void BoardCastMessage(string message)
        {
            SendPacket(new ResultPacket()
            {
                Addon = message,
                Result = true,
                Status = ResultStatus.MESSAGE
            });
        }

        public void Disconnect()
        {
            logger.Warn("Client Disconnect (Address : {0})", ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString());
            if (!(ClientSocket is null)) 
                if (ClientSocket.Connected)
                    ClientSocket.Disconnect(true);
            ClientSocket.Close();
            GC.SuppressFinalize(ClientSocket);
            Adapter.UnRegisterClient(this);
        }

        public void Dispose()
        {
            Disconnect();
            GC.Collect();
        }
    }
}
