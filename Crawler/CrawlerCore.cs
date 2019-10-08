using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Crawler
{
    /// <summary>
    /// 爬虫主类
    /// </summary>
    public abstract class CrawlerCore
    {
        /// <summary>
        /// 无需手动初始化该类的对象
        /// </summary>
        public CrawlerCore()
        {
            this.page = new Dictionary<string, Documents>();
            this.result = new Dictionary<string, string>();
            this.Url = new List<string>();
            this.depth = 1;
        }
        /// <summary>
        /// 设置浏览器标识,爬虫会模拟字符串所指定的设备运行
        /// 注意:不设置该值或将该值置为空也能运行爬虫
        /// </summary>
        /// <param name="ua">可以使用Documents的几个静态成员,也可以自己填写该值</param>
        /// <returns></returns>
        public CrawlerCore setUA(string ua)
        {
            this.ua = ua;
            return this;
        }
        /// <summary>
        /// 添加一个爬取的地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public CrawlerCore addUrl(string url)
        {
            this.Url.Add(url);
            return this;
        }
        /// <summary>
        /// 设置爬虫第一次爬取的地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public CrawlerCore setUrl(string url)
        {
            this.rootUrl = url;
            return this;
        }
        /// <summary>
        /// 设置爬虫的爬取深度,因为爬虫采用深度优先搜索的算法
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public CrawlerCore setDepth(int depth)
        {
            this.depth = depth;
            return this;
        }
        /// <summary>
        /// 初始化爬虫
        /// </summary>
        protected void init()
        {
            if (rootUrl == null) throw new Exception("url不能为空");
            page.Add(rootUrl, (ua == null ? new Documents(rootUrl) : new Documents(rootUrl, ua)));
            this.Url.Add(rootUrl);
        }
        /// <summary>
        /// 获取url列表中的第一个元素
        /// </summary>
        /// <returns></returns>
        protected string getUrl()
        {
            return this.Url.First();
        }
        /// <summary>
        /// 返回所有爬到的网页
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string,string> getResult()
        {
            return this.result;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        protected HtmlAgilityPack.HtmlNodeCollection getDocumentUrls()
        {
            return this.page[this.getUrl()].getUrls();
        }
        /// <summary>
        /// 爬虫的查找算法,由爬虫的run方法自己调用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodes"></param>
        /// <param name="dep"></param>
        protected void find(string parent,HtmlNodeCollection nodes,int dep)
        {
            if (dep >= depth)
            {
                return;
            }
            for(int i=0;i<nodes.Count;i++)
            {
                string href = nodes[i].GetAttributeValue("href", null);
                if (href == "" || href == null)
                    continue;
                href = Documents.urlFormat(parent, href);
                if (!Documents.isExist(href, page))
                {
                    page.Add(href, (ua == null ? new Documents(href) : new Documents(href, ua)));
                    this.pageProcesser(page[href]);
                    try
                    {
                        find(href, page[href].getUrls(), dep + 1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, Documents> page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected List<string> Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected string rootUrl { get; set; }
        /// <summary>
        /// 爬取深度
        /// </summary>
        protected int depth { get; set; }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        protected string ua { get; set; }
        /// <summary>
        /// 通过下面的方法运行爬虫:
        /// Crawler.run("自己构造的子类对象");
        /// 该方法会自动打开网页中的所有超链接,适用于对整个网站进行搜索
        /// </summary>
        /// <param name="c">自己构造的子类对象(必须实现pageProcesser)</param>
        public static void run(CrawlerCore c)
        {
            if (c.rootUrl == null) throw new Exception("先调用setUrl设置初始地址");
            c.init();
            if (c.depth > 1)
            {
                try
                {
                    c.find(c.rootUrl, c.page[c.rootUrl].getUrls(), 0);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (c.depth == 1)
            {
                c.pageProcesser(c.page[c.rootUrl]);
            }
            else throw new Exception("depth值不合法");
        }
        /// <summary>
        /// 依次打开并处理urls中的链接,使用此方法时设置depth是无效的
        /// 该方法会依次打开urls内的网页，适用于对若干网页进行查找
        /// </summary>
        /// <param name="c">自己构造的子类对象(必须实现pageProcesser)</param>
        /// <param name="urls">链接的内容</param>
        public static void run(CrawlerCore c,string[] urls)
        {
            foreach(string url in urls)
            {
                try
                {
                    c.pageProcesser((c.ua == null) ? new Documents(url) : new Documents(url, c.ua));
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }
        /// <summary>
        /// 页面处理方法，需要使用者自己实现
        /// </summary>
        /// <param name="doc"></param>
        protected abstract void pageProcesser(Documents doc);
        /// <summary>
        /// 页面中所有的超链接
        /// </summary>
        protected Dictionary<string, string> result { get; set; }
    }
}
