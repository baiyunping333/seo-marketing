using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace CommonTools
{
    
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CopyToClipBoard();
        }

        static public String GetMyIp() {
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            return ipAddr.ToString();
        }

        static public void CopyToClipBoard(){

            Clipboard.SetDataObject(GetMyIp(), true); 
        }

    }
}
