using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Crawler;
using System.Threading;

namespace UnitTestPrj
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void testUri()
        {
            Uri u = new Uri("http://www.baidu.com/321/432/11.html");
            Uri v = new Uri(u,"http://www.321.com/1/4.html");
            string tmp = v.ToString();
            tmp = "123";
        }
        [TestMethod]
        public void testMultiThread()
        {
            CrawlerCore.run(new Demo("https://zhidao.baidu.com/question/225977538.html").setDepth(3).setThreadNum(8));
        }
        [TestMethod]
        public void testThreads()
        {
            ThreadPool.SetMinThreads(5, 5);
            ThreadPool.SetMaxThreads(10, 10);
            for(int i=0;i<100;i++)
                ThreadPool.QueueUserWorkItem()
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
