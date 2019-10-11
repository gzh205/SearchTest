# SearchTest
一个爬虫的例子  
`Crawler是爬虫文件`  
`UnitTestPrj是单元测试`  
爬虫使用方式：创建一个Crawler的子类，并实现pageProsser方法（在该方法中添加爬虫的爬取规则），最后调用Crawler的静态成员run方法即可运行。  
爬取规则示例代码:  
```
class Demo:CrawlerCore
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
```
  
之前是深度优先搜索，现在为了能让爬虫够执行多线程任务，将其改为了广度优先搜索  
浏览器标识是为了让爬虫能够模拟手机或者iPad设备
