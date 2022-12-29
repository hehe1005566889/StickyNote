using NotesServer.Controller;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Handler
{
    class GetTicketHandler : IHandler
    {
        public int GetReturnPacketID() => 6;
        private readonly PublicNotesFactory Factory = PublicNotesFactory.GetFactory();

        private int Channel;
        private string Name, Author;

        public GetTicketHandler()
        {
            GC.Collect();
        }

        public bool ReadPacket(PacketStream DataStream)
        {
            Channel = DataStream.ReadInt32();
            Name = DataStream.ReadString();
            Author = DataStream.ReadString();

            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            if(Channel is 0)
            {
                ResultStream.WriteBoolean(Factory.GetNote(ResultStream, Name, Author));
            } else
            {
                ResultStream.WriteString("");
                ResultStream.WriteString("");
                ResultStream.WriteBoolean(false);
            }

            return true;
        }
    }
}
