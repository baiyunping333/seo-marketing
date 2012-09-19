using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Awesomium.Windows.Controls;
using System.Reflection;
using Awesomium.Core;
using RobinHood.Models;

namespace RobinHood
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string scriptDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "scripts");

        //private string[] names = new string[] { "Alan", "Adam", "Alex", "Brad", "Ben", "Cuba", "Carl", "Daniel", "Dick", "Eric", "Ellen", "Fred", "Frank", "Greg", "Green", "Golden", "Hawk", "Hill", "Inos", "Ivy", "Jack", "Jim", "Joe", "Jordan", "Jerry", "Karl", "Ken", "Kent", "Larry", "Lorry", "Liam", "Monster", "Monitor", "Nick", "Norton", "Orion", "Polly", "Paul", "Qunta", "Royal", "Roller", "Simon", "Slash", "Tom", "Ura", "Vitas", "William", "Yummy", "Zita" };
        private string[] names = new string[] { "Bang", "Bo", "Bu", "Bin", "Bing", "De", "Du", "Dang", "Ding", "Dong", "Fang", "Fang", "Feng", "Fa", "Fu", "Gu", "Ge", "Hai", "Hu", "Hua", "Huo", "Huan", "Huo", "Hang", "Heng", "Ju", "Jiu", "Jiang", "Ku", "Ken", "Luo", "Lu", "Liang", "Long", "Min", "Ming", "Ma", "Meng", "Nan", "Pu", "Piao", "Peng", "Pang", "Qian", "Qiang", "Qiong", "Qing", "Qin", "Qu", "Rang", "Rong", "Song", "Sang", "Si", "Tian", "Tong", "Teng", "Tang", "Tan", "Tu", "Wang", "Wu", "Wen", "Xu", "Xing", "Xin", "Xiang", "Xa", "Yu", "Yong", "Yang", "Ying", "Yan", "Yin", "Ying" };

        private Random rd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            InitCreditCards();

            webControl.DomReady += new EventHandler(webControl_DomReady);
            webControl.LoadCompleted += new EventHandler(webControl_LoadCompleted);
            webControl.CreateObject("awe");
            webControl.SetObjectCallback("awe", "statusChanged", (sender, e) =>
            {
                tStatus.Text = e.Arguments[0].ToString();
            });
        }

        private void InitCreditCards()
        {
            List<CreditCard> cards = new List<CreditCard>();
            cards.Add(new CreditCard
            {
                Name = "LiZhuoYa_GF",
                CardNumber = "5203821352104098",
                SecurityCode = "002",
                ExpirationMonth = "11",
                ExpirationYear = "2016"
            });

            cards.Add(new CreditCard
            {
                Name = "LiZhuoYa_ZS",
                CardNumber = "4392258324306382",
                SecurityCode = "468",
                ExpirationMonth = "12",
                ExpirationYear = "2014"
            });

            cards.Add(new CreditCard
            {
                Name = "Abu_ZS",
                CardNumber = "5187100014554564",
                SecurityCode = "029",
                ExpirationMonth = "01",
                ExpirationYear = "2015"
            });

            cards.Add(new CreditCard
            {
                Name = "QianWei_GS",
                CardNumber = "4897340026577898",
                SecurityCode = "673",
                ExpirationMonth = "12",
                ExpirationYear = "2016"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_PF",
                CardNumber = "4047390007305024",
                SecurityCode = "517",
                ExpirationMonth = "10",
                ExpirationYear = "2013"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_SH",
                CardNumber = "4026741253120449",
                SecurityCode = "204",
                ExpirationMonth = "02",
                ExpirationYear = "2015"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_ZS",
                CardNumber = "5187100011522523",
                SecurityCode = "475",
                ExpirationMonth = "05",
                ExpirationYear = "2013"
            });

            cards.Add(new CreditCard
            {
                Name = "XuZhiQi_ZS",
                CardNumber = "4392258319390581",
                SecurityCode = "498",
                ExpirationMonth = "03",
                ExpirationYear = "2013"
            });

            cbCreditCard.ItemsSource = cards;
        }

        private void RunScript(string filename)
        {
            string path = Path.Combine(scriptDir, filename);
            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    webControl.ExecuteJavascript(reader.ReadToEnd());
                }
            }
        }

        private void webControl_LoadCompleted(object sender, EventArgs e)
        {
            this.RunScript("core.js");
            this.RunScript("checkout.js");
            this.RunScript("signin.js");
            this.RunScript("data.js");
            this.RunScript("payment.js");
        }

        private void webControl_DomReady(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private bool ProcessTextBox(TextBox tb, string propName)
        {
            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Focus();
                return false;
            }
            else
            {
                webControl.SetObjectProperty("awe", propName, new JSValue(tb.Text));
                return true;
            }
        }

        private string GetName()
        {
            int n = rd.Next(names.Length);
            return names[n];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebCore.ClearCookies();
            string url = string.Empty;
            bool result = true;

            result = result && ProcessTextBox(tbAppleId, "email");
            result = result && ProcessTextBox(tbPassword, "password");
            result = result && ProcessTextBox(tbCardNumber, "cardNumber");
            result = result && ProcessTextBox(tbSecurityCode, "securityCode");
            result = result && ProcessTextBox(tbExpirationMonth, "expirationMonth");
            result = result && ProcessTextBox(tbExpirationYear, "expirationYear");

            webControl.SetObjectProperty("awe", "firstName", new JSValue(GetName()));
            webControl.SetObjectProperty("awe", "lastName", new JSValue(GetName()));

            if (!result)
            {
                MessageBox.Show("Please fill all boxes ");
                return;
            }

            if (colorBlack.IsChecked == true)
            {
                if (model16.IsChecked == true)
                {
                    url = "http://store.apple.com/hk/configure/MD235ZP/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FWW";
                }
                else if (model32.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD241LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model64.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD257LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
            }
            else if (colorWhite.IsChecked == true)
            {
                if (model16.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD237LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model32.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD244LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model64.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD260LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
            }

            if (!string.IsNullOrEmpty(url))
            {
                webControl.Source = new Uri(url);
            }
        }
    }
}
