using StickyNotes.Net.Packets;
using StickyNotes.Net.Packets.ClientBound;
using StickyNotes.Protocol;
using StickyNotes.Protocol.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StickyNotes.Net
{
    public class Client
    {
        public bool IsLogin = false;
        public Socket ClientSocket { get; private set; }
        private bool IsAlive = false;
        public PacketHandler Handler { get; } = new PacketHandler();

        public string username;
        private string code;
        public Client()
        {
            GC.Collect();
        }

        public void ChangeCode(string code)
        {
            this.code = code;
        }

        public bool TryConnect()
        {
            try
            {
                var address = DecodeConnectCode(code);

                var ip = IPAddress.Parse(address.Key);
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int connPort = address.Value;
                ClientSocket.Connect(new IPEndPoint(ip, connPort));
                Debug.WriteLine("Connected To Server!");

                IsAlive = true;
                var receiveThread = new Thread(ReceiveMsg);
                receiveThread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        private void ReceiveMsg()
        {
            while (IsAlive && ClientSocket.Connected)
            {
                try
                {
                    Console.WriteLine("BeginToRead");
                    byte[] buffer = new byte[10240];
                    var length = ClientSocket.Receive(buffer);
                    byte[] data = new byte[length];
                    Array.Copy(buffer, data, length);
                    GC.SuppressFinalize(buffer);
                    Console.WriteLine("Read Done");

                    using (PacketPackage package = new PacketPackage(data))
                    {
                        package.ReadPacket();
                        PacketStream DataStream = package.DecodeStream();
                        int PacketID = package.GetPacketID();

                        Console.WriteLine(PacketID);

                        Handler.ActionPacket(PacketID, DataStream);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    
                    IsAlive = false;
                    GC.Collect();
                }
            }
        }

        public void Send(byte[] send)
        {
            try
            {
                _ = ClientSocket.Send(send);
            } catch(Exception) { }
        }

        public void SendPacket(IClientBoundPacket packet)
        {
            using (PacketStream stream = new PacketStream())
            {
                packet.WritePacket(stream);

                using (PacketPackage package = new PacketPackage(stream))
                {
                    App.Net.Send(package.EncodePacket(packet.GetPacketID()));
                }
            }
        }

        public void Login(string name, string pass)
        {
            using (PacketStream stream = new PacketStream())
            {
                var packaet = new ClientBoundLoginPacket()
                {
                    Password = Convert.ToBase64String(MD5Helper.GetMD5Hash(Encoding.UTF8.GetBytes(pass))),
                    Username = name
                };
                username = name;
                if (!packaet.WritePacket(stream))
                    Console.WriteLine("Error Sending Packet");

                using (PacketPackage package = new PacketPackage(stream))
                {
                    Send(package.EncodePacket(packaet.GetPacketID()));
                    GC.Collect();
                }
            }
        }

        public void SendStatusGet()
        {
            using (PacketStream stream = new PacketStream())
            {
                var packet = new ClientBoundStatusGetPacket(this);
                if (!packet.WritePacket(stream))
                    Console.WriteLine("Error Sending Packet");

                using (PacketPackage package = new PacketPackage(stream))
                {
                    Send(package.EncodePacket(packet.GetPacketID()));
                    GC.Collect();
                }
            }
        }

        private KeyValuePair<string, int> DecodeConnectCode(string ConnectCode)
        {
            var origin = Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String(ConnectCode)));
            var bytes = Convert.FromBase64String(origin.Replace("[_+_=]", "A"));

            using(PacketStream stream = new PacketStream(bytes))
            {
                var port = stream.ReadInt32();
                var ip = stream.ReadString();
                return new KeyValuePair<string, int>(ip, port);
            }
        }
    }
}
