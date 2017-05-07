
app.factory('LoginFactory',['$http','$q','AuthenticationService','authData', function ($http, $q,AuthenticationService,authData) {
    var url = "";
    var loginServiceFactory = {};

    var userInfo;
    var deviceInfo = [];
    var deferred;
    var tosp = "";

    var login = function (collection, baseAddress) {
        var deferred = $q.defer();
        debugger;
        if ((sessionStorage.getItem('Token') != undefined) && (sessionStorage.getItem('Token') != null) && (sessionStorage.getItem('Token') != "")) {

            delete $http.defaults.headers.common['Authorization'];
            delete $http.defaults.headers.common['Content-Type'];
        }
        var tokenUrl = baseAddress.split("api");
        url = tokenUrl[0] + 'token';
        var data = "userName=" + encodeURIComponent(collection.Email) + "&password=" + encodeURIComponent(collection.Password) + "&Scope=" + encodeURIComponent(collection.AccountNumber) + "&grant_type=password";
        $http.post(url, data, {
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).success(function (response)
        {
            debugger;
            if (response.IsSuccess === "True")
            {
                sessionStorage.setItem('Token', response.access_token);
                setHeader($http, response.access_token);
            }

            deferred.resolve(response);
        }).error(function (err, status)
        {
            
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var setHeader = function (http, token) {
        delete http.defaults.headers.common['X-Requested-With'];
        http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
        http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
    }

    var logOut = function ()
    {
        authenticationService.removeToken();
        authData.authenticationData.IsAuthenticated = false;
        authData.authenticationData.userName = "";
    }
    loginServiceFactory.loginUser = login;

    var CheckNSFStatus = function (accountNumber, baseAddress) {
        url = baseAddress + "/PayOnline/CheckNSFStatus/" + accountNumber;
        return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
    };

    loginServiceFactory.loginUser = login;
    loginServiceFactory.CheckNSFStatus = CheckNSFStatus;
    return loginServiceFactory;
}]);
