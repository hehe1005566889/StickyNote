using System;
using System.Collections.Generic;

namespace StickyNotes.Utils.UI
{
    class NaviPages
    {
        public void RegisterPage(AppPage page)
        {
            if (PageCollect.ContainsKey(page.Id))
                return;
            PageCollect.Add(page.Id, page);
            GC.Collect();
        }

        public void KillPage(int id)
        {
            if (id > PageCollect.Count)
                return;
            AppPage page = PageCollect[id];
            page.Dispose();
            GC.SuppressFinalize(page);
            PageCollect.Remove(id);
            GC.Collect();
        }

        public AppPage GetPageById(int id)
        {
            if (id > PageCollect.Count)
                return null;
            return PageCollect[id];
        }
        
        public void KillAllPages()
        {
            foreach(var p in PageCollect)
                GC.SuppressFinalize(p.Value);
            PageCollect.Clear();
            GC.Collect();
        }

        ~NaviPages()
        {
            KillAllPages();
            GC.SuppressFinalize(Pages);
            GC.SuppressFinalize(PageCollect);
            GC.Collect();
        }

        public Dictionary<int, AppPage> GetPages() => PageCollect;
        public int GetSize() => PageCollect.Count;
        public static readonly NaviPages Pages = new NaviPages();
        private readonly Dictionary<int, AppPage> PageCollect = new Dictionary<int, AppPage>();
    }
}
