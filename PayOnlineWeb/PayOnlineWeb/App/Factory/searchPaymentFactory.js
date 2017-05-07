app.factory('searchPaymentFactory',['$http','$q', function ($http, $q) {
    var url = "";
    var searchPaymentFactory = {};
     var GetSearchPayment =  function (AccountNumber, baseAddress) {
            url = baseAddress + "/PayOnline/GetSearchPaymentDetails/"+ AccountNumber;
            return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
     }

     var SearchPaymentSchedule = function (collection, baseAddress) {        
        url = baseAddress + "/Payonline/SearchPaymentDetails";
        return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
     }

     var CancelSchedulePayment = function (collection, baseAddress) {
         url = baseAddress + "/Payonline/CancelSchedulePayment/" ;
         return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
     }

     
     searchPaymentFactory.GetSearchPayment = GetSearchPayment;
     searchPaymentFactory.SearchPaymentSchedule = SearchPaymentSchedule;
     searchPaymentFactory.CancelSchedulePayment = CancelSchedulePayment;
     return searchPaymentFactory;
}]);