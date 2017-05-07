app.service('AutoPayAgreementService', ['$http', '$q', function ($http, $q) {
    var url = "";
    this.getAutoPayPaymentMethod = function (acountNumber, baseAddress) {

        url = baseAddress + "/AutoPayAgreement/GetAutoPayPaymentMethod/" + acountNumber;
        var deferred = $q.defer();
        $http.get(url).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (error, status) {
            deferred.reject(error);
        });
        return deferred.promise;
    };

    this.enrollAutoPay = function (collection, baseAddress) {
        url = baseAddress + "/AutoPayAgreement/EnrollToAutoPay";
        var deferred = $q.defer();
        $http.post(url, collection).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (error, status) {

            deferred.reject(error);
        });
        return deferred.promise;
    };   

}]);

