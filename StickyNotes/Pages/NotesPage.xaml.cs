using HandyControl.Controls;
using HTBInject.Utils;
using StickyNotes.Common;
using StickyNotes.Controllers;
using StickyNotes.Net.Packets.ClientBound;
using StickyNotes.Protocol;
using StickyNotes.Utils;
using StickyNotes.Utils.UI;
using StickyNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace StickyNotes.Pages
{
    [AppPage(1, "便签管理", "\ue71d", typeof(NotesPage))]
    public sealed partial class NotesPage : SimplePanel, IInputBoxController
    {
        public NotesPage()
        {
            DataContext = Model;
            Controller = new NotesPageController(this);
            Controller.ScanNotes();

            InitializeComponent();

            if (Tickets.Items.Count != 0)
                Tickets.SelectedIndex = 0;
            Tickets.MouseDoubleClick += Tickets_MouseDoubleClick;
            Add.Click += Add_Click;
            @public.Click += Public_Click;
            
            NativeImport.CollectRam();
        }

        private void Public_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var ticket = ReadTicket(Tickets.SelectedItem.ToString());
            App.Net.SendPacket(new ClientBoundPublicNotePacket()
            {
                Author = App.Net.username,
                Content = ticket.Value.Value,
                Name = Tickets.SelectedItem.ToString()
            });
            GC.Collect();
        }

        private KeyValuePair<string, string>? ReadTicket(string name)
        {
            if (!File.Exists($"{AssetsManger.GetFolderPath()}\\Tickets\\{name}.Ticket"))
                return null;
            using (PacketPackage packet = new PacketPackage(
                File.ReadAllBytes($"{AssetsManger.GetFolderPath()}\\Tickets\\{name}.Ticket")
            )) {
                packet.ReadPacket();
                _ = packet.GetPacketID();
                PacketStream stream = packet.DecodeStream();

                _ = stream.ReadInt32();
                _ = stream.ReadInt32();
                _ = stream.ReadInt32();
                _ = stream.ReadInt32();

                string text = stream.ReadString();

                GC.Collect();
                return new KeyValuePair<string, string>(name, text);
            }
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Type = NotesPageActionType.OnAdding;
            InputDialog dialog = new InputDialog(this, "添加一个便签", ":) 添加一个便签 \n 在这里输入新的便签应该叫什么吧:>");
            dialog.ShowDialog();
        }

        private void Tickets_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(Tickets.SelectedIndex is -1)
            {
                Growl.WarningGlobal("请选择一项来打开哦~");
                return;
            }
            WindowController.PostWindow(Tickets.SelectedItem.ToString());
            GC.Collect();
        }

        public void OnDone(string content)
        {
            if (Type is NotesPageActionType.OnAdding)
            {
                Growl.InfoGlobal($"创建便签名字为 {content} :>");
                Controller.CreateNewNote(content);
                Controller.ScanNotes();
            }
            GC.Collect();
        }

        ~NotesPage()
        {
            GC.SuppressFinalize(Type);
            GC.SuppressFinalize(Controller);
            GC.SuppressFinalize(Model);
            GC.Collect();
        }

        private NotesPageActionType Type;
        private NotesPageController Controller { get; }
        private NotesPageViewModel Model { get; } = new NotesPageViewModel();
        private NotesWindows WindowController { get; } = NotesWindows.GetController();
    }
}
