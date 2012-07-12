namespace Webflow.Operations
{
    public class ChangeContextOperation : OperationBase
    {
        public override void Execute(WebflowBase webflow)
        {
            var wf = webflow as DocumentWebflow;
            if (wf != null)
            {
                wf.ChangeContext(this.Parameter);
            }
        }
    }
}
