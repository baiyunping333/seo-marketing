using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Webflow.Interop
{
    [ComVisible(true)]
    public class DataContainer
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

        public void WriteFile(string path, string data)
        {
            using (var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(data);
                    writer.Close();
                }

                file.Close();
            }
        }

        public string ReadFile(string path)
        {
            string result = null;

            if (File.Exists(path))
            {
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new StreamReader(file))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }

                    file.Close();
                }
            }

            return result;
        }
    }
}
