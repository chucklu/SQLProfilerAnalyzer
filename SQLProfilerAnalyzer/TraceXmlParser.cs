using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SQLProfilerAnalyzer
{
    public class TraceXmlParser
    {
        public Dictionary<int, CustomEvent> Read(string path)
        {
            RemoveXmlNamespaces(path);
            Dictionary<int, CustomEvent> dic = new Dictionary<int, CustomEvent>();
            var element = XElement.Load(path);//root element TraceData
            var eventsElement = element.Element("Events");
            if (eventsElement == null)
            {
                throw new Exception("Can not find Events element.");
            }
            var eventData = eventsElement.Elements("Event");
            var rpcStartingData = eventData.Where(x => x.Attribute("name")?.Value == "RPC:Starting");
            int i = 0;
            var skipList = new List<string>
            {
                "sp_reset_connection"
            };
            foreach (var item in rpcStartingData)
            {
                CustomEvent customEvent = new CustomEvent();
                var objectName = GetColumnValue(item, nameof(customEvent.ObjectName));
                var textData = GetColumnValue(item, nameof(customEvent.TextData));
                customEvent.ObjectName = objectName;
                customEvent.TextData = textData;
                if (!skipList.Contains(objectName))
                {
                    i++;
                    dic.Add(i, customEvent);
                }
            }

            return dic;
        }


        private void RemoveXmlNamespaces(string path)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            string xml = xmlDocument.InnerXml;
            string strXMLPattern = @"xmlns(:\w+)?=""([^""]+)""|xsi(:\w+)?=""([^""]+)""";
            xmlDocument.InnerXml = Regex.Replace(xml, strXMLPattern, "");
            xmlDocument.Save(path);
        }

        public string GetColumnValue(XElement element,string name)
        {
            var columns = element.Elements("Column");
            var objectNameElement = columns.First(x => x.Attribute("name")?.Value == name);
            return objectNameElement.Value;
        }
    }
}
