using HandyControl.Controls;
using HandyControl.Data;
using StickyNotes.Utils.UI;

namespace HTBInject.Utils
{
    class PageUtils
    {
        public static TransitioningContentControl AnimatedPage(object id, AnimationStyle style)
        {
            return new TransitioningContentControl() {
                Content = id,
                TransitionMode = (TransitionMode)style
            };
        }
    }
}
