
app.controller('searchPaymentController', ['$scope', '$rootScope', '$location', '$http', 'searchPaymentFactory', 'WebApiService', function ($scope, $rootScope, $location, $http, searchPaymentFactory, WebApiService) {    
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.loading = true;
    $scope.dateError = false;
    var AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.Valid = true;
    GetSearchScheduleDetails();
    function GetSearchScheduleDetails() {
        searchPaymentFactory.GetSearchPayment(AccountNumber, $scope.baseAddress).then(function (data) {

            $scope.dataHolder = data.data;
            $scope.loading = false;
        },
           function (err) {
               $scope.model = "Get Search Payment Faild!";
               $scope.loading = false;
           });

    };

    $scope.validateDates = function () {
        if ($scope.fromDate == undefined) {
            $scope.Error = "To date must be fill after From date.";
            $scope.todate = "";
            $scope.dateError = true;
            $scope.Valid = true;
        }
        else if ($scope.todate != "" && new Date($scope.todate) < new Date($scope.fromDate)) {
            $scope.Error = "To date must be greater than From date.";
            $scope.dateError = true;
            $scope.Valid = true;
        }
        else {
            $scope.dateError = false;
            $scope.Valid = false;
        }
    };


    $scope.dateRangeFilter = function () {
        $scope.loading = true;
        var AccountNumber = sessionStorage.getItem('AccountNumber');
        var FromDate = new Date($scope.fromDate);
        var ToDate = new Date($scope.todate);
        var collection = { "AccountNumber": AccountNumber, "FromDate": FromDate, "ToDate": ToDate };

        searchPaymentFactory.SearchPaymentSchedule(collection, $scope.baseAddress).success(function (data) {
            $scope.dataHolder = data;
            $scope.loading = false;
        }).error(function (data) {

            $scope.Error = "Server connection problem ";
        });


    };

    $scope.CancelSchedulePayment = function (data)
    {
      
        $scope.loading = true;
        searchPaymentFactory.CancelSchedulePayment(data, $scope.baseAddress).success(function (data) {
            GetSearchScheduleDetails();
            $scope.loading = false;
        }).error(function (data) {

            $scope.Error = "Server connection problem ";
        });
    };

}]);




