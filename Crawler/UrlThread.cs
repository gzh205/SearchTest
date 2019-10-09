using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crawler
{
    public class UrlThread
    {
        public delegate void delegateRun();
        public event delegateRun eventrun;
        /// <summary>
        /// 构造函数,此类无需使用者自己构造,也无需使用者调用
        /// </summary>
        /// <param name="core"></param>
        public UrlThread(CrawlerCore core)
        {
            this.core = core;
            if (core == null)
                throw new WebException("Crawler的子类没有被初始化");
            threads = new Thread[core.ThreadNum];
            eventrun += new delegateRun(newThread);
        }
        /// <summary>
        /// 线程中的一个原子操作
        /// </summary>
        public void atomOps(int num)
        {
            Documents doc;
            Uri u = core.urls.nextUri();
            if (u == null)
                return;
            else
            {
                if (core.UA == null)
                    doc = new Documents(u);
                else
                    doc = new Documents(u, core.UA);
            }
            core.pageProcesser(doc);
            if (core.urls.objName == "uri" && core.depth > 1)
            {
                string[] dat = doc.getUrls();
                foreach (string t in dat)
                {
                    Uri tmp = Documents.urlFormat(u, t);
                    if (tmp.Host == u.Host)
                        core.urls.addUrls(tmp);
                }
            }
            eventrun();
        }
        public void start()
        {
            for (int i = 0; i < core.depth;)
            {
                while (true)
                {
                    int num = getFinished();
                    if (num == -1)
                        continue;
                    threads[num] = new Thread(atomOps(num));
                    if (core.urls.isEnd() == true)
                    {
                        i++;
                        core.urls.setMaxSize();
                        break;
                    }
                }
            }
        }
        public void newThread()
        {

        }
        public int getFinished()
        {
            for (int i = 0; i < isFinished.Length; i++)
                if (isFinished[i] == true)
                    return i;
            return -1;
        }
        /// <summary>
        /// 线程队列
        /// </summary>
        private Thread[] threads { set; get; }
        /// <summary>
        /// 线程是否已经结束,如果线程执行结束则设为true
        /// </summary>
        private bool[] isFinished { set; get; }
        /// <summary>
        /// 用于操作CrawlerCore中的内容
        /// </summary>
        private CrawlerCore core{ get; set; }
    }
}
