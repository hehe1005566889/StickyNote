using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Name3.Models
{
    class ConfigWriter : IDisposable
    {

        public static ConfigWriter CreateWriter()
        {
            return new ConfigWriter();
        }

        public bool SaveDocument(
            string name,
            ConfigCollect collect
        ) {
            try
            {
                XDocument document = new XDocument();
                collect.WriteTo(document);
                File.WriteAllText($"{name}.config", document.ToString());
                GC.Collect();
                GC.SuppressFinalize(this);
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCApproach();
        }

        ~ConfigWriter()
        {
            Dispose();
        }
    }
}
