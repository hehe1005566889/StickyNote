using StickyNotes.Pages;
using StickyNotes.Protocol;
using System;
using System.IO;

namespace StickyNotes.Net.Packets.ServerBound
{
    class ServerBoundStatusResultPacket : IServerBoundPacket
    {
        private readonly AccontPage Page;
        public ServerBoundStatusResultPacket(AccontPage page)
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
            Page = page;
            GC.Collect();
        }

        public int GetPacketID() => 2;

        public bool ReadPacket(PacketStream DataStream)
        {
            string path = "data//status";
            File.WriteAllText(path, DataStream.ReadString());
            Page.LoadAnnounce(path);
            return true;
        }
    }
}
