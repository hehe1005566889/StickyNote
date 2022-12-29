using StickyNotes.Net.Packets;
using StickyNotes.Net.Packets.ServerBound;
using StickyNotes.Protocol;
using System;
using System.Collections.Generic;

namespace StickyNotes.Net
{
    public class PacketHandler
    {
        private static PacketHandler handler;
        public static PacketHandler GetHandler()
        {
            if (handler is null)
                handler = new PacketHandler();
            return handler;
        }

        public void Remove(int id)
        {
            if (PacketMap.ContainsKey(id))
                PacketMap.Remove(id);
            GC.Collect();
        }

        public PacketHandler()
        {
            GC.Collect();
        }

        public void ActionPacket(
            int packetID,
            PacketStream stream
        ) {
            if (!PacketMap.ContainsKey(packetID))
                return;
            
            IServerBoundPacket packet = PacketMap[packetID];
            if (!packet.ReadPacket(stream))
                Console.WriteLine("Error Reading Packet");

            stream.Dispose();
            GC.Collect();
        }

        public void RegisterPacket(int id, IServerBoundPacket packet)
        {
            if (PacketMap.ContainsKey(id))
                PacketMap.Remove(id);
            PacketMap.Add(id, packet);
        }

        private readonly Dictionary<int, IServerBoundPacket> PacketMap = new Dictionary<int, IServerBoundPacket>()
        {
            { 1, new ServerResultPacket() }
        };
    }
}
