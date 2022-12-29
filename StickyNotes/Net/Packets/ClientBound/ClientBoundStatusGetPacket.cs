using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ClientBound
{
    class ClientBoundStatusGetPacket : IClientBoundPacket
    {
        private Client Client { get; }
        public ClientBoundStatusGetPacket(Client client)
        {
            Client = client;
            GC.Collect();
        }

        public int GetPacketID() => 2;

        public bool WritePacket(PacketStream DataStream)
        {
            DataStream.WriteString(Client.username);
            return true;
        }
    }
}
