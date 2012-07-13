using System;
using System.Net;
using System.Text;
using System.IO;
using Webflow.Extern;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Webflow.Operations
{
    public class HttpRequestOperation : OperationBase
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public Uri Uri { get; set; }

        public string Method { get; set; }

        public bool IsAsync { get; set; }

        public HttpRequestOperation(string param, string url)
            : base(param)
        {
            this.Uri = new Uri(url);
        }

        public HttpRequestOperation(string param, string url, string method, bool isAsync)
            : this(param, url)
        {
            this.Method = method;
            this.IsAsync = isAsync;
        }

        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("user-agent", DefaultUserAgent);
                client.Headers.Add("Cookie", Internet.GetCookieString(this.Uri));

                if (this.IsAsync)
                {
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                    client.UploadStringAsync(this.Uri, this.Parameter);
                }
                else
                {
                    string result = client.UploadString(this.Uri, this.Parameter);
                }
            }
        }

        private void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            string result = e.Result;
        }
    }
}
