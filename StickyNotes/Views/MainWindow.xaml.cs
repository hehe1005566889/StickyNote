using HTBInject.Utils;
using StickyNotes.Common;
using StickyNotes.Pages;
using StickyNotes.Utils;
using StickyNotes.Utils.Configration;
using StickyNotes.Utils.UI;
using StickyNotes.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using static StickyNotes.Utils.NativeImport;
using Window = System.Windows.Window;

namespace StickyNotes.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();

            Tor = new Navigator(new Border() { Margin = new Thickness(48, 0, 0, 0) }, container);
            Tor.RegisterAllPages(Menu);
            LoadEffect();
            GC.Collect();

            chrome_close.Click += (s, e) => Hide();
            chrome_mini.Click += (s, e) => WindowState = WindowState.Minimized;
            chrome_max.Click += (s, e) =>
            {
                if (WindowState is WindowState.Normal)
                {
                    chrome_max.Content = "\ue923";
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    chrome_max.Content = "\ue922";
                    WindowState = WindowState.Normal;
                }
                GC.Collect();
            };
            
            Menu.SelectedIndex = 0;
            CollectRam();
        }

        [Obsolete]
        public void LoadPageV3Manger(int id)
        {
            switch (id)
            {
                case 0:
                    LoadPageV3(typeof(AccontPage));
                    break;
                case 1:
                    LoadPageV3(typeof(NotesPage));
                    break;
                case 2:
                    LoadPageV3(typeof(SettingPage));
                    break;
                default:
                    break;
            }
        }

        [Obsolete]
        public void LoadPageV3(object page)
        {
            ViewModel.Content = null;
            object obj = Activator.CreateInstance((Type)page, null);
            //shower.Child = PageUtils.AnimatedPage((SimplePanel)obj);
            GC.Collect();
        }
        
        private void LoadEffect()
        {
            WindowChrome chrome = new WindowChrome()
            {
                GlassFrameThickness = new Thickness(-1)
            };
            WindowChrome.SetWindowChrome(this, chrome);
            GC.SuppressFinalize(chrome);
            GC.Collect();
        }

        protected override void OnActivated(EventArgs e)
        {
            if (!Setting.EnableEffect)
                return;
            else if (IsEnableEffect)
                return;
            NativeEffectSupport.EnableBlur(this);
            IsEnableEffect = true;
            base.OnActivated(e);
            GC.Collect();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            if (Setting.EnableEffect)
                return;
            else if (!IsEnableEffect)
                return;
            NativeEffectSupport.DisableBlur(this);
            IsEnableEffect = false;
            base.OnDeactivated(e);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DirectoryInfo info = new DirectoryInfo($"{AssetsManger.GetFolderPath()}\\data");
            info.Delete(true);
            base.OnClosing(e);
        }

        ~MainWindow()
        {
            NativeEffectSupport.DisableBlur(this);
            GC.SuppressFinalize(Tor);
            GC.SuppressFinalize(ViewModel);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private bool IsEnableEffect = false;
        private readonly AppSetting Setting = App.Instance.Config;
        private readonly Navigator Tor;
        private readonly MainWindowViewModel ViewModel = new MainWindowViewModel();
    }
}
