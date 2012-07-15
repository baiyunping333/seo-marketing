﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Webflow.Log;

namespace KeywordExtractor
{
    public class TextBoxLogger : ILogger
    {
        private TextBox textBox;

        public TextBoxLogger(TextBox tb)
        {
            this.textBox = tb;
        }

        public void Log(string text)
        {
            this.AppendLine(this.GetLogString(LogType.Information, text));
        }

        public void Log(string format, params object[] arg)
        {
            this.AppendLine(this.GetLogString(LogType.Information, format, arg));
        }

        public void Log(LogType type, string text)
        {
            this.AppendLine(this.GetLogString(type, text));
        }

        public void Log(LogType type, string format, params object[] arg)
        {
            this.AppendLine(this.GetLogString(type, format, arg));
        }

        public void Warning(string text)
        {
            this.AppendLine(this.GetLogString(LogType.Warning, text));
        }

        public void Warning(string format, params object[] arg)
        {
            this.AppendLine(this.GetLogString(LogType.Warning, format, arg));
        }

        public void Error(string text)
        {
            this.AppendLine(this.GetLogString(LogType.Error, text));
        }

        public void Error(string format, params object[] arg)
        {
            this.AppendLine(this.GetLogString(LogType.Error, format, arg));
        }

        public void Clear()
        {
            this.textBox.Text = string.Empty;
        }

        private void AppendLine(string text)
        {
            this.textBox.Text = text + "\r\n" + this.textBox.Text;
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
