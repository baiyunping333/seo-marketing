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
                DocumentWebflow webflow = new DocumentWebflow();
                UrlTrigger trigger = new UrlTrigger("note.sdo.com");
     
                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    $('#username').val('wbxfire@gmail.com');
                    $('#password').val('123456ab');
                    $('#loginbtn').click();
                ", 1500));

                webflow.Triggers.Add(trigger);

                return webflow;
            }
        }

        public static WebflowBase QQMail
        {
            get
            {
                DocumentWebflow webflow = new DocumentWebflow();
                webflow.IncludeScript("http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2.min.js");

                UrlTrigger trigger = new UrlTrigger("mail.qq.com");

                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    $('#uin').val('2269928675');
                    $('#p').val('yanche704640');
                    $('#btlogin').click();
                ", 1500));

                webflow.Triggers.Add(trigger);

                trigger = new UrlTrigger("com/cgi-bin/frame_html");
                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    var mf = document.getElementById('mainFrame');
                    
                    var href = $('#my_note a:eq(0)').attr('href');
                    mf.src = href;
                    
                    setTimeout(function(){
                        var mainDoc = window.frames['mainFrame'].document;
                        $('a.btnThrow',mainDoc).click();
                        $('a.select_box[magicid=0]',mainDoc).click();
                    },2500);
                    setTimeout(function(){
                        var mainDoc = window.frames['mainFrame'].document;
                        $('#bottle_send textarea',mainDoc).text(data.ReadFile('c:\\abc.txt'));
                        $('#bottle_send .send_bottom a[ck=send]',mainDoc).click();
                    },3000);
                "));

                webflow.Triggers.Add(trigger);

//                trigger.Operations.Add(new ExecuteScriptOperation(@"
//                    var href = $('#my_note a:eq(0)').attr('href');
//                    window.location.href = href;
//                ", 1500));

//                webflow.Triggers.Add(trigger);

                return webflow;
            }
        }
    }
}
