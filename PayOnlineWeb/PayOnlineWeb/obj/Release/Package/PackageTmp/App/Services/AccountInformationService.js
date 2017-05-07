
app.service('AccountInformationService',['$http', function ($http) {
    var url = "";
    this.UserAccountInformation = function (AccountNumber, baseAddress)
    {
        url = baseAddress + "/Payonline/GET/" + AccountNumber;
        return $http.get(url);
    }

  
    this.AccountInformationNext = function (collection, baseAddress) {

        url = baseAddress + "/Payonline/post";
        return $http.post(url, collection);
    };
  
    this.FutureDateValidation = function (collection, baseAddress)
    {

        url = baseAddress + "/Payonline/FutureDateValidation";
        return $http.post(url, collection);
    };

    this.CheckValidCardTransaction = function (collection, baseAddress) {
        url = baseAddress + "/Payonline/CheckValidCardTransaction";
        return $http.post(url, collection);
    };
    
}]);