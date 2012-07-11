using System.Collections.Generic;
using Webflow.Triggers;

namespace Webflow
{
    public abstract class WebflowBase
    {
        public string BaseUrl { get; set; }

        private string _currentUrl { get; set; }
        public string CurrentUrl
        {
            get { return this._currentUrl; }
            set
            {
                if (this._currentUrl != value)
                {
                    this._currentUrl = value;
                    this.OnCurrentUrlChanged();
                }
            }
        }

        public DataContext Context { get; set; }

        public List<TriggerBase> Triggers { get; set; }

        public WebflowBase()
        {
            this.Context = new DataContext();
            this.Triggers = new List<TriggerBase>();
        }

        protected void OnCurrentUrlChanged()
        {
            foreach (var t in this.Triggers)
            {
                if (t is UrlTrigger)
                {
                    t.Evaluate(this);
                }
            }
        }
    }
}
