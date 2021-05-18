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
        public XElement RootElement { get; set; }

        public Dictionary<int, CustomEvent> EventData = new Dictionary<int, CustomEvent>();

        public TraceXmlParser(string path)
        {
            RemoveXmlNamespaces(path);
            RootElement = XElement.Load(path);//root element TraceData
        }

        public Dictionary<int, CustomEvent> Read()
        {
            var eventsElement = RootElement.Element("Events");
            if (eventsElement == null)
            {
                throw new Exception("Can not find Events element.");
            }
            var eventData = eventsElement.Elements("Event");

            var eventList = new List<string>()
            {
                "RPC:Starting",
                "SQL:BatchStarting"
            };

            var tempEventData = eventData.Where(x => eventList.Contains(x.Attribute("name")?.Value));
            int i = 0;
            var skipList = new List<string>
            {
                "sp_reset_connection"
            };
            foreach (var item in tempEventData)
            {
                CustomEvent customEvent = new CustomEvent();
                customEvent.EventName = item.Attribute("name")?.Value;
                var objectName = GetColumnValue(item, nameof(customEvent.ObjectName));
                var textData = GetColumnValue(item, nameof(customEvent.TextData));
                customEvent.ObjectName = objectName;
                customEvent.TextData = textData;
                if (!skipList.Contains(objectName))
                {
                    i++;
                    EventData.Add(i, customEvent);
                }
            }

            return EventData;
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
            var objectNameElement = columns.FirstOrDefault(x => x.Attribute("name")?.Value == name);
            return objectNameElement?.Value;
        }
    }
}
