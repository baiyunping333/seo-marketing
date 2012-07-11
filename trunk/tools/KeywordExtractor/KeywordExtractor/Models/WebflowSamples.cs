using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webflow;
using Webflow.Operations;
using Webflow.Triggers;

namespace KeywordExtractor
{
    public static class WebflowSamples
    {
        public static WebflowBase Maiku
        {
            get
            {
                WebflowBase webflow = new DocumentWebflow();
                UrlTrigger trigger = new UrlTrigger("note.sdo.com");

                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    window.alert = function(){};
                    $('#username').val('wbxfire@gmail.com');
                    $('#password').val('123456ab');
                    $('#loginbtn').click();
                ", 1500));

                webflow.Triggers.Add(trigger);

                return webflow;
            }
        }
    }
}
