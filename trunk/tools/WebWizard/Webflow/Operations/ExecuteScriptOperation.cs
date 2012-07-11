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
                wf.InjectScript(this.Parameter, this.Delay);
            }
        }
    }
}
