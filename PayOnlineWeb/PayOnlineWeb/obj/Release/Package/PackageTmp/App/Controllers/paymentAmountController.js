app.controller('paymentAmountController', ['$scope', '$rootScope', '$location', 'AccountInformationService', 'PaymentConfirmationService','paymentAmountService', 'WebApiService', 'vcRecaptchaService', function ($scope, $rootScope, $location, AccountInformationService, PaymentConfirmationService,paymentAmountService, WebApiService, vcRecaptchaService) {

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
    $scope.DaysDue = false;
    var chkBilling = 0, chkUserProfile = 0;
    $scope.chkAccountFuture = false;
    $scope.EditBillingAddress = true;
    $scope.SavedBillingAddress = false;
    $scope.ShowNewCard = true;
    $scope.showNewAch = true;
    $scope.ACHRadio = false;
    var panels = {ACH: true,Card: false}
    var loadingCounter = 0;
    $scope.IsTotalAmountDue = false;
    function increaseLoadingCounter() {
        if (loadingCounter === 0)
            $scope.loading = true;
        loadingCounter++;
    }
    function decreaseLoadingCounter() {
        if (loadingCounter > 0) {
            loadingCounter--;
            if (loadingCounter === 0)
                $scope.loading = false;
        }
    }
    $scope.showHidePanel = panels;
    var savedCardItem = function () {
        this.TokenId = '';
        this.CardNumber = '';
        this.CardType = '';
        this.CardName = '';
        this.CardExpiry = '';
        this.CVV = '';
        this.Selected = false;
    };
    var savedCardItems = [];
    $scope.savedCards = {};
    $scope.SaveCardInfo = false;
    $scope.disableNewAch = false;
    var savedAchItem = function () {
        this.BankABA = '';
        this.BankAccountName = '';
        this.BankAcctType = '';
        this.BankHolder = '';
        this.BankName = '';
        this.BankAcctNo = '';
        this.Email = '';
        this.PrimaryNumber = '';
        this.BankAcctNoView = '';
        this.counter = '';
        this.ID = '';
        this.Selected = false;
    };
    var savedAchItems = [];
    $scope.savedAch = {};
    $scope.showSavedAch = false;    
    $scope.TokenId = '';
    $scope.SavedCvNumber = '';
    $scope.SavedCardName = '';
    $scope.SavedCardExpiry = '';
    $scope.SavedCardNumber = '';
    $scope.SavedCardType = '';
    $scope.btnNext2 = false;
    $scope.errorCvNumber = '';
    $scope.CardType ={Name: 'Visa'};
    $scope.TypeOfDebitCredit ={name: 'New'};
    $scope.chkFuture ={value: false};
    $scope.checkedDebitCredit = false;
    $scope.SaveCardDisabled = false;
    $scope.LastTotalamount = $rootScope.LastTotalamount;
    var CancelEditcollectio = JSON.stringify({});    
    $scope.collections ={data: {}};
    $scope.checkboxFutureAccount ={value: false};
    $scope.chkLastBillAddress ={value:false};
    $scope.chkuserProfileaddress ={value: false};
    $scope.checkBoxSaveTimeMoney ={value: false};
    $scope.AchPaymentDetails ={name: 'New'};
    $scope.AchDetail = {name: 'NewAchDetail'};
    $scope.Amount = {name: 'totalAmount'};
    $scope.rdAccountType ={name: 'Checking'};
    $scope.PaymentType ={name: 'ACH'};
    $scope.PaymentTimeOption ={name :'PayNow'};
    $scope.FutureAmount ={name: ''};   
    var ResetBankRoutingNumber = angular.copy($scope.BankRoutingNumber);
    var ResetCheckingAccountNumber = angular.copy($scope.CheckingAccountNumber);
    var BankAccountName = angular.copy($scope.BankAccountName);
    var Email = angular.copy($scope.email);
    $scope.EditPaymentAmount = false;
    $scope.disabled = true;
    $scope.disabledPayoffTotalAmount = true;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.CheckACN= sessionStorage.getItem('ACNCheck');
    $scope.lastPaymentMade = "";
    var AccountNumber = $scope.AccountNumber;
    $scope.baseAddress = WebApiService.baseAddress;
    var bindStateList = function () {
        increaseLoadingCounter();
        paymentAmountService.GetStateList().then(function (data) {
            var selectedState = $scope.State;
            var states = data.States;
            var selectedOption = states.find(function (state) { return state.Value === selectedState; });
            $scope.StateList = states;
            $scope.selectedState = selectedOption;
            decreaseLoadingCounter();
        },
          function (err) {
              decreaseLoadingCounter();
              $scope.ErrorMessage = "Get State Failed!";
          });
    }     
    $scope.removeErrorPrimaryNumber = function () {
        $scope.errorPrimaryNumber = "";
        $scope.submittedPrimaryNumber = "";
    };
    $scope.removeErrorBankAccountName = function () {
        $scope.errorBankAccountName = "";
        $scope.submittedBankAccountName = "";
    };
    $scope.removeErrorCheckingAccountNumber = function () {
        $scope.errorCheckingAccountNumber = "";
        $scope.submittedCheckingAccountNumber = "";
    };
    $scope.removeErrorRouting = function () {
        $scope.ErrorRouting = "";
        $scope.submittedRouting = "";
    }
    $scope.removeerrorEmail = function () {
        $scope.errorEmail = "";
        $scope.submittedEmail = "";
    }
    $scope.AccountInformationNext = function () {
        
        if ($scope.PaymentTimeOption.name == "PayNow") {
            if ($scope.Amount.name == "otherAmount") {
                var amount = $scope.OtherAmount;
                var regex = /^\d+(\.\d+)?$/;
                if (!regex.test(amount)) {$scope.otereAmountError = "Enter Only Amount";return;}
                var OtherAmount = parseFloat($scope.OtherAmount);
                if (parseInt($scope.AcctDaysPastDue) > 0 && OtherAmount < parseFloat($scope.TotalAmountDue)) {$scope.otereAmountError = "Minimum amount allowed: $" + $scope.TotalAmountDue;return;}
                if (parseFloat($scope.MinimumAmount) > OtherAmount) {$scope.otereAmountError = "Minimum amount allowed: $" + $scope.MinimumAmount;return;}
                if (parseFloat($scope.MaximumAmount) < OtherAmount) { $scope.otereAmountError = "Maximum amount allowed: $" + $scope.MaximumAmount; return; }
            }
            else if ($scope.Amount.name == "totalAmount") {
                if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
                { $scope.otereAmountError = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }
            }
        }
        else {
            if ($scope.FuturePaymentDate == "") { $scope.errorFuturePaymentDate = "Please Enter Future Date";return;}
            var FuturePaymentDate = $scope.FuturePaymentDate.toString().split(" ").slice(0, 3).join(" ");
            var dateDue = new Date($scope.DueDate);
            var FutureDate = new Date(FuturePaymentDate);
            if (dateDue < FutureDate) {$scope.errorFuturePaymentDate = "Future payment date must be less than due date.";return;};
            if ($scope.FutureAmount.name == "otherAmount") {
                var amount = $scope.FutureOtherAmount;
                var regex = /^\d+(\.\d+)?$/;
                if (!regex.test(amount)) {$scope.FutureotereAmountError = "Enter Only Amount";return;}
                var OtherAmount = parseFloat($scope.FutureOtherAmount);
                if (parseInt($scope.AcctDaysPastDue) > 0 && OtherAmount < parseFloat($scope.TotalAmountDue)) {
                    $scope.FutureotereAmountError = "Minimum amount allowed: $" + $scope.TotalAmountDue; return;
                }
                if (parseFloat($scope.MinimumAmount) > OtherAmount) {$scope.FutureotereAmountError = "Minimum amount allowed: $" + $scope.MinimumAmount;return;}
                if (parseFloat($scope.MaximumAmount) < OtherAmount) { $scope.FutureotereAmountError = "Maximum amount allowed: $" + $scope.MaximumAmount; return; }
            }
            else if ($scope.FutureAmount.name == "totalAmount") {
                if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
                { $scope.FutureotereAmountError = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        if ($scope.AchDetail.name === 'NewAchDetail')
        {
            var CheckingAccountNumberregex = /^\d*$/;
            var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
            var regexBankRoutingNumber = /^(\d+){9,}$/;
            var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if ($scope.AccountHolderPhoneNumber == "" || $scope.AccountHolderPhoneNumber == undefined) { $scope.errorPrimaryNumber = "Please Enter Account Holder Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid";   return; }
            else if (!regexPrimaryNumber.test($scope.AccountHolderPhoneNumber.replace(/-/g, ""))) { $scope.errorPrimaryNumber = "Enter Only Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid";   return; }
            else if ($scope.BankAccountName == "" || $scope.BankAccountName == undefined) { $scope.errorBankAccountName = "Please Enter Bank Account Name"; $scope.submittedBankAccountName = "submittedInvalid";   return; }
            else if ($scope.CheckingAccountNumber == "" || $scope.CheckingAccountNumber == undefined) { $scope.errorCheckingAccountNumber = "Please Enter " + $scope.rdAccountType.name + " Account Number"; $scope.submittedCheckingAccountNumber = "submittedInvalid";   return; };
            if (!CheckingAccountNumberregex.test($scope.CheckingAccountNumber)) { $scope.errorCheckingAccountNumber = "Enter Only Numbers"; $scope.submittedCheckingAccountNumber = "submittedInvalid";   return; }
            else if ($scope.BankRoutingNumber == "" || $scope.BankRoutingNumber == undefined) { $scope.ErrorRouting = "Please Enter Bank Routing Number"; $scope.submittedRouting = "submittedInvalid";   return; }
            else if (!regexBankRoutingNumber.test($scope.BankRoutingNumber)) { $scope.ErrorRouting = "Must be 9 numeric values"; $scope.submittedRouting = "submittedInvalid";   return; }
            else if ($scope.email == "" || $scope.email == undefined) { $scope.errorEmail = "Please Enter Email"; $scope.submittedEmail = "submittedInvalid";   return; }
            else if (!regexEmail.test($scope.email)) { $scope.errorEmail = "Not an email!"; $scope.submittedEmail = "submittedInvalid";  return; }
            var CheckingAccountNumber = $scope.CheckingAccountNumber;
            var Amount = "";

            if ($scope.PaymentTimeOption.name == 'PayNow') {
                if ($scope.Amount.name == "totalAmount") { Amount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
                else { Amount = $scope.OtherAmount; $scope.IsTotalAmountDue = false; }
            }
            else {

                if ($scope.FutureAmount.name == "totalAmount") { Amount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
                else { Amount = $scope.FutureOtherAmount; $scope.IsTotalAmountDue = false; }
            }
            var AccountNumber = $scope.accountNumberGet;
            var Routing = $scope.BankRoutingNumber;
            var FuturePaymentDate = $scope.FuturePaymentDate;
            var collection = { "CheckingAccountNumber": CheckingAccountNumber, "Amount": Amount, "AccountNumber": AccountNumber, "Routing": Routing, "FuturePaymentDate": FuturePaymentDate, 'IsTotalAmountDue': $scope.IsTotalAmountDue };
            increaseLoadingCounter();       
            AccountInformationService.AccountInformationNext(collection, $scope.baseAddress).success(function (data) {                             
                $scope.model = data;
                if (data.Error != null || data.ErrorRouting != null) {
                    $scope.MessageError = data.Error;
                    $scope.ErrorRouting = data.ErrorRouting;
                    $scope.errorFuturePaymentDate = data.ErrorFutureDate;
                    $scope.loading = false;return;}
                else {
                    if ($scope.PaymentTimeOption.name !== 'PayNow') {
                        var FuturePaymentDate = $scope.FuturePaymentDate;
                        var collectiondata = { "FuturePaymentDate": FuturePaymentDate };
                        increaseLoadingCounter();
                        AccountInformationService.FutureDateValidation(collectiondata, $scope.baseAddress).success(function (getData) {                      
                            if (getData.ErrorFutureDate != null) {$scope.errorFuturePaymentDate = getData.ErrorFutureDate;}
                            else {$scope.BankName = data.BankName;paymentConfirmation();}
                            decreaseLoadingCounter();
                        }).error(function (error) {
                            decreaseLoadingCounter();
                            if (error == "") {$scope.MainError = "Server connection problem";}
                else {$scope.MainError = error.Message;;}
                        });
                    }
                    else {$scope.BankName = data.BankName;paymentConfirmation();}
                }
                decreaseLoadingCounter();
            }).error(function (error) {
                decreaseLoadingCounter();
                if (error == "") {$scope.MainError = "Server connection problem";}
                else {$scope.MainError = error.Message;;}});
        }
        else {
            var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
            var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if ($scope.AccountHolderPhoneNumber == "" || $scope.AccountHolderPhoneNumber == undefined) { $scope.errorPrimaryNumber = "Please Enter Account Holder Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid";   return; }
            else if (!regexPrimaryNumber.test($scope.AccountHolderPhoneNumber.replace(/-/g, ""))) { $scope.errorPrimaryNumber = "Enter Only Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid";   return; }
            else if ($scope.email == "" || $scope.email == undefined) { $scope.errorEmail = "Please Enter Email"; $scope.submittedEmail = "submittedInvalid";  return; }
            else if (!regexEmail.test($scope.email)) { $scope.errorEmail = "Not an email!"; $scope.submittedEmail = "submittedInvalid";   return; }
            if ($scope.PaymentTimeOption.name !== 'PayNow') {
                var FuturePaymentDate = $scope.FuturePaymentDate;
                var collectiondata = { "FuturePaymentDate": FuturePaymentDate };
                increaseLoadingCounter();
                AccountInformationService.FutureDateValidation(collectiondata, $scope.baseAddress).success(function (data) {
                    if (data.ErrorFutureDate != null) {$scope.errorFuturePaymentDate = data.ErrorFutureDate;}
                    else {
                        //Payment through saved ach details
                        savedAchPayment();
        }
                    decreaseLoadingCounter();
                }).error(function (error) {
                    decreaseLoadingCounter();
                    if (error == "") {$scope.MainError = "Server connection problem";}
                    else {$scope.MainError = error.Message;;}
                });
            }
            else {
                //Payment through saved ach details
                savedAchPayment();
            }           
        }
    };
    $scope.removeOtereAmountError = function () {
        $scope.otereAmountError = '';
        $scope.totalAmountError = '';
        $scope.FutureoteretotalAmountError = '';
        $scope.FutureotereAmountError = '';
    };
    function paymentConfirmation() {        
        var str = "";
        var star = "X";
        var accountNumber = $scope.CheckingAccountNumber;
        var Length = accountNumber.length;
        var split = accountNumber.slice(-3);
        for (var i = 0; i < Length - 3; i++) {
            str += star + '';
        }
        var StarAccountNumber = str + split;
        var PavmentDate = $rootScope.TodayDate;
        var PaymnetAmount = "";
        var AmountName = "";
        if ($scope.PaymentTimeOption.name == 'PayNow')
        {
             PaymnetAmount = $scope.Amount.name == "totalAmount" ? $scope.TotalAmountDue : $scope.OtherAmount;
             AmountName = $scope.Amount.name;
        }
        else
        {
            PaymnetAmount = $scope.FutureAmount.name == "totalAmount" ? $scope.TotalAmountDue : $scope.FutureOtherAmount;
            PavmentDate = $scope.FuturePaymentDate;
            AmountName = $scope.FutureAmount.name;
        }
        var AccountNumber = $scope.accountNumberGet;
        var FullName = $scope.AccountHolder;
        var BankName = $scope.BankName;
        var RoutingNumber = $scope.BankRoutingNumber;
        var BankAccountNumber = StarAccountNumber;
        var AccountType = $scope.rdAccountType.name; 
        var CurrentBalance = $scope.CurrentBalance;
        var AmountPastDue = $scope.AmountPastDue;
        var RegularAmountDue = $scope.RegularAmountDue;
        var OtherAmount = $scope.OtherAmount;
        var TotalAmountDue = $scope.TotalAmountDue;
        var TotalPayoffAmmount = $scope.TotalPayoffAmmount;
        var DueDate = $scope.DueDate;
        var AccountHolderPhoneNumber = $scope.AccountHolderPhoneNumber.replace(/-/g, "");        
        var AchPaymentDetails = $scope.AchPaymentDetails.name
        var OriginalBankAccountNumber = '';
        var FutureAccount = $scope.checkboxFutureAccount.value;
        var BankAccountName = $scope.BankAccountName;
        var FuturePaymentDate = $scope.FuturePaymentDate;
        var SaveTimeAndMoney = $scope.checkBoxSaveTimeMoney.value;
        var Email = $scope.email;
        var PaymentType = $scope.PaymentType.name;
        var previousAccountShow = $scope.previousAccountShow;
        var PaymentTimeOption = $scope.PaymentTimeOption.name;
        var FutureAmount = $scope.FutureAmount.name;
        var FutureOtherAmount = $scope.FutureOtherAmount;                
        var chack = $scope.CheckingAccountNumber;
        var re = /[X]/;
        if (re.test(chack)) {OriginalBankAccountNumber = $scope.BankAccount;}
        else
        {
            var checkingAccountNumber = $scope.CheckingAccountNumber
            var regex = /^\d*$/;
            if (regex.test(checkingAccountNumber)) {OriginalBankAccountNumber = $scope.CheckingAccountNumber;}
            else {$scope.OriginalBankAccountNumberError = "Enter Only Numbers";return}
        }        
        var previousAccountShow = $scope.previousAccountShow;       
        var paymentConfirmationJsionFormat = JSON.stringify({
            PavmentDate: PavmentDate, PaymnetAmount: PaymnetAmount, AccountNumber: AccountNumber, FullName: FullName, BankName: BankName, RoutingNumber: RoutingNumber, BankAccountNumber: BankAccountNumber, AccountType: AccountType, AmountName: AmountName, CurrentBalance: CurrentBalance, AmountPastDue: AmountPastDue, RegularAmountDue: RegularAmountDue, OtherAmount: OtherAmount, TotalAmountDue: TotalAmountDue, AccountHolderPhoneNumber: AccountHolderPhoneNumber, Email: Email, OriginalBankAccountNumber: OriginalBankAccountNumber, AchPaymentDetails: AchPaymentDetails, FutureAccount: FutureAccount, BankAccountName: BankAccountName, FuturePaymentDate: FuturePaymentDate, SaveTimeAndMoney: SaveTimeAndMoney, PaymentType: PaymentType, TotalPayoffAmmount: TotalPayoffAmmount, DueDate: DueDate,
            previousAccountShow: previousAccountShow,
            savedAch: "", PaymentTimeOption: PaymentTimeOption, FutureAmount: FutureAmount, FutureOtherAmount: FutureOtherAmount
        });
        $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(paymentConfirmationJsionFormat);
        if ($scope.result == "Holder data successfully") {
            $rootScope.AchPaymentAmountCheck = false;
            $rootScope.loggedIn = true;
            $location.path('/PaymentConfirmation');
        }
    }
    function savedAchPayment()
    {      
        var str = "";
        var star = "X";
        var accountNumber = $scope.CheckingAccountNumber;
        var Length = accountNumber.length;
        var split = accountNumber.slice(-3);
        for (var i = 0; i < Length - 3; i++) {str += star + '';}
        var StarAccountNumber = str + split;
        var PavmentDate = $rootScope.TodayDate;            
        var PaymnetAmount = "";              
        if ($scope.PaymentTimeOption.name == 'PayNow') {
            if ($scope.Amount.name == "totalAmount") { PaymnetAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
            else { PaymnetAmount = $scope.OtherAmount; $scope.IsTotalAmountDue = false; }
            AmountName = $scope.Amount.name;
        }
        else {

            if ($scope.FutureAmount.name == "totalAmount") { PaymnetAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
            else { PaymnetAmount = $scope.FutureOtherAmount; $scope.IsTotalAmountDue = false; }
            AmountName = $scope.FutureAmount.name;
            PavmentDate = $scope.FuturePaymentDate;
        }
        var FullName, BankName, RoutingNumber, BankAccountNumber;
        var AccountType, Email, AccountHolderPhoneNumber, BankAccountName;        
        var CurrentBalance = $scope.CurrentBalance;
        var AmountPastDue = $scope.AmountPastDue;
        var RegularAmountDue = $scope.RegularAmountDue;
        var OtherAmount = $scope.OtherAmount;
        var TotalAmountDue = $scope.TotalAmountDue;
        var TotalPayoffAmmount = $scope.TotalPayoffAmmount;
        var DueDate = $scope.DueDate;        
        var AchPaymentDetails = '';
        var OriginalBankAccountNumber = '';
        var FutureAccount = false;
        var FuturePaymentDate = $scope.FuturePaymentDate;
        var SaveTimeAndMoney = false;
        var PaymentType = 'ACH';
        var previousAccountShow = false;        
        var PaymentTimeOption = $scope.PaymentTimeOption.name;
        var FutureAmount = $scope.FutureAmount.name;
        var FutureOtherAmount = $scope.FutureOtherAmount;
        savedAchItems.forEach(function (item) {
            if (item.Selected) {
                RoutingNumber = item.BankABA;
                BankAccountName = item.BankAccountName;
                AccountType =item.BankAcctType;
                FullName =item.BankHolder;
                BankName= item.BankName ;
                BankAccountNumber = item.BankAcctNo;                                            
            }
        });
        AccountHolderPhoneNumber = $scope.AccountHolderPhoneNumber.replace(/-/g, "");
        Email = $scope.email;        
        OriginalBankAccountNumber = BankAccountNumber;
        var collection = { "CheckingAccountNumber": BankAccountNumber, "Amount": PaymnetAmount, 
            "AccountNumber": AccountNumber, "Routing": RoutingNumber, "FuturePaymentDate": $scope.FuturePaymentDate, 'IsTotalAmountDue':$scope.IsTotalAmountDue
        };
        increaseLoadingCounter();
        AccountInformationService.AccountInformationNext(collection, $scope.baseAddress).success(function (data) {
            $scope.model = data;
            if (data.Error != null || data.ErrorRouting != null) {
                $scope.MessageError = data.Error;
                $scope.ErrorRouting = data.ErrorRouting;                
                decreaseLoadingCounter();
                return;
            }
            else {
                var paymentConfirmationJsionFormat = JSON.stringify({
                    PavmentDate: PavmentDate, PaymnetAmount: PaymnetAmount, AccountNumber: AccountNumber, FullName: FullName,
                    BankName: BankName, RoutingNumber: RoutingNumber,
                    BankAccountNumber: BankAccountNumber, AccountType: AccountType,
                    AmountName: AmountName, CurrentBalance: CurrentBalance,
                    AmountPastDue: AmountPastDue, RegularAmountDue: RegularAmountDue,
                    OtherAmount: OtherAmount, TotalAmountDue: TotalAmountDue,
                    AccountHolderPhoneNumber: AccountHolderPhoneNumber, Email: Email,
                    OriginalBankAccountNumber: OriginalBankAccountNumber,
                    AchPaymentDetails: AchPaymentDetails, FutureAccount: FutureAccount,
                    BankAccountName: BankAccountName, FuturePaymentDate: FuturePaymentDate,
                    SaveTimeAndMoney: SaveTimeAndMoney, PaymentType: PaymentType,
                    TotalPayoffAmmount: TotalPayoffAmmount, DueDate: DueDate,
                    previousAccountShow: previousAccountShow,
                    savedAch: savedAchItems, PaymentTimeOption: PaymentTimeOption, FutureAmount: FutureAmount, FutureOtherAmount: FutureOtherAmount
                });
                $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(paymentConfirmationJsionFormat);
                if ($scope.result == "Holder data successfully") {
                    $rootScope.AchPaymentAmountCheck = false;
                    $rootScope.loggedIn = true;
                    $location.path('/PaymentConfirmation');
                }
            }
            decreaseLoadingCounter();
        }).error(function (error) {                        
            if (error == ""){$scope.MainError = "Server connection problem";}
            else {$scope.MainError = error.Message;;}
            decreaseLoadingCounter();
        });
    }
    (function () {
        increaseLoadingCounter();     
        var count = $rootScope.sectionPayment++;               
        $rootScope.holderData = PaymentConfirmationService.PaymentConfirmationGet();        
        if ($rootScope.holderData != "")
        {           
            $scope.EditPaymentAmount = true;
            var data = JSON.parse($rootScope.holderData);
            if (data.CardNumber) {                
                $scope.ShowNewCard = true;
                $scope.SaveCardInfo = false;
                $scope.TypeOfDebitCredit.name = "New";
                $scope.PaymentType.name = "PayByDebitCredit";
                $scope.PaymentTypeCCDB = true;                
                $scope.Amount.name = data.SelectedPaymentAmountType;
                $scope.Amount.name == "otherAmount" ? $scope.disabled = false : $scope.disabled = true;
                $scope.FutureAmount.name = data.FutureAmount;
                $scope.FutureAmount.name == "otherAmount" ? $scope.Amountdisabled = false : $scope.Amountdisabled = true;              
                $scope.accountNumberGet = data.AccountNumber;                    
                panels.ACH = false;
                panels.Card = true;
                $scope.showHidePanel = panels;
                $scope.checkedDebitCredit = true;
                $scope.FirstName = data.FirstName;
                $scope.LastName = data.LastName;
                $scope.CurrentBalance = data.CurrentBalance;
                $scope.AmountPastDue = data.CurrentBalance;
                $scope.RegularAmountDue = data.RegularAmountDue;
                $scope.OtherAmount = data.OtherAmount;             
                $scope.TotalAmountDue = data.TotalAmountDue;
                $scope.PrimaryNumber = data.PrimaryNumber;
                $scope.Email = data.Email;
                $scope.Address = data.Address;
                $scope.City = data.City;
                $scope.State = data.State;
                $scope.Zip = data.Zip;
                $scope.DueDate = data.DueDate;               
                savedCardItems = data.SavedCards;              
                if (data.PaymentTimeOption == "SetFuturePayment") {
                    $scope.PaymentTimeOption.name = data.PaymentTimeOption;
                    $scope.FutureAmount.name = data.FutureAmount;
                    $scope.FuturementDisabled = false;
                    $scope.FuturePaymentDate = data.FuturePaymentDate;
                    $scope.FuturementDisabled = false;
                    $scope.payNowDisabled = true;
                }
                else
                {
                    $scope.FuturementDisabled = true;
                    $scope.payNowDisabled = false;
                }
                $scope.FutureOtherAmount = data.FutureOtherAmount;
                bindStateList();
                if (savedCardItems) {
                    $scope.savedCards = savedCardItems;                    
                    if(savedCardItems.find(function (card) { return card.Selected === true; })){
                        savedCardItems.forEach(function (item){item.CVV = '';});
                        $scope.TypeOfDebitCredit.name = "Saved";
                        $scope.savedCards = savedCardItems;                  
                        $scope.SaveCardInfo = true;
                        $scope.ShowNewCard = false;
                        $scope.EditBillingAddress = false;
                        $scope.SavedBillingAddress=true;
                        DisableNewCard();
                    }
                }
                $scope.AccountNumber = data.AccountNumber;
                var BaseAddress = $scope.baseAddress;
            }
            else
            {
                //-----------------------------------------------------ACH--------------------------                
                panels.ACH = true;
                panels.Card = false;
                $scope.PaymentTypeACH = true;
                $scope.showNewAch = true;
                $scope.showSavedAch = false;                
                $scope.AchDetail.name = 'NewAchDetail';
                $scope.Amount.name = data.AmountName;
                $scope.Amount.name == "otherAmount" ? $scope.disabled = false : $scope.disabled = true;
                $scope.FutureAmount.name = data.FutureAmount;
                $scope.FutureAmount.name == "otherAmount" ? $scope.Amountdisabled = false : $scope.Amountdisabled = true;
                $scope.accountNumberGet = data.AccountNumber;
                $scope.AccountHolder = data.FullName;
                $scope.BankName = data.BankName;
                $scope.BankRoutingNumber = data.RoutingNumber;
                $scope.CheckingAccountNumber = data.OriginalBankAccountNumber;
                $scope.rdAccountType.name = data.AccountType;
                $scope.CurrentBalance = data.CurrentBalance;
                $scope.AmountPastDue = data.AmountPastDue;
                $scope.RegularAmountDue = data.RegularAmountDue;
                $scope.OtherAmount = data.OtherAmount;
                $scope.TotalAmountDue = data.TotalAmountDue;
                $scope.AccountHolderPhoneNumber = data.AccountHolderPhoneNumber;
                $scope.AchPaymentDetails.name = data.AchPaymentDetails;
                $scope.checkboxFutureAccount.value = data.FutureAccount;
                $scope.BankAccount = data.OriginalBankAccountNumber;                
                $scope.checkBoxSaveTimeMoney.value = data.SaveTimeAndMoney;
                $scope.email = data.Email;
                $scope.TotalPayoffAmmount = data.TotalPayoffAmmount;
                $scope.DueDate = data.DueDate;
                $scope.PaymentType.name = data.PaymentType;
                $scope.CheckingAccountNumberDisplay = data.BankAccountNumber;
                $scope.previousAccountShow = data.previousAccountShow;                
                $scope.BankAccountName = data.BankAccountName;
                if (data.PaymentTimeOption == "SetFuturePayment")
                {                   
                    $scope.PaymentTimeOption.name = data.PaymentTimeOption;
                    $scope.FutureAmount.name = data.FutureAmount;
                    $scope.FuturementDisabled = false;
                    $scope.payNowDisabled = true;
                }
                else
                {
                    $scope.FuturementDisabled = true;
                    $scope.payNowDisabled = false;
                }                                
                $scope.FutureOtherAmount = data.FutureOtherAmount;                
                if ($scope.checkBoxSaveTimeMoney.value == true) {
                    $scope.PaymentFrequencyVisible = true;
                }
                else {$scope.PaymentFrequencyVisible = false;}
                savedAchItems = data.savedAch;
                if (savedAchItems) {
                    $scope.savedAch = savedAchItems;                    
                    if (savedAchItems.find(function (ach) { return ach.Selected === true; })) {                         
                        $scope.showSavedAch = true;
                        $scope.disableNewAch = true;
                        $scope.checkboxFutureAccount.value = false;
                        $scope.AchDetail.name = 'SavedAchDetail';
                        $scope.showNewAch = false;
                        ClearNewAchDetails();
                    }
                }
                $scope.DaysDue = new Date() < new Date(data.DueDate) ? false : true;
                $scope.FuturePaymentDate = data.FuturePaymentDate;
                CancelEditcollectio = JSON.stringify({ PavmentDate: data.PavmentDate, PaymnetAmount: data.PaymnetAmount, AccountNumber: data.AccountNumber, FullName: data.FullName, BankName: data.BankName, RoutingNumber: data.RoutingNumber, BankAccountNumber: data.BankAccountNumber, AccountType: data.AccountType, AmountName: data.AmountName, CurrentBalance: data.CurrentBalance, AmountPastDue: data.AmountPastDue, RegularAmountDue: data.RegularAmountDue, OtherAmount: data.OtherAmount, TotalAmountDue: data.TotalAmountDue, AccountHolderPhoneNumber: data.AccountHolderPhoneNumber, Email: data.Email, BankAccountName: data.BankAccountName, PaymentType: data.PaymentType, FutureAccount: data.FutureAccount, AchPaymentDetails: data.AchPaymentDetails, TotalPayoffAmmount: data.TotalPayoffAmmount, DueDate: data.DueDate, FuturePaymentDate: data.FuturePaymentDate, OriginalBankAccountNumber: data.OriginalBankAccountNumber, previousAccountShow: data.previousAccountShow });
            }
            PaymentConfirmationService.PaymentConfirmationHolder("");
            decreaseLoadingCounter();
        }
        else {          
            $scope.payNowDisabled = false;
            $scope.FuturementDisabled = true;
            $scope.Amountdisabled = true;
            PaymentConfirmationService.PaymentConfirmationHolder("");
            AccountInformationService.UserAccountInformation(AccountNumber, $scope.baseAddress).success(function (data) {
                userData(data);
            }).error(function (error) {
                decreaseLoadingCounter()
                if (error == ""){$scope.MainError = "Server connection problem";}
                else {$scope.MainError = error.Message;;}
            });
        }

    })();

    function BindSavedAchDetails()
    {
        increaseLoadingCounter();
        paymentAmountService.GetSavedAchDetails(AccountNumber, $scope.baseAddress).then(function (data) {
            savedAchItems = [];       
            for (var counter = 0; counter < data.length; counter++) {
                var item = new savedAchItem();
                item.BankABA = data[counter].BankABA;
                item.BankAccountName = data[counter].BankHolder;
                item.BankAcctType = data[counter].BankAcctType == "P" ? "Checking" : "Savings";
                item.BankHolder = data[counter].CreatedBy;
                item.BankName = data[counter].BankName;
                item.BankAcctNo = data[counter].BankAcctNo;
                item.Email = data[counter].Email;
                item.PrimaryNumber = data[counter].PrimaryNumber;
                item.BankAcctNoView = "XXXXX-" + data[counter].BankAcctNo.substr(-4);
                item.ID = data[counter].ID;
                item.counter = counter;
                if (counter === data.length - 1)
                 item.Selected = true;
                savedAchItems.push(item);
            }
            $scope.savedAch = savedAchItems;           
            if (savedAchItems) {
                if (savedAchItems.length > 0) {                    
                    $scope.PaymentTypeACH = true;
                    $scope.AchDetail.name = "SavedAchDetail"
                    $scope.showSavedAch = true;
                    $scope.showNewAch = false;                    
                }
                else
                {
                    $scope.PaymentTypeACH = false;
                    $scope.AchDetail.name = "NewAchDetail"
                    $scope.showSavedAch = false;
                    $scope.showNewAch = true;                   
                }                                       
            }
            decreaseLoadingCounter();
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }
    function userData(data)
    {       
        $scope.accountNumberGet = data.AccountNumber;
        $scope.AccountHolder = data.AccountHolder;
        $scope.CurrentBalance = data.CurrentBalance;
        $scope.AmountPastDue = data.AmountPastDue;
        $scope.RegularAmountDue = parseFloat(data.RegularAmountDue).toFixed(2);
        $scope.OtherAmount = parseFloat(data.TotalAmountDue).toFixed(2);
        $scope.FutureOtherAmount = parseFloat(data.TotalAmountDue).toFixed(2);
        $scope.TotalAmountDue = parseFloat(data.TotalAmountDue).toFixed(2);
        $scope.AccountHolderPhoneNumber = data.AccountHolderPhoneNumber;
        $rootScope.TodayDate = data.TodayDate;
        $scope.DueDate = data.DueDate;                
        $scope.AcctDaysPastDue = data.AcctDaysPastDue;
        $scope.TotalPayoffAmmount = parseFloat(data.TotalPayoffAmmount).toFixed(2);
        $scope.MinimumAmount = data.MinimumAmount;
        $scope.MaximumAmount = data.MaximumAmount;
        $scope.lastPaymentMade = data.LastBankAccountType == "S" ? "ACH - Savings Account" : data.LastBankAccountType == "P" ? "ACH - Checking Account" : "Not Available";
        $scope.DaysDue = new Date() < new Date(data.DueDate) ? false : true;
        $scope.LastPayment = parseFloat(JSON.parse(data.LastTransactionPayment));
        $scope.LastFee = parseFloat(JSON.parse(data.LastTransactionFee));
        var lastTotalamount = $scope.LastPayment + $scope.LastFee;
        $scope.LastTotalamount = isNaN(lastTotalamount) == true ? "$0.00" : "$" + lastTotalamount;
        $rootScope.LastTotalamount = parseFloat($scope.LastTotalamount).toFixed(2);
        $scope.Error = data.Error;
        $scope.email = sessionStorage.getItem('Email');       
        decreaseLoadingCounter();
        //Bind Saved Ach card
        BindSavedAchDetails();
    }
    function savedAchSelected(data) {
        savedAchItems.forEach(function (item) {
            if (item.BankAccountNumber === data.BankAccountNumber) {item.Selected = true;}
            else { item.Selected = false; }
            //Disable new ach details
            $scope.disableNewAch = true;
            $scope.checkboxFutureAccount.value = false;
            ClearNewAchDetails();            
        });
    }
    function ClearNewAchDetails() {
        $scope.rdAccountType.name = '';
        $scope.CheckingAccountNumber = "";
        $scope.BankRoutingNumber = "";
        $scope.BankAccountName = "";
    }
    $scope.radioSavedAchClicked = function (data) {
       
        savedAchItems.forEach(function (item) {
            if (item.ID === data.ID) {
                item.Selected = true;
                $scope.AchGrid = data.BankAcctType;
                $scope.AccountHolderPhoneNumber = data.PrimaryNumber;
                $scope.email = data.Email;
            }
            else { item.Selected = false; }
        });
        $scope.checkboxFutureAccount.value = false;
        ClearNewAchDetails();        
    };
    $scope.AchDeletePop = function (data)
    {       
        $scope.HoldAchCheckingAccountNumber = data.ID;
        popAchDelete();
    }
    $scope.AchDelete = function () {        
        var ID = $scope.HoldAchCheckingAccountNumber;       
        $scope.btndisabled = true;
        paymentAmountService.DeleteSavedAch(ID, AccountNumber, $scope.baseAddress).then(function (data) {
            $scope.btndisabled = false;                      
            BindSavedAchDetails();
            $('#myModal2 .close').click();          
        },
        function (error) {
            $scope.btndisabled = false;           
            if (error == "") {$scope.AchDelete = "Server connection problem";}
            else {$scope.AchDelete = error.Message;;}
        });
    }
    function popAchDelete() {
        $('#myModal2').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }
    $scope.btnCancel = function () {
            $scope.BankRoutingNumber = '';
            $scope.CheckingAccountNumber = '';
            $rootScope.loggedIn = true;
            $rootScope.leftMenu = true;
            $rootScope.userProfile = true;
            $location.path('/PaymentAgreement');
    }
    $scope.SelectAmount = function () {
        if ($scope.Amount.name == "totalAmount") {
            $scope.disabled = true;
            $scope.disabledPayoffTotalAmount = true;
        }
        if ($scope.Amount.name == "otherAmount") {
            $scope.disabled = false;
            $scope.disabledPayoffTotalAmount = true;
        }
        if ($scope.Amount.name == "PayoffTotalAmount") {
            $scope.disabled = true;
            $scope.disabledPayoffTotalAmount = false;
        }
    };
    $scope.ACHPaymentDetailsNew = function () {
        $scope.BankRoutingNumber = angular.copy(ResetBankRoutingNumber);
        $scope.CheckingAccountNumber = angular.copy(ResetCheckingAccountNumber);
        $scope.BankAccountName = angular.copy(BankAccountName);
        $scope.email = angular.copy(Email);
        $scope.checkboxFutureAccount.value = false;
        $scope.form.$setPristine();
    }    
    $scope.SaveTimeMoney = function () {
        var TimeMoney = $scope.checkBoxSaveTimeMoney.value;
        if (TimeMoney == true) {$scope.PaymentFrequencyVisible = true;}
        else {$scope.PaymentFrequencyVisible = false;}
    }
    $scope.PaymentTypeAch = function () {
        panels.ACH = true;
        panels.Card = false;
        BindSavedAchDetails();
    }
    $scope.PaymentTypeACHType = function () {
        if ($scope.AchDetail.name === "NewAchDetail")
        {
            $scope.showSavedAch = false;
            $scope.showNewAch = true;            
            $scope.rdAccountType.name = 'Checking';
            $scope.PrimaryNumber = $scope.HolderPrimaryNumber;
            $scope.Email = sessionStorage.getItem('Email');
        }
        else
        {
            $scope.showSavedAch = true;
            $scope.showNewAch = false;
            BindSavedAchDetails();
        }
    }      
    $scope.removeerrorFuturePaymentDate = function () {$scope.errorFuturePaymentDate = "";}
    $scope.Back = function () {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/AccountInformation');
    }
    //  -------------------------- Rahul Card Info-------------------------
    if ($scope.CheckACN == "C" || $rootScope.ACHBlock === "Blocked") {
        $scope.ACHRadio = true;
        $scope.PaymentType.name = "PayByDebitCredit";
        PaymentByDebitCreditCardOnly();
    }
    $scope.PaymentTypeDebitCredit = function () {PaymentByDebitCreditCardOnly();}
    function PaymentByDebitCreditCardOnly()
    {
        increaseLoadingCounter();
        ClearBillingAddress();
        panels.ACH = false;
        panels.Card = true;
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        paymentAmountService.GetCardDetails(AccountNumber, BaseAddress).then(function (data) {
            savedCardItems = [];
            for (var counter = 0; counter < data.length; counter++) {
                var item = new savedCardItem();
                item.TokenId = data[counter].TokenId;
                item.CardNumber = data[counter].CardNumber;
                item.CardType = data[counter].CardType;
                item.CardName = data[counter].CardName;
                item.CardExpiry = data[counter].CardExpiry;
                if (counter === data.length - 1) {
                    item.Selected = true;
                GetLastBillingAddress(item.TokenId);
                }
                savedCardItems.push(item);
            }
            $scope.savedCards = savedCardItems;
            if ($scope.savedCards.length > 0) {
                $scope.PaymentTypeCCDB = true;
                $scope.TypeOfDebitCredit.name = 'Saved';
                $scope.SaveCardInfo = true;
                $scope.ShowNewCard = false;
                $scope.EditBillingAddress = false;
                $scope.SavedBillingAddress = true;
            }
            else
            {
                $scope.PaymentTypeCCDB = false;               
                $scope.TypeOfDebitCredit.name = 'New';
                $scope.SaveCardInfo = false;
                $scope.ShowNewCard = true;
                $scope.EditBillingAddress = true;
                $scope.SavedBillingAddress = false;
            }
            decreaseLoadingCounter();
            bindStateList();          
        }, function (error) {
            if (error == "") {$scope.Error = "Server connection problem";}
            else {$scope.Error = error.Message;;}
        });
    }
    $scope.EditAddress = function () {
        $scope.SavedBillingAddress = false;
        $scope.EditBillingAddress = true;
    };
    $scope.PaymentTypeDebitCreditType = function () {
        if ($scope.TypeOfDebitCredit.name === "New") {
            $scope.SaveCardInfo = false;
            $scope.ShowNewCard = true;
            $scope.SavedBillingAddress = false;
            $scope.EditBillingAddress = true;
        }
        else {          
            $scope.SaveCardInfo = true;
            $scope.ShowNewCard = false;
            $scope.EditBillingAddress = false;
            savedCardItems.forEach(function (item, index) {
                if (index === savedCardItems.length - 1)
                {
                    item.Selected = true;
                    GetLastBillingAddress(item.TokenId);
                }
            });
            $scope.SavedBillingAddress = true;
        }        
    }; 
    $scope.radioSavedCard = function (data) {       
        savedCardItems.forEach(function (item) {            
            if (item.TokenId === data.TokenId) {
                item.Selected = true;
                GetLastBillingAddress(item.TokenId);
                chkBilling = 0, chkUserProfile = 0;
            }
            else { item.Selected = false; }
        });
        $scope.SavedBillingAddress = true;
        $scope.EditBillingAddress = false;
        HideBillingError();
        DisableNewCard();
    }
    function DisableNewCard()
    {
        $scope.TypeOfDebitCredit.name = 'Saved';
        $scope.CardType.Name = "";
        $scope.CardName = "";
        $scope.CardNumber = "";
        $scope.ExpiryYear = "";
        $scope.ExpiryDate = "";
        $scope.CvNumber = "";
        $scope.chkuserProfileaddress.value = false;        
        $scope.chkLastBillAddress.value = false;
        ClearValidation();
    }
    $scope.radioNewCard = function () {savedCardItems.forEach(function (item) {item.Selected = false; });
        $scope.checkedDebitCredit = false;
        $scope.TypeOfDebitCredit.name = 'New';
        $scope.CardType.Name = "Visa";
        $scope.chkLastBillAddress =false   
        $scope.chkuserProfileaddress =false       
        ClearBillingAddress();
    }
     $scope.updateSavedCardCVV = function (data) {
        savedCardItems.forEach(function (item) {
            if (item.TokenId === data.TokenId) {
                item.CVV = data.CVV;
            }
        });
    };
    function CardValidation()
    {                
        var RetunValue = true;
        var FirstName = $('#FirstName');
        var CardName = $('#CardName');
        var CardNumber = $('#CardNumber');
        var ExpiryDate = $('#ExpiryDate');
        var ExpiryYear = $('#ExpiryYear');       
        var CvNumber = $('#CvNumber');
        var PrimaryNumber = $('#PrimaryNumber');
        var Email = $('#Email');
        var Address = $('#Address');
        var City = $('#City');
        var State = $('#state');
        var Zip = $('#Zip');
        var LastName = $('#LastName'); 
       var regexZip = /^(\d{5}-\d{4}|\d{5})$/;      
        if (FirstName.val() == '') { FirstName.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { FirstName.css('border-color', '#e6e6e6'); RetunValue = true; }
        if (LastName.val() == '') { LastName.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { LastName.css('border-color', '#e6e6e6'); RetunValue = true; }
        if (PrimaryNumber.val() == '') { PrimaryNumber.css('border-color', 'red'); RetunValue = false; return RetunValue; }
        else if (PrimaryNumber.val().length != 12) { PrimaryNumber.css('border-color', 'red'); RetunValue = false; return RetunValue; }
        else { PrimaryNumber.css('border-color', '#e6e6e6'); RetunValue = true; }
        if (Email.val() == '') { Email.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { Email.css('border-color', '#e6e6e6'); RetunValue = true; }
        if (Address.val() == '') { Address.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { Address.css('border-color', '#e6e6e6'); RetunValue = true; }
        if (City.val() == '') { City.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { City.css('border-color', '#e6e6e6'); RetunValue = true; }
        if ($scope.selectedState == undefined || $scope.selectedState == '') { State.css('display', 'block'); RetunValue = false; return RetunValue; } else { State.css('display', 'none'); RetunValue = true; }
        if (Zip.val() == '') { Zip.css('border-color', 'red'); RetunValue = false; return RetunValue; }
        else if (!regexZip.test(Zip.val())) { Zip.css('border-color', 'red'); RetunValue = false; return RetunValue; }
        else { Zip.css('border-color', '#e6e6e6'); RetunValue = true; }       

        if ($('#radioNewCard').is(':checked')) {
            if (CardName.val() == '') { CardName.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { CardName.css('border-color', '#e6e6e6'); RetunValue = true; }
            if (CardNumber.val() == '' || CardNumber.val().length != '16') { CardNumber.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { CardNumber.css('border-color', '#e6e6e6'); RetunValue = true; }
            if (ExpiryDate.val() == '? undefined:undefined ?' || ExpiryDate.val() == '? string: ?') { ExpiryDate.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { ExpiryDate.css('border-color', '#e6e6e6'); RetunValue = true; }
            if (ExpiryYear.val() == '? undefined:undefined ?' || ExpiryYear.val() == '? string: ?') { ExpiryYear.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { ExpiryYear.css('border-color', '#e6e6e6'); RetunValue = true; }
            if (CvNumber.val() == '' || CvNumber.val().length != '3') { CvNumber.css('border-color', 'red'); RetunValue = false; return RetunValue; } else { CvNumber.css('border-color', '#e6e6e6'); RetunValue = true; }
        }        
        return RetunValue;        
    }
    function ClearValidation()
    {
        $('#CardName').css('border-color', '#e6e6e6');
        $('#CardNumber').css('border-color', '#e6e6e6');
        $('#ExpiryDate').css('border-color', '#e6e6e6');
        $('#ExpiryYear').css('border-color', '#e6e6e6'); 
        $('#CvNumber').css('border-color', '#e6e6e6');
    }    
    function ClearBillingAddress() {      
        $scope.Email = '';
        $scope.Address = '';
        $scope.PrimaryNumber = '';
        $scope.Address = '';
        $scope.City = '';
        $scope.LastName = '';
        $scope.FirstName = '';
        $scope.Zip = '';
        $scope.selectedState = '';
        HideBillingError();
    }
    function HideBillingError()
    {
        $('#state').css('display', 'none');
        $('#FirstName').css('border-color', '#e6e6e6');
        $('#PrimaryNumber').css('border-color', '#e6e6e6');
        $('#Email').css('border-color', '#e6e6e6');
        $('#Address').css('border-color', '#e6e6e6');
        $('#City').css('border-color', '#e6e6e6');
        $('#Zip').css('border-color', '#e6e6e6');
        $('#LastName').css('border-color', '#e6e6e6');
    }
    $scope.CardPaymentNext = function () {
       
        if (CardValidation() == true) {                                  
            if ($scope.PaymentTimeOption.name == "PayNow") {
                if ($scope.Amount.name == "otherAmount") {
                    var amount = $scope.OtherAmount;
                    var regex = /^\d+(\.\d+)?$/;
                    if (!regex.test(amount)) {$scope.otereAmountError = "Enter Only Amount"; return;}
                    var OtherAmount = parseFloat($scope.OtherAmount);
                    if (parseInt($scope.AcctDaysPastDue) > 0 && OtherAmount < parseFloat($scope.TotalAmountDue)) {
                        $scope.otereAmountError = "Minimum amount allowed: $" + $scope.TotalAmountDue;return;}
                    if (parseFloat($scope.MinimumAmount) > OtherAmount) { $scope.otereAmountError = "Minimum amount allowed: $" + $scope.MinimumAmount; return; }
                    if (parseFloat($scope.MaximumAmount) < OtherAmount) { $scope.otereAmountError = "Maximum amount allowed: $" + $scope.MaximumAmount; return; }                   
                }
                else if ($scope.Amount.name == "totalAmount")
                {
               
                    if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
                    { $scope.otereAmountError = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }
                }
            }
            else {
                if ($scope.FuturePaymentDate == "") {$scope.errorFuturePaymentDate = "Please Enter Future Date";return;}
                var FuturePaymentDate = $scope.FuturePaymentDate.toString().split(" ").slice(0, 3).join(" ");
                var dateDue = new Date($scope.DueDate);
                var FutureDate = new Date(FuturePaymentDate);
                if (dateDue < FutureDate) {$scope.errorFuturePaymentDate = "Future payment date must be less than due date.";return;}
                if ($scope.FutureAmount.name == "otherAmount") {
                    var amount = $scope.FutureOtherAmount;
                    var regex = /^\d+(\.\d+)?$/;
                    if (!regex.test(amount)) {$scope.FutureotereAmountError = "Enter Only Amount";return;}
                    var OtherAmount = parseFloat($scope.FutureOtherAmount);
                    if (parseInt($scope.AcctDaysPastDue) > 0 && OtherAmount < parseFloat($scope.TotalAmountDue)) {
                        $scope.FutureotereAmountError = "Minimum amount allowed: $" + $scope.TotalAmountDue; return;}                    
                    if (parseFloat($scope.MinimumAmount) > OtherAmount) {$scope.FutureotereAmountError = "Minimum amount allowed: $" + $scope.MinimumAmount;return;}
                    if (parseFloat($scope.MaximumAmount) < OtherAmount) {$scope.FutureotereAmountError = "Maximum amount allowed: $" + $scope.MaximumAmount;return;}
                }
                else if ($scope.FutureAmount.name == "totalAmount")
                {

                    if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
                    { $scope.FutureotereAmountError = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }

                }
            }
            //---------------------------------------------------------------------------------------------
            var Amount = $scope.Amount.name == "totalAmount" ? $scope.TotalAmountDue : $scope.OtherAmount;
            var AccountNumber = $scope.accountNumberGet;            
                if ($scope.TypeOfDebitCredit.name == "New") {
                if ($scope.PaymentTimeOption.name !== 'PayNow') {
                    var FuturePaymentDate = $scope.FuturePaymentDate;
                    var collectiondata = { "FuturePaymentDate": FuturePaymentDate };
                    AccountInformationService.FutureDateValidation(collectiondata, $scope.baseAddress).success(function (data) {
                        if (data.ErrorFutureDate != null) {$scope.errorFuturePaymentDate = data.ErrorFutureDate;}
                        else {CardpaymentConfirmation();}
                    }).error(function (error) {if (error == "") {$scope.MainError = "Server connection problem";}else {$scope.MainError = error.Message;;}});
                }
                else {CardpaymentConfirmation();}}
                else
                {
                    if ($scope.PaymentTimeOption.name !== 'PayNow')
                    {
                    var FuturePaymentDate = $scope.FuturePaymentDate;
                    var collectiondata = { "FuturePaymentDate": FuturePaymentDate };
                    AccountInformationService.FutureDateValidation(collectiondata, $scope.baseAddress).success(function (data) {
                        if (data.ErrorFutureDate != null) {$scope.errorFuturePaymentDate = data.ErrorFutureDate;}
                        else {SavedCardConfirmation();}
                    }).error(function (error) {
                        if (error == "") {$scope.MainError = "Server connection problem";}
                        else {$scope.MainError = error.Message;;}});
                }
                else {SavedCardConfirmation();}               
            }
        }
    };
        
    function CardpaymentConfirmation() {               
        var PaymentDate = $rootScope.TodayDate;
        var CardPaymentAmount = "";
        if ($scope.PaymentTimeOption.name == 'PayNow')
        {
            if ($scope.Amount.name == "totalAmount") { CardPaymentAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true;}
            else { CardPaymentAmount = $scope.OtherAmount; $scope.IsTotalAmountDue = false; }
        }
        else
        {

            if ($scope.FutureAmount.name == "totalAmount") { CardPaymentAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
            else { CardPaymentAmount = $scope.FutureOtherAmount; $scope.IsTotalAmountDue = false; }
            PaymentDate = $scope.FuturePaymentDate;
        }
        var AccountNumber = $scope.accountNumberGet;
        var FirstName = $scope.FirstName;
        var LastName = $scope.LastName;
        var CardName = $scope.CardName;

        if ($scope.CardType.Name == 'Visa') {
            var CardType = 'Visa';
        }
        else {
            var CardType = 'Master';
        }

        var CardNumber = $scope.CardNumber;
        var ExpiryDate = $scope.ExpiryDate;
        var ExpiryYear = $scope.ExpiryYear;
        var CvNumber = $scope.CvNumber;
        var CurrentBalance = $scope.CurrentBalance;
        var AmountPastDue = $scope.AmountPastDue;
        var RegularAmountDue = $scope.RegularAmountDue;
        var OtherAmount = $scope.OtherAmount;
        var TotalAmountDue = $scope.TotalAmountDue;
        var DueDate = $scope.DueDate;
        var PrimaryNumber = $scope.PrimaryNumber.replace(/-/g, "");
        var Email = $scope.Email;
        var SaveFuture = $scope.chkFuture.value;

        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!regexEmail.test(Email)) { $scope.errorEmail = "Not an email!"; $scope.Loadname = "";  return; }

        var Address = $scope.Address;
        var City = $scope.City;
        var State = $scope.selectedState.Value;
        var Zip = $scope.Zip;
        var FuturePaymentDate = $scope.FuturePaymentDate;
        var PaymentType = 'Credit/Debit';        
     
        var PaymentTimeOption = $scope.PaymentTimeOption.name;
        var FutureAmount = $scope.FutureAmount.name;
        var FutureOtherAmount = $scope.FutureOtherAmount;

            $scope.errorCvNumber = "";
            var CardPaymentConfirmationJsonFormat = JSON.stringify({ TokenId: $scope.TokenId, CardName: CardName, FirstName: FirstName, PaymentDate: PaymentDate, CardPaymentAmount: CardPaymentAmount, AccountNumber: AccountNumber, CardType: CardType, CardNumber: CardNumber, ExpiryDate: ExpiryDate, ExpiryYear: ExpiryYear, CurrentBalance: CurrentBalance, AmountPastDue: AmountPastDue, RegularAmountDue: RegularAmountDue, OtherAmount: OtherAmount, TotalAmountDue: TotalAmountDue, CvNumber: CvNumber, Email: Email, DueDate: DueDate, PrimaryNumber: PrimaryNumber, 
                Address: Address, City: City, FuturePaymentDate: FuturePaymentDate, City: City, PaymentType: PaymentType, State: State, Zip: Zip, 
                SaveFuture: SaveFuture, LastName: LastName, SelectedPaymentAmountType: $scope.Amount.name, SavedCards: savedCardItems, PaymentTimeOption: PaymentTimeOption, FutureAmount: FutureAmount, FutureOtherAmount: FutureOtherAmount
            });
            var collectiondata = { "AccountNumber": $scope.AccountNumber, "Amount": CardPaymentAmount, "FuturePaymentDate": FuturePaymentDate, 'IsTotalAmountDue': $scope.IsTotalAmountDue };
            AccountInformationService.CheckValidCardTransaction(collectiondata, $scope.baseAddress).success(function (data) {
                if (JSON.parse(data) == "true") {
                    $scope.MainCardError = "";
            $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(CardPaymentConfirmationJsonFormat);
            if ($scope.result == "Holder data successfully") {
                $rootScope.loggedIn = true;
                $location.path('/CardPaymentConfirmation');}        }
                else{$scope.MainCardError = JSON.parse(data);}
            }).error(function (error) {
                if (error == "") {$scope.MainError = "Server connection problem";}
                else {$scope.MainError = error.Message;;}});          
    }

    function SavedCardConfirmation()
    {       
        var PaymentDate = $rootScope.TodayDate;
        var CardPaymentAmount = "";
        if ($scope.PaymentTimeOption.name == 'PayNow') {
            if ($scope.Amount.name == "totalAmount") { CardPaymentAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
            else { CardPaymentAmount = $scope.OtherAmount; $scope.IsTotalAmountDue = false; }
        }
        else {

            if ($scope.FutureAmount.name == "totalAmount") { CardPaymentAmount = $scope.TotalAmountDue; $scope.IsTotalAmountDue = true; }
            else { CardPaymentAmount = $scope.FutureOtherAmount; $scope.IsTotalAmountDue = false; }
            PaymentDate = $scope.FuturePaymentDate;
        }
        var FuturePaymentDate = $scope.FuturePaymentDate;
        var AccountNumber = $scope.accountNumberGet;
        var Email = $scope.Email;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!regexEmail.test(Email)) { $scope.errorEmail = "Not an email!"; return; }
        var PaymentType = 'Credit/Debit';        
        savedCardItems.forEach(function (item) {
           
            if (item.Selected) {
                $scope.TokenId = item.TokenId;
                $scope.SavedCvNumber = item.CVV;
                $scope.SavedCardName = item.CardName;
                $scope.SavedCardExpiry = item.CardExpiry;
                $scope.SavedCardNumber = item.CardNumber;
                $scope.SavedCardType = item.CardType == "V" ? "Visa" : "Master";
            }
        });
        
        var PaymentTimeOption = $scope.PaymentTimeOption.name;
        var FutureAmount = $scope.FutureAmount.name;
        var FutureOtherAmount = $scope.FutureOtherAmount;
        if ($scope.SavedCvNumber == '') { $scope.errorCvNumber = "Please enter CVV number!"; } else {
            $scope.errorCvNumber = "";                 

            var CardPaymentConfirmationJsonFormat = JSON.stringify({
                TokenId: $scope.TokenId,
                CardName: $scope.SavedCardName, FirstName: $scope.FirstName, PaymentDate: PaymentDate, CardPaymentAmount: CardPaymentAmount,
                AccountNumber: AccountNumber, CardType: $scope.SavedCardType, CardNumber: $scope.SavedCardNumber, ExpiryDate: $scope.SavedCardExpiry.substring(0, 2),
                ExpiryYear: $scope.SavedCardExpiry.substring(3, 7), CurrentBalance: $scope.CurrentBalance, AmountPastDue: $scope.AmountPastDue,
                RegularAmountDue: $scope.RegularAmountDue, OtherAmount: $scope.OtherAmount, TotalAmountDue: $scope.TotalAmountDue, CvNumber: $scope.SavedCvNumber,
                Email: Email, DueDate: $scope.DueDate, PrimaryNumber: $scope.PrimaryNumber.replace(/-/g, ""), Address: $scope.Address, City: $scope.City, FuturePaymentDate: $scope.FuturePaymentDate,
                PaymentType: PaymentType, State: $scope.selectedState.Value, Zip: $scope.Zip, SaveFuture: $scope.chkFuture.value, LastName: $scope.LastName,
                SavedCards: savedCardItems, SelectedPaymentAmountType: $scope.Amount.name, PaymentTimeOption: PaymentTimeOption, FutureAmount: FutureAmount, FutureOtherAmount: FutureOtherAmount
            });
            var collectiondata = { "AccountNumber": $scope.AccountNumber, "Amount": CardPaymentAmount, "FuturePaymentDate": FuturePaymentDate, 'IsTotalAmountDue': $scope.IsTotalAmountDue };
            AccountInformationService.CheckValidCardTransaction(collectiondata, $scope.baseAddress).success(function (data) {
                if (JSON.parse(data) == "true") {
                    $scope.MainCardError = "";
            $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(CardPaymentConfirmationJsonFormat);
            if ($scope.result == "Holder data successfully") {
                $rootScope.loggedIn = true;
                $location.path('/CardPaymentConfirmation');
            }
                }else {$scope.MainCardError = JSON.parse(data);}
            }).error(function (error) {
                if (error == "") {$scope.MainError = "Server connection problem";}
                else {$scope.MainError = error.Message;;}});            
        }
    }    
 
    $scope.DeleteCard = function () {
        increaseLoadingCounter();
        $('#CardConfirmModal .close').click();
        var BaseAddress = $scope.baseAddress;
        paymentAmountService.DeleteCard($scope.TokenId, BaseAddress).then(function (data) {
            $scope.PaymentTypeDebitCredit();
            decreaseLoadingCounter();
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }
    $scope.DeleteConfirm=function(data)
    {               
        $scope.TokenId = data.TokenId;
        PopCardDelete();
    }

    function PopCardDelete() {
        $('#CardConfirmModal').modal({

            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }
    $scope.GetAddress = function () {        
        if ($scope.chkLastBillAddress.value == true && chkBilling==0)
        {
            chkBilling = 1;
            chkUserProfile = 0;
            $scope.chkuserProfileaddress.value = false;
            GetLastBillingAddress(0);
        }
        else if ($scope.chkuserProfileaddress.value == true && chkUserProfile==0)
        {
            chkUserProfile = 1;
            chkBilling = 0;
            $scope.chkLastBillAddress.value = false;
            GetUserProfileAddress();            
        }
        else
        {            
            chkBilling = 0, chkUserProfile = 0;
            ClearBillingAddress();
        }
    };
    function GetUserProfileAddress() {
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        paymentAmountService.GetUserProfileAddress(AccountNumber, BaseAddress).then(function (data) {
            $scope.FirstName = data.FullName.substring(0, data.FullName.lastIndexOf(" "));
            $scope.LastName = data.FullName.substring(data.FullName.lastIndexOf(" ") + 1);
            $scope.Email = data.Email;
            $scope.PrimaryNumber = data.CellNumber;                
            $scope.Address = data.Address1;
            $scope.City = data.City;
            $scope.State = data.State;
            $scope.Zip = data.Zip;
            bindStateList();
            decreaseLoadingCounter();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }

    function GetLastBillingAddress(tokenId)
    {
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        paymentAmountService.GetBillingAddress(AccountNumber, tokenId, BaseAddress).success(function (data) {
                $scope.FirstName = data.FirstName;
                $scope.LastName = data.LastName;
                $scope.PrimaryNumber = data.PrimaryNumber;
                $scope.Email = data.EmailID;
                $scope.Address = data.Address;
                $scope.City = data.City;
                $scope.State = data.State;
                $scope.Zip = data.Zip;                
            bindStateList();
            decreaseLoadingCounter();
        }).error(function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }
    $scope.RemoveEmailError = function () {
        $scope.errorEmail = "";
    };
    $scope.payNow = function () {
        $scope.Amount.name = 'totalAmount';
        $scope.FutureAmount.name = '';
        $scope.payNowDisabled = false;
        $scope.FuturementDisabled = true;
        $scope.FuturePaymentDate = "";
        $scope.Amountdisabled = true;
        $scope.FutureOtherAmount = $scope.TotalAmountDue;
        $scope.checkboxFutureAccount.value = false;
        $scope.chkFuture.value = false;
        $scope.chkAccountFuture = false;
    };
    $scope.setFuturement = function () {
        $scope.Amount.name = '';
        $scope.FutureAmount.name = 'totalAmount';
        $scope.payNowDisabled = true;
        $scope.FuturementDisabled = false;
        $scope.disabled = true;
        $scope.OtherAmount = $scope.TotalAmountDue;
        $scope.checkboxFutureAccount.value = true;
        $scope.chkFuture.value = true;
        $scope.chkAccountFuture = true;
    };
    $scope.FuturementSelectAmount = function () {        
        if ($scope.FutureAmount.name == "totalAmount") {
            $scope.Amountdisabled = true;           
        }
        if ($scope.FutureAmount.name == "otherAmount") {
            $scope.Amountdisabled = false;     
    }
    };
}]);


