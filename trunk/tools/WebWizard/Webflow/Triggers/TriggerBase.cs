using System.Collections.Generic;
using Webflow.Operations;

namespace Webflow.Triggers
{
    public abstract class TriggerBase
    {
        public List<OperationBase> Operations { get; set; }
        public abstract void Evaluate(WebflowBase webflow);

        public TriggerBase()
        {
            this.Operations = new List<OperationBase>();
        }
    }
}
