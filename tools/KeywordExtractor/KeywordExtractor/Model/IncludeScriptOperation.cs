using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;

namespace KeywordExtractor
{
    public class IncludeScriptOperation : Operation
    {
        public override void Execute()
        {
            var script = this.Workflow.Document.createElement("script") as IHTMLScriptElement;
            script.src = this.Parameter;
            this.Workflow.Document.appendChild(script as IHTMLDOMNode);
        }
    }
}
