using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Crawler
{
    public abstract class Crawler
    {
        /// <summary>
        /// 无需手动初始化该类的对象
        /// </summary>
        public Crawler()
        {
            this.page = new Dictionary<string, Documents>();
            this.client = new WebClient();
            this.result = new Dictionary<string, string>();
            this.Url = new List<string>();
        }
        /// <summary>
        /// 获取指定url对应的Document页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected Documents getPage(string url)
        {
            return this.page[url];
        }
        /// <summary>
        /// 设置浏览器标识,爬虫会模拟字符串所指定的设备运行
        /// 注意:不设置该值或将该值置为空也能运行爬虫
        /// </summary>
        /// <param name="ua">可以使用Documents的几个静态成员,也可以自己填写该值</param>
        /// <returns></returns>
        public Crawler setUA(string ua)
        {
            this.ua = ua;
            return this;
        }
        /// <summary>
        /// 添加一个爬取的地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Crawler addUrl(string url)
        {
            this.Url.Add(url);
            return this;
        }
        /// <summary>
        /// 获取第一个爬取的地址
        /// </summary>
        /// <returns></returns>
        protected string getRootUrl()
        {
            return this.rootUrl;
        }
        /// <summary>
        /// 设置爬虫第一次爬取的地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Crawler setUrl(string url)
        {
            this.rootUrl = url;
            return this;
        }
        /// <summary>
        /// 设置爬虫的爬取深度,因为爬虫采用深度优先搜索的算法
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Crawler setDepth(int depth)
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
            string data = Encoding.GetEncoding("utf-8").GetString(client.DownloadData(rootUrl));
            page.Add(rootUrl, (ua == null ? new Documents(data) : new Documents(data, ua)));
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
            if (dep >= 5)
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
        /// 我也记不清这个方法是干什么的了,可能是弃用的方法
        /// </summary>
        /// <param name="url"></param>
        protected void loadPage(string url)
        {
            if (url == null) throw new Exception("loadPage加载页面失败，URL为空");
            
        }
        protected Dictionary<string, Documents> page;
        protected List<string> Url;
        protected string rootUrl;
        protected WebClient client;
        protected int depth;
        protected string ua;
        /// <summary>
        /// 通过下面的方法运行爬虫:
        /// Crawler.run("自己构造的子类对象");
        /// </summary>
        /// <param name="c">自己构造的子类对象(必须实现pageProcesser)</param>
        public static void run(Crawler c)
        {
            if (c.getRootUrl() == null) throw new Exception("先调用setUrl设置初始地址");
            c.init();
            c.find(c.getRootUrl(),c.page[c.getRootUrl()].getUrls(),0);
        }
        protected abstract void pageProcesser(Documents doc);
        protected Dictionary<string,string> result;
    }
}
