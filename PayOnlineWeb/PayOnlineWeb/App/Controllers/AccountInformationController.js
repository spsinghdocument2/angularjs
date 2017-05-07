
app.controller('AccountInformationController', ['$scope', '$rootScope', '$location', 'AccountInformationService', 'PaymentConfirmationService', 'WebApiService', 'vcRecaptchaService', function ($scope, $rootScope, $location, AccountInformationService, PaymentConfirmationService, WebApiService, vcRecaptchaService) {

    $scope.PaymentFrequency = $rootScope.PaymentFrequency;
    $scope.loading = true;
    $scope.accountNumberGet = '';
    $scope.AccountHolder = '';
    $scope.CurrentBalance = '';
    $scope.AmountPastDue = '';
    $scope.RegularAmountDue = '';
    $scope.OtherAmount = '';
    $scope.TotalAmountDue = '';
    $scope.AccountHolderPhoneNumber = '';
    $scope.Error = '';
    $scope.CheckingAccountNumber = '';
    $scope.BankRoutingNumber = '';
    $scope.BankRoutingError = '';
    $scope.ErrorRouting = '';
    $scope.BankName = '';
    $scope.loadingOverlay = "normal";
    $rootScope.holderData = '';
    $scope.BankAccount = "";
    $scope.BankAccountName = "";
    $scope.FuturePaymentDate = "";
    $scope.email = "";
    $scope.DueDate = "";

    $scope.LastTotalamount = $rootScope.LastTotalamount;
   

    $scope.EditPaymentAmount = false;
    $scope.disabled = true;
    $scope.disabledPayoffTotalAmount = true;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.lastPaymentMade = "";
    var AccountNumber = $scope.AccountNumber;
    $scope.baseAddress = WebApiService.baseAddress;

    (function () {

        var count = $rootScope.sectionPayment++;


        $rootScope.holderData = PaymentConfirmationService.PaymentConfirmationGet();
        if ($rootScope.holderData != "") {

            $scope.EditPaymentAmount = true;
            // var PaymnetAmount = $scope.Amount.name == "totalAmount" ? $scope.TotalAmountDue : $scope.OtherAmount;

            var data = JSON.parse($rootScope.holderData);
            $scope.Amount.name = data.AmountName;
            $scope.Amount.name == "otherAmount" ? $scope.disabled = false : $scope.disabled = true;
            //  $scope.Amount.name == "totalAmount" ? $scope.TotalAmountDue : $scope.OtherAmount;
            //  $scope.PaymnetAmount = data.PaymnetAmount;           
            $scope.accountNumberGet = data.AccountNumber;
            $scope.AccountHolder = data.FullName;
            $scope.BankName = data.BankName;
            $scope.BankRoutingNumber = data.RoutingNumber;
            $scope.CheckingAccountNumber = data.BankAccountNumber;
            $scope.rdAccountType.name = data.AccountType;

            $scope.CurrentBalance = data.CurrentBalance;
            $scope.AmountPastDue = data.AmountPastDue;
            $scope.RegularAmountDue = data.RegularAmountDue;
            $scope.OtherAmount = data.OtherAmount;
            $scope.TotalAmountDue = data.TotalAmountDue;

            $scope.AccountHolderPhoneNumber = data.AccountHolderPhoneNumber;
            $scope.loadingOverlay = "none";

            $scope.AchPaymentDetails.name = data.AchPaymentDetails;
            $scope.checkboxFutureAccount.value = data.FutureAccount;
            $scope.BankAccount = data.OriginalBankAccountNumber;
            //  $scope.checkboxRecurring.value = data.RecurringPayment;
            $scope.checkBoxSaveTimeMoney.value = data.SaveTimeAndMoney;
            $scope.email = data.Email;

            $scope.TotalPayoffAmmount = data.TotalPayoffAmmount;
            $scope.DueDate = data.DueDate;

            $scope.PaymentType.name = data.PaymentType;
            $scope.CheckingAccountNumberDisplay = data.BankAccountNumber;
            $scope.previousAccountShow = data.previousAccountShow;
            //  alert(data.previousAccountShow);

            if ($scope.checkBoxSaveTimeMoney.value == true) {
                $scope.PaymentFrequencyVisible = true;
            }
            else {
                $scope.PaymentFrequencyVisible = false;
            }

            $scope.BankAccountName = data.BankAccountName;
            $scope.FuturePaymentDate = data.FuturePaymentDate;
            CancelEditcollectio = JSON.stringify({ PavmentDate: data.PavmentDate, PaymnetAmount: data.PaymnetAmount, AccountNumber: data.AccountNumber, FullName: data.FullName, BankName: data.BankName, RoutingNumber: data.RoutingNumber, BankAccountNumber: data.BankAccountNumber, AccountType: data.AccountType, AmountName: data.AmountName, CurrentBalance: data.CurrentBalance, AmountPastDue: data.AmountPastDue, RegularAmountDue: data.RegularAmountDue, OtherAmount: data.OtherAmount, TotalAmountDue: data.TotalAmountDue, AccountHolderPhoneNumber: data.AccountHolderPhoneNumber, Email: data.Email, BankAccountName: data.BankAccountName, PaymentType: data.PaymentType, FutureAccount: data.FutureAccount, AchPaymentDetails: data.AchPaymentDetails, TotalPayoffAmmount: data.TotalPayoffAmmount, DueDate: data.DueDate, FuturePaymentDate: data.FuturePaymentDate, OriginalBankAccountNumber: data.OriginalBankAccountNumber, previousAccountShow: data.previousAccountShow });

            PaymentConfirmationService.PaymentConfirmationHolder("");
            $scope.loading = false;

        }
        else {
            PaymentConfirmationService.PaymentConfirmationHolder("");
            AccountInformationService.UserAccountInformation(AccountNumber, $scope.baseAddress).success(function (data) {

                userData(data);

            }).error(function (error) {
                $scope.loading = false;
                if (error == "") {
                    $scope.MainError = "Server connection problem";
                }
                else {

                    $scope.MainError = error.Message;;
                }

            });
        }

    })();



    function userData(data) {
        $scope.accountNumberGet = data.AccountNumber;
        $scope.AccountHolder = data.AccountHolder;
        $scope.CurrentBalance = data.CurrentBalance;
        $scope.AmountPastDue = data.AmountPastDue;
        $scope.RegularAmountDue = data.RegularAmountDue;
        $scope.OtherAmount = data.OtherAmount;
        $scope.TotalAmountDue = data.TotalAmountDue;
        $scope.AccountHolderPhoneNumber = data.AccountHolderPhoneNumber;
        // $scope.DueDate = data.DueDate;
        var DueDate = data.DueDate.toString().split(" ").slice(0, 3).join(" ");
        var dateDue = new Date(DueDate);

        $scope.DueDate = dateDue;

        $scope.TotalPayoffAmmount = parseFloat(data.TotalPayoffAmmount).toFixed(2);
      //  var lastPaymentMade = data.BankAcctType == "S" ? "ACH - Savings Account" : data.BankAcctType == "P" ? "ACH - Checking Account" : data.BankAcctType == "V" ? "Visa" : data.BankAcctType == "M" ? "Master" : "Not Available";
      

        $scope.lastPaymentMade = data.LastBankAccountType == "S" ? "ACH - Savings Account" : data.LastBankAccountType == "P" ? "ACH - Checking Account" : data.LastBankAccountType == "V" ? "Visa" : data.LastBankAccountType == "M" ? "MasterCard" : "Not Available";



        $scope.LastPayment = parseFloat(JSON.parse(data.LastTransactionPayment));
        $scope.LastFee = parseFloat(JSON.parse(data.LastTransactionFee));
        var lastTotalamount = $scope.LastPayment + $scope.LastFee;
        $scope.LastTotalamount = isNaN(lastTotalamount) == true ? "$0.00" : "$" + parseFloat(lastTotalamount).toFixed(2);
        $rootScope.LastTotalamount = $scope.LastTotalamount;


        $scope.Error = data.Error;
        $scope.email = sessionStorage.getItem('Email');
        $scope.loadingOverlay = 'none';
        $scope.loading = false;

    }

    

    $scope.Recaptchasignup = function () {

        try {       
            //if ($scope.response == null)
            //{
            //    $scope.MainError = "Please resolve the captcha and Continue";
            //}
            //else
            //{

            // vcRecaptchaService.reload($scope.recaptchaId);
            $rootScope.loggedIn = true;
            $rootScope.leftMenu = true;
            $rootScope.userProfile = true;
            $location.path('/PaymentAmount');

            // }
        }
        catch (e) {

        }
    };

    $scope.FuturePayAccountInfoContinue = function ()
    {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/FuturePay');
    }

}]);




