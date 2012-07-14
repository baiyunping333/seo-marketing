using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webflow
{
    public interface ILogger
    {
        void WriteLine(string text);
        void Clear();
    }
}
