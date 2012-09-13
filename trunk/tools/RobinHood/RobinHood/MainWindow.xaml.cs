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
                ExpirationYear = "16"
            });

            cards.Add(new CreditCard
            {
                Name = "LiZhuoYa_ZS",
                CardNumber = "4392258324306382",
                SecurityCode = "468",
                ExpirationMonth = "12",
                ExpirationYear = "14"
            });

            cards.Add(new CreditCard
            {
                Name = "Abu_ZS",
                CardNumber = "5187100014554564",
                SecurityCode = "029",
                ExpirationMonth = "01",
                ExpirationYear = "15"
            });

            cards.Add(new CreditCard
            {
                Name = "QianWei_GS",
                CardNumber = "4897340026577898",
                SecurityCode = "673",
                ExpirationMonth = "12",
                ExpirationYear = "16"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_PF",
                CardNumber = "4047390007305024",
                SecurityCode = "517",
                ExpirationMonth = "10",
                ExpirationYear = "13"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_SH",
                CardNumber = "4026741253120449",
                SecurityCode = "204",
                ExpirationMonth = "02",
                ExpirationYear = "15"
            });

            cards.Add(new CreditCard
            {
                Name = "Shelly_ZS",
                CardNumber = "5187100011522523",
                SecurityCode = "475",
                ExpirationMonth = "05",
                ExpirationYear = "13"
            });

            cards.Add(new CreditCard
            {
                Name = "XuZhiQi_ZS",
                CardNumber = "4392258319390581",
                SecurityCode = "498",
                ExpirationMonth = "03",
                ExpirationYear = "13"
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

            if (!result)
            {
                MessageBox.Show("Please fill all boxes ");
                return;
            }

            if (colorBlack.IsChecked == true)
            {
                if (model16.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD234LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
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
