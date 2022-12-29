using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Name3.Models
{
    public class ConfigCollect : IDisposable
    {
        private Dictionary<string, ConfigObject> Objects { get; } = new Dictionary<string, ConfigObject>();
        public ConfigCollect()
        { 
            Objects.Clear();
            GC.Collect();
        }

        public ConfigCollect AddObject(ConfigObject config)
        {
            try
            {
                if (Objects.ContainsKey(config.Nood))
                {
                    Objects.Remove(config.Nood);
                    Objects.Add(config.Nood, config);
                    return this;
                }
                Objects.Add(config.Nood, config);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return this;
        }

        public bool WriteTo(XDocument document)
        {
            try
            {
                var list = new XElement("Configs");

                foreach (var nood in Objects)
                {
                    var oElement = nood.Value;
                    var cElement = new XElement("Setter");

                    cElement.SetAttributeValue("Nood", oElement.Nood);
                    cElement.SetAttributeValue("Value", oElement.Value);
                    cElement.SetAttributeValue("Type", oElement.Type);
                    
                    list.Add(cElement);
                }

                Console.WriteLine(list.ToString());
                var queue = document.Element("Configs");
                if (!(queue is null))
                    queue.Remove();

                document.Add(list);

                GC.Collect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReadFrom(XDocument document)
        {
            Objects.Clear();
            GC.Collect();
            try
            {
                var list = document.Element("Configs");
                if (list is null)
                    throw new Exception("Error Find Root");

                foreach (var item in list.Elements())
                {
                    AddObject(new ConfigObject()
                    {
                        Nood = (string)item.Attribute("Nood"),
                        Value = (string)item.Attribute("Value"),
                        Type = (string)item.Attribute("Type")
                    });
                }
                GC.Collect();
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public ConfigObject? GetConfig(string nood)
        {
            if (!Objects.ContainsKey(nood))
                return null;
            return Objects[nood];
        }

        public bool RemoveConfig(string nood)
        {
            if (!Objects.ContainsKey(nood))
                return false;
            Objects.Remove(nood);
            return true;
        }

        [Obsolete]
        public bool ReplaceConfig(string nood, ConfigObject config)
        {
            return false;
        }

        public void Dispose()
        {
            Objects.Clear();
            GC.SuppressFinalize(Objects);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public List<ConfigObject> GetAllObjects()
        {
            return Objects.Values.ToList();
        }

        ~ConfigCollect()
        {
            Dispose();
        }
    }
}
