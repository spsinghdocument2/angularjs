app.service('futurePayService',['$http', function ($http) {
    var url = "";
    this.UserAccountInformation = function (AccountNumber, IsFuturePayWithAgreement,baseAddress) {
        url = baseAddress + "/FuturePay/GetAccountInfo/" + AccountNumber + "/" + IsFuturePayWithAgreement;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetSavedAchDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/FuturePay/GetSavedAchDetail/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetSavedCardDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/FuturePay/CardDetails/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetStateList = function () {
        url = "data/State.json";
        return $http.get(url);
    };

    this.GetBillingAddress = function (accountNumber, tokenId, baseAddress) {
        url = baseAddress + "/FuturePay/BillingAddress/" + accountNumber + "/" + tokenId;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;        
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteSavedAch = function (ID, accountNumber, baseAddress) {
        url = baseAddress + "/FuturePay/DeleteSavedAch/" + ID + "/" + accountNumber;
        return $http.delete(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteCard = function (tokenId, baseAddress) {
        url = baseAddress + "/FuturePay/DeleteCard/" + tokenId;
        return $http.post(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.getFee = function (paymentMethod,acountNumber, baseAddress) {
        url = baseAddress + "/FuturePay/GetFee/" + paymentMethod + "/" + acountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.ACHFuturePayConfirm = function (collectionData, baseAddress) {
        url = baseAddress + "/FuturePay/ACHFuturePay";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.CardFuturePayConfirm = function (collectionData, baseAddress) {
        url = baseAddress + "/FuturePay/CardPayment";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.ValidateTransaction = function (collectionData, baseAddress) {
        url = baseAddress + "/FuturePay/ValidateTransaction";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.futurePayWithAgreement = false;
    this.setFuturePayWithAgreement = function (data) {
        this.futurePayWithAgreement = data;
        return true;
    };

    this.getFuturePayWithAgreement = function () {        
        return this.futurePayWithAgreement;
    };
}]);