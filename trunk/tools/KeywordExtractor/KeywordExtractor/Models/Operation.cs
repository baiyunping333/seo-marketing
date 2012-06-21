using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeywordExtractor
{
    public abstract class Operation
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public OperationStatus Status { get; set; }
        public Operation()
        {
            this.Status = OperationStatus.NotStarted;
        }

        public abstract void Execute();
    }
}
