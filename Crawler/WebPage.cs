using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace Crawler
{
    class WebPage
    {
        private XmlDocument data;
        public XmlDocument getData() { return this.data; }
        private string url;
        public string getUrl() { return this.url; }
        public WebPage(string url,string data)
        {
            this.url = url;
            this.data = new XmlDocument();
            this.data.Load(url);
        }
        string getElement()
        {
            return "";
        }
        public Dictionary<int,string> getElements(string xPath)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            XmlNodeList list = data.SelectNodes(xPath);
            int i = 0;
            foreach(XmlNode n in list)
            {
                result.Add(i++, n.InnerXml);
            }
            return result;
        }
    }
}
