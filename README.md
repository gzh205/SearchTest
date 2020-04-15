# SearchTest
一个爬虫的例子  
`Crawler是爬虫文件`  
`UnitTestPrj是单元测试`  
## 爬虫使用方式：  
创建一个Crawler的子类，并实现pageProsser方法（在该方法中添加爬虫的爬取规则），最后调用Crawler的静态成员run方法即可运行。  
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
## 最近一次更新的内容   
1.之前是深度优先搜索，现在为了能让爬虫够执行多线程任务，将其改为了广度优先搜索  
2.设置浏览器标识是为了让爬虫能够模拟手机或者iPad设备  
## 该爬虫运行的原理  
该爬虫会创建多个线程并执行同一个函数。每个线程都会自动的从爬取队列中获取一个url，随后打开对应网页并且运行PageProsser算法，之后遍历网页的HTML代码找出所有的超链接，然后自地将页面内部的超链接保存进爬取队列中，这样便完成了一次函数的运行。该程序使用了生产者-消费者的线程管理模式防止在多线程的运行过程中产生线程间的协调问题。并且在打开网页是可以设置UserAgent属性让爬虫模拟成手机、平板电脑等设备，以此来爬取一些只能允许移动设备连接的网站。  
## 使用的第三方程序包  
由于该爬虫需要支持xPath对于HTML文档的快速查询，因此引用了HtmlAgilityPack这个第三方包。  
