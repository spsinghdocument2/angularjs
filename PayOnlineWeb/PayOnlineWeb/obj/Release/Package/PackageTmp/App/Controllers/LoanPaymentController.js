app.controller('LoanPaymentController', ['$scope', '$rootScope', '$location', 'LoanPaymentService', 'WebApiService', function ($scope, $rootScope, $location, LoanPaymentService, WebApiService) {

    $scope.baseAddress = WebApiService.baseAddress;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $rootScope.Email = sessionStorage.getItem('Email');
    $rootScope.alerts = [];
    $rootScope.alertCount = 0;
    $scope.loading = true;
    $scope.ActionLogSubType = "";
    $scope.AcctNextDueDate = "";
    $scope.TranDate = "";

    var Alerts = function () {
        this.Head = '';
        this.Description = '';
        this.Action = '';
        this.ActionHeader = '';
    };
    var AlertsItems = [];

    (function () {     
        LoanPaymentService.LoanPaymentGet($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            LoanPaymentService.GetAlerts($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            
                AlertsItems = [];
       
                for (var counter = 0; counter < data.length; counter++)
                {
                    var item = new Alerts();
                    item.Head = data[counter].Head;
                    item.Description = data[counter].Description;
                    item.Action = data[counter].Action;
                    item.ActionHeader = data[counter].ActionHeader;
                    AlertsItems.push(item);
                }

                $rootScope.alerts = AlertsItems;
                $rootScope.alertCount = data.length;
            }, function (error) {
                if (error == "") {
                    $scope.Error = "Server connection problem";
                }
                else {
                    $scope.Error = error.Message;
                }
            });

            LoanPayment(data);
        }, function (error) {
            if (error == "")
            {
                $scope.Error = "Server connection problem";
            }
            else
            {
  
                $scope.Error = error.Message;;
            }         
        });
        
    })();
    function LoanPayment(data)
    {
        $scope.CurrentBalance =data.AcctCurrentBal;
        $scope.AmountDue = data.AcctPastDueAmt == null ? "$0.00" : "$" + data.AcctPastDueAmt;
        $scope.DaysDue = data.AcctDaysPastDue;
        if (parseInt(data.AcctDaysPastDue) > 0 && data.AcctDaysPastDue!='')
        {
            $scope.PastDueValue = {
                "border": "1px solid #f00",
                "color":"#f00"               
            };
            $scope.DayPastText = {
                "background": "#f00",
                "border": "1px solid #f00"
            }
        }
        $scope.LastReferenceNumber = data.ConfirmationID == null ? "Not Available" : data.ConfirmationID;
        $scope.LastTransactionDate = data.TranDate;
        $scope.LastPayment = parseFloat(JSON.parse(data.TranPayment));
        $scope.LastFee = parseFloat(JSON.parse(data.TranFee));
        var lastTotalamount = $scope.LastPayment + $scope.LastFee;
        $scope.LastTotalamount = isNaN(lastTotalamount) == true ? "$0.00" : "$" +parseFloat(lastTotalamount).toFixed(2);
        $rootScope.LastTotalamount = $scope.LastTotalamount;
        $scope.LastRouting = data.BankABA;
        $scope.LastBankAccountNumber = data.BankAcctNo;
        $scope.BankName = data.BankName;
        $scope.BankHolde = data.BankHolder;
        var lastPaymentMade = data.BankAcctType == "S" ? "Savings Account" : data.BankAcctType == "P" ? "Checking Personal" : data.BankAcctType == "V" ? "Visa" : data.BankAcctType == "M" ? "MasterCard" : "Not Available";
        $scope.ActionLogSubType = data.ActionLogSubType;
        $scope.AcctNextDueDate = data.NextDueDate;
        $scope.TranDate = data.LastTransactionDate;
        $scope.PaymentMethod = lastPaymentMade ;
        $scope.Status = data.Status;
        $rootScope.BankHolde = $scope.BankHolde;
        
        $rootScope.CurrentBalance = $scope.CurrentBalance;
        $scope.PaymentFrequency = data.AcctFreq == "W" ? "Weekly" : data.AcctFreq == "M" ? "Monthly" : data.AcctFreq == "Y" ? "Yearly" : data.AcctFreq == "B" ? "Bi-Weekly" : data.AcctFreq == "S" ? "Semi-Monthly" : data.AcctFreq == "D" ? "Daily" : data.AcctFreq;
        $rootScope.PaymentFrequency = $scope.PaymentFrequency;
        $rootScope.PaymentFrequency2 = data.AcctFreq;
        var BaseData=$scope.baseAddress.substr(0, $scope.baseAddress.lastIndexOf('/'));
        if (data.ProfilePicture != null) { $rootScope.ProfilePicture = BaseData + data.ProfilePicture.substr(1); }
        else { $rootScope.ProfilePicture = '../../images/imgres.png'; }
       
       sessionStorage.setItem('lastPaymentMade', lastPaymentMade);
        $scope.loading = false;

    }
  

}]);

