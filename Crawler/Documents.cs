using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace Crawler
{
    /// <summary>
    /// 包含常见的对页面操作的方法
    /// </summary>
    public class Documents
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="html">需要打开网页的地址</param>
        public Documents(string url)
        {
            this.doc = new HtmlAgilityPack.HtmlDocument();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            Stream strean = request.GetResponse().GetResponseStream();
            this.doc = new HtmlDocument();
            this.doc.Load(strean);
        }
        /// <summary>
        /// 新添加的构造函数，支持模拟设备
        /// </summary>
        /// <param name="url">需要打开网页的地址</param>
        /// <param name="UA">浏览器标识,这个值为Documents的几个静态成员</param>
        public Documents(string url,string UA)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.UserAgent = UA;
            request.Timeout = 5000;
            request.Accept = "Accept:text/html, application/xhtml+xml, */*";
            Stream strean = request.GetResponse().GetResponseStream();
            this.doc = new HtmlDocument();
            this.doc.Load(strean);
            strean.Dispose();
        }
        /// <summary>
        /// 获取网页文件的,以字符串的形式存放网页的HTML和JavaScript等代码
        /// </summary>
        /// <returns></returns>
        public string geHtmlDoc()
        {
            return this.doc.Text;
        }
        /// <summary>
        /// 获取网页文档的对象
        /// </summary>
        /// <returns></returns>
        public HtmlDocument getDocument()
        {
            return this.doc;
        }
        /// <summary>
        /// 根据xpath匹配网页中的标签
        /// </summary>
        /// <param name="xpath">请输入一个有效的xpath值</param>
        /// <returns>可以像字典一样使用HtmlNodeCollection</returns>
        public HtmlNodeCollection getXPath(string xpath)
        {
            return this.doc.DocumentNode.SelectNodes(xpath);
        }
        /// <summary>
        /// 获取网页中所有的超链接地址
        /// </summary>
        /// <returns>可以像字典一样使用HtmlNodeCollection</returns>
        public HtmlNodeCollection getUrls()
        {
            HtmlNodeCollection res = this.doc.DocumentNode.SelectNodes("//a");
            if (res == null) throw new WebException("element not found");
            return res;
        }
        /// <summary>
        /// 将图片保存到磁盘
        /// </summary>
        /// <param name="filename">图片的文件名称</param>
        /// <param name="img">Image对象</param>
        public static void SaveImage(string filename,Image img)
        {
            FileStream sw = new FileStream(filename, FileMode.OpenOrCreate);
            byte[] dat = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
            sw.Write(dat, 0, dat.Length);
            sw.Close();
        }
        /// <summary>
        /// 从指定url下载图片,循环下载，直到成功为止，每次重试前都会暂停0.1秒
        /// </summary>
        /// <param name="url"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Image GetImage(string url,out int num)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            byte[] pageData = null;
            num = 0;
            while (pageData == null)
            {
                try
                {
                    pageData = wc.DownloadData(url);
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    num++;
                }
            }
            return Image.FromStream(new MemoryStream(pageData));
        }
        /// <summary>
        /// 从指定url下载图片,返回图片的Image对象,下载失败返回null
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image GetImage(string url)
        {
            HttpWebRequest hwq = (HttpWebRequest)WebRequest.Create(url);
            hwq.Method = "Get";
            hwq.UserAgent = Documents.WindowsUA;
            hwq.Accept = "Accept:text/html, application/xhtml+xml, */*";
            return Image.FromStream(hwq.GetResponse().GetResponseStream());
        }
        /// <summary>
        /// 格式化URL,获取其中的域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getDemain(string url)
        {
            MatchCollection mc = Regex.Matches(url, "/(\\w+):\\/\\/([^/:]+)(:\\d*)?([^# ]*)/");
            return mc[2].Value;
        }
        /// <summary>
        /// 将流格式转化为字符串
        /// </summary>
        /// <param name="s">输入一个流对象</param>
        /// <returns>返回的字符串</returns>
        public static string StreamToString(Stream s)
        {
            char[] datc = new char[s.Length];
            byte[] datb = new byte[s.Length];
            s.Read(datb,0,Convert.ToInt32(s.Length));
            for (int i = 0; i < s.Length; i++)
                datc[i] = (char)datb[i];
            return new string(datc);
        }
        /// <summary>
        /// Web页面是否已经记录在字典中
        /// </summary>
        /// <param name="url">指定的页面</param>
        /// <param name="src">字典</param>
        /// <returns></returns>
        public static bool isExist(string url,Dictionary<string,Documents> src)
        {
            bool result = false;
            foreach (KeyValuePair<string,Documents> pair in src)
            {
                if (pair.Key == url)
                    result = true;
            }
            return result;
        }
        /// <summary>
        /// 将地址进行格式化,即将href的内容补全
        /// </summary>
        /// <param name="url">主机名称</param>
        /// <param name="href">跳转链接</param>
        /// <returns></returns>
        public static string urlFormat(string url,string href)
        {
            if (url == null || href == null)
                return null;
            if (href == "#")
                return url;
            if (href.Contains("http") || href.Contains("https"))
                return href;
            string output = "";
            string[] result = url.Split('/');
            int len = result.Length;
            string[] format = href.Split('/');
            List<string> tmp = new List<string>();
            for(int i = 0; i < format.Length; i++)
            {
                if (format[i].Trim().Length == 0)
                    continue;
                else if (format[i].Trim() == ".")
                    continue;
                else if (format[i].Trim() == "..")
                    len--;
                else
                    tmp.Add(format[i]);
            }
            for(int i = 0; i < len - 1; i++)
            {
                output += result[i] + "/";
            }
            foreach(string i in tmp)
            {
                output += i + "/";
            }
            return output.Substring(0, output.Length - 1);
        }
        ///<summary>
        ///Windows平台的浏览器标识
        ///</summary>
        public static string WindowsUA = "User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3100.0 Safari/537.36";
        ///<summary>
        ///iphone手机的浏览器标识
        ///</summary>
        public static string IPhoneUA = "User-Agent:Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1";
        ///<summary>
        ///android手机的浏览器标识
        ///</summary>
        public static string AndroidUA = "User-Agent:Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Mobile Safari/537.36";
        ///<summary>
        ///iPad的浏览器标识
        ///</summary>
        public static string IPadUA = "User-Agent:Mozilla/5.0 (iPad; CPU OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1";
        /// <summary>
        /// 网页文档
        /// </summary>
        private HtmlDocument doc { get; set; }
    }
}
