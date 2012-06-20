using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;

namespace KeywordExtractor
{
    public class ScriptReference
    {
        public string Name{get;set;}
        public string Url { get; set; }
        public bool IsPersistent { get; set; }
    }
}
