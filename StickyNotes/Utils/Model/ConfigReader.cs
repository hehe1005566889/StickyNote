using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Name3.Models
{
    class ConfigReader : IDisposable
    {
        public static ConfigReader CreateReader()
        {
            return new ConfigReader();
        }
        public ConfigCollect ReadDocument(
               string name
        ) {
            try
            {
                XDocument document = XDocument.Load($"{name}.config");
                var cfgCollect = new ConfigCollect();
                if (!cfgCollect.ReadFrom(document))
                    throw new Exception("Error Read Config");
                GC.SuppressFinalize(document);
                GC.Collect();
                return cfgCollect;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~ConfigReader()
        {
            Dispose();
        }

    }
}
