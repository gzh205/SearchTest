using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Crawler
{
    class Documents
    {
        private HtmlDocument doc;
        public Documents(string html)
        {
            this.doc = new HtmlAgilityPack.HtmlDocument();
            this.doc.LoadHtml(html);
        }
        public HtmlDocument getDocument()
        {
            return this.doc;
        }
        public HtmlNodeCollection getUrls()
        {
            HtmlNodeCollection res = this.doc.DocumentNode.SelectNodes("//a");
            if (res == null) throw new WebException("element not found");
            return res;
        }
        public static string getDemain(string url)
        {
            MatchCollection mc = Regex.Matches(url, "/(\\w+):\\/\\/([^/:]+)(:\\d*)?([^# ]*)/");
            return mc[2].Value;
        }
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
        public static string urlFormat(string url,string href)
        {
            if (url == null || href == null)
                return null;
            if (href == "#")
                return url;
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
            for(int i = 0; i < len; i++)
            {
                output += result[i] + "/";
            }
            foreach(string i in tmp)
            {
                output += i + "/";
            }
            return output.Substring(0, output.Length - 1);
        }
    }
}
