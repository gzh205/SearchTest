using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    class Demo:Crawler
    {

        public override void pageProcesser(Documents doc)
        {
            writeTxt(doc.getDocument().Text);
            try
            {
                HtmlAgilityPack.HtmlNodeCollection nodes = doc.getUrls();
                foreach (HtmlAgilityPack.HtmlNode i in nodes)
                {
                    writeTxt(i.GetAttributeValue("href", "null"));
                }
            }
            catch(WebException we)
            {
                writeTxt(we.getMessage());
                return;
            }
        }

        public void writeTxt(string dat)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("data.txt",true);
            sw.Write(dat);
            sw.WriteLine();
            sw.Write("---------------------page------------------------");
            sw.WriteLine();
            sw.Close();
        }
    }
}
