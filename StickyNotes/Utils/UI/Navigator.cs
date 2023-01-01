using HandyControl.Controls;
using HTBInject.Utils;
using StickyNotes.Utils.Configration;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StickyNotes.Utils.UI
{
    public class Navigator
    {

        public Navigator(Border frame,SimplePanel container)
        {
            this.frame = frame;
            container.Children.Add(frame);
            GC.Collect();
        }

        private readonly Assembly asm = Assembly.GetExecutingAssembly();
        public void RegisterAllPages(ListBox box)
        {
            foreach(var p in asm.GetTypes())
            {
                var atts = p.GetCustomAttributes() as object[];
                if (atts.Length is 0)
                    continue;
                if (!(atts[0] is AppPage))
                    continue;
                AppPage page = atts[0] as AppPage;
                PageCollect.RegisterPage(page);

            }
            GC.Collect();
            AppendToUI(box);
            box.SelectionChanged += Box_SelectionChanged;
        }

        private void AppendToUI(ListBox box)
        {
            for (int i = 0; i <= PageCollect.GetSize() - 1; i++)
            {
                AppPage page = PageCollect.GetPageById(i);

                box.Items.Add(new ListBoxItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Content = page.Info.Icon,
                    ToolTip = page.Info.Name,
                    Margin = new Thickness(2, 5, 2, 0),
                    FontSize = 15,
                    Background = null
                });
            }
            GC.Collect();
        }

        private void Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = sender as ListBox;
            if (box.SelectedIndex is -1) return;

            ListBoxItem item = box.SelectedItem as ListBoxItem;
            item.Background = ColorCollect.blue;
            item.Foreground = ColorCollect.white;

            foreach (ListBoxItem lti in box.Items)
            {
                if (!lti.Equals(box.SelectedItem))
                {
                    lti.Background = ColorCollect.none;
                    lti.Foreground = ColorCollect.black;
                };
            }
            NavigatePage(box.SelectedIndex);
        }

        public void NavigatePage(int id)
        {
            var obj = frame.Child;
            if(obj != null) GC.SuppressFinalize(obj);

            AppPage page = PageCollect.GetPageById(id);
            if (page is null)
                return;
            SimplePanel instance = (SimplePanel)Activator.CreateInstance(page.Page);

            if (Setting.EnableAnimation)
                frame.Child = PageUtils.AnimatedPage(instance, Style);
            else
                frame.Child = instance;

            NaviTime++;
            GC.Collect();
        }

        ~Navigator()
        {
            GC.SuppressFinalize(Style);
            GC.SuppressFinalize(PageCollect);
            GC.SuppressFinalize(frame.Child);
            GC.SuppressFinalize(frame);
            GC.Collect();
        }

        public AnimationStyle Style { private get; set; } = AnimationStyle.D;
        private readonly NaviPages PageCollect = NaviPages.Pages;
        private readonly AppSetting Setting = App.Instance.Config;
        private readonly Border frame;
        private int NaviTime = 0;
    }
}
