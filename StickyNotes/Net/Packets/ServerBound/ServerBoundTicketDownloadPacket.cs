using StickyNotes.Pages;
using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ServerBound
{
    class ServerBoundTicketDownloadPacket : IServerBoundPacket
    {
        private readonly AccontPage Page;
        public ServerBoundTicketDownloadPacket(AccontPage page)
        {
            Page = page;
            GC.Collect();
        }

        public int GetPacketID() => 6;

        public bool ReadPacket(PacketStream DataStream)
        {
            string name = DataStream.ReadString();
            string content = DataStream.ReadString();
            bool result = DataStream.ReadBoolean();

            if(result)
                Page.OpenOnlineDialog(name, content);

            return true;
        }
    }
}
