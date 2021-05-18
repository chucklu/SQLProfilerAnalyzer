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
        public Dictionary<int,string> Read(string path)
        {
            RemoveXmlNamespaces(path);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var element = XElement.Load(path);//root element TraceData
            var eventsElement = element.Element("Events");
            if (eventsElement == null)
            {
                throw new Exception("Can not find Events element.");
            }
            var eventData = eventsElement.Elements("Event");
            var rpcStartingData = eventData.Where(x => x.Attribute("name")?.Value == "RPC:Starting");
            int i = 0;
            foreach (var item in rpcStartingData)
            {
                i++;
                var objectName = GetObjectName(item);
                dic.Add(i, objectName);
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

        public string GetObjectName(XElement element)
        {
            var columns = element.Elements("Column");
            var objectNameElement = columns.First(x => x.Attribute("name")?.Value == "ObjectName");
            return objectNameElement.Value;
        }
    }
}
