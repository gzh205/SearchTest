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
            Crawler.run(new Demo().setUrl(url).setDepth(5));
            this.richTextBox1.Text += "end";
        }
        
    }
}
