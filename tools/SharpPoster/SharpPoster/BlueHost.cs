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
            //tBpixftYtAtSzcoOGIYYdIZtlcPvJqRc
            string sid = GetSessionString();
            var url = string.Format("http://webmail.mail.163.com/js4/s?sid={0}&func=mbox:compose&cl_send=2&l=compose&action=deliver", sid);
            var data = string.Format("<?xml version='1.0'?><object><string name='id'>c:1341067642789</string><object name='attrs'><string name='account'>'afei_test001'&lt;afei_test001@163.com&gt;</string><boolean name='showOneRcpt'>false</boolean><array name='to'><string>afei_test001@163.com</string></array><array name='cc'/><array name='bcc'/><string name='subject'>adsadsasa</string><boolean name='isHtml'>true</boolean><string name='content'>&lt;div style='line-height:1.7;color:#000000;font-size:14px;font-family:arial'&gt;qwewqeqwqew&lt;/div&gt;</string><int name='priority'>3</int><boolean name='saveSentCopy'>true</boolean><boolean name='requestReadReceipt'>false</boolean><string name='charset'>GBK</string></object><boolean name='returnInfo'>false</boolean><string name='action'>deliver</string><int name='saveSentLimit'>1</int></object>", from, to, subject, content);
            data = "var=" + HttpUtility.UrlEncode(data);

            //这里的sid需要在Set-Cookie中提取，用这个方法：GetSessionString()
            string res = client.UploadString(url, data);
            return res;
        }

        /*
         for(string line=reader.ReadLine(); line!=null; line.reader.ReadLine()){
            source.Text+=line+'\n';
          }
         
         */

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
            var obj=client.ResponseHeaders;
        }


        /*
         * MaiKU:https://note.sdo.com/my
         * afei_test001@163.com|happy123
         */

        public static string MaiKuLogin()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            //client.Headers.Add("Referer", "https://note.sdo.com/my");
            string res = client.UploadString("https://cas.sdo.com/dplogin", "username=afei_test001@163.com&password=happy123&appArea=0&appId=306&service=http://passport.note.sdo.com/account/loginresult?type=snda&returnUrl=https%3a%2f%2fnote.sdo.com%2fmy&cururl=http%3a%2f%2fnote.sdo.com%2f&target=top&code=2&pageType=0&saveTime=0&loginCustomerUrl=http://note.sdo.com/&encryptFlag=0&ptname=afei_test001@163.com&ptpwd=happy123");
            return res;
        }

        public static string WriteShareNote()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            StringBuilder postData = new StringBuilder();
            postData.Append("{\"noteid\":\"\",\"importance\":\"0\",\"title\":\"title\",\"categoryid\":\"pYyDa~jZDtUVnM2Mo002oN\",\"tags\":\"\",\"sourceurl\":\"\",\"notecontent\":\"<p>content</p>\"}");

            string res = client.UploadString("https://note.sdo.com/note/save", string.Format("importance=0&title=title&categoryid={0}&notecontent={1}", HttpUtility.UrlEncode("\"pYyDa~jZDtUVnM2Mo002oN\""), HttpUtility.UrlEncode("\"<p>content</p>\"")));
            return res;
            /*
            StringBuilder postData = new StringBuilder();
            postData.Append("{\"noteid\":\"\",\"importance\":\"0\",\"title\":\"title\",\"categoryid\":\"pYyDa~jZDtUVnM2Mo002oN\",\"tags\":\"\",\"sourceurl\":\"\",\"notecontent\":\"<p>content</p>\"}");
            byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString());
            client.Headers.Add("ContentLength", sendData.Length.ToString());

            byte[] recData = client.UploadData("https://note.sdo.com/note/save", "POST", sendData);
            string res=Encoding.UTF8.GetString(recData);
            return res;

            */
            
            
            //client.Headers.Add("Origin", "http://login.sdo.com");
            //client.Headers.Add("Host", "cas.sdo.com");
            //client.Headers.Add("Referer", "http://login.sdo.com/sdo/Login/LoginFrame.php?appId=306&areaId=1&returnURL=http%3a%2f%2fpassport.note.sdo.com%2faccount%2floginresult%3ftype%3dsnda%26returnUrl%3dhttps%253a%252f%252fnote.sdo.com%252fmy%26cururl%3dhttp%253a%252f%252fnote.sdo.com%252f%26target%3dtop&CSSURL=http%3a%2f%2fnote.sdo.com%2fstatic%2fcss%2fpages%2floginframe.login.css&curURL=http%3a%2f%2fnote.sdo.com%2f");
            //client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            //var obj = client.CookieContainer.GetCookies(new Uri("https://note.sdo.com"));
            //string title=
            //string data = "?importance=0&" + "categoryid=" + HttpUtility.UrlEncode("pYyDa~jZDtUVnM2Mo002oM") + "&title=" + HttpUtility.UrlEncode("TItle") + "&notecontent=" + HttpUtility.UrlEncode("<p>contetne!!!!</p>");
            //string res = client.UploadString("https://note.sdo.com/note/save", data);
            //return res;


        }


        /*
         * 1.登录百度：https://passport.baidu.com/v2/api/?login
         * 2.相关信息：afei_test|happy_123
         * 3.
         * ppui_logintime=8489&charset=UTF-8&token=9b409b980f9841c108ca8eaa092f734d&isPhone=false&index=0&u=http://www.baidu.com/&safeflg=0&staticpage=https://passport.baidu.com/v2Jump.html&loginType=1&tpl=mn&callback=parent.bdPass.api.login._postCallback&username=afei_test001&password=happy_123&mem_pass=on
         * 
         */


        /*
         * 1.淘宝登录：
         * afeion|happy_128.com
         * 2.
         * 
         * ua:180N5fmXZX+XZL7WJf7W5X/XPs=|N5TmN4SJBNSlAcPoQJT6QISdBciuA9G5TvuX|N5XmN5bmXIr7Xp76QJD8W4r7Xp76QJ76XIr7Xp76QJH9XPuX|N57mToSX|N5/mToSX|N5f7QP3oBNK+HNXwQ4mmA8GjAoi+DcmoDcnkD8mnQ8uvAcSvHommA8GjAoigBNKnAJm5HMv3Xpb7X4j7QpbkXoCsUdKlHIC4CcKjHsOpGPOYIJuiGNK6SZWLSZSMSZSMBdKvAYi+DcmoDcnkD8mnSZSMBdKvAYiiGMvvX+C5HMvvX+KrXpX6Hoj7Qpf6QpDvXpCjCIP5KJf/VZD9WJL5VZT/SZT8M9PvX+L+HZG/X9X6XJH9ToroBNK+HJzlQ8++CcvkGMelDselQsWlAYmjGMOnQs6+AZm5HMv3DZT5XNTkXYj7XIj8Ss+uUZf/VZD9WJL5VZT/Svm/UZK7W9P5H5b6W5HoMfs=|N5f4QISmA8GjAoSX|N5f6QISRWJb6XJDmMIT7X5L6VJbzVZT4VZ/5EJbkW5TyWZX/XJ/yXpP8WZL5X/roQJeXTvs=|N5f5QP3oO8+kCMm9H4aSPITmTvGDIob7XYr5QJX6XIr4WZHoMfs=|N5PmN4ToQP38WpLmXpL7MYr4QIToQJD+VPuX|N5PmN4ToQP39VZ7mXZH8MYr6QIToQJf7X5//VfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXYr7XZL6XZ+XMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWpPmXIr7XZL9X5CXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5bmXIr7XZLyWJOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWp/mXIr7XZLyVZOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5XmXIr7XZP6XZaXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5/mXIr7XZP4XpeXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW57mXIr7XZP5W5OXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmVYr6QJf7WZD9XfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXIr7XZP8W5KXMQ==|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJf7WZD9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor5Xor6QJf7W5D9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor7W4r6QJf7VJL7WvuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yX/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yVfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ77VPuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ74X/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/WfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/VfuX|N5LmN4SePOqVGdWvHsirAcOVXYTmW5X+QJf9VIr4XpL9XZaXMQ==|N5PmN4SePOqVHMe5H9GlHsKVXYTmN5H8VYr4XJGXQJbmToTmXpT/XJ77Mfs=|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WZbyWfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VJ6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZOXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5TmXIr4XpD5XJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpPmXIr4XpD+VZGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD8Xp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD9VJKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJ/mXIr4XpH4Wp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZDmXIr4XpH9WZeXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7zQJLmXpTyXJX7Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWJ/mXIr4Xp7+VJ6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZbmXIr4Xp79X5SXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZDmXIr4Xp/6VJSXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ/6QJbmXpTzW5L5Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpHmXIr4X5b6WZaXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5/mXIr4X5b4Xp+XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5HmXIr4X5b+W5aXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJKXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WJ/+X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZaXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WZ74WpCXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+Xp6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJWXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJCXMQ==|N5PmN4ToQP39VZfmX5f8MYr6QIToQJT8Xp/8XfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpTzWpCXMQ==
TPL_username:afeion
TPL_password:happy_128.com
         */

        public static string TaoBaoLogin() {
            
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("ua", HttpUtility.UrlEncode("180N5fmXZX+XZL7WJf7W5X/XPs=|N5TmN4SJBNSlAcPoQJT6QISdBciuA9G5TvuX|N5XmN5bmXIr7Xp76QJD8W4r7Xp76QJ76XIr7Xp76QJH9XPuX|N57mToSX|N5/mToSX|N5f7QP3oBNK+HNXwQ4mmA8GjAoi+DcmoDcnkD8mnQ8uvAcSvHommA8GjAoigBNKnAJm5HMv3Xpb7X4j7QpbkXoCsUdKlHIC4CcKjHsOpGPOYIJuiGNK6SZWLSZSMSZSMBdKvAYi+DcmoDcnkD8mnSZSMBdKvAYiiGMvvX+C5HMvvX+KrXpX6Hoj7Qpf6QpDvXpCjCIP5KJf/VZD9WJL5VZT/SZT8M9PvX+L+HZG/X9X6XJH9ToroBNK+HJzlQ8++CcvkGMelDselQsWlAYmjGMOnQs6+AZm5HMv3DZT5XNTkXYj7XIj8Ss+uUZf/VZD9WJL5VZT/Svm/UZK7W9P5H5b6W5HoMfs=|N5f4QISmA8GjAoSX|N5f6QISRWJb6XJDmMIT7X5L6VJbzVZT4VZ/5EJbkW5TyWZX/XJ/yXpP8WZL5X/roQJeXTvs=|N5f5QP3oO8+kCMm9H4aSPITmTvGDIob7XYr5QJX6XIr4WZHoMfs=|N5PmN4ToQP38WpLmXpL7MYr4QIToQJD+VPuX|N5PmN4ToQP39VZ7mXZH8MYr6QIToQJf7X5//VfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXYr7XZL6XZ+XMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWpPmXIr7XZL9X5CXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5bmXIr7XZLyWJOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWp/mXIr7XZLyVZOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5XmXIr7XZP6XZaXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5/mXIr7XZP4XpeXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW57mXIr7XZP5W5OXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmVYr6QJf7WZD9XfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXIr7XZP8W5KXMQ==|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJf7WZD9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor5Xor6QJf7W5D9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor7W4r6QJf7VJL7WvuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yX/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yVfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ77VPuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ74X/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/WfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/VfuX|N5LmN4SePOqVGdWvHsirAcOVXYTmW5X+QJf9VIr4XpL9XZaXMQ==|N5PmN4SePOqVHMe5H9GlHsKVXYTmN5H8VYr4XJGXQJbmToTmXpT/XJ77Mfs=|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WZbyWfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VJ6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZOXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5TmXIr4XpD5XJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpPmXIr4XpD+VZGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD8Xp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD9VJKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJ/mXIr4XpH4Wp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZDmXIr4XpH9WZeXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7zQJLmXpTyXJX7Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWJ/mXIr4Xp7+VJ6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZbmXIr4Xp79X5SXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZDmXIr4Xp/6VJSXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ/6QJbmXpTzW5L5Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpHmXIr4X5b6WZaXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5/mXIr4X5b4Xp+XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5HmXIr4X5b+W5aXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJKXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WJ/+X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZaXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WZ74WpCXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+Xp6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJWXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJCXMQ==|N5PmN4ToQP39VZfmX5f8MYr6QIToQJT8Xp/8XfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpTzWpCXMQ=="));
            //client.Headers.Add("Referer", "https://note.sdo.com/my");
            string res = client.UploadString("https://login.taobao.com/member/login.jhtml", string.Format("ua={0}&TPL_username={1}&TPL_password={2}", HttpUtility.UrlEncode("180N5fmXZX+XZL7WJf7W5X/XPs=|N5TmN4SJBNSlAcPoQJT6QISdBciuA9G5TvuX|N5XmN5bmXIr7Xp76QJD8W4r7Xp76QJ76XIr7Xp76QJH9XPuX|N57mToSX|N5/mToSX|N5f7QP3oBNK+HNXwQ4mmA8GjAoi+DcmoDcnkD8mnQ8uvAcSvHommA8GjAoigBNKnAJm5HMv3Xpb7X4j7QpbkXoCsUdKlHIC4CcKjHsOpGPOYIJuiGNK6SZWLSZSMSZSMBdKvAYi+DcmoDcnkD8mnSZSMBdKvAYiiGMvvX+C5HMvvX+KrXpX6Hoj7Qpf6QpDvXpCjCIP5KJf/VZD9WJL5VZT/SZT8M9PvX+L+HZG/X9X6XJH9ToroBNK+HJzlQ8++CcvkGMelDselQsWlAYmjGMOnQs6+AZm5HMv3DZT5XNTkXYj7XIj8Ss+uUZf/VZD9WJL5VZT/Svm/UZK7W9P5H5b6W5HoMfs=|N5f4QISmA8GjAoSX|N5f6QISRWJb6XJDmMIT7X5L6VJbzVZT4VZ/5EJbkW5TyWZX/XJ/yXpP8WZL5X/roQJeXTvs=|N5f5QP3oO8+kCMm9H4aSPITmTvGDIob7XYr5QJX6XIr4WZHoMfs=|N5PmN4ToQP38WpLmXpL7MYr4QIToQJD+VPuX|N5PmN4ToQP39VZ7mXZH8MYr6QIToQJf7X5//VfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXYr7XZL6XZ+XMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWpPmXIr7XZL9X5CXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5bmXIr7XZLyWJOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmWp/mXIr7XZLyVZOXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5XmXIr7XZP6XZaXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW5/mXIr7XZP4XpeXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmW57mXIr7XZP5W5OXMQ==|N5DmN4SePOqVGdWvHsirAcOVXYTmVYr6QJf7WZD9XfuX|N5HmN4SePOqVGdWvHsirAcOVXYTmXIr7XZP8W5KXMQ==|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJf7WZD9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor5Xor6QJf7W5D9VfuX|N5DmN4SAM/WrCsOGA8GjAuWiCcWhTor7W4r6QJf7VJL7WvuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yX/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJf7VJ7yVfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ77VPuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor7QJT4XZ74X/uX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/WfuX|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WJf/VfuX|N5LmN4SePOqVGdWvHsirAcOVXYTmW5X+QJf9VIr4XpL9XZaXMQ==|N5PmN4SePOqVHMe5H9GlHsKVXYTmN5H8VYr4XJGXQJbmToTmXpT/XJ77Mfs=|N5HmN4SAM/WrCsOGA8GjAuWiCcWhTor6QJT4WZbyWfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VJ6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4XpP6VZOXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5TmXIr4XpD5XJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpPmXIr4XpD+VZGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD8Xp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJbmXIr4XpD9VJKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmVJ/mXIr4XpH4Wp6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZDmXIr4XpH9WZeXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7zQJLmXpTyXJX7Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWJ/mXIr4Xp7+VJ6XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZbmXIr4Xp79X5SXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmWZDmXIr4Xp/6VJSXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ/6QJbmXpTzW5L5Mfs=|N5DmN4SePOqVHMe5H9GlHsKVXYTmWpHmXIr4X5b6WZaXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5/mXIr4X5b4Xp+XMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmW5HmXIr4X5b+W5aXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4X5f8VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJKXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WJ7zXJGXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WJ/+X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WJ/8X5GXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZaXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZH4XZKXMQ==|N5DmN4SePOqVHMe5H9GlHsKVXYTmXZ7mXIr4WZ74WpCXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+Xp6XMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WZ7+X5WXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VJGXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXYr4WZ/7VZeXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJWXMQ==|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpT7WJCXMQ==|N5PmN4ToQP39VZfmX5f8MYr6QIToQJT8Xp/8XfuX|N5HmN4SePOqVHMe5H9GlHsKVXYTmXIr4WpTzWpCXMQ=="),"afeion","happy_128.com"));
            return res;
        }






    }
}
