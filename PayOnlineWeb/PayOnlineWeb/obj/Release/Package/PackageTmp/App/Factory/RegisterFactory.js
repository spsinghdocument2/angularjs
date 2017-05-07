
app.factory('RegisterFactory',['$http','$q', function ($http, $q) {
    var url = "";
    var RegisterServiceFactory = {};



    var SecurityQuestions = function () {

        var deferred = $q.defer();
        url = "data/SecurityQuestionsTable.json";

        $http.get(url).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var Save = function (collection, baseAddress) {
       
        var deferred = $q.defer();
        url = baseAddress + "/Register";

        $http.post(url, collection).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
         
            deferred.reject(err);
        });

        return deferred.promise;
    };


    
    RegisterServiceFactory.GetSecurityQuestions = SecurityQuestions;
    RegisterServiceFactory.SaveUser = Save;
    return RegisterServiceFactory;

}]);