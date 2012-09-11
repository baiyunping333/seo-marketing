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
        fillInput: function (selector, text) {
            var te = document.createEvent('TextEvent');
            te.initTextEvent('textInput', true, true, window, text);
            var el = jQuery(selector)[0];
            if (el) {
                try {
                    el.focus();
                    el.select();
                    el.dispatchEvent(te);
                    el.blur();

                } catch (e) {
                    console.log('Error:' + e);
                };
            }
        }
    };

    jq.onload = function () {
        jQuery.noConflict();
        jQueryReady = true;
        var readyFn = rh.readyFn;

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

        for (var i = 0; i < readyFn.length; i++) {
            readyFn[i]();
        }
    }

})();