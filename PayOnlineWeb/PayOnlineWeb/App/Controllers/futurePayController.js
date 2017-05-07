app.controller('futurePayController', ['$scope', '$rootScope', '$location', 'futurePayService', 'WebApiService', function ($scope, $rootScope, $location, futurePayService, WebApiService) {
    //------------------------------------- Global Section --------------------------------------------------
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.CheckACN = sessionStorage.getItem('ACNCheck');
    $scope.baseAddress = WebApiService.baseAddress;    
    $scope.ACHPanel = true;
    $scope.CardPanel = false;
    $scope.PaymentType = { name: 'ACH' };
    $scope.FutureAmount = { name: 'TotalAmount' };
    $scope.SelectACHTypeValue = { name: 'NewACH' };
    $scope.ACHAccountType = { name: 'Checking' };
    $scope.SelectCardTypeValue = { name: 'NewCard' };
    $scope.CardType ={Name: 'V'};
    $scope.DisableOtherAmountTxt = true;
    $scope.chkLastBillingAddress = false;
    $scope.chkUserProfileAddress = false;
    $scope.NewACH = true;
    $scope.SavedACH = false;
    $scope.NewCard = true;
    $scope.SavedCard = false;
    $scope.SavedAddress = false;
    $scope.NewAddress = true;
    $scope.PaymentTypeACH = false;
    $scope.btnDeleteACHDisabled = false;
    $scope.FuturePayPanel = true;
    $scope.ACHConfirmationPanel = false;
    $scope.CardConfirmationPanel = false;
    $scope.SavedACHRadio = false;
    $scope.amount = null;
    $scope.AccountType = null;
    $scope.FirstACHDiv = false;
    $scope.secondACHDiv = false;
    $scope.FirstCardDiv = false;
    $scope.SecondCardDiv = false;
    $scope.ACHRadio = false;    
    $scope.achEmail = '';
    $scope.IsTotalAmountDue = false;
    var chkBilling = 0, chkUserProfile = 0;
    var savedAchItem = function () {
        this.BankABA = '';
        this.BankAccountName = '';
        this.BankAcctType = '';
        this.BankHolder = '';
        this.BankName = '';
        this.BankAcctNo = '';
        this.achEmail = '';
        this.PrimaryNumber = '';
        this.BankAcctNoView = '';
        this.counter = '';
        this.ID = '';
        this.Selected = false;
    };
    var savedAchItems = [];
    $scope.savedAch = {};

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

    var loadingCounter = 0;
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

    //-------------------------------------- Main Section ---------------------------------------------------
    (function () {
       increaseLoadingCounter();
       futurePayService.UserAccountInformation($scope.AccountNumber,futurePayService.getFuturePayWithAgreement(), $scope.baseAddress).success(function (data) {
            userData(data);
            BindSavedAchDetails();
        }).error(function (error) {
           decreaseLoadingCounter();
            if (error == "") {
                $scope.MainError = "Server connection problem";
            }
            else {

                $scope.MainError = error.Message;;
            }
        });
    })();


    
    // Get Main Information of account.
    function userData(data) {
       $scope.achEmail = sessionStorage.getItem('Email');
        if (futurePayService.getFuturePayWithAgreement()) {
            $scope.FuturePayWithAgreement = true;
            $scope.FuturePayWithOutAgreement = false;
        }
        else
        {
            $scope.FuturePayWithAgreement = false;
            $scope.FuturePayWithOutAgreement = true;
        }
        $scope.TotalAmountDue = parseFloat(data.Amount).toFixed(2);
        $scope.OtherTotalAmountDue = parseFloat(data.Amount).toFixed(2);
        $scope.DueDate = data.DueDate;
        $scope.NextDueDate = data.NextDueDate;
        $scope.Name = data.Name;
        $scope.CellNumber = data.CellNumber;
        $scope.AcctDaysPastDue = data.AcctDaysPastDue;        
        $scope.Error = data.Error;
        $scope.MinimumAmount = data.AcctDaysPastDue > 0 ? data.Amount : data.MinimumAmount;
        $scope.MaximumAmount = data.MaximumAmount;
        if ($scope.FutureAmount.name === "TotalAmount") { $scope.amount = parseFloat($scope.TotalAmountDue).toFixed(2);}
        else if ($scope.FutureAmount.name === "OtherAmount") $scope.amount = parseFloat($scope.OtherTotalAmountDue).toFixed(2);
       decreaseLoadingCounter();
    }

    $scope.btnCancel = function () {
        $scope.BankRoutingNumber = '';
        $scope.CheckingAccountNumber = '';
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;        
        $location.path('/FuturePayAgreement');
    }

    this.setFuturePayWithAgreement = function () {
        futurePayService.setFuturePayWithAgreement(true);
    };
   

    //------------------------------------- Set Future Pay Section ---------------------------------------

    //Select amount function.
    $scope.SelectAmount = function () {
        if ($scope.FutureAmount.name === 'TotalAmount') $scope.DisableOtherAmountTxt = true;
        else $scope.DisableOtherAmountTxt = false;

    };

    //------------------------------------- Select Payment Type Section ----------------------------------

    if ($scope.CheckACN == "C" || $rootScope.ACHBlock === "Blocked") {
        $scope.ACHRadio = true;
        $scope.PaymentType.name = "Card";
        $scope.ACHPanel = false;
        $scope.CardPanel = true;
        GetSavedCardDetails();
    }

    $scope.SelectPaymentType = function () {
        if ($scope.PaymentType.name === "ACH") { BindSavedAchDetails(); $scope.ACHPanel = true; $scope.CardPanel = false; }
        else { GetSavedCardDetails(); $scope.ACHPanel = false; $scope.CardPanel = true; }
    };

    //------------------------------------- ACH Section ---------------------------------------------------
    $scope.SelectACHType = function () {
        if ($scope.SelectACHTypeValue.name === "NewACH") { ClearACHDetails();$scope.NewACH = true; $scope.SavedACH = false; }
        else { BindSavedAchDetails(); $scope.NewACH = false; $scope.SavedACH = true; }
    }

    function BindSavedAchDetails() {
       increaseLoadingCounter();
        futurePayService.GetSavedAchDetails($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            savedAchItems = [];
            for (var counter = 0; counter < data.data.length; counter++) {
                var item = new savedAchItem();
                item.BankABA = data.data[counter].BankABA;
                item.BankAccountName = data.data[counter].BankHolder;
                item.BankAcctType = data.data[counter].BankAcctType == "P" ? "Checking" : "Savings";
                item.BankHolder = data.data[counter].CreatedBy;
                item.BankName = data.data[counter].BankName;
                item.BankAcctNo = data.data[counter].BankAcctNo;
                item.achEmail = data.data[counter].Email;
                item.PrimaryNumber = data.data[counter].PrimaryNumber;
                item.BankAcctNoView = "XXXXX-" + data.data[counter].BankAcctNo.substr(-4);
                item.ID = data.data[counter].ID;
                item.counter = counter;
                if (counter === data.data.length - 1)
                    item.Selected = true;
                savedAchItems.push(item);
            }
            $scope.savedAch = savedAchItems;

            if (savedAchItems) {
                if (savedAchItems.length > 0) {                   
                    $scope.SelectACHTypeValue.name = "SavedACH"
                    $scope.SavedACH = true;
                    $scope.NewACH = false;
                    $scope.SavedACHRadio = true;
                }
                else {
                    $scope.SavedACH = false;
                    $scope.SelectACHTypeValue.name = "NewACH"
                    $scope.NewACH = true;
                    $scope.SavedACHRadio = false;
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
    };

    $scope.radioSavedAch= function (data) {
        savedAchItems.forEach(function (item) {
            if (item.ID === data.ID) {
                item.Selected = true;
                $scope.AchGrid = data.BankAcctType;
                $scope.AccountHolderPhoneNumber = data.PrimaryNumber;
                $scope.achEmail = data.achEmail;
            }
            else { item.Selected = false; }
        });
        $scope.checkboxFutureAccount.value = false;
        ClearNewAchDetails();
        $scope.btnNext = false;
    };

    $scope.AchDeletePop = function (data) {
        $scope.ACHId = data.ID;
        popAchDelete();
    }

    $scope.AchDelete = function () {
        var ID = $scope.ACHId
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDeleteACHDisabled = true;
        futurePayService.DeleteSavedAch(ID, $scope.AccountNumber, $scope.baseAddress).then(function (data) {
                BindSavedAchDetails();
                $('#myModal2 .close').click();                      
        },
        function (error) {
            $scope.btnDeleteACHDisabled = false;
            $scope.Loadname = "";
            if (error == "") {
                $scope.AchDelete = "Server connection problem";
            }
            else {
                $scope.AchDelete = error.Message;;
            }
        });
    }

    function popAchDelete() {
        $('#myModal2').modal('show');
        $('#myModal2').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    function ClearACHDetails()
    {
       // $scope.BankAccountName = "";
      //  $scope.CheckingAccountNumber = "";
       // $scope.BankRoutingNumber = "";
        $scope.errorCellNumber = "";
        $scope.errorBankAccountName = "";
        $scope.errorCheckingAccountNumber = "";
        $scope.ErrorRouting = "";
        $scope.errorEmail = "";
    }


    //------------------------------------- Card Section --------------------------------------------------
    $scope.SelectCardType = function () {
        if ($scope.SelectCardTypeValue.name === "NewCard") { ClearBillingAddress(); $scope.NewCard = true; $scope.SavedCard = false; $scope.SavedAddress = false; $scope.NewAddress = true; }
        else { GetSavedCardDetails(); ClearNewCard(); $scope.NewCard = false; $scope.SavedCard = true; $scope.SavedAddress = true; $scope.NewAddress = false; }
    }
        
    function GetSavedCardDetails() {        
       increaseLoadingCounter();
        futurePayService.GetSavedCardDetails($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            savedCardItems = [];
            for (var counter = 0; counter < data.data.length; counter++) {
                var item = new savedCardItem();
                item.TokenId = data.data[counter].TokenId;
                item.CardNumber = data.data[counter].CardNumber;
                item.CardType = data.data[counter].CardType;
                item.CardName = data.data[counter].CardName;
                item.CardExpiry = data.data[counter].CardExpiry;
                if (counter === data.data.length - 1) {
                    item.Selected = true;
                    GetLastBillingAddress(item.TokenId);
                }
                savedCardItems.push(item);
            }
            $scope.savedCards = savedCardItems;
            if ($scope.savedCards.length > 0) {
                $scope.PaymentTypeCard = true;
                $scope.SelectCardTypeValue.name = 'SavedCard';
                $scope.SavedCard = true;
                $scope.NewCard = false;
                $scope.NewAddress = false;
                $scope.SavedAddress = true;
            } else { $scope.PaymentTypeCard = false; }
           decreaseLoadingCounter();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
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
        $scope.SavedAddress = true;
        $scope.NewAddress = false;
        ClearNewCard();
    }
    $scope.updateSavedCardCVV = function (data) {
        savedCardItems.forEach(function (item) {
            if (item.TokenId === data.TokenId) {
                item.CVV = data.CVV;
            }            

        });
    };
    function ClearNewCard() {
        $scope.CardType = { Name: "V" };
        $scope.CardName = "";
        $scope.CardNumber = "";
        $scope.ExpiryYear = "";
        $scope.ExpiryMonth = "";
        $scope.CvNumber = "";
        $scope.errorCardName = "";
        $scope.errorCardNumber = "";
        $scope.errorExpiryMonth = "";
        $scope.errorExpiryYear = "";
        $scope.errorCVVNumber = "";
        $scope.chkuserProfileaddress = false;
        $scope.chkLastBillAddress = false;

    }

    function bindStateList() {
        futurePayService.GetStateList().then(function (data) {
            var selectedState = $scope.State;
            var states = data.data.States;
            var selectedOption = states.find(function (state) { return state.Value === selectedState; });
            $scope.StateList = states;
            $scope.selectedState = selectedOption;
        },
          function (err) {
              $scope.ErrorMessage = "Get State Failed!";
          });
    };

    $scope.GetAddress = function () {
        if ($scope.chkLastBillAddress == true && chkBilling == 0) {
            chkBilling = 1;
            chkUserProfile = 0;
            $scope.chkuserProfileaddress = false;
            GetLastBillingAddress(0);
        }
        else if ($scope.chkuserProfileaddress == true && chkUserProfile == 0) {
            chkUserProfile = 1;
            chkBilling = 0;
            $scope.chkLastBillAddress = false;
            GetUserProfileAddress(0);
        }
        else {
            chkBilling = 0, chkUserProfile = 0;
            ClearBillingAddress();
        }
    };

    function GetUserProfileAddress() {      
       increaseLoadingCounter();
        futurePayService.GetUserProfileAddress($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            $scope.FirstName = data.data.FullName.substring(0, data.data.FullName.lastIndexOf(" "));
            $scope.LastName = data.data.FullName.substring(data.data.FullName.lastIndexOf(" ") + 1);
            $scope.CardEmail = data.data.Email;
            $scope.PrimaryNumber = data.data.CellNumber;
            $scope.Address = data.data.Address1;
            $scope.City = data.data.City;
            $scope.State = data.data.State;
            $scope.Zip = data.data.Zip;
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
    };

    function GetLastBillingAddress(tokenId) {
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        futurePayService.GetBillingAddress(AccountNumber, tokenId, BaseAddress).success(function (data) {
            $scope.FirstName = data.FirstName;
            $scope.LastName = data.LastName;
            $scope.PrimaryNumber = data.PrimaryNumber;
            $scope.CardEmail = data.EmailID;
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

    $scope.EditAddress = function () {        
        $scope.SavedAddress = false;
        $scope.NewAddress = true;
    };

    function ClearBillingAddress() {
        $scope.CardEmail = '';
        $scope.Address = '';
        $scope.PrimaryNumber = '';
        $scope.Address = '';
        $scope.City = '';
        $scope.LastName = '';
        $scope.FirstName = '';
        $scope.Zip = '';
        $scope.selectedState = '';
        $scope.errorFirstName = "";
        $scope.errorLastName = "";
        $scope.errorPrimaryNumber = "";
        $scope.errorCardEmail = "";
        $scope.errorAddress = "";
        $scope.errorselectedState = "";
        $scope.errorZip = "";
    };

    $scope.DeleteCard = function () {        
        futurePayService.DeleteCard($scope.TokenId, $scope.baseAddress).then(function (data) {
            $('#CardConfirmModal .close').click();
            GetSavedCardDetails();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }
    $scope.DeleteConfirm = function (data) {
        $scope.TokenId = data.TokenId;
        PopCardDelete();
    }

    function PopCardDelete() {
        $('#CardConfirmModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }



    //------------------------------------------------ Future Payment Section----------------------------------------
    //paymentMethod =1=ACH,2=Card
    function GetFee(paymentMethod)
    {
       increaseLoadingCounter();
        futurePayService.getFee(paymentMethod, $scope.AccountNumber, $scope.baseAddress).success(function (data) {            
            $scope.fee = data.Fee;
            $scope.totalAmount = (parseFloat($scope.amount) + parseFloat($scope.fee)).toFixed(2);
           decreaseLoadingCounter();
        }, function (error) {
            if (error == "") {
                $scope.MainError = "Server connection problem";
            }
            else {
                $scope.MainError = error.Message;;
            }
        });
    }
    
    function MainValidation() {
       
        var returnValue = true;        
        var amountRegex = /^\d+(\.\d+)?$/;        
        if ($scope.FutureDate == "" || $scope.FutureDate == undefined) { $scope.dateError = "Please select date"; return returnValue = false; }
        else { $scope.dateError = ""; returnValue = true; }
        if (!amountRegex.test($scope.OtherTotalAmountDue)) { $scope.AmountError = "Enter Only Amount"; return returnValue = false; }
        else if (parseInt($scope.AcctDaysPastDue) > 0 && parseFloat($scope.OtherTotalAmountDue) < parseFloat($scope.TotalAmountDue)) { $scope.AmountError = "Amount allow Min: $" + $scope.TotalAmountDue; return returnValue = false; }
        else if ($scope.FutureAmount.name === "OtherAmount" && parseFloat($scope.OtherTotalAmountDue) < parseFloat($scope.MinimumAmount)) { $scope.AmountError = "Minimum amount allowed: $" + $scope.MinimumAmount; return returnValue = false; }
        else if ($scope.FutureAmount.name === "OtherAmount" && parseFloat($scope.OtherTotalAmountDue) > parseFloat($scope.MaximumAmount)) { $scope.AmountError = "Maximum amount allowed: $" + $scope.MaximumAmount; return returnValue = false; }
        else if ($scope.FutureAmount.name === "TotalAmount" && (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount)) < parseFloat($scope.TotalAmountDue))
        { $scope.AmountError = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }
        else { $scope.AmountError = ""; returnValue = true; }

        return returnValue;
    };

    function AchPaymentValidation() {
       
        var returnValue = true;
        var CheckingAccountNumberregex = /^\d*$/;
        var regexCellNumber = /^[0-9+-]{10,12}$/;
        var regexBankRoutingNumber = /^(\d+){9,}$/;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        
        if ($scope.CellNumber == "" || $scope.CellNumber == undefined) { $scope.errorCellNumber = "Please Enter Account Holder Primary Number"; return returnValue = false; }
        else if (!regexCellNumber.test($scope.CellNumber)) { $scope.errorCellNumber = "Enter Only Primary Number"; return returnValue = false; }
        else { $scope.errorCellNumber = ""; returnValue = true; }

        if ($scope.BankAccountName == "" || $scope.BankAccountName == undefined) { $scope.errorBankAccountName = "Please Enter Bank Account Name"; return returnValue = false; }
        else { $scope.errorBankAccountName = ""; returnValue = true; }

        if ($scope.CheckingAccountNumber == "" || $scope.CheckingAccountNumber == undefined) { $scope.errorCheckingAccountNumber = "Please Enter " + $scope.ACHAccountType.name + " Account Number"; return returnValue = false; }
        else if (!CheckingAccountNumberregex.test($scope.CheckingAccountNumber)) { $scope.errorCheckingAccountNumber = "Enter Only Numbers"; return returnValue = false; }
        else { $scope.errorCheckingAccountNumber = ""; returnValue = true; }

        if ($scope.BankRoutingNumber == "" || $scope.BankRoutingNumber == undefined) { $scope.ErrorRouting = "Please Enter Bank Routing Number"; return returnValue = false; }
        else if (!regexBankRoutingNumber.test($scope.BankRoutingNumber)) { $scope.ErrorRouting = "Must be 9 numeric values"; return returnValue = false; }
        else { $scope.ErrorRouting = ""; returnValue = true; }

        if ($scope.achEmail == "" || $scope.achEmail == undefined) { $scope.errorEmail = "Please Enter Email"; return returnValue = false; }
        else if (!regexEmail.test($scope.achEmail)) { $scope.errorEmail = "Not an email!"; return returnValue = false; }
        else { $scope.errorEmail = ""; returnValue = true; }
        
        return returnValue;

    };

    function cardValidation()
    {
    
        var returnValue = true;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var regaxZip = /^(\d{5}-\d{4}|\d{5})$/;

        if ($scope.SelectCardTypeValue.name === "NewCard") {
            if ($scope.CardName == "" || $scope.CardName == undefined) { $scope.errorCardName = "Please Enter Name"; return returnValue = false; }
            else { $scope.errorCardName = ""; returnValue = true; }

            if ($scope.CardNumber == "" || $scope.CardNumber == undefined || $scope.CardNumber.length != '16') { $scope.errorCardNumber = "Please Enter vaild Card"; return returnValue = false; }
            else { $scope.errorCardNumber = ""; returnValue = true; }

            if ($scope.ExpiryMonth == "" || $scope.ExpiryMonth == undefined) { $scope.errorExpiryMonth = "Please Select"; return returnValue = false; }
            else { $scope.errorExpiryMonth = ""; returnValue = true; }

            if ($scope.ExpiryYear == "" || $scope.ExpiryYear == undefined) { $scope.errorExpiryYear = "Please Select"; return returnValue = false; }
            else { $scope.errorExpiryYear = ""; returnValue = true; }

            if ($scope.CvNumber == "" || $scope.CvNumber == undefined) { $scope.errorCVVNumber = "Please Enter CVV Number"; return returnValue = false; }
            else { $scope.errorCVVNumber = ""; returnValue = true; }
        }

        if ($scope.FirstName == "" || $scope.FirstName == undefined) { $scope.errorFirstName = "Please Enter First Name"; return returnValue = false; }
        else { $scope.errorFirstName = ""; returnValue = true; }

        if ($scope.LastName == "" || $scope.LastName == undefined) { $scope.errorLastName = "Please Enter Last Name"; return returnValue = false; }
        else { $scope.errorLastName = ""; returnValue = true; }

        if ($scope.PrimaryNumber == "" || $scope.PrimaryNumber == undefined) { $scope.errorPrimaryNumber = "Please Enter Primary Number"; return returnValue = false; }
        else { $scope.errorPrimaryNumber = ""; returnValue = true; }

        if ($scope.CardEmail == "" || $scope.CardEmail == undefined) { $scope.errorCardEmail = "Please Enter Email"; return returnValue = false; }
        else if (!regexEmail.test($scope.CardEmail)) { $scope.errorCardEmail = "Not an email!"; return returnValue = false; }
        else { $scope.errorCardEmail = ""; returnValue = true; }

        if ($scope.Address == "" || $scope.Address == undefined) { $scope.errorAddress = "Please Enter Address"; return returnValue = false; }
        else { $scope.errorAddress = ""; returnValue = true; }

        if ($scope.City == "" || $scope.City == undefined) { $scope.errorCity = "Please Enter City"; return returnValue = false; }
        else { $scope.errorCity = ""; returnValue = true; }

        if ($scope.selectedState== "" || $scope.selectedState == undefined) { $scope.errorselectedState = "Please Please Select State"; return returnValue = false; }
        else { $scope.errorselectedState = ""; returnValue = true; }

        if ($scope.Zip == "" || $scope.Zip == undefined) { $scope.errorZip = "Please Enter Zip"; return returnValue = false; }
        else if (!regaxZip.test($scope.Zip)) { $scope.errorZip = "Not an Zip!"; return returnValue = false; }
        else { $scope.errorZip = ""; returnValue = true; }

                
        return true;
    }

    $scope.FuturePayment = function () {
        if (MainValidation() == true) {

            if ($scope.FutureAmount.name === 'TotalAmount') {
                $scope.amount = $scope.TotalAmountDue;
                $scope.IsTotalAmountDue = true;
            }
            else {
                $scope.amount = parseFloat($scope.OtherTotalAmountDue).toFixed(2);
                $scope.IsTotalAmountDue = false;
            }
            //--------------- ACH Radio selection------------------
            if ($scope.SelectACHTypeValue.name === "SavedACH") {
                savedAchItems.forEach(function (item) {
                    if (item.Selected) {
                        $scope.BankRoutingNumber = item.BankABA;
                        $scope.BankAccountName = item.BankAccountName;
                        $scope.AccountType = item.BankAcctType;
                        $scope.Name = item.BankHolder;
                        $scope.BankName = item.BankName;
                        $scope.BankAccountNumber = item.BankAcctNo;
                    }
                });
            }

            //--------------- Card Radio selection---------------------
            if ($scope.SelectCardTypeValue.name === "SavedCard") {
                savedCardItems.forEach(function (item) {
                    if (item.Selected) {
                        $scope.TokenId = item.TokenId;
                        $scope.CvNumber = item.CVV;
                        $scope.CardName = item.CardName;
                        $scope.ExpiryMonth = item.CardExpiry.substring(0, 2);
                        $scope.ExpiryYear = item.CardExpiry.substring(3, 7);
                        $scope.CardNumber = item.CardNumber;
                        $scope.CardLastDigit = item.CardNumber;
                        $scope.CardType = item.CardType == "V" ? "Visa" : "Master";
                    }
                });
            }

            // ACH Payment ---------------
            if ($scope.PaymentType.name === "ACH") {                
                GetFee("1");//paymentMethod =1=ACH,2=Card 
                $scope.FirstACHDiv = true;
                var ValidTransactionData = { "IsFuturePayWithAgreement": futurePayService.getFuturePayWithAgreement(), "AccountNumber": $scope.AccountNumber, "Amount": $scope.amount, "Routing": $scope.BankRoutingNumber, "FuturePaymentDate": $scope.FutureDate, "PaymentMethod": "ACH",'IsTotalAmountDue': $scope.IsTotalAmountDue };
                futurePayService.ValidateTransaction(ValidTransactionData, $scope.baseAddress).success(function (data) {
                    if (data.Error != null) $scope.error= data.Error;
                    else if (data.ErrorRouting != null) $scope.error = data.ErrorRouting;
                    else if (data.ErrorFutureDate != null) $scope.error = data.ErrorFutureDate;
                    else
                    {  
                        $scope.AccountType = $scope.ACHAccountType.name;                        
                        if (AchPaymentValidation() == true && $scope.SelectACHTypeValue.name === "NewACH") {
                            $scope.BankAccountNumber = $scope.CheckingAccountNumber;
                            $scope.BankName = data.BankName;
                            ConfirmScreenRedirection("NewACH")                                                                                              
                        }
                        else if ($scope.SelectACHTypeValue.name === "SavedACH") {                    
                           
                            ConfirmScreenRedirection("SavedACH");
                        }
                    }
                   decreaseLoadingCounter();
                }).error(function (error) {
                   decreaseLoadingCounter();
                    if (error == "") {

                        $scope.error = "Server connection problem";
                    }
                    else {
                        $scope.error = error.Message;;
                    }
                });
            }

            //  Card Payment-------------------------

            if ($scope.PaymentType.name === "Card" && cardValidation() == true) {
                GetFee("2");//paymentMethod =1=ACH,2=Card  
                $scope.FirstCardDiv = true;
                var ValidTransactionData = { "IsFuturePayWithAgreement": futurePayService.getFuturePayWithAgreement(), "AccountNumber": $scope.AccountNumber, "Amount": $scope.amount, "FuturePaymentDate": $scope.FutureDate, "PaymentMethod": "Card", 'IsTotalAmountDue': $scope.IsTotalAmountDue };
                futurePayService.ValidateTransaction(ValidTransactionData, $scope.baseAddress).success(function (data) {
                    if (data.Error != null) $scope.error= data.Error;
                    else if (data.ErrorRouting != null) $scope.error = data.ErrorRouting;
                    else if (data.ErrorFutureDate != null) $scope.error = data.ErrorFutureDate;
                    else
                    { 
                       if ($scope.SelectCardTypeValue.name === "NewCard")
                       {
                           $scope.CardLastDigit = $scope.CardNumber.substr(-4);
                           $scope.CardType = $scope.CardType.Name == "V" ? "Visa" : "Master"; 
                           ConfirmScreenRedirection("NewCard");                          
                       }
                       else if ($scope.SelectCardTypeValue.name === "SavedCard")
                       {
                           
                           if ($scope.CvNumber == '') { $scope.errorCvNumber = "Please enter CVV number!"; } else {
                               $scope.errorCvNumber = "";
                               ConfirmScreenRedirection("SavedCard");                               
                           }
                       }
                    }
                   decreaseLoadingCounter();
                }).error(function (error) {
                   decreaseLoadingCounter();
                    if (error == "") {

                        $scope.error = "Server connection problem";
                    }
                    else {
                        $scope.error = error.Message;;
                    }
                });
            }
        }
    };

    function ConfirmScreenRedirection(Type)
    {
        if(Type=="NewACH" ||Type=="SavedACH")
        {
            $scope.FuturePayPanel = false;
            $scope.ACHConfirmationPanel = true;
            $scope.CardConfirmationPanel = false;
       
        }       
        else if(Type=="NewCard" ||Type=="SavedCard")
        {
            $scope.FuturePayPanel = false;
            $scope.ACHConfirmationPanel = false;
            $scope.CardConfirmationPanel = true;
        }
    }

    $scope.ACHFuturePayConfirm = function () {
        increaseLoadingCounter();
     
        var ACHFuturePaydata = {
            "AccountNumber": $scope.AccountNumber, "PaymentAmount": $scope.amount, "FeeAmoun": $scope.fee, "RountingNumber": $scope.BankRoutingNumber,
            "BankAccountNumber": $scope.BankAccountNumber, "BankName": $scope.BankName, "BankHolder": $scope.Name, "AccountType": $scope.AccountType=="Checking" ? "P" : "S" ,
            "UpdatedPhone": $scope.CellNumber, "Email": $scope.achEmail, "BankAccountName": $scope.BankAccountName, "FuturePaymentDate": $scope.FutureDate
        };

        futurePayService.ACHFuturePayConfirm(ACHFuturePaydata, $scope.baseAddress).success(function (data) {
            if (JSON.parse(data) == "You are trying to make the same payment again.") {
                $scope.Error = JSON.parse(data);
               decreaseLoadingCounter();
                return;
            }
            PaymentConfirmationAlert();
            $scope.FirstACHDiv = false;
            $scope.secondACHDiv = true;
            $scope.confirmation = JSON.parse(data);            
           decreaseLoadingCounter();
        }).error(function (error) {
           decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };

    $scope.CardFuturePayConfirm = function () {
       increaseLoadingCounter();
        var cardPaymentConfirmationdata = {
            "CardName": $scope.CardName, "Fee": $scope.fee, "TokenId": $scope.TokenId,
            "FirstName": $scope.FirstName, "LastName": $scope.LastName, "Street": $scope.Address, "City": $scope.City, "State": $scope.State, "PostelCode": $scope.Zip,
            "Email": $scope.CardEmail, "CardNumber": $scope.CardNumber, "ExpirationMonth": $scope.ExpiryMonth, "ExpirationYear": $scope.ExpiryYear, "FuturePaymentDate": $scope.FutureDate,
            "CardType": $scope.CardType== "Visa" ? "V" : "M", "CvNumber": $scope.CvNumber, "Amount": $scope.amount, "AccountNumber": $scope.AccountNumber, "PrimaryNumber": $scope.PrimaryNumber.replace(/-/g, "")
        };

        futurePayService.CardFuturePayConfirm(cardPaymentConfirmationdata, $scope.baseAddress).success(function (data) {
            var result = JSON.parse(data);
            if (result.length == 10) {
                $scope.confirmation = result;
                $scope.errortext = false;
                $scope.ConfirmationNumberShow = true;
                $scope.FirstCardDiv = false;
                $scope.SecondCardDiv = true;
                PaymentConfirmationAlert();
            }
            else {
                $scope.FirstCardDiv = true;
                $scope.SecondCardDiv = false;
                $scope.errortext = true;
                $scope.ConfirmationNumberShow = false;
                $scope.error = result;
            }
           decreaseLoadingCounter();
        }).error(function (error) {
           decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };
    
    function PaymentConfirmationAlert() {
        $('#ConfirmationModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }
    $scope.Edit = function () {        
        ClearACHDetails();
        ClearNewCard();        
        $scope.error = "";
        $scope.errorCvNumber = "";
        $scope.FirstACHDiv = false;
        $scope.secondACHDiv = false;
        $scope.FirstCardDiv = false;
        $scope.SecondCardDiv = false;
        $scope.FuturePayPanel = true;
        $scope.ACHConfirmationPanel = false;
        $scope.CardConfirmationPanel = false;
    };



}]);