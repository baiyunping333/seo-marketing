﻿(function () {
    rh.ready(function () {
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
})();