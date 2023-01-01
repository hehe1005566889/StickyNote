using FlyBirdCommon.Logger;
using StickyNoteCommon.Net;
using StickyNoteCommon.Net.API;
using StickyNoteCommon.Net.Basic;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NotesServer.NetWork
{
    class ClientModel : IPacketManager
    {

        public void OnActive() => GC.Collect();

        public void OnAddCollect(PacketManagerList collect, int id)
        {
            clientID = id;
            GC.Collect();
        }

        public void OnClose()
        {
            logger.Info("Client Closed Connect");
            GC.Collect();
        }

        public void OnCollectCallKill(PacketManagerList collect)
        {
            logger.Info("Call Kill Event By Server Adapter");
            manager.KillManager();
        }

        public void OnInit(PacketManager manager)
        {
            var info = manager.GetInfo();
            logger.Info("New Client Connect Form {0}(Port {1}, Local Port {2})", info.IP, info.Port, info.LocalPort);
            this.manager = manager;
        }

        public void OnRecievePacket(IPacket packet, Packet info)
        {

        }

        public void OnSendPacket(IPacket packet, Packet info)
        {

        }

        private int clientID;
        private PacketManager manager;
        private readonly Logger logger = SharedLoggers.GetNetLogger();
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
