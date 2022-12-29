using NotesServer.Protocol.Net.Handler;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Packets
{
    class ResultPacket : IPacket
    {
        public ResultStatus Status { get; set; }
        public bool Result { get; set; } = false;
        public string Addon { get; set; } = null;

        public ResultPacket()
        {
            GC.Collect();
        }

        public int GetReturnPacketID() => 1;

        public bool WriteReturn(PacketStream ResultStream)
        {
            ResultStream.WriteInt32((int)Status);
            ResultStream.WriteBoolean(Result);
            if (Addon != null)
                ResultStream.WriteString(Addon);
            return true;
        }
    }
}
