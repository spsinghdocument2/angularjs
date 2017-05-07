
app.service('paymentAmountService',['$http','$q', function ($http, $q) {
    var url = "";
    this.GetCardDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/Payonline/CardDetails/" + accountNumber;
        deferred = $q.defer();
        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }
    this.GetStateList = function () {

        var deferred = $q.defer();
        url = "data/State.json";

        $http.get(url).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };
    

    this.GetBillingAddress = function (accountNumber,tokenId, baseAddress) {
        url = baseAddress + "/Payonline/BillingAddress/" + accountNumber + "/" + tokenId;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    }

    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;
        deferred = $q.defer();
        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }

    this.DeleteCard = function (tokenId, baseAddress) {
        url = baseAddress + "/Payonline/DeleteCard/" + tokenId;
        deferred = $q.defer();
        $http.post(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }


    this.GetSavedAchDetails = function (accountNumber, baseAddress) {
        url = baseAddress + "/Payonline/GetSavedAch/" + accountNumber;
        deferred = $q.defer();
        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;
    };

    this.DeleteSavedAch = function (ID, accountNumber, baseAddress) {
        url = baseAddress + "/Payonline/DeleteSavedAch/" + ID + "/" + accountNumber;
        deferred = $q.defer();
        $http.delete(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }

}]);

