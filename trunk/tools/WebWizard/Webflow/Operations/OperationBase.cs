namespace Webflow.Operations
{
    public abstract class OperationBase
    {
        public string Parameter { get; set; }
        public abstract void Execute(WebflowBase webflow);

        public OperationBase()
        {
        }

        public OperationBase(string param)
        {
            this.Parameter = param;
        }
    }
}
