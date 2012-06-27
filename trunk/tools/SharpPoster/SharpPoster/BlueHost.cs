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

        public static string AddSubDomain(string sub,string rdomain,string docroot)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            return client.UploadString("https://my.bluehost.com/cgi/dm/subdomain/add", string.Format("sub={0}&rdomain={1}&docroot={2}", sub,rdomain,docroot));
        }

        public static string DeleteDomain(string domainFix,string domain) 
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            return client.UploadString("https://my.bluehost.com/cgi/dm/subdomain/delete", string.Format("processing=1&domainkey={0}_{1}&delsub=Delete Subdomain", domainFix, domain));
        
        }
    }
}
