using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KeywordExtractor
{
    public class InjectionSetting
    {
        public string Name { get; set; }
        public Regex UrlPattern { get; set; }
        public string ScriptText { get; set; }
    }
}
