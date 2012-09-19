var shipping = [{
    selector: '#shipping-user-firstName',
    value: awe.firstName
}, {
    selector: '#shipping-user-lastName',
    value: awe.lastName
}, {
    selector: '#shipping-user-companyName',
    value: 'Cisco'
}, {
    selector: '#shipping-user-daytimePhoneAreaCode',
    //value: '408'
    value: '852'
}, {
    selector: '#shipping-user-daytimePhone',
    //value: '5264000'
    value: '85225883100'
}, {
    selector: '#shipping-user-street',
    //value: '170 West Tasman Dr. San Jose'
    value:'31st Floor'
}, {
    selector: '#shipping-user-street2',
    value: 'Great Eagle Centre 23 Harbour Road Wan Chai'
}, {
    selector: '#shipping-user-postalCode',
    value: '95134'
}, {
    selector: '#shipping-user-emailAddress',
    value: awe.email
}, {
    selector: '#shipping-user-mobilePhoneAreaCode'
}, {
    selector: '#shipping-user-mobilePhone'
}];

var billing = [{
    selector: '#payment-credit-user-address-firstName',
    value: awe.firstName
}, {
    selector: '#payment-credit-user-address-lastName',
    value: awe.lastName
}, {
    selector: '#payment-credit-user-address-daytimePhoneAreaCode',
    //value: '408'
    value: '852'
}, {
    selector: '#payment-credit-user-address-daytimePhone',
    //value: '5264000'
    value: '85225883100'
}, {
    selector: '#payment-credit-user-address-emailAddress',
    value: awe.email
}, {
    selector: '#payment-credit-user-address-street',
    //value: '170 West Tasman Dr. San Jose'
    value: '31st Floor'
}, {
    selector: '#payment-credit-user-address-street2',
    value: 'Great Eagle Centre 23 Harbour Road Wan Chai'
}, {
    selector: '#payment-credit-user-address-postalCode',
    value: '95134'
}];

var creditCard = {
    cardNumber: awe.cardNumber,
    securityCode: awe.securityCode,
    expirationMonth: awe.expirationMonth,
    expirationYear: awe.expirationYear
};

var account = [{
    selector: '#account-password',
    value: awe.password
}, {
    selector: '#account-passwordAgain',
    value: awe.password
}];