using System;

namespace StickyNotes.Utils.UI
{
    class AppPage : Attribute
    {
        public int Id { get; }
        public PageInfo Info { get; private set; }
        public Type Page { get; private set; }

        public AppPage(int id, string name, string icon, Type page)
        {
            Info = new PageInfo(name, icon);
            Page = page;
            Id = id;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(Page);
            GC.SuppressFinalize(Info);
            GC.Collect();
        }
    }

    class PageInfo
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }

        public PageInfo(string name, string icon)
        {
            Name = name;
            Icon = icon;
        }
    }

    public enum AnimationStyle
    {
        A = 4, B = 5, C = 6, D = 7, Fade = 8
    }
}
