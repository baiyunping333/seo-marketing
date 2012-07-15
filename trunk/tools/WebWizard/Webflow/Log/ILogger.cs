using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webflow.Log
{
    public interface ILogger
    {
        void Log(string text);
        void Log(string format, params object[] arg);
        void Log(LogType type, string text);
        void Log(LogType type, string format, params object[] arg);
        void Warning(string text);
        void Warning(string format, params object[] arg);
        void Error(string text);
        void Error(string format, params object[] arg);
        void Clear();
    }
}
