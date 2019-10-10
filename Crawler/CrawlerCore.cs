using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    /// <summary>
    /// 爬虫主类,不能直接使用该类,因为没有实现PageProcessor
    /// </summary>
    public abstract class CrawlerCore
    {
        ///<summary>
        ///Windows平台的浏览器标识
        ///</summary>
        public static string WindowsUA { get; } = "User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3100.0 Safari/537.36";
        ///<summary>
        ///iphone手机的浏览器标识
        ///</summary>
        public static string IPhoneUA { get; } = "User-Agent:Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1";
        ///<summary>
        ///android手机的浏览器标识
        ///</summary>
        public static string AndroidUA { get; } = "User-Agent:Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Mobile Safari/537.36";
        ///<summary>
        ///iPad的浏览器标识
        ///</summary>
        public static string IPadUA { get; } = "User-Agent:Mozilla/5.0 (iPad; CPU OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1";
        /// <summary>
        /// 浏览器标识,配合4个静态变量使用
        /// </summary>
        public string UA {  get; private set; }
        /// <summary>
        /// 开启的线程数量
        /// </summary>
        public int ThreadNum { get; private set; }
        /// <summary>
        /// 使用自动打开超链接的模式时,打开链接的深度
        /// </summary>
        public int depth { get; private set; }
        /// <summary>
        /// 存放网站的uri信息
        /// </summary>
        public UrlOptions urls { get; private set; }
        /// <summary>
        /// 多线程读取网页
        /// </summary>
        private UrlThread threads { set; get; }
        public CrawlerCore(string[] uris)
        {
            this.urls = new UrlOptionArrImpl(str2uri(uris));
            init();
        }
        public CrawlerCore(string link)
        {
            this.urls = new UrlOptionUriImpl(new Uri(link));
            init();
        }
        public void init()
        {
            ThreadNum = 1;
            this.depth = 1;
            threads = new UrlThread(this);
            this.urls.setMaxSize();
        }
        public CrawlerCore setDepth(int num)
        {
            this.depth = num;
            return this;
        }
        public CrawlerCore setUA(string ua)
        {
            this.UA = ua;
            return this;
        }
        public CrawlerCore setThreadNum(int num)
        {
            this.ThreadNum = num;
            return this;
        }
        public abstract void pageProcesser(Documents doc);
        public static void run(CrawlerCore c)
        {
            if (c == null)
                return;
            else if (c.depth == 0 || c.ThreadNum == 0)
                return;
            else
                c.threads.start(c.ThreadNum);
        }
        public static Uri[] str2uri(string[] uri)
        {
            if (uri == null)
                return null;
            Uri[] result = new Uri[uri.Length];
            for(int i = 0; i < uri.Length; i++)
            {
                result[i] = new Uri(uri[i]);
            }
            return result;
        }
    }
}
