using FlyBirdCommon.Logger;
using StickyNoteCommon.Net;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NotesServer.NetWork
{
    class ClientModel
    {
        private readonly Socket clientSocket;
        private readonly ServerAdapter adapeter;
        private readonly NetworkStream nStream;
        private readonly Logger logger = SharedLoggers.GetNetLogger();
        private readonly bool IsDebugMode = true;
        private bool IsCallToDead = false;

        public ClientModel(Socket clientSocket, ServerAdapter adapter)
        {
            this.clientSocket = clientSocket;
            adapeter = adapter;
            nStream = new NetworkStream(clientSocket);

            GC.Collect();
        }

        public PacketReadBack ReadPacketFromStream()
        {
            using (PacketReader reader = new PacketReader(nStream))
            {
                var packet = reader.ReadPacket(adapeter.Mapping);
                var info = reader.GetPacketInfo(packet);
                return new PacketReadBack(packet, info);
            }
        }

        private Task ReadPacketTask()
        {
            try
            {
                PacketReadBack back = ReadPacketFromStream();
                
            }catch(Exception e)
            {
                logger.Warn("Error When Reading Packet, {0}", IsDebugMode ? e.ToString() : "You Can Report It To Us");
                GC.Collect();
            }

            if (IsCallToDead)
                KillClient();
            else
                return Task.Run(ReadPacketTask);
            return Task.CompletedTask;
        }

        private void KillClient()
        {
            clientSocket.Disconnect(true);
            clientSocket.Close();
            clientSocket.Dispose();
            adapeter.UnRegisterClient(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void KillThis() => IsCallToDead = true;
        ~ClientModel() => KillThis();
    }

    struct PacketReadBack
    {
        public IPacket Packet    { get; set; }
        public Packet PacketInfo { get; set; }

        public PacketReadBack(IPacket packet, Packet info) : this()
        {
            Packet = packet ?? throw new Exception("Empty Packeet");
            PacketInfo = info ?? throw new Exception("Empty Packet Info");
            GC.Collect();
        }
    }
}
