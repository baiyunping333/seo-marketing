using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Webflow.Extern;

namespace Webflow.Operations
{
    public class HttpRequestOperation : OperationBase
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public Uri Uri { get; set; }

        public string Parameter { get; set; }

        public string Method { get; set; }

        public bool IsAsync { get; set; }

        public HttpRequestOperation(string url, string param)
            : base()
        {
            this.Uri = new Uri(url);
            this.Parameter = param;
            this.Method = "POST";
        }

        public HttpRequestOperation(string url, NameValueCollection param)
            : base()
        {
            this.Uri = new Uri(url);
            this.Parameter = this.SerializeParameter(param);
            this.Method = "POST";
        }

        public HttpRequestOperation(string url, string param, string method, bool async)
            : base()
        {
            this.Uri = new Uri(url);
            this.Parameter = param;
            this.Method = method;
            this.IsAsync = async;
        }

        public HttpRequestOperation(string url, NameValueCollection param, string method, bool async)
            : base()
        {
            this.Uri = new Uri(url);
            this.Parameter = this.SerializeParameter(param);
            this.Method = method;
            this.IsAsync = async;
        }

        public HttpRequestOperation(string name, string url, string param, string method, bool async, Action<object> callback)
            : base(name, callback)
        {
            this.Uri = new Uri(url);
            this.Parameter = param;
            this.Method = method;
            this.IsAsync = async;
        }

        public HttpRequestOperation(string name, string url, NameValueCollection param, string method, bool async, Action<object> callback)
            : base(name, callback)
        {
            this.Uri = new Uri(url);
            this.Parameter = this.SerializeParameter(param);
            this.Method = method;
            this.IsAsync = async;
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

                this.Status = OperationStatus.Executing;
                wf.Logger.Log(string.Format("执行Http请求'{0}':地址({1}),方法({2}),参数({3}),异步({4}),", this.Name, this.Uri.ToString(), this.Method, this.Parameter, this.IsAsync));

                if (this.IsAsync)
                {
                    client.UploadStringCompleted += (sender, e) =>
                    {
                        wf.Logger.Log(string.Format("Http请求'{0}'执行完毕", this.Name));
                        this.Status = OperationStatus.Completed;
                        this.InvokeCallback(e.Result);
                    };
                    client.UploadStringAsync(this.Uri, this.Parameter);
                }
                else
                {
                    string result = client.UploadString(this.Uri, this.Parameter);
                    this.Status = OperationStatus.Completed;
                    wf.Logger.Log(string.Format("Http请求'{0}'执行完毕", this.Name));
                    this.InvokeCallback(result);
                }
            }
        }

        private string SerializeParameter(NameValueCollection param)
        {
            StringBuilder result = new StringBuilder();

            foreach (var key in param.AllKeys)
            {
                result.Append(key);
                result.Append("=");
                result.Append(param[key]);
                result.Append("&");
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }

            return result.ToString();
        }
    }
}
