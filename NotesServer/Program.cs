using StickyNoteCommon.Framework;

namespace NotesServer
{
    class Program
    {
        public static void Main(string[] args)
            => ServiceManger.Instance.ExcutingApplication();
    }
}
