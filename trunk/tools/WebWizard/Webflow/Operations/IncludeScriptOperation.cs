namespace Webflow.Operations
{
    public class IncludeScriptOperation : OperationBase
    {
        public bool Persist { get; set; }

        public IncludeScriptOperation()
            : base()
        {
            this.Persist = true;
        }

        public IncludeScriptOperation(string param)
            : base(param)
        {
            this.Persist = true;
        }

        public IncludeScriptOperation(string param, bool persist)
            : base(param)
        {
            this.Persist = persist;
        }

        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                wf.IncludeScript(this.Parameter, this.Persist);
            }
        }
    }
}
