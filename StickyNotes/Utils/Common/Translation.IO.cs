using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils.Common
{
    public sealed partial class Translation
    {
		public void ReadFile(string file)
        {
            LangMap.Clear();
            string[] texts = File.ReadAllLines(file);
            foreach (var line in texts)
            {
                if (line is null || line.Length is 0)
                    continue;
                if (line.StartsWith("#"))
                    continue;
                if (line.StartsWith("{"))
                    DealWithPlaceHelder(line);
                if (!line.Contains("=") || line.EndsWith("="))
                    continue;
                string[] kv = line.Split('=');
                LangMap.Add(kv[0], kv[1]);
            }

            GC.SuppressFinalize(texts);
            GC.Collect();
        }

		private void DealWithPlaceHelder(string line)
        {
            if (line.StartsWith("{LnName}"))
                LangName = line.Replace("{LnName}", "").Replace("(", "").Replace(")","");
            if (line.StartsWith("{Author}"))
                LangAuthor = line.Replace("{Author}", "").Replace("(", "").Replace(")","");
            GC.Collect();
        }

        public string LangName { get; private set; }
        public string LangAuthor { get; private set; }
        private readonly Dictionary<string, string> LangMap = new Dictionary<string, string>();
    }
}
