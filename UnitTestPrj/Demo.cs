using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler;
using System.IO;

namespace UnitTestPrj
{
    class Demo : CrawlerCore
    {
        public Demo(string link):base(link)
        {
        }
        public Demo(string[] urls):base(urls)
        {
        }
        public override void pageProcesser(Documents doc)
        {
            sw.WriteLine(doc.geHtmlDoc());
            sw.Write("------------------------------page-------------------------------------");
            sw.WriteLine();
        }
        public static StreamWriter sw = new StreamWriter(new FileStream("temp.txt", FileMode.Append));
    }
}
