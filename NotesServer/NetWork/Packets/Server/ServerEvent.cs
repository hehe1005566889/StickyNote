using StickyNoteCommon.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesServer.NetWork.Packets.Server
{
    [Packet(ID = 1, PacketName = "ServerEventPacket", Support = PacketSupport.Server)]
    class ServerEvent : IPacket
    {
        [PacketField(Type = FieldType.Int)]
        public ServerEventType Type;
        [PacketField(Type = FieldType.Bool)]
        public bool Value;
        [PacketField(Skip = true, Type = FieldType.String)]
        public string Addon;
    }

    enum ServerEventType
    {
        LoginResult = 0
    }
}
