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

                wf.WriteLog(string.Format("执行Http请求'{0}':地址({1}),方法({2}),参数({3}),异步({4}),", this.Name, this.Uri.ToString(), this.Method, this.Parameter, this.IsAsync));
                if (this.IsAsync)
                {
                    client.UploadStringCompleted += (sender, e) =>
                    {
                        wf.WriteLog(string.Format("Http请求'{0}'执行完毕", this.Name));
                        this.InvokeCallback(e.Result);
                    };
                    client.UploadStringAsync(this.Uri, this.Parameter);
                }
                else
                {
                    string result = client.UploadString(this.Uri, this.Parameter);
                    wf.WriteLog(string.Format("Http请求'{0}'执行完毕", this.Name));
                    this.InvokeCallback(result);
                }
            }
        }
    }
}
