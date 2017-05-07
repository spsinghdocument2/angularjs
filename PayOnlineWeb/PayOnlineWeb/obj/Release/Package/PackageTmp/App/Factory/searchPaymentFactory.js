app.factory('searchPaymentFactory',['$http','$q', function ($http, $q) {
    var url = "";
    var searchPaymentFactory = {};
     var GetSearchPayment =  function (AccountNumber, baseAddress) {
            url = baseAddress + "/PayOnline/GetSearchPaymentDetails/"+ AccountNumber;
            return $http.get(url);
     }

     var SearchPaymentSchedule = function (collection, baseAddress) {        
        url = baseAddress + "/Payonline/SearchPaymentDetails";
        return $http.post(url, collection);
     }

     var CancelSchedulePayment = function (collection, baseAddress) {
         url = baseAddress + "/Payonline/CancelSchedulePayment/" ;
         return $http.post(url, collection);
     }

     
     searchPaymentFactory.GetSearchPayment = GetSearchPayment;
     searchPaymentFactory.SearchPaymentSchedule = SearchPaymentSchedule;
     searchPaymentFactory.CancelSchedulePayment = CancelSchedulePayment;
     return searchPaymentFactory;
}]);