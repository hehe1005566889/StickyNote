using HandyControl.Controls;
using HTBInject.Utils;
using StickyNotes.Common;
using StickyNotes.Net.Packets.ClientBound;
using StickyNotes.Net.Packets.ServerBound;
using StickyNotes.Pages.Dialogs;
using StickyNotes.Utils;
using StickyNotes.Utils.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace StickyNotes.Pages
{
    [AppPage(0, "便签页", "\ue80f", typeof(AccontPage))]
    public sealed partial class AccontPage : SimplePanel
    {
        public AccontPage()
        {
            InitializeComponent();

            connect.Visibility = App.Net is null ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            connect.Click += Connect_Click;
            refrush.Click += Refrush_Click;
            display2.MouseDoubleClick += Display2_MouseDoubleClick;
            account.Click += Account_Click;
            
            if (!Directory.Exists($"{AssetsManger.GetFolderPath()}\\data"))
                _ = Directory.CreateDirectory($"{AssetsManger.GetFolderPath()}\\data");

            if (File.Exists($"{AssetsManger.GetFolderPath()}\\data\\status"))
                LoadAnnounce($"{AssetsManger.GetFolderPath()}\\data\\status");
            else
                if (App.Net != null)
                   LoadAnnounces();
            
            NativeImport.CollectRam();
        }

        private void Account_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            accont.IsOpen = true;
            GC.Collect();
        }

        private void Display2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PublicNoteInfo info = (PublicNoteInfo)display2.SelectedItem;
            App.Net.Handler.RegisterPacket(6, new ServerBoundTicketDownloadPacket(this));
            App.Net.SendPacket(new DownloadTicketPacket()
            {
                Author = info.Author,
                Name = info.Name,
                Channel = Channel.PUBLIC
            });
        }

        public void OpenOnlineDialog(string name, string content)
        {
            WindowController.PostWindow(name, content);
            App.Net.Handler.Remove(6);
            GC.Collect();
        }

        private void Refrush_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Net.Handler.RegisterPacket(4, new ServerBoundGetPublicNotesIndexPacket(this));
            App.Net.SendPacket(new ClientBoundPublicNotesGetIndexPacket()
            {
                Index = 0,
                Offset = 10
            });
        }

        public void LoadAnnounce(string path)
        {
            display.Dispatcher.Invoke(() => display.Child = (Border)XamlReader.Load(XmlReader.Create(path)));
            if(!(App.Net is null)) App.Net.Handler.Remove(2);
            GC.Collect();
        }

        private void LoadAnnounces()
        {
            Console.WriteLine("Get Announces");
            App.Net.Handler.RegisterPacket(2, new ServerBoundStatusResultPacket(this));
            App.Net.SendStatusGet();
            GC.Collect();
        }

        private void Connect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dialogs = Dialog.Show(new LoginPage(this));
        }

        public void CloseDialog()
        {
            Dialogs.Dispatcher.Invoke(() => Dialogs.Close());
            if (App.Net != null)
                LoadAnnounces();
        }

        public void DisplayIndexes(List<PublicNoteInfo> Infos)
        {
            display2.Dispatcher.Invoke(() => display2.ItemsSource = Infos);
            App.Net.Handler.Remove(4);
        }

        ~AccontPage()
        {
            GC.Collect();
        }

        private Dialog Dialogs; 
        private NotesWindows WindowController { get; } = NotesWindows.GetController();
    }
}
