using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound.Sync
{
    class ClientDownloadPacket : IClientBoundPacket
    {
        public ClientDownloadPacket() => GC.Collect();
        public int GetPacketID() => 8;

        public bool WritePacket(PacketStream DataStream)
        {
            return true;
        }
    }
}
