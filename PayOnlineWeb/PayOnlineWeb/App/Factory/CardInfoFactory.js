app.factory('CardInfoFactory', function ($http, $q) {

    var cardInfoFactory = {};
    var cardInfo = function (accountNumber, baseAddress) {
       
        var deferred = $q.defer();
        url = baseAddress + "/CardInfo/GetCardInfo/";

        $http.get(url + accountNumber).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    cardInfoFactory.CardInfo = cardInfo;
    return cardInfoFactory;
})