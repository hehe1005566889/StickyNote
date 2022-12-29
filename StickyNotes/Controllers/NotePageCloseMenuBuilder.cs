using HandyControl.Controls;
using StickyNotes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StickyNotes.Views
{
    sealed class NotePageCloseMenuBuilder
    {
        public NotePageCloseMenuBuilder(NotePage page)
        {

            MenuItem item = new MenuItem()
            {
                Header = "保存"
            };
            item.Click += (s, a) =>
            {
                page.Controller.SaveAll(page.Model.Title);
                NotificationIcon.Instance.SendMessage(page.Model.Title, "保存成功~", MessageType.Info);
                GC.Collect();
            };
            menu.Items.Add(item);

            item = new MenuItem()
            {
                Header = "固定"
            };
            item.Click += (s, a) => { };
            menu.Items.Add(item);

            item = new MenuItem()
            {
                Header = "上传"
            };
            item.Click += (s, a) => { };
            menu.Items.Add(item);

            item = new MenuItem()
            {
                Header = "关闭"
            };
            item.Click += (s, a) => page.Close();
            menu.Items.Add(item);

            GC.SuppressFinalize(item);
            GC.Collect();
        }

        public void OpenMenu()
        {
            menu.IsOpen = true;
            GC.Collect();
        }

        ~NotePageCloseMenuBuilder()
        {
            menu.Items.Clear();
            GC.SuppressFinalize(menu);
        }

        private readonly ContextMenu menu = new ContextMenu();
    }
}
