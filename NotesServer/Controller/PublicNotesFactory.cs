using FlyBirdCommon.Logger;
using StickyNotes.Protocol;
using StickyNotes.Protocol.Net;
using StickyNotes.Protocol.Tools;
using System;
using System.IO;
using System.Text;

namespace NotesServer.Controller
{
    class PublicNotesFactory
    {
        private static PublicNotesFactory Instance;
        public static PublicNotesFactory GetFactory()
        {
            if (Instance is null)
                Instance = new PublicNotesFactory();
            return Instance;
        }

        private Logger logger = SharedLoggers.GetPlatformLogger();
        private PublicNotesFactory()
        {
            if (!Directory.Exists("PublicTickets"))
                Directory.CreateDirectory("PublicTickets");
            GC.Collect();
        }

        public bool GetNote(PacketStream stream, string name, string author)
        {
            string names = Convert.ToBase64String(
                   Gzip.CompressBytes(Encoding.UTF8.GetBytes(
                       string.Format("{0}+=+=+{1}", name, author)
                       ))
                   ).Replace("A", ";,");
            string titleBlock = string.Format("PublicTickets//{0}", names);

            if (File.Exists(titleBlock))
            {

                using (PacketPackage package = new PacketPackage(File.ReadAllBytes(titleBlock))) {
                    package.ReadPacket();
                    using (PacketStream str = package.DecodeStream())
                    {
                        stream.WriteString(str.ReadString());
                        stream.WriteString(str.ReadString());
                    }
                }
                return true;
            }
            return false;
        }

        public bool AddNote(string name, string content, string author)
        {
            try
            {
                using (PacketStream stream = new PacketStream()) {
                    stream.WriteString(name);
                    stream.WriteString(content);

                    string names = Convert.ToBase64String(
                        Gzip.CompressBytes(Encoding.UTF8.GetBytes(
                            string.Format("{0}+=+=+{1}", name, author)
                            ))
                        ).Replace("A", ";,");
                    string titleBlock = string.Format("PublicTickets//{0}", names.Replace("/", "[_]"));

                    using (PacketPackage package = new PacketPackage(stream))
                    File.WriteAllBytes(titleBlock, package.EncodePacket(-5));
                }
                return true;
            }
            catch(Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool GetNotes(PacketStream stream, int offset, int index)
        {
            string[] names = Directory.GetFiles("PublicTickets");
            var max = names.Length - 1;
            if(index + offset > max)
                offset = max - index;
            stream.WriteInt32(offset);
            ClientModel.Debug("[DEBUG] Queue Offset {0}", offset.ToString());

            for(int i = index; i < index + offset + 1; i++)
            {
                try
                {
                    string origin = names[i].Replace(@"PublicTickets\", "").Replace("[_]", "/").Replace(";,", "A");
                    origin = Encoding.UTF8.GetString(
                        Gzip.Decompress(Convert.FromBase64String(origin))
                        );

                    string[] title = origin.Split("+=+=+");
                    stream.WriteString(title[0]);
                    stream.WriteString(title[1]);
                }catch(Exception e)
                {
                    logger.Error(e.ToString());
                }
            }
            return true;
        }

        public bool Remove(int index)
        {
            string[] names = Directory.GetFiles("PublicTickets");

            var max = names.Length - 1;
            if (index > max)
                return false;

            try
            {
                File.Delete(names[index]);
                GC.Collect();
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool Remove(int index, string name)
        {
            string[] names = Directory.GetFiles("PublicTickets");

            var max = names.Length - 1;
            if (index > max)
                return false;

            try
            {
                string origin = names[index].Replace(@"PublicTickets\", "").Replace("[_]", "/").Replace(";,", "A");
                origin = Encoding.UTF8.GetString(
                        Gzip.Decompress(Convert.FromBase64String(origin))
                        );

                if (origin.Split("+=+=+")[1] != name)
                    return false;

                File.Delete(origin);
                GC.Collect();
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }
    }
}
