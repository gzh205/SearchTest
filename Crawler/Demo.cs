using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crawler;
namespace Crawler
{
    /// <summary>
    /// 含有爬取规则的子类
    /// 此类仅仅作为一个例子
    /// </summary>
    public class Demo : CrawlerCore
    {
        /// <summary>
        /// 实现了CrawlerCore的pageProcesser方法
        /// </summary>
        /// <param name="doc"></param>
        protected override void pageProcesser(Documents doc)
        {
            writeTxt(doc.getDocument().Encoding.GetString(Encoding.Default.GetBytes(doc.getDocument().Text)));
            try
            {
                HtmlAgilityPack.HtmlNodeCollection nodes = doc.getUrls();
                if (nodes == null) writeTxt("ndoes为空");
                else
                {
                    foreach (HtmlAgilityPack.HtmlNode i in nodes)
                    {
                        writeTxt(i.GetAttributeValue("href",null));
                    }
                }
            }
            catch(WebException we)
            {
                writeTxt(we.getMessage());
                return;
            }
        }
        /// <summary>
        /// 将dat字符串写入文本中
        /// </summary>
        /// <param name="dat"></param>
        public void writeTxt(string dat)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("d:\\data.txt",true);
            sw.Write(dat);
            sw.WriteLine();
            sw.Write("---------------------page------------------------");
            sw.WriteLine();
            sw.Close();
        }
    }
}
