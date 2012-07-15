using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webflow.Log
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string text)
        {
            Console.WriteLine(this.GetLogString(LogType.Information, text));
        }

        public void Log(string format, params object[] arg)
        {
            Console.WriteLine(this.GetLogString(LogType.Information, format, arg));
        }

        public void Log(LogType type, string text)
        {
            Console.WriteLine(this.GetLogString(type, text));
        }

        public void Log(LogType type, string format, params object[] arg)
        {
            Console.WriteLine(this.GetLogString(type, format, arg));
        }

        public void Warning(string text)
        {
            this.Log(LogType.Warning, text);
        }

        public void Warning(string format, params object[] arg)
        {
            this.Log(LogType.Warning, format, arg);
        }

        public void Error(string text)
        {
            this.Log(LogType.Error, text);
        }

        public void Error(string format, params object[] arg)
        {
            this.Log(LogType.Error, format, arg);
        }

        public void Clear()
        {
            Console.Clear();
        }

        private string GetLogString(LogType type, string text)
        {
            return string.Format("[{0}]: {1}", type, text);
        }

        private string GetLogString(LogType type, string format, params object[] arg)
        {
            return string.Format("[{0}]: {1}", type, string.Format(format, arg));
        }
    }
}
