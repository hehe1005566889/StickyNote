using StickyNotes.Protocol;

namespace StickyNotes.Net.Packets
{
    public interface IServerBoundPacket : IPacket
    {
        bool ReadPacket(PacketStream DataStream);
    }
}
