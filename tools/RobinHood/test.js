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
		var handler_btn = new Handler(function(){
			var btnContinue = jQuery('#shipmethod-continue-button');
			var btnEdit = jQuery('#shipmethod-edit-button');

			if(btnContinue.is(':visible')){
				console.log('btnContinue');
				btnContinue[0].click();
			}
			if(btnEdit.is(':visible')){
				console.log('btnEdit');
				btnEdit[0].click();
			}
		});
		
		handlers.push(handler_btn);
	}
})();