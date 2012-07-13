using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Webflow.Extern
{
    public static class Internet
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetGetCookie(
          string url, string name, StringBuilder data, ref int dataSize);

        public static string GetCookieString(Uri uri)
        {
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
              ref datasize))
            {
                if (datasize < 0)
                    return null;

                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }

            return cookieData.ToString();
        }

        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            string cookieString = GetCookieString(uri);

            if (cookieString.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieString.Replace(';', ','));
            }
            return cookies;
        }
    }
}
