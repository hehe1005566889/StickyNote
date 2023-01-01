using HandyControl.Controls;
using Name3.Models;
using StickyNotes.Utils;
using StickyNotes.Utils.Configration;
using StickyNotes.Utils.UI;
using StickyNotes.ViewModels;
using System;
using System.Windows.Controls.Primitives;

namespace StickyNotes.Pages
{
    [AppPage(2, "SettingPage", "\ue713", typeof(SettingPage))]
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

        private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
            => Save(sender as ToggleButton);
        private void ToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
            => Save(sender as ToggleButton);

        private void Save(ToggleButton button)
        {
            string label = button.ToolTip as string;
            var rawObj = Setting.GetCollect().GetConfig(label);
            if (!rawObj.HasValue)
                return;
            ConfigObject obj = (ConfigObject)rawObj.Value;
            obj.Value = button.IsChecked.ToString();
            Setting.GetCollect().AddObject(obj);
            Setting.SaveConfig();
            Setting.UpdateConfig();

            GC.SuppressFinalize(button);
            GC.SuppressFinalize(rawObj);
            GC.SuppressFinalize(obj);
            GC.Collect();
        }

        ~SettingPage()
        {
            GC.SuppressFinalize(model);
            GC.Collect();
        }
        
        private readonly AppSetting Setting = App.Instance.Config;
        private readonly SettingPageViewModel model = new SettingPageViewModel();
    }
}
