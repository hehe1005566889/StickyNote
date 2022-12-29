using NotesServer.Controller;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Handler
{
    class PublicNoteHandler : IHandler
    {
        private readonly PublicNotesFactory Handler = PublicNotesFactory.GetFactory();
        public int GetReturnPacketID() => 1;
        private bool IsAdded = false;

        public PublicNoteHandler()
        {
            GC.Collect();
        }

        public bool ReadPacket(PacketStream DataStream)
        {
            var name = DataStream.ReadString();
            var author = DataStream.ReadString();
            var content = DataStream.ReadString();

            IsAdded = Handler.AddNote(name, content, author);

            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            ResultStream.WriteInt32((int)ResultStatus.PUBLICNOTE);
            ResultStream.WriteBoolean(IsAdded);
            return true;
        }
    }
}
