using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crawler
{
    public abstract class UrlOptions
    {
        /// <summary>
        /// 构造函数,该类无需使用者调用
        /// </summary>
        /// <param name="uri"></param>
        public UrlOptions(Uri uri)
        {
            uris = new List<Uri>();
            saves = new List<Uri>();
            tmp = new List<Uri>();
            cursor = -1;
            uris.Add(uri);
        }
        /// <summary>
        /// 构造函数,该类无需使用者调用
        /// </summary>
        /// <param name="uriArr"></param>
        public UrlOptions(Uri[] uriArr)
        {
            this.uris = new List<Uri>(uriArr);
            cursor = -1;
        }
        /// <summary>
        /// 获取当前的uri
        /// </summary>
        /// <returns></returns>
        public Uri thisUri()
        {
            return this.uris[this.cursor];
        }
        /// <summary>
        /// 将指针向下移动一格并获取下一个uri
        /// </summary>
        /// <returns></returns>
        public Uri nextUri()
        {
            if (cursor + 1 < uris.Count)
                return uris[++cursor];
            else
                return null;
        }
        /// <summary>
        /// 保存uri
        /// </summary>
        public void save()
        {
            this.saves.AddRange(this.uris);
            this.uris = this.tmp;
            this.tmp = new List<Uri>();
        }
        /// <summary>
        /// 判断某一层是否结束
        /// </summary>
        /// <returns></returns>
        public bool isEnd()
        {
            if (cursor >= uris.Count)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 获取当前uri对应的主机名称
        /// </summary>
        /// <returns></returns>
        public string getHost()
        {
            return this.uris[this.cursor].Host;
        }
        public int getUrisLength()
        {
            return this.uris.Count;
        }
        /// <summary>
        /// 添加一些链接,由子类实现
        /// </summary>
        public abstract void addUrls(Uri uri);
        /// <summary>
        /// 返回对象所属类的名称
        /// </summary>
        public string objName { get; protected set; }
        /// <summary>
        /// 网页链接的列表
        /// </summary>
        public List<Uri> uris { get; private set; }
        /// <summary>
        /// 存储所有找到的网页链接
        /// </summary>
        public List<Uri> saves { get; private set; }
        /// <summary>
        /// 存储本次深度优先搜索查找到的超链接
        /// </summary>
        public List<Uri> tmp { get; private set; }
        /// <summary>
        /// uris的指针,最初为-1表示没有任何元素
        /// </summary>
        protected int cursor { get; set; }
    }
}
