

app.factory('AuthenticateFactory',['$http','$q', function ($http, $q) {
    var url = "";
    var AuthenticateFactory = {};

    var Verify = function (Verify, baseAddress) {
       
        var deferred = $q.defer();
        url = baseAddress + "/Authenticate";
       
        $http.post(url, Verify).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
         
            deferred.reject(err);
        });

        return deferred.promise;
    };

   
    AuthenticateFactory.VerifyUser = Verify;
    return AuthenticateFactory;
}]);