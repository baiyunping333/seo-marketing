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