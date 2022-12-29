using NotesServer.Protocol.Net.Packets;
using StickyNoteCommon.Net;
using System;
using System.Collections.Generic;
using System.Text;
using IPacket = StickyNoteCommon.Net.IPacket;

namespace NotesServer.NetWork.Packets.Client
{
    [Packet(ID = 1, PacketName = "LoginPacket", Support = PacketSupport.Client)]
    class ClientLogin : IPacket
    {
        [PacketField(Skip = false, Type = FieldType.String)]
        public string UserName;
        [PacketField(Skip = false, Type = FieldType.String)]
        public string Password;
    }
}
