using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound
{
    class DownloadTicketPacket : IClientBoundPacket
    {
        public Channel Channel { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public DownloadTicketPacket()
        {
            GC.Collect();
        }

        public int GetPacketID() => 6;

        public bool WritePacket(PacketStream DataStream)
        {
            DataStream.WriteInt32((int)Channel);
            DataStream.WriteString(Name);
            DataStream.WriteString(Author);

            return true;
        }
    }

    enum Channel
    {
        PUBLIC = 0,
        PRIVATE = 1
    }
}
