
app.factory('LoginFactory',['$http','$q','AuthenticationService','authData', function ($http, $q,AuthenticationService,authData) {
    var url = "";
    var loginServiceFactory = {};

    //var userInfo;
    //var loginServiceURL = 'http://localhost:49512/' + 'token';
    //var deviceInfo = [];
    //var deferred;
    //var tosp = "";

    var login = function (data, baseAddress) {       
        var deferred = $q.defer();
          url = baseAddress + "/Login/LoginUser";
        $http.post(url, data).success(function (response)
        {
            deferred.resolve(response);
        }).error(function (err, status)
        {
           
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var logOut = function ()
    {
        authenticationService.removeToken();
        authData.authenticationData.IsAuthenticated = false;
        authData.authenticationData.userName = "";
    }
    loginServiceFactory.loginUser = login;

    var CheckNSFStatus = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayOnline/CheckNSFStatus/" + accountNumber;
        return $http.get(url);
    };

    loginServiceFactory.loginUser = login;
    loginServiceFactory.CheckNSFStatus = CheckNSFStatus;
    return loginServiceFactory;
}]);
