    app.controller('PaymentHistoryController', ['$scope', '$rootScope', '$location', 'PaymentHistoryService', 'WebApiService', function ($scope, $rootScope, $location, PaymentHistoryService, WebApiService) {
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.AccountHolder = sessionStorage.getItem('AccountHolder');
    $scope.CurrentBalance = sessionStorage.getItem('CurrentBalance') == "null" ? "0.00" : sessionStorage.getItem('CurrentBalance');
    $scope.dataHolder = '';
    $scope.fromDate = '';
    $scope.todate = '';
   // $scope.loading = true;
    $scope.Valid = true;
    function GetPaymentHistory() {

       // $scope.loading = true;
        PaymentHistoryService.PaymentHistoryGet($scope.AccountNumber, $scope.baseAddress).success(function (data) {
            $scope.dataHolder = data;
            $scope.loading = false;

        }).error(function (data) {
            $scope.Error = "Server connection problem ";
            $scope.loading = false;
        });
    };

        (function ()
        {
            $scope.loading = true;
            PaymentHistoryService.LastPaymentGet($scope.AccountNumber, $scope.baseAddress).success(function (data)
            {            
            var amount = parseFloat(JSON.parse(data.TranPayment == null ? "0.00" : data.TranPayment));
            var fee = parseFloat(JSON.parse(data.TranFee == null ? "0.00" : data.TranFee));
            var totalPayment = parseFloat(amount) + parseFloat(fee);
            var lastPayment = totalPayment == 0 ? "0.00" : parseFloat(totalPayment).toFixed(2);
            $scope.lastPayment = "$" + lastPayment;
            GetPaymentHistory();
        }).error(function (data)
        {         
            $scope.Error = "Server connection problem ";
            $scope.loading = false;
        });

    })();


    $scope.Gobacktomyaccount = function()
    {     
        $rootScope.loggedIn = true;
        $rootScope.sectionPayment = 1;
        $location.path('/PaymentAmount');
    }
  
    $scope.dateRangeFilter = function () {
        $scope.loading = true;
        var AccountNumber = $scope.AccountNumber;
        var FromDate = new Date($scope.fromDate);
        var ToDate = new Date($scope.todate);
        var collection = { "AccountNumber": AccountNumber, "FromDate": FromDate, "ToDate": ToDate };

        PaymentHistoryService.Paymentsearch(collection, $scope.baseAddress).success(function (data) {
            $scope.dataHolder = data;
            $scope.loading = false;
        }).error(function (data) {

            $scope.Error = "Server connection problem ";
        });


    };

    $scope.ViewPaymentHistory = function(data)
    {
        $scope.bankNameShow = true;
        if (data.BankName == null)
        {
            $scope.bankNameShow = false;

        }
        $scope.ViewBankHolder = data.BankHolder;
        $scope.ViewBankName = data.BankName;
        $scope.ViewRoutingNumber = data.BankABA;
        $scope.ViewConfirmation = data.ConfirmationID;
        $scope.ViewTranDate = data.TranDate;
        $scope.BankAcctNo = data.BankAcctNo;
        $scope.ViewTranPayment = "$"+data.TranPayment;
        $scope.ViewTranFee = "$" + data.TranFee;
        $scope.BankAcctType = data.BankAcctType;
        var amount = JSON.parse(data.TranPayment);
        var fee = JSON.parse(data.TranFee);
        var totalAmount = parseFloat(amount) + parseFloat(fee);
        $scope.ViewTotalAmount = "$" + parseFloat(totalAmount).toFixed(2);
        $scope.ViewType = data.BankAcctType == "S" ? "Savings Account" : data.BankAcctType == "P" ? "Checking Personal" : data.BankAcctType == "V" ? "Card" : "Card";
        $scope.ViewStatus = data.Status;       
    }

    $scope.validateDates = function ()
    {
        if ($scope.fromDate == undefined || $scope.fromDate == "")
        {
            $scope.toError = "To date must be fill after From date.";
            $scope.todate = "";
            $scope.dateError = true;
            $scope.Valid = true;
        }
        else if ($scope.todate == undefined || $scope.todate == "") {
            $scope.toError = "To Date cannot be blank.";
            $scope.todate = "";
            $scope.dateError = true;
            $scope.Valid = true;
        }
        else if ($scope.todate != "" && new Date($scope.todate) < new Date($scope.fromDate)) 
        {
            $scope.toError = "To date must be greater than From date.";
            $scope.dateError = true;
            $scope.Valid = true;
        }
        else
        {
            $scope.dateError = false;
            $scope.Valid = false;
        }
    }
  

}]);
