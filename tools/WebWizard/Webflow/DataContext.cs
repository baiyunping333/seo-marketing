using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Webflow
{
    [ComVisible(true)]
    public class DataContext
    {
        private Dictionary<string, string> data = new Dictionary<string, string>();

        public void SetValue(string key, string value)
        {
            this.data[key] = value;
        }

        public string GetValue(string key)
        {
            string result = string.Empty;

            if (data.ContainsKey(key))
            {
                result = data[key];
            }

            return result;
        }
    }
}
