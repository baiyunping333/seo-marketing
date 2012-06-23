using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace KeywordExtractor
{
    [ComVisible(true)]
    public class UserData
    {
        public ObservableCollection<KeyValuePair<string, string>> Data { get; set; }
        public UserData()
        {
            this.Data = new ObservableCollection<KeyValuePair<string, string>>();
        }

        public void SetValue(string key, string value)
        {
            int i = 0;
            var pair = new KeyValuePair<string, string>(key, value);
            for (; i < this.Data.Count; i++)
            {
                if (this.Data[i].Key == key)
                {
                    this.Data[i] = pair;
                    break;
                }
            }

            if (i >= this.Data.Count)
            {
                this.Data.Add(pair);
            }
        }

        public string GetValue(string key)
        {
            string value = this.Data.SingleOrDefault(x => x.Key == key).Value;
            return value == null ? string.Empty : value;
        }
    }
}
