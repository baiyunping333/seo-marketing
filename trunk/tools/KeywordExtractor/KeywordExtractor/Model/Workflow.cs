﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;

namespace KeywordExtractor
{
    public class Workflow
    {
        public string Name { get; set; }
        public LinkedListNode<Operation> CurrentOperation { get; set; }
        public LinkedList<Operation> Operations { get; set; }
        public HTMLDocument Document { get; set; }

        public Workflow()
        {
            
        }

        public void AddOperation(Operation op)
        {
            this.Operations.AddLast(op);
        }

        public void Start()
        {
            this.ProcessOperation(this.Operations.First);
        }

        public void Pause()
        {
        }

        public void Continue()
        {
        }

        public void Stop()
        {
        }

        private void ProcessOperation(LinkedListNode<Operation> op)
        {
            this.CurrentOperation = op;
            if (op != null)
            {
                op.Value.Execute();
                this.ProcessOperation(op.Next);
            }
        }
    }
}
