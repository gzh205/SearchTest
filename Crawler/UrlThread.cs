using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class UrlThread
    {
        /// <summary>
        /// 构造函数,此类无需使用者自己构造,也无需使用者调用
        /// </summary>
        /// <param name="core"></param>
        public UrlThread(CrawlerCore core)
        {
            this.core = core;
            if (core == null)
                throw new WebException("Crawler的子类没有被初始化");           
        }
        public void atomOps()
        {
            //打开网页
            Documents doc;
            Uri u = core.urls.nextUri();
            if (u == null)
                return;
            //设置User-Agent进行设备的模拟
            else
            {
                if (core.UA == null)
                    doc = new Documents(u);
                else
                    doc = new Documents(u, core.UA);
            }
            //调用PageProcessor
            core.pageProcesser(doc);
            //添加当前页面中找到的所有超链接
            if (core.urls.objName == "uri" && core.depth > 1)
            {
                if (!core.urls.isEnd())
                {
                    string[] dat = doc.getUrls();
                    if(dat != null)
                        foreach (string t in dat)
                        {
                            Uri tmp = Documents.urlFormat(u, t);
                            if (tmp.Host == u.Host)
                                core.urls.addUrls(tmp);
                        }
                }
            }
        }
        public void start(int num)
        {
            ThreadPool.SetMinThreads(num,)
            //遍历uri列表的每一层
            for (int i = 0; i < core.depth; i++)
            {
                //遍历某一层的所有页面
                do
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(atomOps));
                    Thread t = new Thread(atomOps);                   
                    t.Start();
                } while (core.urls.isEnd());
            }
        }
        public int getFinished()
        {
            for (int i = 0; i < isFinished.Length; i++)
                if (isFinished[i] == true)
                    return i;
            return -1;
        }

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
