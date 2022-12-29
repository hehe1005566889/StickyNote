using HTBInject.Utils;
using StickyNotes.Common;
using StickyNotes.Pages;
using StickyNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StickyNotes.Controllers
{
    sealed class NotesPageController
    {
        private readonly string path = string.Format("{0}//{1}", AssetsManger.GetFolderPath(), "Tickets");
        public NotesPageController(NotesPage page)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Page = page;
            Model = (NotesPageViewModel)page.DataContext;
            GC.Collect();
        }

        public void ScanNotes()
        {
            FileMaps.Clear();
            string[] names = Directory.GetFiles(path);
            foreach (string name in names)
            {
                if (File.Exists(name))
                {
                    string display = name.Replace($@"{path}\", "").Replace(".Ticket", "");
                    FileMaps.Add(display, name);
                }
            }
            Model.UpdateTickets(FileMaps.Keys.ToList());
            GC.Collect();
        }

        public void CreateNewNote(string name)
        {
            WindowController.PostWindow(name);
            GC.Collect();
        }

        ~NotesPageController()
        {
            FileMaps.Clear();
            GC.SuppressFinalize(FileMaps);
            GC.Collect();
        }

        private NotesWindows WindowController { get; } = NotesWindows.GetController();
        private NotesPageViewModel Model { get; }
        private NotesPage Page { get; }
        private Dictionary<string, string> FileMaps { get; } = new Dictionary<string, string>();
    }
}
