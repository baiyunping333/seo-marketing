using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Awesomium.Core;

namespace MailRegisterV2
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
            init();
        }

        public void init() {

            WebCore.Initialize(WebCoreConfig.Default, true);
            wb.Source = new Uri("http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=email163&regPage=163");
            wb.LoadCompleted += new EventHandler(wb_LoadCompleted);
        }

        void wb_LoadCompleted(object sender, EventArgs e)
        {
            wb.CreateObject("awe");
            wb.SetObjectProperty("awe", "name", new JSValue("zhengfei"));
            wb.SetObjectCallback("awe", "callMe", (s, a) =>
            {
                MessageBox.Show(a.Arguments[0].ToString());
            });
            wb.ExecuteJavascript(@"

(function () {
    var jQueryReady = false;
    
    var jq = document.createElement('script');
    jq.setAttribute('src', 'https://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js');
    document.body.appendChild(jq);
    
    var rh = window.rh = {
        readyFn: [],
        ready: function (fn) {
            var readyFn = this.readyFn;
            readyFn.push(fn);
            for (var i = 0; i < readyFn.length; i++) {
                readyFn[i]();
            }
        },
        fillForm: function (data) {
            var i, length = data.length;
            var field;
            for (i = 0; i < length; i++) {
                field = data[i];
                if (field.value) {
                    jQuery(field.selector).fillText(field.value);
                }
            }
        },
        waitUntil: function (fnExe, fnChk, delay) {
            var interval = delay || 200;
            var timer = setInterval(function () {
                if (fnChk()) {
                    fnExe();
                    clearInterval(timer);
                }
            }, interval);
        },
        doUntil: function (fnExe, fnChk, delay) {
            var interval = delay || 100;
            var timer = setInterval(function () {
                fnExe();
                if (!fnChk()) {
                    clearInterval(timer);
                }
            }, interval);
        }
    };

    jq.onload = function () {
        jQuery.noConflict();
        jQueryReady = true;
        var readyFn = rh.readyFn;

        jQuery.fn.fillText = function (text) {
            return this.each(function () {
                var el = this;
                if (el) {
                    try {
                        var te = document.createEvent('TextEvent');
                        te.initTextEvent('textInput', true, true, window, text);
                        el.focus();
                        el.select();
                        el.dispatchEvent(te);
                        el.blur();
                    }
                    catch (e) {
                        console.log('Error:' + e);
                    };
                }
            });
        };

        jQuery.fn.domClick = function () {
            return this.each(function () {
                var el = this;
                if (el) {
                    try {
                        var me = document.createEvent('MouseEvents');
                        me.initEvent('click', true, false);
                        el.dispatchEvent(me);
                    }
                    catch (e) {
                        console.log('Error:' + e);
                    };
                }
            });
        }

        jQuery.fn.domSelect = function (val) {
            return this.each(function () {
                var el = this;
                if (el) {
                    try {
                        el.value = val;
                        var e = document.createEvent('Event');
                        e.initEvent('change', true, false);
                        el.dispatchEvent(e);
                    }
                    catch (e) {
                        console.log('Error:' + e);
                    };
                }
            });
        }

        for (var i = 0; i < readyFn.length; i++) {
            readyFn[i]();
        }
    }

})();

var data=[{
    selector:'#nameIpt',
    value:'afei_test024'
},{
    selector:'#mainPwdIpt',
    value:'happy_123'
},{
    selector:'#mainCfmPwdIpt',
    value:'happy_123'
},{
    selector:'#vcodeIpt',
    value:awe.vCode
}];

rh.ready(function(){
rh.fillForm(data);
var leftTop={
    'position':'absolute',
    'left':0,
    'top':0
};
$('#vcodeDl,#vcodeImg').css(leftTop);

});

            ");
        }

        private void btn_GetVcode_Click(object sender, EventArgs e)
        {
            wb.CopyImageAt(0, 0);
            picBox_Vcode.Image = Clipboard.GetImage();
            //wb.
        }
        public Bitmap CopyPrimaryScreen()
        {
            Screen s = Screen.PrimaryScreen;
            Rectangle r = s.Bounds;
            int w = r.Width;
            int h = r.Height;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen
            (
            new Point(0, 0),
            new Point(0, 0),
            new Size(w, h)
            );
            return bmp;
        }


        void wb_DomReady(object sender, EventArgs e)
        {
            
            //wb.CopyImageAt()
        }
    }
}
