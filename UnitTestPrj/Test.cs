using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestPrj
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void testUA()
        {
            Crawler.Documents doc = new Crawler.Documents("https://nbx2.kongzhong.com/r/jw/180319110923", Crawler.Documents.AndroidUA);//使用安卓手机访问百度移动网站
            writeIntoFile("testua.html",doc.geHtmlDoc());
        }
        [TestMethod]
        public void testDownloadImage()
        {
            int n;
            Crawler.Documents.SaveImage("tmp.jpg",Crawler.Documents.GetImage("http://pm.weigox.com:8008/uploadfile/gx02/190915/bb19.jpg", out n));
        }
        /// <summary>
        /// 将字符串写进文件中
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="text">待写入的文本</param>
        public void writeIntoFile(string filename,string text)
        {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            fs.Seek(0, SeekOrigin.Begin);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(text);
            sw.Close();
        }
    }
}
