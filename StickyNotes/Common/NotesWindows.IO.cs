using HTBInject.Utils;
using StickyNotes.Protocol;
using StickyNotes.Utils.Common;
using StickyNotes.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Common
{
    public sealed partial class NotesWindows
    {

        public void SaveAllWindowNames()
        {
            if (File.Exists(Path))
                File.Delete(Path);

            List<string> names = new List<string>();
            List<NotePage> pages = new List<NotePage>(Windows.Values);
            foreach (var w in pages)
            {
                NoteWindowInfo info = w.Info;
                if (info.IsOnline)
                    continue;
                names.Add(info.FilePath);
                try
                {
                    w.ForcedToSave();
                    w.Close();
                    GC.SuppressFinalize(w);
                    GC.Collect();
                }
                catch (Exception) { }
            }

            using (PacketStream stream = new PacketStream())
            {
                stream.WriteInt32(names.Count);
                foreach (var n in names)
                    stream.WriteString(n);

                using (PacketPackage package = new PacketPackage(stream))
                    File.WriteAllBytes(Path, package.EncodePacket(1919810));
            }

            names.Clear();
            GC.SuppressFinalize(names);
            GC.Collect();
        }

        public void LoadAllWindows()
        {
            if (!File.Exists(Path))
                return;

            List<string> names = new List<string>();
            using (PacketPackage package = new PacketPackage(File.ReadAllBytes(Path)))
            {
                package.ReadPacket();
                using (PacketStream stream = package.DecodeStream())
                {
                    int size = stream.ReadInt32();
                    int index = 0;
                    while(index < size)
                    {
                        names.Add(stream.ReadString());
                        index++;
                    }
                    index = 0;
                }
            }

            foreach(var n in names)
                PostWindow(n);
            names.Clear();
            GC.SuppressFinalize(names);
            GC.Collect();
        }

        private readonly string Path = $"{AssetsManger.GetFolderPath()}//WindowInfo";
    }
}
