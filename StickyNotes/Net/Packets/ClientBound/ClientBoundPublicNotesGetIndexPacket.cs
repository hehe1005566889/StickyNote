using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound
{
    class ClientBoundPublicNotesGetIndexPacket : IClientBoundPacket
    {
        public int Index { get; set; }
        public int Offset { get; set; } = 10;

        public ClientBoundPublicNotesGetIndexPacket() => GC.Collect();
        public int GetPacketID() => 4;

        public bool WritePacket(PacketStream DataStream)
        {
            DataStream.WriteInt32(Index);
            DataStream.WriteInt32(Offset);
            return true;
        }
    }
}
