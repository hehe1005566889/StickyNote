using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound
{
    class ClientBoundPublicNotePacket : IClientBoundPacket
    {
        public string Name { set; get; }
        public string Author { set; get; }
        public string Content { set; get; }

        public ClientBoundPublicNotePacket() => GC.Collect();
        public int GetPacketID() => 3;

        public bool WritePacket(PacketStream DataStream)
        {
            DataStream.WriteString(Name);
            DataStream.WriteString(Author);
            DataStream.WriteString(Content);
            return true;
        }
    }
}
