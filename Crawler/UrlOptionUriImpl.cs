using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    class UrlOptionUriImpl : UrlOptions
    {
        public UrlOptionUriImpl(Uri uri):base(uri)
        {
            this.objName = "uri";
        }

        public override void addUrls(Uri uri)
        {
            if (uri.Host != uris[0].Host)
                return;
            if (uris.IndexOf(uri) == -1 && saves.IndexOf(uri) == -1 && tmp.IndexOf(uri) == -1)
                this.tmp.Add(uri);
        }
    }
}
