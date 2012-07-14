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
                    this.Logger.WriteLine(string.Format("当前地址：{0}", this.CurrentUrl));
                    this.OnCurrentUrlChanged();
                }
            }
        }

        public ILogger Logger { get; set; }

        public DataContainer Data { get; set; }

        public List<TriggerBase> Triggers { get; set; }

        public WebflowBase()
        {
            this.Data = new DataContainer();
            this.Triggers = new List<TriggerBase>();
        }

        public void WriteLog(string text)
        {
            if (this.Logger != null)
            {
                this.Logger.WriteLine(text);
            }
        }

        public void ClearLog()
        {
            if (this.Logger != null)
            {
                this.Logger.Clear();
            }
        }

        protected virtual void OnCurrentUrlChanged()
        {
        }
    }
}
