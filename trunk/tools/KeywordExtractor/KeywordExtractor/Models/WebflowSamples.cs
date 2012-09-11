using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webflow;
using Webflow.Operations;
using Webflow.Triggers;
using System.Web;
using System.Windows;

namespace KeywordExtractor
{
    public static class WebflowSamples
    {
        public static WebflowBase Maiku
        {
            get
            {
                DocumentWebflow webflow = new DocumentWebflow();
                UrlTrigger trigger = new UrlTrigger("note.sdo.com/$");

                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    //$('#username').val('wbxfire@gmail.com');
                    //$('#password').val('123456ab');
                    $('#username').val('afei_test001@163.com');
                    $('#password').val('happy123');
                    $('#loginbtn').click();
                ", 1500));
                StringBuilder postString = new StringBuilder();
                string title = HttpUtility.UrlEncode("titlecontent!");
                string categoryid = HttpUtility.UrlEncode("pYyDa~jZDtUVnM2Mo002oM");
                string notecontent = HttpUtility.UrlEncode("<p>ccc</p>");
                postString.Append("noteid=&");
                postString.Append("importance=0&");
                postString.Append("title=" + title + "&");
                postString.Append("categoryid=" + categoryid + "&");
                postString.Append("tags=&");
                postString.Append("sourceurl=&");
                postString.Append("notecontent=" + notecontent);

                //MessageBox.Show(postString);
                //MessageBox.Show(postString.ToString());

                webflow.Triggers.Add(trigger);

                trigger = new UrlTrigger("https://note.sdo.com/my#!note/list");
                trigger.Operations.Add(new HttpRequestOperation(
                        postString.ToString(),
                        "https://note.sdo.com/note/save", 
                        "POST", 
                        true));
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
                    $('#uin').val('2271137761');
                    $('#p').val('zhanla568629');
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
                        $('#bottle_send textarea',mainDoc).text(data.ReadFile('data/content.txt'));
                        $('#bottle_send .send_bottom a[ck=send]',mainDoc).click();
                    },3000);
                    location.href='http://m43.mail.qq.com/cgi-bin/frame_html';
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

        public static WebflowBase BlueHost
        {
            get
            {
                DocumentWebflow webflow = new DocumentWebflow();
                webflow.ClearCookie();
                UrlTrigger trigger = new UrlTrigger("my.bluehost.com/cgi-bin/cplogin$");

                trigger.Operations.Add(new ExecuteScriptOperation(@"
                    $('input[name=ldomain]').val('zhenfeic');
                    $('input[name=lpass]').val('aaAbc123456!');
                    $('input.submitButton').click();
                ", 1500));
                webflow.Triggers.Add(trigger);

                trigger = new UrlTrigger("frontend/bluehost/index.html");

                for (var i = 0; i < 12; i++)
                {
                    string sub = string.Format("cool{0}", i);

                    trigger.Operations.Add(new HttpRequestOperation(
                        "https://my.bluehost.com/cgi/dm/subdomain/add",
                        string.Format("sub={0}&rdomain={1}&docroot={2}", sub, "zhenfei.com", sub),
                        "POST", true));
                }

                webflow.Triggers.Add(trigger);

                return webflow;
            }
        }
    }
}
