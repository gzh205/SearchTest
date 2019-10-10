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
            FileStream fs = new FileStream("temp.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(doc.geHtmlDoc());
            sw.WriteLine();
            sw.Write("------------------------------page-------------------------------------");
            sw.WriteLine();
            sw.Close();
            sw.Dispose();
            fs.Close();
        }
    }
}
