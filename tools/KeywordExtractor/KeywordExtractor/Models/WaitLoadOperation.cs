using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeywordExtractor
{
    public abstract class WaitLoadOperation
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public OperationStatus Status { get; set; }
        public Workflow Workflow { get; set; }
        public WaitLoadOperation(Workflow workflow)
        {
            this.Status = OperationStatus.NotStarted;
            this.Workflow = workflow;
        }

        public abstract void Execute();
    }
}
