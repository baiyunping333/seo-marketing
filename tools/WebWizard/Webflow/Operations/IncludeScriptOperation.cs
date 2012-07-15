using System;
namespace Webflow.Operations
{
    public class IncludeScriptOperation : OperationBase
    {
        public string ScriptUrl { get; set; }
        public bool Persist { get; set; }

        public IncludeScriptOperation(string url)
            : base()
        {
            this.ScriptUrl = url;
            this.Persist = true;
        }

        public IncludeScriptOperation(string url, bool persist)
            : base()
        {
            this.ScriptUrl = url;
            this.Persist = persist;
        }

        public IncludeScriptOperation(string name, string url, bool persist, Action<object> callback)
            : base(name, callback)
        {
            this.ScriptUrl = url;
            this.Persist = persist;
        }

        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                this.Status = OperationStatus.Executing;
                wf.Logger.Log(string.Format("引用脚本'{0}':地址({1})", this.Name, this.ScriptUrl));
                wf.IncludeScript(this.ScriptUrl, this.Persist);
                this.Status = OperationStatus.Completed;
                this.InvokeCallback(null);
            }
        }
    }
}
