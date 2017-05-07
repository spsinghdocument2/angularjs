
app.service('PaymentHistoryService',['$http', function ($http) {


    this.PaymentHistoryGet = function (acountNumber, baseAddress) {

        url = baseAddress + "/Payonline/PaymentHistory/" + acountNumber;
        return $http.get(url);
    }

    this.LastPaymentGet = function (acountNumber, baseAddress) {
        url = baseAddress + "/Payonline/LastpaymentHistory/" + acountNumber;
        return $http.get(url);
    }


    this.Paymentsearch = function (collection, baseAddress) {
       
        url = baseAddress + "/Payonline/PaymentSearch";
        return $http.post(url, collection);
    }

}]);

