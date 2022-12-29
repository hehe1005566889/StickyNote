using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StickyNotes.Utils
{
    class ColorCollect
    {
        public static SolidColorBrush blue = new SolidColorBrush(Color.FromArgb(153, 30, 75, 175));
        public static SolidColorBrush none = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public static SolidColorBrush white = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public static SolidColorBrush black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        private static readonly Dictionary<string, SolidColorBrush> ColorMap = new Dictionary<string, SolidColorBrush>();

        public static void PutColor(string id, SolidColorBrush color)
        {
            if (ColorMap.ContainsKey(id))
                return;
            ColorMap.Add(id, color);
        }

        public static SolidColorBrush GetColor(string id)
        {
            if (!ColorMap.ContainsKey(id))
                return null;
            return ColorMap[id];
        }

        public static void ClearMap() => ColorMap.Clear();
    }
}
