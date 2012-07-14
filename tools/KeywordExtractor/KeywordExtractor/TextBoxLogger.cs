using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Webflow;

namespace KeywordExtractor
{
    public class TextBoxLogger : ILogger
    {
        private TextBox textBox;

        public TextBoxLogger(TextBox tb)
        {
            this.textBox = tb;
        }

        public void WriteLine(string text)
        {
            this.textBox.Text = text + "\r\n" + this.textBox.Text;
        }

        public void Clear()
        {
            this.textBox.Text = string.Empty;
        }
    }
}
