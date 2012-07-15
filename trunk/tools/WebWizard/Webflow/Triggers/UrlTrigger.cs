using System.Text.RegularExpressions;

namespace Webflow.Triggers
{
    public class UrlTrigger : TriggerBase
    {
        public Regex UrlPattern { get; set; }

        public UrlTrigger()
            : base()
        {
        }

        public UrlTrigger(string pattern)
            : base()
        {
            this.UrlPattern = new Regex(pattern);
        }

        public override void Evaluate(WebflowBase webflow)
        {
            if (this.UrlPattern.IsMatch(webflow.CurrentUrl))
            {
                webflow.Logger.Log(string.Format("Url触发器'{0}'", this.UrlPattern.ToString()));
                foreach (var op in this.Operations)
                {
                    op.Execute(webflow);
                }
            }
        }
    }
}
