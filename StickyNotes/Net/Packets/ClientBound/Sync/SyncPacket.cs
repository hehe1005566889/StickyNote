using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound.Sync
{
    class SyncPacket : IClientBoundPacket, IServerBoundPacket
    {
        public SyncPacket() => GC.Collect();
        public int GetPacketID() => 7;

        public bool ReadPacket(PacketStream DataStream)
        {
            int size = DataStream.ReadInt32();
            
            return true;
        }

        public bool WritePacket(PacketStream DataStream)
        {
             return true;
        }
    }
}
