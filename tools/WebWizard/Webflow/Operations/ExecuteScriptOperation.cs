using System;
namespace Webflow.Operations
{
    public class ExecuteScriptOperation : OperationBase
    {
        public string ScriptText { get; set; }
        public int Delay { get; set; }

        public ExecuteScriptOperation(string script)
            : base()
        {
            this.ScriptText = script;
        }

        public ExecuteScriptOperation(string script, int delay)
            : base()
        {
            this.ScriptText = script;
            this.Delay = delay;
        }

        public ExecuteScriptOperation(string name, string script, int delay, Action<object> callback)
            : base(name, callback)
        {
            this.ScriptText = script;
            this.Delay = delay;
        }

        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                this.Status = OperationStatus.Executing;
                wf.WriteLog(string.Format("执行脚本'{0}':...", this.Name));
                wf.InjectScript(this.ScriptText, this.Delay);
                this.Status = OperationStatus.Completed;
                wf.WriteLog(string.Format("脚本'{0}'执行完毕.", this.Name));
                this.InvokeCallback(null);
            }
        }
    }
}
