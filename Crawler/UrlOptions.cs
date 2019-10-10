using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crawler
{
    public abstract class UrlOptions
    {
        public UrlOptions(Uri uri)
        {
            uris = new List<Uri>();
            cursor = 0;
            uris.Add(uri);
        }
        public UrlOptions(Uri[] uriArr)
        {
            this.uris = new List<Uri>(uriArr);
            cursor = 0;
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
            if (++cursor <= uris.Count)
                return uris[cursor];
            else
                return null;
        }
        /// <summary>
        /// 判断某一层是否结束
        /// </summary>
        /// <returns></returns>
        public bool isEnd()
        {
            if (cursor >= MaxSize)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 将MaxSize设为新的uris的大小
        /// </summary>
        public void setMaxSize()
        {
            this.MaxSize = uris.Count;
        }
        /// <summary>
        /// 获取当前uri对应的主机名称
        /// </summary>
        /// <returns></returns>
        public string getHost()
        {
            return this.uris[this.cursor].Host;
        }
        /// <summary>
        /// 添加一些uri
        /// </summary>
        public abstract void addUrls(Uri uri);
        /// <summary>
        /// 返回对象所属类的名称
        /// </summary>
        public string objName { get; protected set; }
        /// <summary>
        /// 网页的uri列表
        /// </summary>
        protected List<Uri> uris { get; }
        /// <summary>
        /// uris的指针,最初为0表示指向第一个元素
        /// </summary>
        protected int cursor { get; set; }
        /// <summary>
        /// 每一层的最大大小
        /// </summary>
        protected int MaxSize { get; set; }
    }
}
