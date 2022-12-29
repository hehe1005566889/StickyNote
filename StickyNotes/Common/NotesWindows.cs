using StickyNotes.Utils;
using StickyNotes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StickyNotes.Common
{
    public sealed partial class NotesWindows
    {
        public static NotesWindows GetController()
        {
            if (WindowCollect is null)
                WindowCollect = new NotesWindows();
            return WindowCollect;
        }

        public void PostWindow(string name)
        {
            if (Windows.ContainsKey(name))
                return;
            NotePage window = new NotePage(name);
            Windows.Add(name, window);
            window.Show();
            window.Closed += (s, e) =>
            {
                Windows.Remove(name);
                NativeImport.CollectRam();
            };
            GC.Collect();
        }

        public void PostWindow(string name, string content)
        {
            Thread UIThread = new Thread(() =>
            {
                if (Windows.ContainsKey(name))
                    return;
                NotePage window = new NotePage(name, content);
                Windows.Add(name, window);
                window.Show();
                window.Closed += (s, e) =>
                {
                    Windows.Remove(name);
                    NativeImport.CollectRam();
                };
                NativeImport.CollectRam();
                Application.Run();
            })
            {
                Name = name,
                IsBackground = true
            };
            UIThread.SetApartmentState(ApartmentState.STA);
            UIThread.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NotePage page = (NotePage)sender;
            _ = Windows.Remove(page.Title);
            NativeImport.CollectRam();
        }

        public List<string> GetStartedWindows()
        {
            return Windows.Keys.ToList();
        }
        
        ~NotesWindows()
        {
            Windows.Clear();
            GC.SuppressFinalize(Windows);
            GC.SuppressFinalize(NotesWindows.WindowCollect);
            GC.Collect();
        }

        private static NotesWindows WindowCollect;
        private Dictionary<string, NotePage> Windows = new Dictionary<string, NotePage>();
    }
}
