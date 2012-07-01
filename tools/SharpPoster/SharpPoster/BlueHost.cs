using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;
using System.Web;

namespace SharpPoster
{
    public static class BlueHost
    {
        private static CookieAwareWebClient client = new CookieAwareWebClient();

        //private static WebClient client = new WebClient();

        public static string Login(string username, string password)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            return client.UploadString("https://my.bluehost.com/cgi-bin/cplogin", string.Format("ldomain={0}&lpass={1}", username, password));

        }

        public static string AddSubDomain(string sub, string rdomain, string docroot)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            return client.UploadString("https://my.bluehost.com/cgi/dm/subdomain/add", string.Format("sub={0}&rdomain={1}&docroot={2}", sub, rdomain, docroot));
        }

        public static string DeleteDomain(string domainFix, string domain)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            return client.UploadString("https://my.bluehost.com/cgi/dm/subdomain/delete", string.Format("processing=1&domainkey={0}_{1}&delsub=Delete Subdomain", domainFix, domain));

        }

        /*
         * 登录163邮箱
         */
        public static string NELogin(string userName, string passWord)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string res = client.UploadString("https://ssl.mail.163.com/entry/coremail/fcg/ntesdoor2", string.Format("df=webmail163&from=web&funcid=loginone&iframe=1&language=-1&net=t&passtype=1&product=mail163&race=-2_-2_-2_db&style=-1&uid=afei_test001@163.com&username={0}&password={1}", userName, passWord));
            return res;
        }

        /*
         * 写邮件
         */
        public static string NEWriteEmail(string from, string to, string subject, string content)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            string sid = GetSessionString();
            var url = string.Format("http://webmail.mail.163.com/js4/s?sid={0}&func=mbox:compose&cl_send=2&l=compose&action=deliver", sid);
            var data = string.Format("<?xml version='1.0'?><object><string name='id'>c:1341067642789</string><object name='attrs'><string name='account'>'afei_test001'&lt;afei_test001@163.com&gt;</string><boolean name='showOneRcpt'>false</boolean><array name='to'><string>afei_test001@163.com</string></array><array name='cc'/><array name='bcc'/><string name='subject'>adsadsasa</string><boolean name='isHtml'>true</boolean><string name='content'>&lt;div style='line-height:1.7;color:#000000;font-size:14px;font-family:arial'&gt;qwewqeqwqew&lt;/div&gt;</string><int name='priority'>3</int><boolean name='saveSentCopy'>true</boolean><boolean name='requestReadReceipt'>false</boolean><string name='charset'>GBK</string></object><boolean name='returnInfo'>false</boolean><string name='action'>deliver</string><int name='saveSentLimit'>1</int></object>", from, to, subject, content);
            data = "var=" + HttpUtility.UrlEncode(data);

            //这里的sid需要在Set-Cookie中提取，用这个方法：GetSessionString()
            string res = client.UploadString(url, data);
            return res;
        }

        /*
         * 获取sid对应的字符串
         * 第一个返回的Javascript里取也可以
         * 提取自：Set-Cookie
         */
        private static string GetSessionString()
        {
            string CookieString = client.ResponseHeaders["Set-Cookie"];
            string[] sArray = CookieString.Split(';');
            return sArray[19].Split('%')[1];
        }

        private static string GetUserId()
        {
            string CookieString = client.ResponseHeaders["Set-Cookie"];
            string[] sArray = CookieString.Split(';');
            return sArray[19].Split('%')[1];
        }

        public static string CSDNLogin()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Referer", "http://passport.csdn.net/account/loginbox?callback=logined&hidethird=1&from=http%3a%2f%2fwww.csdn.net%2f");

            string res = client.DownloadString("http://passport.csdn.net/ajax/accounthandler.ashx?" + "t=log&u=afei_test001&p=happy_123&remember=0&f=http%3A%2F%2Fwww.csdn.net%2F&rand=0.1062355195172131");
            return res;
        }


        /*
         * MaiKU:https://note.sdo.com/my
         * afei_test001@163.com|happy123
         */

        public static string MaiKuLogin()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string res = client.UploadString("https://cas.sdo.com/dplogin", "username=afei_test001@163.com&password=happy123&appArea=0&appId=306&service=http://passport.note.sdo.com/account/loginresult?type=snda&returnUrl=https%3a%2f%2fnote.sdo.com%2fmy&cururl=http%3a%2f%2fnote.sdo.com%2f&target=top&code=2&pageType=0&saveTime=0&loginCustomerUrl=http://note.sdo.com/&encryptFlag=0&ptname=afei_test001@163.com&ptpwd=happy123");
            return res;
        }

        public static string WriteShareNote()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            //var obj = client.CookieContainer.GetCookies(new Uri("https://note.sdo.com"));
            string res = client.UploadString("https://note.sdo.com/note/save", "{\"noteid\":\"\",\"importance\":\"0\",\"title\":\"ttt\",\"categoryid\":\"pYyDa~jZDtUVnM2Mo002oM\",\"tags\":\"\",\"sourceurl\":\"\",\"notecontent\":\"<p>dddd</p>\"}");
            return res;


        }




    }
}
