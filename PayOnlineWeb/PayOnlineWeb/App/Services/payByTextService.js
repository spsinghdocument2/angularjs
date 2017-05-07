app.service('payByTextService',['$http', function ($http) {
    var url = "";
    this.UserAccountInformation = function (AccountNumber, baseAddress) {
        url = baseAddress + "/PayByText/GetAccountInfo/" + AccountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetSavedAchDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayByText/GetSavedAchDetail/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetSavedCardDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayByText/CardDetails/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.GetStateList = function () {
        url = "data/State.json";
        return $http.get(url);
    };

    this.GetBillingAddress = function (accountNumber, tokenId, baseAddress) {
        url = baseAddress + "/PayByText/BillingAddress/" + accountNumber + "/" + tokenId;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };


    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;        
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteSavedAch = function (ID, accountNumber, baseAddress) {
        url = baseAddress + "/PayByText/DeleteSavedAch/" + ID + "/" + accountNumber;
        return $http.delete(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.DeleteCard = function (tokenId, baseAddress) {
        url = baseAddress + "/PayByText/DeleteCard/" + tokenId;
        return $http.post(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.getFee = function (paymentMethod,acountNumber, baseAddress) {
        url = baseAddress + "/PayByText/GetFee/" + paymentMethod + "/" + acountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.achMethodSetup = function (collectionData, baseAddress) {
        url = baseAddress + "/PayByText/PayByTextACHSetup";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.cardMethodSetup = function (collectionData, baseAddress) {
        url = baseAddress + "/PayByText/PayByTextCardSetup";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.ValidateTransaction = function (collectionData, baseAddress) {
        url = baseAddress + "/PayByText/ValidateTransaction";
        return $http.post(url, collectionData, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.getOptedInTextNumbers = function (acountNumber, baseAddress) {
        url = baseAddress + "/TextNotification/GetUserDetails/" + acountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    }

    this.deleteSetup = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayByText/DeletePayByText/" + accountNumber;
        return $http.delete(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };
    this.getPayBySetup = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayByText/GetSetup/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

}]);