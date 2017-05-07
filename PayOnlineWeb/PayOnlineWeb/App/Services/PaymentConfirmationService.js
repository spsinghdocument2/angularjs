app.service('PaymentConfirmationService',['$http', function ($http) {
    var PaymentHolder = '';
    var url = "";
    this.PaymentConfirmationHolder = function (Payment) {
        PaymentHolder = Payment;
        return "Holder data successfully";
    }

    this.PaymentConfirmationGet = function () {
    
        return PaymentHolder;
    }

    var paymentTransactionData = '';

    //save payment transaction data for print
    this.savePaymentTransactionData = function (data)
    {
        sessionStorage.paymentTransactionData = angular.toJson(data);
        return true;
    }

    //get payment transaction data for print
    this.getPaymentTransactionData = function () {
        paymentTransactionData = angular.fromJson(sessionStorage.paymentTransactionData);
        return paymentTransactionData;
    }

    this.FeeAmountGet = function (accountNumber, baseAddress)
    {
     
        url = baseAddress + "/PaymentConfirmation/GET/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    }
    this.CardFeeAmountGet = function (accountNumber, baseAddress) {

        url = baseAddress + "/PaymentConfirmation/GETCardFee/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    }

    this.paymentConform = function (collectionData, baseAddress)
    {
       
        url = baseAddress + "/PaymentConfirmation/Post";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });

    }

    this.CardPaymentConfirm = function (collectionData, baseAddress) {

        url = baseAddress + "/PaymentConfirmation/CardPayment";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });

    }

    this.SavedCardPaymentConfirm = function (collectionData, baseAddress) {

        url = baseAddress + "/PaymentConfirmation/SavedCardPayment";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });

    }

  

}]);