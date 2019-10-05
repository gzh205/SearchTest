# SearchTest
一个爬虫的例子  
Form1是测试文件  
爬虫使用方式：创建一个Crawler的子类，并实现pageProsser方法（在该方法中添加爬虫的爬取规则），最后调用Crawler的静态成员run方法即可运行。  
爬取规则示例代码:  
`
class Demo:Crawler
{
    //爬虫打开每一个页面都会调用一次该函数
    protected override void pageProcesser(Documents doc)
    {
        writeTxt(doc.getDocument().Text);
        try
        {
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.getUrls();//获取页面中所有的超链接
            if (nodes == null) writeTxt("ndoes为空");
            else  
            {  
                //遍历这些链接
                foreach (HtmlAgilityPack.HtmlNode i in nodes)  
                {  
                    writeTxt(i.GetAttributeValue("href",null));  
                }  
            }  
        }  
        catch(WebException we)  
        {  
            writeTxt(we.getMessage());  
            return;  
        }  
    }  
    public void writeTxt(string dat)  
    {  
        System.IO.StreamWriter sw = new System.IO.StreamWriter("data.txt",true);  
        sw.Write(dat);  
        sw.Close();  
    }  
}
`
调用爬虫示例代码:  
Crawler.run(new Demo().new Demo().setUrl("网址").setDepth(深度).setUA("浏览器标识"))  
  
其中深度就是爬虫之心深度优先搜索时打开页面的深度,因为爬虫不能无限制地打开新的页面  
浏览器标识是为了让爬虫能够模拟手机或者iPad设备
