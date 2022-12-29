using HandyControl.Controls;
using StickyNotes.Utils;
using StickyNotes.Utils.UI;
using StickyNotes.ViewModels;
using System;

namespace StickyNotes.Pages
{
    [AppPage(2, "设置页", "\ue713", typeof(SettingPage))]
    public sealed partial class SettingPage : SimplePanel
    {
        public SettingPage()
        {
            model.InitConfigView();
            DataContext = model;
            InitializeComponent();

            box.SelectionChanged += Box_SelectionChanged;

            GC.Collect();
            NativeImport.CollectRam();
        }

        private void Box_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            box.SelectedIndex = -1;
        }

        ~SettingPage()
        {
            GC.SuppressFinalize(model);
            GC.Collect();
        }

        private readonly SettingPageViewModel model = new SettingPageViewModel();
    }
}
