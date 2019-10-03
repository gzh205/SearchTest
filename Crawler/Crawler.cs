using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Crawler
{
    abstract class Crawler
    {
        public Crawler()
        {
            this.page = new Dictionary<string, Documents>();
            this.client = new WebClient();
            this.result = new Dictionary<string, string>();
            this.Url = new List<string>();
        }
        public Documents getPage(string url)
        {
            return this.page[url];
        }
        public Crawler addUrl(string url)
        {
            this.Url.Add(url);
            return this;
        }
        public string getRootUrl()
        {
            return this.rootUrl;
        }
        public Crawler setUrl(string url)
        {
            this.rootUrl = url;
            return this;
        }
        public Crawler setDepth(int depth)
        {
            this.depth = depth;
            return this;
        }
        public void init()
        {
            if (rootUrl == null) throw new Exception("url不能为空");
            string data = Encoding.GetEncoding("utf-8").GetString(client.DownloadData(rootUrl));
            page.Add(rootUrl, new Documents(data));
            this.Url.Add(rootUrl);
        }
        public string getUrl()
        {
            return this.Url.First();
        }
        public Dictionary<string,string> getResult()
        {
            return this.result;
        }
        protected HtmlAgilityPack.HtmlNodeCollection getDocumentUrls()
        {
            return this.page[this.getUrl()].getUrls();
        }
        public void find(string parent,HtmlNodeCollection nodes,int dep)
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
                    page.Add(href,new Documents(href));
                    this.pageProcesser(page[href]);
                    find(href,page[href].getUrls(), dep + 1);
                }
            }
        }
        public void loadPage(string url)
        {
            if (url == null) throw new Exception("loadPage加载页面失败，URL为空");
            
        }
        protected Dictionary<string, Documents> page;
        protected List<string> Url;
        protected string rootUrl;
        protected WebClient client;
        protected int depth;
        public static void run(Crawler c)
        {
            if (c.getRootUrl() == null) throw new Exception("先调用setUrl设置初始地址");
            c.init();
            c.find(c.getRootUrl(),c.page[c.getRootUrl()].getUrls(),0);
        }
        public abstract void pageProcesser(Documents doc);
        protected Dictionary<string,string> result;
    }
}
