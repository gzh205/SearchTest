using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace Crawler
{
    class UrlOptionArrImpl : UrlOptions
    {
        public UrlOptionArrImpl(Uri[] uris):base(uris)
        {
            this.objName = "arr";
        }

        public override void addUrls(Uri uri)
        {
            throw new NotImplementedException("接受uri数组之后便无法再调用addUrls添加链接了");
        }
        
    }
}
