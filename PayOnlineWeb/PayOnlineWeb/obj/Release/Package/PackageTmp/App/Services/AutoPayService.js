
app.service('AutoPayService', ['$http', '$q', function ($http, $q) {
    var url = "";
    this.GetAccountInformation = function (acountNumber, baseAddress) {

        url = baseAddress + "/Payonline/GET/" + acountNumber;
     var   deferred = $q.defer();
        $http.get(url).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;
    };

    this.CheckRoutingNumber = function (collection, baseAddress) {
        url = baseAddress + "/Payonline/Post";
        var deferred = $q.defer();
        $http.post(url, collection).success(function (response) {
   
            deferred.resolve(response);
        })
        .error(function (err, status) {
           
            deferred.reject(err);
        });
        return deferred.promise;

    };


    this.GetAchDetail = function (acountNumber, baseAddress)
    {
        url = baseAddress + "/AutoPay/GETAchDetail/" + acountNumber;
        var deferred = $q.defer();
        $http.get(url).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;
    };


    this.AchPaymentSave = function (collection, baseAddress)
    {
        url = baseAddress + "/AutoPay/SaveAchAmount";
        var deferred = $q.defer();
        $http.post(url, collection).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };
     

    this.GetAutoPayDelete = function (AccountNumber, baseAddress)
    {
       
        url = baseAddress + "/AutoPay/GetAutoPayRecurring/" + AccountNumber;
        var deferred = $q.defer();
        $http.get(url).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };

    this.AutoPayDelete = function (accountNumber, baseAddress)
    {
        url = baseAddress + "/AutoPay/DeleteAutoPayRecurring/" + accountNumber;
        var deferred = $q.defer();
        $http.delete(url).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };





    this.CardAutoPaySchedule = function (collection, baseAddress)
    {
       
        url = baseAddress + "/AutoPay/CardAutoPaySchedule";
        var deferred = $q.defer();
        $http.post(url, collection).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };


    this.SaveCardAutoPaySchedule = function (collection, baseAddress) {
     
        url = baseAddress + "/AutoPay/SaveCardAutoPaySchedule";
        var deferred = $q.defer();
        $http.post(url, collection).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };
    this.GetBillingAddress = function (accountNumber, tokenId, baseAddress) {
        url = baseAddress + "/FuturePay/BillingAddress/" + accountNumber + "/" + tokenId;
        return $http.get(url);
    };
    
    this.GetStateList = function ()
    {
     
        var deferred = $q.defer();
        url = "data/State.json";

        $http.get(url).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    this.GetAchFee = function (AccountNumber, baseAddress)
    {
      
        url = baseAddress + "/AutoPay/GetAchFeeAutopay/" + AccountNumber;
        var deferred = $q.defer();
        $http.get(url).success(function (response)
        {
        
            deferred.resolve(response);
        })
        .error(function (err, status) {
          
            deferred.reject(err);
        });
        return deferred.promise;

    };


    this.GetCardFee = function (AccountNumber, baseAddress) {

        url = baseAddress + "/AutoPay/GetCardFeeAutopay/" + AccountNumber;
        var deferred = $q.defer();
        $http.get(url).success(function (response) {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };

    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;
        deferred = $q.defer();
        $http.get(url).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    };
    this.GetAutoPayDuplicatePayment = function (accountNumber, baseAddress) {
        url = baseAddress + "/AutoPay/AutoPayDuplicatePayment/" + accountNumber;
        return $http.get(url);
    };
    this.CheckValidCardTransaction = function (collection, baseAddress) {
    url = baseAddress + "/Payonline/CheckValidCardTransaction";
    return $http.post(url, collection);
    };
}]);

