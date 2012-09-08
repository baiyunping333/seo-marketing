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
using System.Windows.Shapes;
using Awesomium.Windows.Controls;

namespace RobinHood
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webControl.Source =  new Uri("http://store.apple.com/us/configure/MD234LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS");
            webControl.DomReady += new EventHandler(webControl_DomReady);
            webControl.LoadCompleted += new EventHandler(webControl_LoadCompleted);
        }

        void webControl_LoadCompleted(object sender, EventArgs e)
        {
            webControl.ExecuteJavascript(@"
(function(){
	var INTERVAL = 200;
	var handlers = [];
	var jq = document.createElement('script');
	jq.setAttribute('src','https://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js');
	document.body.appendChild(jq);
	
	setInterval(function(){
		for(var i = 0;i < handlers.length;i++){
			handlers[i].process();
		}
	},INTERVAL);
	
	function Fill(selector, text){
		var te = document.createEvent('TextEvent');
		te.initTextEvent('textInput', true, true, window, text);
		var el = jQuery(selector)[0];
		if(el){
			el.focus();
			el.dispatchEvent(te);
			el.blur();
		}
	}
	
	function Handler(fn){
		this.enabled = true;
		this.fn = fn;
	}
	
	Handler.prototype.process = function(){
		if(this.enabled && this.fn){
			this.fn();
		}
	}
	
	var handler_jquery = new Handler(function(){
		if(window.jQuery){
			window.jQuery.noConflict();
			this.enabled = false;
			
			jQuery.fn.fillText = function(text){
				return this.each(function() {
					var el = $(this);						
					if(el){
						try{
							var te = document.createEvent('TextEvent');
							te.initTextEvent('textInput', true, true, window, text);
							el.focus();
							el.select();
							el.dispatchEvent(te);
							el.blur();
						}
						catch(e){
							console.log('Error:'+e);
						};
					}
				});
			};
			
			jQueryReady();
		}
	});
	
	handlers.push(handler_jquery);
	
	function getValue(key){
		if(typeof(__model)=='object'){
			return __model[key];
		}
	}
	
	function jQueryReady() {
		var handler_checkout = new Handler(function(){
            var btnCheck = jQuery('#checkout-now');
            if(btnCheck.is(':visible')){
				console.log('btnCheck');
				btnCheck[0].click();
                handler_checkout.enabled = false;
			}
		});

        var handler_signin = new Handler(function(){
            var inputId = jQuery('#login-appleId');
            var inputPwd = jQuery('#login-password');
            var btnSignin = jQuery('#sign-in');
            if(inputId.length>0 && inputPwd.length>0){
				inputId.fillText('angely0111@gmail.com');
				inputPwd.fillText('woaiBAMA870111');
                btnSignin[0].click();
                handler_signin.enabled = false;
			}
		});
		
		handlers.push(handler_checkout);
        handlers.push(handler_signin);
	}
})();
");
        }

        void webControl_DomReady(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
        }
    }
}
