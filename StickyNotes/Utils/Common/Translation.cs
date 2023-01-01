using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils.Common
{
    public sealed partial class Translation
    {
        public Translation(string file)
        {
            TransFileName = $"{file}.llang";
            ReadFile(TransFileName);
            GC.Collect();
        }
        
        public void ApplyTranslation(Type type, object Instance)
        {
            foreach(var f in type.GetFields())
            {
                object[] atts = f.GetCustomAttributes(true) as object[];
                if (atts.Length is 0)
                    continue;

                Translable translable = GetTranslable(atts);
                if (translable is null)
                    continue;

                string key = $"{type.Namespace}.{type.Name}.{f.Name}";
                if (!LangMap.ContainsKey(key))
                    f.SetValue(Instance, key);
                else
                    f.SetValue(Instance, LangMap[key]);
                GC.SuppressFinalize(translable);
            }
            GC.Collect();
            GC.WaitForFullGCApproach();
        }

        public string GetValue(string key)
        {
            if (LangMap.ContainsKey(key))
                return LangMap[key];
            return key;
        }

        private Translable GetTranslable(object[] atts)
        {
            foreach (var o in atts)
                if (o is Translable)
                    return o as Translable;
            return null;
        }

        private readonly string TransFileName;
    }

    public sealed class Translable : Attribute
    {

    }
}
