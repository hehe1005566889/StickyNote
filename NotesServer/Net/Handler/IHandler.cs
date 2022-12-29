using StickyNotes.Protocol;

namespace NotesServer.Protocol.Net.Handler
{
    internal interface IHandler
    {
        bool ReadPacket(PacketStream DataStream);
        bool WriteReturn(PacketStream ResultStream);
        int GetReturnPacketID();
    }
}
