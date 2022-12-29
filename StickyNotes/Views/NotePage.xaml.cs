using StickyNotes.Common;
using StickyNotes.Controllers;
using StickyNotes.Utils;
using StickyNotes.Utils.Common;
using StickyNotes.ViewModels;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;
using Window = System.Windows.Window;

namespace StickyNotes.Views
{
    public partial class NotePage : Window, IInputBoxController
    {
        public NotePage(string name)
        {
            DataContext = Model;
            Model.SetTitle(name);
            Controller = new NotePageController(this);
            Controller.LoadAll(name);
            Info = new NoteWindowInfo()
            {
                FilePath = name,
                IsOnline = false
            };

            InitializeComponent();
            LoadEffect();
            Loc = new Vector(Left, Top);

            NativeImport.CollectRam();
        }

        public NotePage(string name, string content)
        {
            Console.WriteLine(name);
            DataContext = Model;
            Controller = new NotePageController(this);
            Controller.LoadAll(name, content);
            Info = new NoteWindowInfo()
            {
                FilePath = name,
                IsOnline = true
            };

            InitializeComponent();
            LoadEffect();
            Loc = new Vector(Left, Top);

            NativeImport.CollectRam();
        }

        protected override void OnInitialized(EventArgs e)
        {
            Menu = new NotePageCloseMenuBuilder(this);
            GC.Collect();
            
            base.OnInitialized(e);
        }

        public void HandleAttributes()
        {

        }
        

        protected override void OnClosed(EventArgs e)
        {
            if (Controller is null)
                return;
            Controller.SaveAll(Model.Title);
            base.OnClosed(e);
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

        private void Chrome_menu_Click(object sender, RoutedEventArgs e)
        => Menu.OpenMenu();
        

        public void OnDone(string content)
        {

        }
        
        protected override void OnActivated(EventArgs e)
        {
            NativeEffectSupport.EnableBlur(this);
            base.OnActivated(e);
            GC.Collect();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            NativeEffectSupport.DisableBlur(this);
            base.OnDeactivated(e);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void ForcedToSave()
        {
            if (Controller is null)
                return;
            Controller.SaveAll(Model.Title);
            GC.Collect();
        }

        ~NotePage()
        {
            GC.SuppressFinalize(Input);
            GC.SuppressFinalize(Info);
            GC.SuppressFinalize(Menu);
            GC.SuppressFinalize(Controller);
            GC.SuppressFinalize(Model);
            GC.Collect();
        }
        
        private Vector Loc { get; set; }
        public NoteWindowInfo Info { get; set; }
        private NotePageCloseMenuBuilder Menu { get; set; }
        public NotePageController Controller { get; }
        public NoteWindowViewModel Model { get; } = new NoteWindowViewModel();
        
    }
}
