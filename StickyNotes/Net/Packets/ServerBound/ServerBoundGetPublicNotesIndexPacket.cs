using StickyNotes.Pages;
using StickyNotes.Protocol;
using System;
using System.Collections.Generic;

namespace StickyNotes.Net.Packets.ServerBound
{
    class ServerBoundGetPublicNotesIndexPacket : IServerBoundPacket
    {
        private readonly AccontPage Page;

        public ServerBoundGetPublicNotesIndexPacket(AccontPage box)
        {
            Page = box;
            GC.Collect();
        }

        public int GetPacketID() => 4;

        public bool ReadPacket(PacketStream DataStream)
        {
            List<PublicNoteInfo> list = new List<PublicNoteInfo>();
            int index = DataStream.ReadInt32();
            for(int i = 0; i <= index; i++)
            {
                string name = DataStream.ReadString();
                string author = DataStream.ReadString();
                list.Add(new PublicNoteInfo()
                {
                    Author = author,
                    Name = name,
                    Index = $"{name} (上传者: {author})"
                });
            }
            Page.DisplayIndexes(list);

            return true;
        }
    }

    public struct PublicNoteInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Index { get; set; }
    }
}
