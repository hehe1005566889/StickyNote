using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound
{
    class ClientBoundLoginPacket : IClientBoundPacket
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsLogin { get; private set; }

        public ClientBoundLoginPacket() => GC.Collect();
        public int GetPacketID() => 0;

        public bool WritePacket(PacketStream DataStream)
        {
            DataStream.WriteString(Username);
            DataStream.WriteString(Password);
            GC.Collect();
            return true;
        }
    }
}
