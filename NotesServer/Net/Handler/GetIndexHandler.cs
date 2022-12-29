using NotesServer.Controller;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Handler
{
    internal class GetIndexHandler : IHandler
    {
        public GetIndexHandler()
        {
            GC.Collect();
        }

        public int GetReturnPacketID() => 4;
        private readonly PublicNotesFactory Factory = PublicNotesFactory.GetFactory();

        private int index, offset;
        public bool ReadPacket(PacketStream DataStream)
        {
            index  = DataStream.ReadInt32();
            offset = DataStream.ReadInt32();

            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            ResultStream.WriteBoolean(Factory.GetNotes(ResultStream, offset, index));
            return true;
        }
    }
}
