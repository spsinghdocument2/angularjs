app.controller('PaymentConfirmationController', ['$scope', '$rootScope', '$location', '$window', 'PaymentConfirmationService', 'WebApiService', function ($scope, $rootScope, $location, $window, PaymentConfirmationService, WebApiService) {
    $scope.PavmentDate = '';
    $scope.PaymnetAmount = '';
    $rootScope.AccountNumber = '';
    $scope.FullName = '';
    $scope.BankName = '';
    $scope.RoutingNumber = '';
    $scope.BankAccountNumber = '';
    $scope.AccountType = '';
    $scope.AmountName = '';
    $scope.CurrentBalance = '';
    $scope.AmountPastDue = '';
    $scope.RegularAmountDue = '';
    $scope.OtherAmount = '';
    $scope.TotalAmountDue = '';
    $scope.AccountHolderPhoneNumber = '';
    $scope.Fee = '';
    $scope.OriginalBankAccountNumber = '';
    $scope.AchPaymentDetails = ''
    $scope.FutureAccount = '';
    $scope.Email = '';
    $scope.BankAccountName = '';
    $scope.FuturePaymentDate = '';
    $scope.SaveTimeAndMoney = '';

    $scope.totalamount = '';
    $scope.confirmation = '';
    $scope.name = '';
    $scope.ThirdDiv = false;
    $scope.btnSubmit = false;
    $scope.PaymentType = '';
    $scope.TotalPayoffAmmount = '';
    $scope.DueDate = '';
    $scope.previousAccountShow = true;
    $scope.PaymentTimeOption = '';
    $scope.FutureAmount = '';
    $scope.FutureOtherAmount = '';
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.firstDiv = true;
    $scope.secondDiv = false;
    $scope.savedAch = [];
    (function () {
        var holderData = PaymentConfirmationService.PaymentConfirmationGet();

        if (holderData == "") {
            $rootScope.leftMenu = false;
            $rootScope.userProfile = false;
            $rootScope.hideMenu = "hide";
            $location.path('/login');
            window.location.reload();
        }

        var data = JSON.parse(holderData);
        $scope.PavmentDate = data.PavmentDate;
        $scope.PaymnetAmount = data.PaymnetAmount;
        $scope.ViewPaymnetAmount = "$" + parseFloat(data.PaymnetAmount).toFixed(2);
        $rootScope.AccountNumber = data.AccountNumber;
        $scope.FullName = data.FullName;
        $scope.BankName = data.BankName;
        $scope.RoutingNumber = data.RoutingNumber;
        $scope.BankAccountNumber = data.BankAccountNumber;
        $scope.AccountType = data.AccountType;       
        $scope.AccountTypeGet = $scope.AccountType == "Saving" ? "Savings Account" : $scope.AccountType == "Savings" ? "Savings Account" : "Checking Personal";
        $scope.name = $scope.AccountTypeGet == "Savings Account" ? "Name on the ACH" : $scope.AccountTypeGet == "Checking Personal" ? "Name on the ACH" : "Name on the Debit/Credit Card";
        $scope.AmountName = data.AmountName; 
        $scope.CurrentBalance = data.CurrentBalance;
        $scope.AmountPastDue = data.AmountPastDue;
        $scope.RegularAmountDue = data.RegularAmountDue;
        $scope.OtherAmount = parseFloat(data.OtherAmount);
        $scope.TotalAmountDue = parseFloat(data.TotalAmountDue);
        $scope.AccountHolderPhoneNumber = data.AccountHolderPhoneNumber.replace(/-/g, "");
        $scope.Email = data.Email;
        $scope.OriginalBankAccountNumber = data.OriginalBankAccountNumber;
        $scope.AchPaymentDetails = data.AchPaymentDetails;
        $scope.FutureAccount = data.FutureAccount;
        $scope.BankAccountName = data.BankAccountName;
        $scope.FuturePaymentDate = data.FuturePaymentDate;
        $scope.SaveTimeAndMoney = data.SaveTimeAndMoney;
        $scope.PaymentType = data.PaymentType;
        $scope.TotalPayoffAmmount = data.TotalPayoffAmmount;
        $scope.DueDate = data.DueDate;
        $scope.previousAccountShow = data.previousAccountShow;
        $scope.savedAch = data.savedAch;
        $scope.PaymentTimeOption = data.PaymentTimeOption;
        $scope.FutureAmount = data.FutureAmount;
        $scope.FutureOtherAmount = data.FutureOtherAmount;
        PaymentConfirmationService.PaymentConfirmationHolder("");
    })();
    (function () {
        $scope.loading = true;
        PaymentConfirmationService.FeeAmountGet($rootScope.AccountNumber, $scope.baseAddress).success(function (data) {
            var Fee = parseFloat(JSON.parse(data));
            var PaymnetAmount = parseFloat($scope.PaymnetAmount);
            $scope.Fee = Fee;
            $scope.ViewFee = "$" + JSON.parse(data);
            $scope.totalamount = parseFloat(PaymnetAmount + Fee);
            $scope.ViewTotalamount = "$" + parseFloat(PaymnetAmount + Fee).toFixed(2);
            $scope.loading = false;
        }).error(function (error) {
            $scope.loading = false;
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }
        });

    })();



    $scope.edit = function () {
        var PavmentDate = $scope.PavmentDate;
        var PaymnetAmount = $scope.PaymnetAmount;
        var AccountNumber = $rootScope.AccountNumber;
        var FullName = $scope.FullName;
        var BankName = $scope.BankName;
        var RoutingNumber = $scope.RoutingNumber;
        var BankAccountNumber = $scope.BankAccountNumber;
        var AccountType = $scope.AccountType;
        var AmountName = $scope.AmountName;
        var CurrentBalance = $scope.CurrentBalance;
        var AmountPastDue = $scope.AmountPastDue;
        var RegularAmountDue = $scope.RegularAmountDue;
        var OtherAmount = $scope.OtherAmount;
        var TotalAmountDue = $scope.TotalAmountDue;
        var AccountHolderPhoneNumber = $scope.AccountHolderPhoneNumber.replace(/-/g, "");
        var OriginalBankAccountNumber = $scope.OriginalBankAccountNumber;
        var AchPaymentDetails = $scope.AchPaymentDetails;
        var FutureAccount = $scope.FutureAccount;
        var Email = $scope.Email;
        var BankAccountName = $scope.BankAccountName;
        var FuturePaymentDate = $scope.FuturePaymentDate;
        var SaveTimeAndMoney = $scope.SaveTimeAndMoney;
        var PaymentType = $scope.PaymentType;
        var TotalPayoffAmmount = $scope.TotalPayoffAmmount;
        var DueDate = $scope.DueDate;
        var previousAccountShow = $scope.previousAccountShow;
        var savedAch = $scope.savedAch;
        var PaymentTimeOption = $scope.PaymentTimeOption;
        var FutureAmount = $scope.FutureAmount;
        var FutureOtherAmount = $scope.FutureOtherAmount;
       
        var AccountInformationJsionFormat = JSON.stringify({
            PavmentDate: PavmentDate, PaymnetAmount: PaymnetAmount, AccountNumber: AccountNumber, FullName: FullName, BankName: BankName, RoutingNumber: RoutingNumber, BankAccountNumber: BankAccountNumber, AccountType: AccountType, AmountName: AmountName, CurrentBalance: CurrentBalance, AmountPastDue: AmountPastDue, RegularAmountDue: RegularAmountDue, OtherAmount: OtherAmount, TotalAmountDue: TotalAmountDue,
            AccountHolderPhoneNumber: AccountHolderPhoneNumber, OriginalBankAccountNumber: OriginalBankAccountNumber, AchPaymentDetails: AchPaymentDetails, FutureAccount: FutureAccount, Email: Email, BankAccountName: BankAccountName, FuturePaymentDate: FuturePaymentDate, SaveTimeAndMoney: SaveTimeAndMoney, PaymentType: PaymentType, TotalPayoffAmmount: TotalPayoffAmmount, DueDate: DueDate,
            previousAccountShow: previousAccountShow, savedAch: savedAch, PaymentTimeOption: PaymentTimeOption, FutureAmount: FutureAmount, FutureOtherAmount: FutureOtherAmount
        });
       
        $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(AccountInformationJsionFormat);
        if ($scope.result == "Holder data successfully") {
            $rootScope.sectionPayment = 1;
            $location.path('/PaymentAmount');
        }
    }
    $scope.submit = function () {

        $scope.loading = true;
       
        $scope.btnSubmit = true;
        var AccountNumber = $rootScope.AccountNumber;
        var PaymentAmount = parseFloat($scope.PaymnetAmount).toFixed(2);
        var FeeAmoun = parseFloat($scope.Fee).toFixed(2);
        var RountingNumber = $scope.RoutingNumber;
        var BankAccountNumber = $scope.OriginalBankAccountNumber;
        var BankName = $scope.BankName;
        var BankHolder = $scope.FullName; 
        var AccountType = $scope.AccountType == "Saving" ? "S" : $scope.AccountType == "Savings" ? "S": "P";
        var UpdatedPhone = $scope.AccountHolderPhoneNumber.replace(/-/g, "");
        var UpdatedPhoneDate = $scope.PavmentDate;
        var RecurringPayment = $scope.RecurringPayment;
        // var Email = sessionStorage.getItem('Email');
        var Email = $scope.Email;
        var SaveAccountFuture = $scope.FutureAccount == true ? "1" : "0";
        var BankAccountName = $scope.BankAccountName;
        var FuturePaymentDate = $scope.FuturePaymentDate;

        var paymentConfirmationdata = { "AccountNumber": AccountNumber, "PaymentAmount": PaymentAmount, "FeeAmoun": FeeAmoun, "RountingNumber": RountingNumber, "BankAccountNumber": BankAccountNumber, "BankName": BankName, "BankHolder": BankHolder, "AccountType": AccountType, "UpdatedPhone": UpdatedPhone, "UpdatedPhoneDate": UpdatedPhoneDate, "RecurringPayment": RecurringPayment, "Email": Email, "SaveAccountFuture": SaveAccountFuture, "BankAccountName": BankAccountName, "FuturePaymentDate": FuturePaymentDate };

        //console.log(paymentConfirmationdata);
        PaymentConfirmationService.paymentConform(paymentConfirmationdata, $scope.baseAddress).success(function (data) {
            $scope.btnSubmit = false;
         
            $scope.loading = false;
            if (JSON.parse(data) == "You are trying to make the same payment again.") {
                $scope.Error = JSON.parse(data);
                $scope.loading = false;
                return;
            }
            $scope.confirmation = JSON.parse(data);
            $scope.firstDiv = false;
            $scope.secondDiv = true;

            $rootScope.AchPaymentAmountCheck = true;
            PaymentConfirmationAlert();
        }).error(function (error) {
            $scope.loading = false;
            $scope.btnSubmit = true;
          
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }


        });

    }

    function PaymentConfirmationAlert() {
        $('#ConfirmationPaymentModal').modal({

            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    $scope.AutoPay = function () {        
        $location.path('/AutoPay');
    };
    $scope.Finish = function () {
        $location.path('/LoanPayment');
    };

    $scope.print = function ()
    {
        var paymentTransactionDataCollection = JSON.stringify({
            paymentDate: $scope.PavmentDate,
            paymentAmount: $scope.ViewPaymnetAmount,
            fee: $scope.ViewFee,
            totalAmount: $scope.ViewTotalamount,
            accountNumber: $scope.AccountNumber,
            fullName: $scope.FullName,
            bankAccountName: $scope.BankAccountName,
            bankName: $scope.BankName,
            routingNumber: $scope.RoutingNumber,
            bankAccountNumber: $scope.BankAccountNumber,
            accountType: $scope.AccountType,
            confirmationNumber: $scope.confirmation
        });
       
        localStorage.paymentTransactionData = paymentTransactionDataCollection;
        $window.open('/PrintConfirmation', '_blank');
    }
    this.setFuturePayWithOutAgreement = function () {
        futurePayService.setFuturePayWithAgreement(false);
    };

}]);