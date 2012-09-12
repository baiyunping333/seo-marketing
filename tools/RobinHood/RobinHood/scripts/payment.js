(function () {
    rh.ready(function () {

        var btnShipping = jQuery('#shipping-continue-button');
        var btnShipmethod = jQuery('#shipmethod-continue-button');
        var btnPayment = jQuery('#payment-continue-button');
        var btnSignIn = jQuery('#account-sign-in');
        var btnGuest = jQuery('#account-continue-as-guest');
        var btnCreate = jQuery('#account-create-account');

        rh.waitUntil(function () {
            awe.statusChanged('shipping');
            rh.fillForm(shipping);
            setTimeout(function () {
                btnShipping.domClick();
            }, 200);
        },
        function () {
            return btnShipping.is(':visible');
        });

        rh.waitUntil(function () {
            btnShipmethod.domClick();
        },
        function () {
            return btnShipmethod.is(':visible');
        });

        rh.waitUntil(function () {
            awe.statusChanged('payment');
            setTimeout(function () {
                rh.fillForm(billing);
                jQuery('#payment-credit-method-cc0-expirationMonth').domSelect(creditCard.expirationMonth);
                jQuery('#payment-credit-method-cc0-expirationYear').domSelect(creditCard.expirationYear);
                jQuery('#payment-credit-method-cc0-cardNumber').fillText(creditCard.cardNumber);
                jQuery('#payment-credit-method-cc0-security-code').fillText(creditCard.securityCode);
            }, 200);
            setTimeout(function () {
                btnPayment.domClick();
            }, 600);
        },
        function () {
            return btnPayment.is(':visible');
        });

        rh.waitUntil(function () {
            awe.statusChanged('account');
            setTimeout(function () {
                rh.fillForm(account);
            }, 200);

            setTimeout(function () {
                btnSignIn.domClick();
            }, 600);
        },
        function () {
            return btnSignIn.is(':visible');
        });

        rh.waitUntil(function () {
            setTimeout(function () {
                rh.fillForm(account);
            }, 200);

            setTimeout(function () {
                btnCreate.domClick();
            }, 600);
        },
        function () {
            return btnGuest.is(':visible');
        });

    });
})();