using System;
using System.Collections;
using System.Collections.Generic;
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
            this.uriQueue = new Queue<Uri>();
        }
        public void atomOps(object state)
        {
            //打开网页
            Documents doc;
            Uri u = (Uri)state;
            if (u == null)
                return;
            //设置User-Agent
            else
            {
                if (core.UA == null)
                    doc = new Documents(u);//不使用User-Agent
                else
                    doc = new Documents(u, core.UA);//使用User-Agent,core.UA就是User-Agent
            }
            //调用PageProcessor,该函数由core对象的子类实现
            core.pageProcesser(doc);
            //添加当前页面中找到的所有超链接
            if (core.urls.objName == "uri" && core.depth > 1)//如果搜索的深度小于等于1,则没有必要执行这一步
            {
                string[] dat = doc.getUrls();//查找页面中的全部超链接
                if(dat != null)//如果页面确实没有任何超链接,则不执行下面循环
                    foreach (string t in dat)
                    {
                        Uri tmp = Documents.urlFormat(u, t);//将相对路径的超链接径补全,成为一个完整的链接
                        if (tmp.Host == u.Host)//只有两个超链接的主机名称完全相同才能够存储,如果是跨站的超链接,则忽略
                            core.urls.addUrls(tmp);//保存页面中的超链接
                    }
            }
        }
        public void start(int num)
        {                    
            //遍历uri列表的每一层
            for (int i = 0; i < core.depth; i++)
            {
                Task[] t = new Task[num];
                //遍历某一层的所有页面
                do
                {
                    uriQueue.Enqueue(core.urls.nextUri());//将List中的数据存放进Queue中,每取一次,用于遍历List的指针自动+1
                } while (core.urls.isEnd());//判断List是否遍历完毕
                //创建若干线程
                for (int j = 0; j < num; j++)
                {
                    t[j] = new Task(ThreadProc);
                    t[j].Start();//执行线程
                }
                //先阻塞主线程,等所有支线程全部执行完毕,继续运行
                Task.WaitAll(t);
                //将获取到的超链接保存进之前的List中,下一次循环时继续对新的数据操作
                core.urls.save();
            }
        }
        public void ThreadProc()
        {
            while(uriQueue.Count > 0)//从队列中取出数据
            {
                atomOps(uriQueue.Dequeue());//调用一个原子操作
            }
        }
        public int getFinished()
        {
            for (int i = 0; i < isFinished.Length; i++)
                if (isFinished[i] == true)
                    return i;
            return -1;
        }
        //private Semaphore mainThread;
        private Queue<Uri> uriQueue;
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
