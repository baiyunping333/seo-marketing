using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace SharpPoster
{
    public static class BlueHost
    {
        private static WebClient client = new WebClient();

        public static string Login(string username, string password)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            return client.UploadString("https://my.bluehost.com/cgi-bin/cplogin", string.Format("ldomain={0}&lpass={1}", username, password));
        }

        public static string AddSubDomain()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            return client.UploadString("https://my.bluehost.com/cgi/dm/subdomain/add", "sub=zxc333&rdomain=zhenfei.com&docroot=zxc333");
        }
    }
}
