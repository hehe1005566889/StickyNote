using HandyControl.Controls;
using HandyControl.Data;
using HTBInject.Utils;
using StickyNotes.Utils.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StickyNotes.Common
{
    public sealed class NotificationIcon
    {
        private NotificationIcon()
        {
            Icon = new NotifyIcon();
            Menu = new ContextMenu();
            MenuItems = new Dictionary<MenuInfo, Action>();

            GC.Collect();
        }

        public void LoadIcon()
        {
            var icon = AssetsManger.GetAssets(AssetsManger.Type.type_image, "notes") as ImageSource;
            Icon.Icon = icon;
            Icon.Init();
            Icon.Visibility = Visibility.Visible;
            Icon.ContextMenu = Menu;

            App.Instance.Trans.ApplyTranslation(typeof(NotificationIcon), this);
            LoadMenus();
            BuildUpMenu();
        }

        private void LoadMenus()
        {
            MenuItems.Add(new MenuInfo(){ Icon = "\ue8bb", Name = Exit }, new Action(() => App.Instance.ExitApp()));
            MenuItems.Add(new MenuInfo(){ Icon = "\ue897", Name = About }, new Action(() => 
                SendMessage("About", AssetsManger.GetAssets(AssetsManger.Type.type_string, "About") as string, MessageType.Info)
            ));
        }

        public void RegisterMenuItem(MenuInfo name, Action action)
        {
            MenuItems.Add(name, action);
            BuildUpMenu();
            GC.Collect();
        }

        private void BuildUpMenu()
        {
            Menu.Items.Clear();
            foreach (var btn in MenuItems)
            {
                MenuItem item = new MenuItem()
                {
                    Header = btn.Key.Name,
                    Icon = new TextBlock() { Text = btn.Key.Icon, FontFamily = new FontFamily("Segoe MDL2 Assets") }
                };
                item.Click += Btn_Click;
                Menu.Items.Insert(0, item);
            }
            Icon.ContextMenu = Menu;
            GC.Collect();
        }

        public void SendMessage(string title, string content, MessageType type)
        {
            Icon.ShowBalloonTip(title, content, (NotifyIconInfoType) type);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MenuItem ac = sender as MenuItem;
            MenuInfo info = new MenuInfo() { Icon = (ac.Icon as TextBlock).Text, Name = ac.Header as string };
            Action d = MenuItems[info];
            GC.SuppressFinalize(info);
            if (d is null)
                return;
            d.Invoke();
            GC.Collect();
        }

        public void KillIcon()
        {
            Icon.Visibility = Visibility.Collapsed;
            Icon.Dispose();
            GC.Collect();
        }

        ~NotificationIcon()
        {
            KillIcon();
            GC.SuppressFinalize(Icon);
            GC.Collect();
        }

        [Translable]
        public string Exit;
        [Translable]
        public string About;

        public static readonly NotificationIcon Instance = new NotificationIcon();
        public readonly NotifyIcon Icon;
        private readonly ContextMenu Menu;
        private readonly Dictionary<MenuInfo, Action> MenuItems;
    }

    public enum MessageType
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public struct MenuInfo
    {
        public string Icon { get; set; }
        public string Name { get; set; }
    }
}
