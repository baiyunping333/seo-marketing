using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeywordExtractor
{
    public class WaitLoadOperation
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public OperationStatus Status { get; set; }
        public Webflow Workflow { get; set; }
        public WaitLoadOperation(Webflow workflow)
        {
            this.Status = OperationStatus.NotStarted;
            this.Workflow = workflow;
        }

        public void Execute()
        {
        }
    }
}
