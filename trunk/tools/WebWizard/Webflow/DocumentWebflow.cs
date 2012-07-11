using System.Collections.Generic;
using mshtml;

namespace Webflow
{
    public class DocumentWebflow : WebflowBase
    {
        private List<string> persistentScriptUrls = new List<string>();

        private HTMLDocument _document;
        public HTMLDocument Document
        {
            get { return this._document; }
            set
            {
                this._document = value;
                this.OnDocumentChanged();
            }
        }

        public void IncludeScript(string scriptUrl, bool persist = true)
        {
            if (this.Document != null)
            {
                if (!persist || !this.persistentScriptUrls.Contains(scriptUrl))
                {
                    var script = this.Document.createElement("script") as IHTMLScriptElement;
                    var body = this.Document.body as IHTMLDOMNode;
                    script.src = scriptUrl;
                    body.appendChild(script as IHTMLDOMNode);
                    this.persistentScriptUrls.Add(scriptUrl);
                }
            }
        }

        public void InjectScript(string scriptText, int delay = -1)
        {
            if (this.Document != null)
            {
                var script = this.Document.createElement("script") as IHTMLScriptElement;
                var body = this.Document.body as IHTMLDOMNode;
                if (delay > 0)
                {
                    scriptText = string.Format("setTimeout(function(){{{0}}},{1})", scriptText, delay);
                }
                script.text = scriptText;
                body.appendChild(script as IHTMLDOMNode);
            }
        }

        protected void OnDocumentChanged()
        {
            this.CurrentUrl = this.Document.url;
            this.ResolvePersistentScripts();
        }

        private void ResolvePersistentScripts()
        {
            if (this.Document != null)
            {
                var body = this.Document.body as IHTMLDOMNode;
                foreach (var scriptUrl in this.persistentScriptUrls)
                {
                    var script = this.Document.createElement("script") as IHTMLScriptElement;
                    script.src = scriptUrl;
                    body.appendChild(script as IHTMLDOMNode);
                }
            }
        }
    }
}
