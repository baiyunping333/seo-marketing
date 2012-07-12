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
            if (!persist || !this.persistentScriptUrls.Contains(scriptUrl))
            {
                this.persistentScriptUrls.Add(scriptUrl);

                if (this.Document != null)
                {
                    var script = this.Document.createElement("script") as IHTMLScriptElement;
                    var body = this.Document.body as IHTMLDOMNode;
                    script.src = scriptUrl;
                    body.appendChild(script as IHTMLDOMNode);
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
                script.text = string.Format("(function(){{var data=window.top.external;{0}}})();", scriptText);
                body.appendChild(script as IHTMLDOMNode);
            }
        }

        public void ChangeContext(string frameId)
        {
            if (this.Document != null)
            {
                var window = this.Document.parentWindow;
                var frame = window.frames.item(frameId) as IHTMLWindow2;
                if (frame != null)
                {
                    this.Document = frame.document as HTMLDocument;
                }
            }
        }

        protected void OnDocumentChanged()
        {
            this.ResolvePersistentScripts();
            this.CurrentUrl = this.Document.url;
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
