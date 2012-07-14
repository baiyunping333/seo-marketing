namespace Webflow.Operations
{
    public class ExecuteScriptOperation : OperationBase
    {
        public int Delay { get; set; }

        public ExecuteScriptOperation() :
            base()
        {
            this.Delay = -1;
        }

        public ExecuteScriptOperation(string param)
            : base(param)
        {
            this.Delay = -1;
        }

        public ExecuteScriptOperation(string param, int delay)
            : base(param)
        {
            this.Delay = delay;
        }

        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                wf.WriteLog(string.Format("执行脚本'{0}':...", this.Name));
                wf.InjectScript(this.Parameter, this.Delay);
                wf.WriteLog(string.Format("脚本'{0}'执行完毕.", this.Name));
                this.InvokeCallback(null);
            }
        }
    }
}
