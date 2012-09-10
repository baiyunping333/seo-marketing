(function () {
    var INTERVAL = 200;
    var handlers = [];
    var jq = document.createElement('script');
    jq.setAttribute('src', 'https://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js');
    document.body.appendChild(jq);

    setInterval(function () {
        for (var i = 0; i < handlers.length; i++) {
            handlers[i].process();
        }
    }, INTERVAL);

    function Fill(selector, text) {
        var te = document.createEvent('TextEvent');
        te.initTextEvent('textInput', true, true, window, text);
        var el = jQuery(selector)[0];
        if (el) {
            el.focus();
            el.dispatchEvent(te);
            el.blur();
        }
    }

    function Handler(fn) {
        this.enabled = true;
        this.fn = fn;
    }

    Handler.prototype.process = function () {
        if (this.enabled && this.fn) {
            this.fn();
        }
    }

    var handler_jquery = new Handler(function () {
        if (window.jQuery) {
            window.jQuery.noConflict();
            this.enabled = false;

            jQuery.fn.fillText = function (text) {
                return this.each(function () {
                    var el = $(this);
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

            jQueryReady();
        }
    });

    handlers.push(handler_jquery);

    function getValue(key) {
        if (typeof (__model) == 'object') {
            return __model[key];
        }
    }

    function jQueryReady() {
        var handler_checkout = new Handler(function () {
            var btnCheck = jQuery('#checkout-now');
            if (btnCheck.is(':visible')) {
                console.log('btnCheck');
                btnCheck[0].click();
                handler_checkout.enabled = false;
            }
        });

        var handler_signin = new Handler(function () {
            var inputId = jQuery('#login-appleId');
            var inputPwd = jQuery('#login-password');
            var btnSignin = jQuery('#sign-in');
            if (inputId.length > 0 && inputPwd.length > 0) {
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