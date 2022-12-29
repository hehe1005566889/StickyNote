using HTBInject.Utils;
using StickyNotes.Net;
using StickyNotes.Protocol;
using StickyNotes.ViewModels;
using StickyNotes.Views;
using System;
using System.IO;

namespace StickyNotes.Controllers
{
    public sealed class NotePageController
    {
        public NotePageController(NotePage page)
        {
            if(!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);
            Model = (NoteWindowViewModel)page.DataContext;
            Window = page;
            GC.Collect();
        }

        public void LoadAll(string name)
        {
            if (!File.Exists($"{path}\\{name}.Ticket"))
            {
                Model.SetSize(275, 175);
                Model.SetLeftTop(100, 100);

                SaveAll(name);

                GC.Collect();
                return;
            }
            using (PacketPackage packet = new PacketPackage(
                File.ReadAllBytes($"{path}\\{name}.Ticket")
            )) {
                packet.ReadPacket();
                _ = packet.GetPacketID();
                PacketStream stream = packet.DecodeStream();


                int top = stream.ReadInt32();
                int left = stream.ReadInt32();
                Console.WriteLine("Top {0}, Left {1}", top, left);
                int width = stream.ReadInt32();
                int height = stream.ReadInt32();

                string text = stream.ReadString();

                try
                {
                    int version = stream.ReadInt32();
                    bool pinded = stream.ReadBoolean();
                    Window.Info.IsPined = pinded;
                }
                catch (Exception) { }

                Model.SetLeftTop(top, left);
                Model.SetSize(width, height);
                Model.SetText(text);

                GC.Collect();
            }
        }

        public void LoadAll(string name, string content)
        {
            IsLocal = false;
            Model.SetLeftTop(100, 100);
            Model.SetSize(275, 175);
            Model.SetText(content);
            Model.SetTitle(name);
        }

        public void SaveAll(string name)
        {
            if (!IsLocal)
                return;
            using (PacketStream Stream = new PacketStream())
            {
                Stream.WriteInt32((int)Window.Top);
                Stream.WriteInt32((int)Window.Left);
                Stream.WriteInt32((int)Window.Width);
                Stream.WriteInt32((int)Window.Height);
                Stream.WriteString(Model.InputText);
                Stream.WriteInt32(Version);

                //Added From Version -> 2
                Stream.WriteBoolean(Window.Info.IsPined);

                using (PacketPackage packet = new PacketPackage(Stream))
                {
                    File.WriteAllBytes($"{path}\\{name}.Ticket", packet.EncodePacket(-1));
                };
            }
            GC.Collect();
        }

        ~NotePageController()
        {
            GC.Collect();
        }

        private readonly int Version = 2;
        private readonly NotePage Window;
        private bool IsLocal = true;
        private NoteWindowViewModel Model { get; }
        public readonly string path = string.Format("{0}//{1}", AssetsManger.GetFolderPath(), "Tickets");
    }
}
