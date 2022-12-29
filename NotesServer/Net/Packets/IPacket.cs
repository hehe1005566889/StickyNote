using StickyNotes.Protocol;

namespace NotesServer.Protocol.Net.Packets
{
    internal interface IPacket
    {
        bool WriteReturn(PacketStream ResultStream);
        int GetReturnPacketID();
    }
}
