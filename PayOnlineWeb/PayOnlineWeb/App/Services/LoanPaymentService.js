
app.service('LoanPaymentService',['$http','$q', function ($http, $q) {
    var url = "";
    this.LoanPaymentGet = function (acountNumber, baseAddress) {
      
        url = baseAddress + "/LoanPayment/GetLoanPayment/" + acountNumber;
        deferred = $q.defer();
        $http.get(url
            ).success(function (response) {
               
            deferred.resolve(response);
        })
        .error(function (err, status) {
         
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }

    this.GetAlerts = function (acountNumber, baseAddress) {
        debugger;
        url = baseAddress + "/LoanPayment/GetAlerts/" + acountNumber;
        deferred = $q.defer();
        $http.get(url).success(function (response) {
         
            deferred.resolve(response);
        })
        .error(function (err, status) {
          
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }
}]);

