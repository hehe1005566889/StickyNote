using StickyNotes.Net;
using StickyNotes.Protocol;

namespace StickyNotes.Net.Packets
{
    public interface IClientBoundPacket : IPacket
    {
        bool WritePacket(PacketStream DataStream);
    }
}
