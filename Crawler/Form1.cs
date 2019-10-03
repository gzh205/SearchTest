using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }
        void init()
        {
            string url = "http://www.ciscn.cn/competition";
            Demo d = new Demo();
            Crawler.run(d.setUrl(url).setDepth(5));
            //Dictionary<string,string> res = d.getResult();
            //richTextBox1.Text += d.getPage(url).getDocument().Text;
           // for (int i = 0; i < res.Count; i++)
            //    richTextBox1.Text += res[""+i];
        }
        
    }
}
