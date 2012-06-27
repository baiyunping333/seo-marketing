using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace SharpPoster
{
    public static class BlueHost
    {
        private static WebClient client = new WebClient();


        /*
         * 登录163邮箱
         */
        public static string NELogin(string userName, string passWord)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            return client.UploadString("https://ssl.mail.163.com/entry/coremail/fcg/ntesdoor2", string.Format("df=webmail163&from=web&funcid=loginone&iframe=1&language=-1&net=t&passtype=1&product=mail163&race=-2_-2_-2_db&style=-1&uid=afei_test001@163.com&username={0}&password={1}", userName, passWord));
        }

        /*
         * 写邮件
         */

        public static void NEWriteEmail(string from, string to, string subject, string content)
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Cookie", client.ResponseHeaders["Set-Cookie"]);
            client.UploadString("https://ssl.mail.163.com/entry/coremail/fcg/ntesdoor2", string.Format("<?xml version=\"1.0\"?><object><string name=\"id\">c:1340811329343</string><object name=\"attrs\"><string name=\"account\">\"{0}\"&lt;{0}@163.com&gt;</string><boolean name=\"showOneRcpt\">false</boolean><array name=\"to\"><string>{1}</string></array><array name=\"cc\"/><array name=\"bcc\"/><string name=\"subject\">{2}</string><boolean name=\"isHtml\">true</boolean><string name=\"content\">&lt;div style='line-height:1.7;color:#000000;font-size:14px;font-family:arial'&gt;{3}&lt;/div&gt;</string><int name=\"priority\">3</int><boolean name=\"saveSentCopy\">true</boolean><boolean name=\"requestReadReceipt\">false</boolean><string name=\"charset\">GBK</string></object><boolean name=\"returnInfo\">false</boolean><string name=\"action\">deliver</string><int name=\"saveSentLimit\">1</int></object>", from, to, subject, content));
        }


    }
}
