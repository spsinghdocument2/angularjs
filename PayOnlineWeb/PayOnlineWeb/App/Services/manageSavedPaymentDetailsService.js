app.service('manageSavedPaymentDetailsService', ['$http', function ($http) {
    var url = "";

    this.GetSavedCardDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/FuturePay/CardDetails/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetStateList = function () {
        url = "data/State.json";
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetBillingAddress = function (accountNumber, tokenId, baseAddress) {
        url = baseAddress + "/FuturePay/BillingAddress/" + accountNumber + "/" + tokenId;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteCard = function (tokenId, baseAddress) {
        url = baseAddress + "/FuturePay/DeleteCard/" + tokenId;
        return $http.post(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.SaveCardAddress = function (collectionData, baseAddress) {
        url = baseAddress + "/Payonline/SaveCardAddress";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteSavedAch = function (ID, accountNumber, baseAddress) {
        url = baseAddress + "/Payonline/DeleteSavedAch/" + ID + "/" + accountNumber;
        return $http.delete(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetSavedAchDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/Payonline/GetSavedAch/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

}]);