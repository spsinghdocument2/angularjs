
app.service('AccountInformationService',['$http', function ($http) {
    var url = "";
    this.UserAccountInformation = function (AccountNumber, baseAddress)
    {
        url = baseAddress + "/Payonline/GET/" + AccountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    }

  
    this.AccountInformationNext = function (collection, baseAddress) {

        url = baseAddress + "/Payonline/post";
        return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };
  
    this.FutureDateValidation = function (collection, baseAddress)
    {

        url = baseAddress + "/Payonline/FutureDateValidation";
        return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    this.CheckValidCardTransaction = function (collection, baseAddress) {
        url = baseAddress + "/Payonline/CheckValidCardTransaction";
        return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };
    
}]);