(function () {
    rh.ready(function () {
        var inputQuantity = jQuery('#cart-product-list li.quantity-select input');
        var btnCheck = jQuery('#checkout-now');

        if (inputQuantity.length > 0) {
            inputQuantity.fillText('2');
        }

        if (btnCheck.is(':visible')) {
            console.log('btnCheck');
            btnCheck[0].click();
        }
    });
})();